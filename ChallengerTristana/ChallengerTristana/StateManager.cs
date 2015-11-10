using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ChallengerTristana
{
    internal class StateManager
    {
        public static Menu SpellMenu, KSMenu;

        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            SpellMenu = Program.TristanaMenu.AddSubMenu("Skills", "skill");
            SpellMenu.AddGroupLabel("Skills");

            SpellMenu.AddLabel("Combo");
            SpellMenu.Add("QCombo", new CheckBox("Use Q"));
            SpellMenu.Add("ECombo", new CheckBox("Use E"));

            SpellMenu.AddLabel("Harass");
            SpellMenu.Add("QHarass", new CheckBox("Use Q"));
            SpellMenu.Add("EHarass", new CheckBox("Use E"));
            SpellMenu.Add("HarassMana", new Slider("Mana Percentage", 40, 1, 100));

            SpellMenu.AddLabel("LaneClear/JungleClear");
            SpellMenu.Add("QClear", new CheckBox("Use Q"));
            SpellMenu.Add("EClear", new CheckBox("Use E"));
            SpellMenu.Add("EOnTower", new CheckBox("Use E on Tower"));
            SpellMenu.Add("ClearMana", new Slider("Mana Percentage", 40));

            SpellMenu.AddLabel("Don't use E");
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                SpellMenu.Add("DontUseE" + enemy.ChampionName, new CheckBox("Don't use E on " + enemy.ChampionName, false));
            }

            KSMenu = Program.TristanaMenu.AddSubMenu("KillSteal", "KS");
            KSMenu.AddGroupLabel("KillSteal");

            KSMenu.AddLabel("KS Spells");
            KSMenu.Add("WKS", new CheckBox("Use W"));
            KSMenu.Add("ERKS", new CheckBox("Use ER"));
            KSMenu.Add("RKS", new CheckBox("Use R"));

            KSMenu.AddLabel("Misc");
            KSMenu.Add("WDive", new CheckBox("Don't use W into Enemy Tower"));

            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            ResetAA();

            if (Player.IsDead || MenuGUI.IsChatOpen || Player.IsRecalling())
                return;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                Combo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                Harass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                LaneClear();

            KillSteal();
        }

        public static void ResetAA()
        {
            var target = Orbwalker.LastTarget;

            if (!target.IsValidTarget(Player.AttackRange) || target.IsDead)
                Orbwalker.ForcedTarget = null;
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Physical);
            if (target == null)
                return;
            var Quse = SpellMenu["QCombo"].Cast<CheckBox>().CurrentValue;
            var Euse = SpellMenu["ECombo"].Cast<CheckBox>().CurrentValue;
            var DontE = SpellMenu["DontUseE" + target.ChampionName].Cast<CheckBox>().CurrentValue;

            if (SpellManager.Q.IsReady() && Quse && target.IsValidTarget(SpellManager.Q.Range) && !target.IsZombie)
            {
                SpellManager.Q.Cast();
            }
            if (SpellManager.E.IsReady() && Euse && target.IsValidTarget(SpellManager.E.Range) && !target.IsZombie && !DontE)
            {
                SpellManager.E.Cast(target);
            }
            if (target.HasBuff("TristanaECharge") && target.IsValidTarget(Player.AttackRange))
            {
                Orbwalker.ForcedTarget = target;
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Physical);
            if (target == null || !target.IsValid)
                return;
            var Quse = SpellMenu["QHarass"].Cast<CheckBox>().CurrentValue;
            var Euse = SpellMenu["EHarass"].Cast<CheckBox>().CurrentValue;
            var ManaPercent = SpellMenu["HarassMana"].Cast<Slider>().CurrentValue;

            if (Quse && Euse)
            {
                if (SpellManager.E.IsReady() && target.IsValidTarget(SpellManager.E.Range) && !target.IsZombie && Player.ManaPercent >= ManaPercent)
                    SpellManager.E.Cast(target);
                if (SpellManager.Q.IsReady() && target.IsValidTarget(SpellManager.Q.Range) && target.HasBuff("TristanaECharge") && !target.IsZombie)
                    SpellManager.Q.Cast();
            }
            if (SpellManager.Q.IsReady() && target.IsValidTarget(SpellManager.Q.Range) && !target.IsZombie && Quse && !Euse)
                SpellManager.Q.Cast();
            if (SpellManager.E.IsReady() && target.IsValidTarget(SpellManager.E.Range) && !target.IsZombie && !Quse && Euse && Player.ManaPercent >= ManaPercent)
                SpellManager.E.Cast(target);
        }

        private static void LaneClear()
        {
            Tower();

            var Quse = SpellMenu["QClear"].Cast<CheckBox>().CurrentValue;
            var Euse = SpellMenu["EClear"].Cast<CheckBox>().CurrentValue;
            var ManaPercent = SpellMenu["ClearMana"].Cast<Slider>().CurrentValue;
           // var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, ObjectManager.Player.Position, 600, true);

            /*if (minion != null)
            {
                if (SpellManager.E.IsReady() && Euse)
                {
                    SpellManager.E.Cast(minion);
                    Chat.Print("E cast to Minion", Color.Aqua);
                }
            }*/
            if (SpellManager.E.IsReady() && Euse && Player.ManaPercent >= ManaPercent)
            {
                var minion = ObjectManager.Get<Obj_AI_Minion>().OrderBy(m => m.Health).FirstOrDefault(m => m.IsEnemy && m.IsValidTarget(SpellManager.E.Range));

                if (minion == null || !minion.IsValidTarget())
                    return;

                SpellManager.Q.Cast();
            }

            if (!SpellManager.E.IsReady())
            {
                var minion = ObjectManager.Get<Obj_AI_Minion>().Where(m => m.IsEnemy && m.IsValidTarget(SpellManager.E.Range));
                if (minion != null)
                {
                    foreach (var Eminion in minion.Where(Eminion => Eminion.HasBuff("TristanaECharge")))
                        Orbwalker.ForcedTarget = Eminion;
                }
            }
        }
        private static void KillSteal()
        {
            var target = TargetSelector.GetTarget(SpellManager.W.Range, DamageType.Physical);
            if (target == null || !target.IsValidTarget())
                return;
            var Wuse = KSMenu["WKS"].Cast<CheckBox>().CurrentValue;
            var Ruse = KSMenu["RKS"].Cast<CheckBox>().CurrentValue;
            var ERuse = KSMenu["ERKS"].Cast<CheckBox>().CurrentValue;
            var WTower = KSMenu["WDive"].Cast<CheckBox>().CurrentValue;
            var turret = EntityManager.Turrets.Enemies.FirstOrDefault(a => !a.IsDead && a.Distance(target) <= Player.BoundingRadius + (target.BoundingRadius / 2) + 44.2);

            if (SpellManager.W.IsReady() && Wuse && target.IsValidTarget(SpellManager.W.Range) && !target.IsZombie && target.Health + target.AttackShield <= Player.GetSpellDamage(target, SpellSlot.W))
            {
                if (!WTower)
                    SpellManager.W.Cast(target);
                if (WTower && target.Position.CountEnemiesInRange(800) == 1 && turret == null)
                    SpellManager.W.Cast(target);
            }
                
            if (SpellManager.R.IsReady() && Ruse && target.IsValidTarget(SpellManager.R.Range) && !target.IsZombie && target.Health + target.AttackShield <= Player.GetSpellDamage(target, SpellSlot.R))
                SpellManager.R.Cast(target);
            if (SpellManager.R.IsReady() && ERuse && target.IsValidTarget(SpellManager.R.Range) && !target.IsZombie && target.HasBuff("TristanaECharge"))
            {
                var EStack = target.GetBuffCount("TristanaECharge");
                var AD = Player.FlatMagicDamageMod + Player.BaseAttackDamage;
                var AP = Player.FlatMagicDamageMod + Player.BaseAbilityDamage;

                if (target.Health + target.AttackShield <= (Player.GetSpellDamage(target, SpellSlot.E) * ((0.3 * EStack) + 1) + AD * (0.50 + (0.15 * (Player.Level - 1))) + (AP * 0.5)) + Player.GetSpellDamage(target, SpellSlot.R))
                    SpellManager.R.Cast(target);
            }
        }

        private static void Tower()
        {
            var tower = ObjectManager.Get<Obj_AI_Turret>().FirstOrDefault(tw => tw.IsEnemy && tw.Distance(Player) < Player.AttackRange);
            if (tower == null || !tower.IsValidTarget())
                return;
            var Quse = SpellMenu["QClear"].Cast<CheckBox>().CurrentValue;
            var EOnTower = SpellMenu["EOnTower"].Cast<CheckBox>().CurrentValue;
            var ManaPercent = SpellMenu["ClearMana"].Cast<Slider>().CurrentValue;

            if (SpellManager.Q.IsReady() && Quse)
                SpellManager.Q.Cast();
            if (SpellManager.E.IsReady() && EOnTower && Player.ManaPercent >= ManaPercent)
                SpellManager.E.Cast(tower);
            if (tower.HasBuff("TristanaECharge"))
                Orbwalker.ForcedTarget = tower;
        }
    }
}