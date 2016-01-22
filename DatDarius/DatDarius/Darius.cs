using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Linq;
using Color = System.Drawing.Color;

namespace DatDarius
{
    public class Darius
    {
        private static Spell.Active Q = SpellManager.Q;
        private static Spell.Active W = SpellManager.W;
        private static Spell.Skillshot E = SpellManager.E;
        private static Spell.Targeted R = SpellManager.R;
        private static Spell.Skillshot Flash = SpellManager.Flash;
        private static Spell.Targeted Ignite = SpellManager.Ignite;
        private static Item Randuin;
        private static Item RavenousHydra;
        private static Item TitanicHydra;

        private static AIHeroClient Player = ObjectManager.Player;
        public static AIHeroClient QTarget = null;
        public static AIHeroClient ETarget = null;

        private static DamageIndicator Indicator;

        public static bool PlayerIsAttacking = false;
        public static bool CastingE = false;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete; ;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.BaseSkinName != "Darius")
                return;

            Randuin = new Item(3143, 500);
            RavenousHydra = new Item(3074, 400);
            TitanicHydra = new Item(3748, 385);

            Config.Initialize();
            Debug.Initialize();
            Indicator = new DamageIndicator();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnTick += Game_OnTick;
            Game.OnUpdate += Game_OnUpdate;
            Orbwalker.OnAttack += Orbwalker_OnAttack;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Interrupt.OnInterruptableTarget += Interrupt_OnInterruptableTarget;
            Dash.OnDash += Dash_OnDash;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;

            Chat.Print("Dat Darius Loaded", Color.LawnGreen);
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!Config.Menu["skin"].Cast<CheckBox>().CurrentValue)
                return;

            var skin = Config.Menu["sID"].DisplayName;
            
            switch (skin)
            {
                case "Classic":
                    Player.SetSkinId(0);
                    break;
                case "Lord Darius":
                    Player.SetSkinId(1);
                    break;
                case "Bioforge Darius":
                    Player.SetSkinId(2);
                    break;
                case "Woad King Darius":
                    Player.SetSkinId(3);
                    break;
                case "Dunkmaster Darius":
                    Player.SetSkinId(4);
                    break;
                case "Academy Darius":
                    Player.SetSkinId(8);
                    break;
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
                return;

            if (args.Slot == SpellSlot.E)
            {
                CastingE = true;
                Core.DelayAction(() => CastingE = false, 500);
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead || !Config.Menu["drawing"].Cast<CheckBox>().CurrentValue)
                return;
            
            if (Config.DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue && Q.IsLearned && Player.VisibleOnScreen)
                new Circle
                {
                    Color = Q.IsReady() && !Q.IsOnCooldown ? Color.LawnGreen : Color.Red,
                    BorderWidth = 2,
                    Radius = Q.Range
                }.Draw(Player.Position);

            if (Config.DrawMenu["drawE"].Cast<CheckBox>().CurrentValue && E.IsLearned && Player.VisibleOnScreen)
            {
                if (ETarget.IsValidTarget())
                    new Geometry.Polygon.Sector(Player.Position, Utility.PositionPrediction(ETarget, 0.25f), SpellManager.eAngle, E.Range)
                        .Draw(Utility.PositionPrediction(ETarget, 0.25f).Distance(Player.ServerPosition) < E.Range ? Color.Red : Color.Orange);
                else
                    new Geometry.Polygon.Sector(Player.Position, Game.CursorPos, SpellManager.eAngle, E.Range)
                        .Draw(Color.LawnGreen);
            }

            if (Config.DrawMenu["drawR"].Cast<CheckBox>().CurrentValue && R.IsLearned && Player.VisibleOnScreen)
                new Circle
                {
                    Color = R.IsReady() && !R.IsOnCooldown ? Color.LawnGreen : Color.Orange,
                    BorderWidth = 2,
                    Radius = R.Range
                }.Draw(Player.Position);

            // FlashE
            // FlashR

            if (Config.DrawMenu["drawText"].Cast<CheckBox>().CurrentValue)
            {
                if (Config.SpellMenu["autoR"].Cast<KeyBind>().CurrentValue)
                    Drawing.DrawText(Player.Position.WorldToScreen() + new Vector2(-50, 30), Color.LawnGreen, "AutoUlt Enabled", 10);
                else
                    Drawing.DrawText(Player.Position.WorldToScreen() + new Vector2(-50, 30), Color.Gray, "AutoUlt Disabled", 10);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.IsDead)
                return;

            QTarget = TargetSelector.GetTarget(Q.Range + 150, DamageType.Physical);
            ETarget = TargetSelector.GetTarget(E.Range + 150, DamageType.Physical);

            Logic.AutoIgnite();
            Logic.AutoUlt();
            Logic.TowerE();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                Mode.Combo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                Mode.Harass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                Mode.LaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                Mode.JungleClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                Mode.Flee();
        }
        
        public class Mode
        {
            public static void Combo()
            {
                if (Config.OrbMenu["useQcombo"].Cast<CheckBox>().CurrentValue)
                {
                    Logic.CastQ();
                }

                if (Config.OrbMenu["useEcombo"].Cast<CheckBox>().CurrentValue)
                {
                    Logic.CastE();
                }

                if (Config.OrbMenu["useRcombo"].Cast<CheckBox>().CurrentValue)
                {
                    Logic.CastR();
                }
            }

            public static void Harass()
            {
                if (Config.OrbMenu["useQharass"].Cast<CheckBox>().CurrentValue)
                {
                    Logic.CastQ();
                }

                if (Config.OrbMenu["useEharass"].Cast<CheckBox>().CurrentValue)
                {
                    Logic.CastE();
                }

                if (Config.OrbMenu["useRharass"].Cast<CheckBox>().CurrentValue)
                {
                    Logic.CastR();
                }
            }

            public static void LaneClear()
            {
                Logic.LaneClear();
            }

            public static void JungleClear()
            {
                Logic.JungleClear();
            }

            public static void Flee()
            {

            }
        }

        public class Logic
        {
            public static void CastQ()
            {
                if (!Q.IsReady() || !QTarget.IsValidTarget() || QTarget.IsZombie)
                    return;

                if (Config.SpellMenu["saveRMana"].Cast<CheckBox>().CurrentValue && Player.Mana - Utility.QMana() <= Utility.RMana())
                    return;

                if (PlayerIsAttacking || CastingE)
                    return;

                if (QTarget.GetResult().IsKillable)
                    return;

                var distance = QTarget.ServerPosition.Distance(Player.ServerPosition);
                var predDistance = Utility.PositionPrediction(QTarget, 0.25f).Distance(Utility.PositionPrediction(Player, 0.25f));

                if (Player.AttackRange < distance && distance < Q.Range && Player.AttackRange < predDistance)
                    Q.Cast();

                #region WIP Logic
                /*
                var MySpeed = Player.MoveSpeed;
                var TargetSpeed = Qtarget.MoveSpeed;
                var TargetBoundingRadius = Qtarget.BoundingRadius;
                var Distance = Utility.PositionPrediction(Qtarget, 0.75f).Distance(Utility.PositionPrediction(Player, 0.75f)) + 100;
                //var Distance = Qtarget.ServerPosition.Distance(Player.ServerPosition);

                if (Qtarget.IsMoving)
                {
                    if (Distance < Q.Range && Distance > Q.Range - 220)
                        Q.Cast();
                }
                else
                {
                    if (Utility.PositionPrediction(Player, 0.75f).Distance(Qtarget.ServerPosition) + 200 < Q.Range &&
                        Utility.PositionPrediction(Player, 0.75f).Distance(Qtarget.ServerPosition) > Q.Range - 220)
                        Q.Cast();
                }


                // Minimun distance
                if (Distance > Q.Range - 220)
                {
                    // Check target is moving
                    if (Qtarget.IsMoving)
                    {
                        // Chasing enemy
                        if (Player.IsFacing(Qtarget) && !Qtarget.IsFacing(Player))
                        {
                            if (Distance - (MySpeed * 0.75f) + (TargetSpeed * 0.75f) < Q.Range + TargetBoundingRadius)
                            {
                                Q.Cast();
                            }
                        }
                        // Running away from enemy
                        else if (!Player.IsFacing(Qtarget) && Qtarget.IsFacing(Player))
                        {
                            if (Distance + (MySpeed * 0.75f) - (TargetSpeed * 0.75f) < Q.Range + TargetBoundingRadius)
                            {
                                Q.Cast();
                            }
                        }
                        // Facing each other
                        else if (Player.IsBothFacing(Qtarget))
                        {
                            if (Distance > Q.Range + TargetBoundingRadius)
                            {
                                if (Distance + 100 - (MySpeed * 0.75f) - (TargetSpeed * 0.75f) < Q.Range + TargetBoundingRadius)
                                {
                                    Q.Cast();
                                }
                            }
                            else if (Distance < Q.Range + TargetBoundingRadius)
                            {
                                Q.Cast();
                            }
                        }
                    }
                    else
                    {
                        // Move to enemy
                        if (Player.IsFacing(Qtarget))
                        {
                            if (Distance - (MySpeed * 0.75f) / 2 < Q.Range + TargetBoundingRadius)
                            {
                                Q.Cast();
                            }
                        }
                    }
                }
                */
                #endregion
            }

            public static void CastE()
            {
                if (!E.IsReady() || !ETarget.IsValidTarget() || ETarget.IsZombie || ETarget.HasSpellShield())
                    return;

                if (Config.SpellMenu["saveRMana"].Cast<CheckBox>().CurrentValue && Player.Mana - Utility.QMana() <= Utility.RMana())
                    return;

                var minRange = Config.SpellMenu["minErange"].Cast<Slider>().CurrentValue;

                if (ETarget.IsMoving)
                {
                    var distance = Utility.PositionPrediction(ETarget, 0.25f).Distance(Player.ServerPosition) - ETarget.BoundingRadius;

                    if (minRange < distance && distance < E.Range)
                        E.Cast(Utility.PositionPrediction(ETarget, 0.25f));
                }
                else
                {
                    var distance = ETarget.ServerPosition.Distance(Player.ServerPosition) - ETarget.BoundingRadius;

                    if (minRange < distance && distance < E.Range)
                        E.Cast(ETarget.ServerPosition);
                }
            }

            public static void CastR()
            {
                if (!R.IsReady())
                    return;

                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget() && !e.IsZombie))
                {
                    UltimateOutPut result = enemy.GetResult();

                    if (result.Unnecessary)
                        continue;

                    if (result.Range == UltRange.RRange)
                    {
                        if (result.IsKillable)
                            R.Cast(enemy);

                        if (result.LanePhase && result.LetItGo)
                            R.Cast(enemy);
                    }
                }
            }

            public static void TowerE()
            {
                if (!Config.SpellMenu["towerE"].Cast<CheckBox>().CurrentValue || !E.IsReady())
                    return;

                if (Config.SpellMenu["saveRMana"].Cast<CheckBox>().CurrentValue && Player.Mana - Utility.QMana() <= Utility.RMana())
                    return;

                var tower = EntityManager.Turrets.Allies.OrderBy(t => t.Distance(Player)).First();

                if (tower == null)
                    return;

                if (tower.Distance(Player) < 750 && ETarget.IsValidTarget() && Player.HealthPercent > 30 && Player.CountEnemiesInRange(E.Range) <= 2)
                    CastE();
            }

            public static void AutoUlt()
            {
                if (!R.IsReady() || !Config.SpellMenu["autoR"].Cast<KeyBind>().CurrentValue)
                    return;

                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget() && !e.IsZombie))
                {
                    UltimateOutPut result = enemy.GetResult();

                    if (result.Unnecessary)
                        continue;

                    if (result.Range == UltRange.RRange)
                    {
                        if (result.IsKillable)
                            R.Cast(enemy);
                    }
                }
            }

            public static void AutoIgnite()
            {
                if (Ignite == null || !Ignite.IsReady() && !Config.SpellMenu["useIgnite"].Cast<CheckBox>().CurrentValue)
                    return;

                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(Ignite.Range) && !e.IsZombie))
                {
                    if (enemy.GetResult().IsKillable && enemy.GetResult().Range == UltRange.RRange && R.IsReady())
                        continue;

                    if (enemy.ServerPosition.Distance(Player.ServerPosition) < Player.AttackRange && Player.HealthPercent < 30)
                        continue;

                    if (Config.SpellMenu["1tick"].Cast<CheckBox>().CurrentValue)
                        if (DamageManager.IgniteDamage(1) > enemy.Health + enemy.AllShield && enemy.IsValidTarget(Ignite.Range) &&
                            enemy.PassiveDamage() / 2 < enemy.Health + enemy.AllShield + enemy.HPRegenRate * enemy.BuffRemainTime("dariushemo"))
                            Ignite.Cast(enemy);

                    int tick = Config.SpellMenu["igniteTick"].Cast<Slider>().CurrentValue;

                    if (DamageManager.IgniteDamage(tick) + enemy.PassiveDamage(tick) > enemy.Health + enemy.AllShield + enemy.HPRegenRate(tick, true) * 0.6f)
                        Ignite.Cast(enemy);
                }
            }

            public static void LaneClear()
            {
                if (Config.OrbMenu["laneMana"].Cast<Slider>().CurrentValue > Player.ManaPercent)
                    return;
                
                if (Config.OrbMenu["useQlaneclear"].Cast<CheckBox>().CurrentValue)
                {
                    if (Config.SpellMenu["saveRMana"].Cast<CheckBox>().CurrentValue && Player.Mana - Utility.QMana() <= Utility.RMana())
                        return;

                    var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.IsValidTarget(Q.Range));

                    if (minion == null || !minion.Any(t => t.IsValidTarget(Q.Range)) || PlayerIsAttacking)
                        return;

                    if (Q.IsReady() && minion.Count(m => m.Distance(Player.ServerPosition) < Q.Range) >= 4)
                        Q.Cast();
                }
            }

            public static void JungleClear()
            {
                if (Config.OrbMenu["jungleMana"].Cast<Slider>().CurrentValue > Player.ManaPercent)
                    return;

                if (Config.OrbMenu["useQjungleclear"].Cast<CheckBox>().CurrentValue)
                {
                    if (Config.SpellMenu["saveRMana"].Cast<CheckBox>().CurrentValue && Player.Mana - Utility.QMana() <= Utility.RMana())
                        return;

                    var mob = EntityManager.MinionsAndMonsters.Monsters.Where(m => m.IsValidTarget(Q.Range));

                    if (mob == null || !mob.Any(t => t.IsValidTarget(Q.Range)) || PlayerIsAttacking)
                        return;

                    if (Config.OrbMenu["useWjungleclear"].Cast<CheckBox>().CurrentValue && W.IsReady())
                        return;

                    if (Q.IsReady() && mob.Count(m => m.Distance(Player.ServerPosition) < Q.Range) >= 1)
                        Q.Cast();
                }
            }
        }

        private static void Orbwalker_OnAttack(AttackableUnit target, EventArgs args)
        {
            PlayerIsAttacking = true;
            Core.DelayAction(() => PlayerIsAttacking = false, (int)Player.AttackCastDelay * 1000);
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!Config.SpellMenu["aaReset"].Cast<CheckBox>().CurrentValue || target == null || !W.IsReady())
                return;

            if (Config.SpellMenu["saveRMana"].Cast<CheckBox>().CurrentValue && Player.Mana - Utility.WMana() <= Utility.RMana())
                return;

            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo && !Config.OrbMenu["useWcombo"].Cast<CheckBox>().CurrentValue)
                return;

            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass && !Config.OrbMenu["useWharass"].Cast<CheckBox>().CurrentValue)
                return;

            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.JungleClear && !Config.OrbMenu["useWjungleclear"].Cast<CheckBox>().CurrentValue)
                return;

            var h = target as AIHeroClient;

            if (h.IsValidTarget() && W.IsReady())
            {
                W.Cast();
                Orbwalker.ResetAutoAttack();
                if (target != null)
                    EloBuddy.Player.IssueOrder(GameObjectOrder.AttackUnit, target);

                return;
            }


            if (Config.SpellMenu["towerW"].Cast<CheckBox>().CurrentValue && Config.OrbMenu["laneMana"].Cast<Slider>().CurrentValue < Player.ManaPercent)
            {
                var t = target as Obj_AI_Turret;

                if (t.IsValidTarget() && W.IsReady())
                {
                    W.Cast();
                    Orbwalker.ResetAutoAttack();
                    if (target != null)
                        EloBuddy.Player.IssueOrder(GameObjectOrder.AttackUnit, target);

                    return;
                }
            }
                
            if (Config.OrbMenu["jungleMana"].Cast<Slider>().CurrentValue < Player.ManaPercent)
            {
                var j = target as Obj_AI_Base;

                if (j.IsMonster && j.IsValidTarget() && W.IsReady())
                {
                    W.Cast();
                    Orbwalker.ResetAutoAttack();
                    if (target != null)
                        EloBuddy.Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                }
            }
        }

        private static void Interrupt_OnInterruptableTarget(AIHeroClient sender, Interrupt.InterruptableTargetEventArgs args)
        {
            if (!Config.SpellMenu["interruptE"].Cast<CheckBox>().CurrentValue || Player.IsDead || Player.IsRecalling() || !sender.IsEnemy)
                return;

            if (sender.IsValidTarget(E.Range) && E.IsReady())
                E.Cast(sender.ServerPosition);
        }

        private static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            if (!Config.SpellMenu["dashE"].Cast<CheckBox>().CurrentValue || Player.IsDead || Player.IsRecalling() || !sender.IsEnemy ||!E.IsReady())
                return;

            if (Config.SpellMenu["saveRMana"].Cast<CheckBox>().CurrentValue && Player.Mana - Utility.EMana() <= Utility.RMana())
                return;

            if (sender.IsValidTarget(E.Range))
                E.Cast(sender);
        }
    }
}
