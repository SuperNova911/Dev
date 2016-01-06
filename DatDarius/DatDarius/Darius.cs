using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System.Collections.Generic;

namespace DatDarius
{
    public class Darius
    {
        public static Spell.Active Q = new Spell.Active(SpellSlot.Q, 425);
        public static Spell.Active W = new Spell.Active(SpellSlot.W, 175);
        public static Spell.Skillshot E = new Spell.Skillshot(SpellSlot.E, 540, SkillShotType.Cone, 250, int.MaxValue, 225);
        public static Spell.Targeted R = new Spell.Targeted(SpellSlot.R, 460);
        public static Spell.Skillshot Flash;
        public static Spell.Targeted Ignite;
        public static float Angle = 50 * (float)Math.PI / 180;

        public static AIHeroClient Player { get { return ObjectManager.Player; } }
        public static AIHeroClient ETarget = null;

        public static Item Randuin;
        public static Item RavenousHydra;
        public static Item TitanicHydra;

        public static Menu Menu;
        public static DamageIndicator Indicator;

        /// <summary>
        /// Check enemy target has spell shield.
        /// </summary>
        /// <param name="unit">The target.</param>
        /// <returns>true = true, false = false.</returns>
        public static bool SpellShield(AIHeroClient unit)
        {
            return unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield);
        }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.BaseSkinName != "Darius")
                return;

            E.ConeAngleDegrees = 50;
            E.MinimumHitChance = HitChance.Medium;

            Randuin = new Item(3143, 500);
            RavenousHydra = new Item(3074, 400);
            TitanicHydra = new Item(3748, 385);

            SpellDataInst flash = Player.Spellbook.Spells.Where(s => s.Name.Contains("summonerflash")).Any()
                ? Player.Spellbook.Spells.Where(spell => spell.Name.Contains("summonerflash")).First() : null;
            SpellDataInst ignite = Player.Spellbook.Spells.Where(s => s.Name.Contains("summonerdot")).Any()
                ? Player.Spellbook.Spells.Where(spell => spell.Name.Contains("summonerdot")).First() : null;
            if (flash != null)
            {
                Flash = new Spell.Skillshot(flash.Slot, 425, SkillShotType.Linear);
            }
            if (ignite != null)
            {
                Ignite = new Spell.Targeted(ignite.Slot, 600);
            }

            Menu = MainMenu.AddMenu("Dat Darius", "Dat Darius");

            Menu.AddGroupLabel("Combo");
            {
                Menu.Add("useQcombo", new CheckBox("Use Q"));
                Menu.Add("useWcombo", new CheckBox("Use W"));
                Menu.Add("useEcombo", new CheckBox("Use E"));
                Menu.Add("useRcombo", new CheckBox("Use R"));
                Menu.Add("IgniteTime", new Slider("Ignite Tick", 3, 1, 5));
                Menu.AddSeparator();
            }

            Menu.AddGroupLabel("Harass");
            {
                Menu.AddSeparator();
            }

            Menu.AddGroupLabel("LastHit");
            {
                Menu.AddSeparator();
            }

            Menu.AddGroupLabel("LaneClear");
            {
                Menu.AddSeparator();
            }

            Menu.AddGroupLabel("Flee");
            {
                Menu.AddSeparator();
            }

            Menu.AddSubMenu("Misc");

            Menu.AddSubMenu("Draw");

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Dash.OnDash += Dash_OnDash;
            SDK.Interrupt.OnInterruptableTarget += Interrupt_OnInterruptableTarget;

            Debug.Initialize();
            Indicator = new DamageIndicator();

            Chat.Print("Dat Darius Loaded", Color.OrangeRed);
        }
        
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;
            /*
            new Circle
            {
                Color = Q.IsReady() ? Color.LawnGreen : Color.Red,
                BorderWidth = 2,
                Radius = Q.Range
            }.Draw(Player.Position);

            if (ETarget.IsValidTarget() && Player.VisibleOnScreen)
            {
                if (ETarget.ServerPosition.Distance(Player.ServerPosition) < E.Range)
                    new Geometry.Polygon.Sector(Player.Position, PositionPrediction(ETarget, 0.25f), Angle, E.Range).Draw(Color.Red);
                else
                    new Geometry.Polygon.Sector(Player.Position, PositionPrediction(ETarget, 0.25f), Angle, E.Range).Draw(Color.Orange);
            }
            else
                new Geometry.Polygon.Sector(Player.Position, Game.CursorPos, Angle, E.Range).Draw(Color.LawnGreen);
                */
            

            #region Debug
            // Debug zone
            if (false)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.VisibleOnScreen))
                {
                    var i = 0;
                    const int step = 20;

                    var Damage = new Dictionary<string, object>
                            {
                                { "Passive Stack", enemy.HasBuff("dariushemo") ? enemy.GetBuffCount("dariushemo") : 0 },
                                { "Passive Remain Time", enemy.HasBuff("dariushemo") ? enemy.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                                                        .Where(buff => buff.Name == "dariushemo")
                                                        .Select(buff => buff.EndTime)
                                                        .FirstOrDefault() - Game.Time
                                                        : 0 },
                                { "Passive Damage", DamageHandler.PassiveDamage(enemy) },
                                { "R Min Damage", new double[] { 100, 200, 300 }[SpellManager.R.Level - 1] + (0.75 * ObjectManager.Player.FlatPhysicalDamageMod) },
                                { "R Max Damage", DamageHandler.RDamage(enemy) },
                                { "Passive + Ignite", DamageHandler.PassiveDamage(enemy) + DamageHandler.IgniteDamage(true) },
                                { "Health", enemy.Health },
                                { "Hp Regen rate", enemy.HPRegenRate }
                            };
                    Drawing.DrawText(enemy.Position.WorldToScreen() + new Vector2(0, i), Color.Orange, "Damage properties", 10);
                    i += step;
                    foreach (var dataEntry in Damage)
                    {
                        Drawing.DrawText(enemy.Position.WorldToScreen() + new Vector2(0, i), Color.NavajoWhite, string.Format("{0}: {1}", dataEntry.Key, dataEntry.Value), 10);
                        i += step;
                    }

                    var Qlogic = new Dictionary<string, object>
                            {
                                { "Chasing    ", Player.IsFacing(enemy) && !enemy.IsFacing(Player) ? "true" : "false" },
                                { "Run away   ", !Player.IsFacing(enemy) && enemy.IsFacing(Player) ? "true" : "false" },
                                { "Both Facing", Player.IsBothFacing(enemy) ? "true" : "false" },
                                { "Distance ", Player.ServerPosition.Distance(enemy.ServerPosition) },
                                { "Speed", Player.MoveSpeed > enemy.MoveSpeed ? "Slow" : "Fast" }
                            };
                    Drawing.DrawText(enemy.Position.WorldToScreen() + new Vector2(0, i), Color.Orange, "Q Logic", 10);
                    i += step;
                    foreach (var dataEntry in Qlogic)
                    {
                        Drawing.DrawText(enemy.Position.WorldToScreen() + new Vector2(0, i), Color.NavajoWhite, string.Format("{0}: {1}", dataEntry.Key, dataEntry.Value), 10);
                        i += step;
                    }
                }

                if (!Player.IsDead)
                {
                    var i = 0;
                    const int step = 20;

                    var Damage = new Dictionary<string, object>
                            {
                                { "Path Length", Player.Path.Length }
								//{ "Path Long Length", Player.Path.LongLength }
							};
                    Drawing.DrawText(Player.Position.WorldToScreen() + new Vector2(0, i), Color.Orange, "General properties", 10);
                    i += step;
                    foreach (var dataEntry in Damage)
                    {
                        Drawing.DrawText(Player.Position.WorldToScreen() + new Vector2(0, i), Color.NavajoWhite, string.Format("{0}: {1}", dataEntry.Key, dataEntry.Value), 10);
                        i += step;
                    }
                }
                for (var i = 1; Player.Path.Length > i; i++)
                {
                    Drawing.DrawLine(Drawing.WorldToScreen(Player.Path[i - 1]), Drawing.WorldToScreen(Player.Path[i]), 5, Color.Green);
                }
            }
            #endregion
        }

        public static Vector3 DirectionVector(AIHeroClient unit)
        {
            Vector3 Kappa;

            Kappa.X = (unit.Direction.X * (float)Math.Cos(90 * Math.PI / 180)) - (unit.Direction.Y * (float)Math.Sin(90 * Math.PI / 180));
            Kappa.Y = (unit.Direction.X * (float)Math.Sin(90 * Math.PI / 180)) - (unit.Direction.Y * (float)Math.Cos(90 * Math.PI / 180));
            Kappa.Z = 0;

            return Kappa;
        }

        public static Vector3 UnitVector(Vector3 vec3)
        {
            Vector3 Kappa;

            var length = Math.Sqrt(Math.Pow(vec3.X, 2) + Math.Pow(vec3.Y, 2) + Math.Pow(vec3.Z, 2));
            Kappa.X = vec3.X / (float)length;
            Kappa.Y = vec3.Y / (float)length;
            Kappa.Z = vec3.Z / (float)length;

            return Kappa;
        }

        public static Vector3 PositionPrediction(AIHeroClient unit, float sec)
        {
            Vector3 Kappa;

            Kappa = unit.Position + (UnitVector(DirectionVector(unit)) * (unit.MoveSpeed * sec));

            return Kappa;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.IsDead || MenuGUI.IsChatOpen)
                return;
            
            AutoUlt();
            KillSecure();
            TowerE();

            ETarget = TargetSelector.GetTarget(E.Range + 150, DamageType.Physical);

            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Mode.Combo();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    Mode.Harass();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                    Mode.LastHit();
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    Mode.LaneClear();
                    break;
                case Orbwalker.ActiveModes.Flee:
                    Mode.Flee();
                    break;
            }
        }

        public class Mode
        {
            public static void Combo()
            {
                var ComboQ = Menu["useQcombo"].Cast<CheckBox>().CurrentValue;
                var Qtarget = TargetSelector.GetTarget(1000, DamageType.Physical);

                if (Qtarget.IsValidTarget() && Q.IsReady() && ComboQ)
                {
                    CastQ(Qtarget);
                }

                var ComboE = Menu["useEcombo"].Cast<CheckBox>().CurrentValue;
                if (ETarget.IsValidTarget() && E.IsReady() && ComboE)
                {
                    CastE(ETarget);
                }

                // 1 vs 1
                if (Player.Position.CountEnemiesInRange(2000) == 1)
                {

                }
            }

            public static void Harass()
            {

            }

            public static void LastHit()
            {

            }

            public static void LaneClear()
            {

            }

            public static void Flee()
            {

            }
        }

        public static void AutoUlt()
        {
            foreach (var target in EntityManager.Heroes.Enemies.Where(target => target.Distance(Player.ServerPosition) < R.Range + Flash.Range))
            {
                if (DamageHandler.RDamage(target) > target.Health + target.AllShield + target.HPRegenRate &&
                    R.IsReady() && target.IsValidTarget(R.Range) && !SpellShield(target) && !target.HasBuff("kindredrnodeathbuff"))
                {
                    R.Cast(target);
                    Chat.Print(DamageHandler.RDamage(target));
                }
            }
        }

        public static void KillSecure()
        {
            if (Player.IsDead)
                return;

            if (Ignite.IsReady())
            {
                var IgniteTarget = EntityManager.Heroes.Enemies.FirstOrDefault(t => DamageHandler.IgniteDamage(true) >= t.Health);

                if (!IgniteTarget.IsValidTarget(Ignite.Range + Flash.Range))
                    return;

                // Damage Prediction
                var time = Menu["IgniteTime"].Cast<Slider>().CurrentValue;
                var heal = HealHandler.Potion.GetHeal(IgniteTarget, time);

                if (heal == 0)
                {
                    if (DamageHandler.IgniteDamage(false) * time >= IgniteTarget.Health
                        - DamageHandler.PassiveDamage(IgniteTarget)
                        + (IgniteTarget.HPRegenRate * time) / 2
                        + HealHandler.Potion.HealTick(IgniteTarget) * (time - 1))
                        Ignite.Cast(IgniteTarget);
                }
                else if (heal > 0)
                {
                    if (DamageHandler.IgniteDamage(false) * time >= IgniteTarget.Health
                        - DamageHandler.PassiveDamage(IgniteTarget)
                        + (IgniteTarget.HPRegenRate * time) / 2
                        + heal)
                        Ignite.Cast(IgniteTarget);
                }
            }
        }

        /// <summary>
        /// Q logic.
        /// </summary>
        public static void CastQ(AIHeroClient Qtarget)
        {
            var MySpeed = Player.MoveSpeed;
            var TargetSpeed = Qtarget.MoveSpeed;
            var TargetBoundingRadius = Qtarget.BoundingRadius;
            var Distance = PositionPrediction(Qtarget, 0.75f).Distance(PositionPrediction(Player, 0.75f)) + 100;
            //var Distance = Qtarget.ServerPosition.Distance(Player.ServerPosition);

            if (Qtarget.IsMoving)
            {
                if (Distance < Q.Range && Distance > Q.Range - 220)
                    Q.Cast();
            }
            else
            {
                if (PositionPrediction(Player, 0.75f).Distance(Qtarget.ServerPosition) + 200 < Q.Range &&
                    PositionPrediction(Player, 0.75f).Distance(Qtarget.ServerPosition) > Q.Range - 220)
                    Q.Cast();
            }

            /*
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
        }

        /// <summary>
        /// E Logic.
        /// </summary>
        /// <param name="target"></param>
        public static void CastE(AIHeroClient target)
        {
            if (!Player.IsFacing(target) && target.IsFacing(Player))
                return;

            var Distance = PositionPrediction(target, 0.25f).Distance(Player.ServerPosition);

            if (target.IsMoving)
            {
                if (target.MoveSpeed < Player.MoveSpeed)
                {
                    if (Distance < E.Range && Distance > 450)
                        E.Cast(PositionPrediction(target, 0.25f));
                }
                else
                {
                    if (Distance < E.Range && Distance > 450)
                        E.Cast(PositionPrediction(target, 0.25f));
                }
            }
            else
            {
                if (target.ServerPosition.Distance(Player.ServerPosition) < E.Range &&
                    target.ServerPosition.Distance(Player.ServerPosition) > 450)
                    E.Cast(target.ServerPosition);
            }
        }

        public static void TowerE()
        {
            if (E.IsReady() && ETarget.IsValidTarget())
            {
                Obj_AI_Turret Turret = EntityManager.Turrets.Allies.FirstOrDefault(t => t.Distance(Player.ServerPosition) < 1000);

                if (Turret.IsValidTarget() && !Turret.IsDead && Player.ServerPosition.Distance(Turret) < Turret.AttackRange - 50)
                {
                    if (Player.Health > ETarget.Health)
                    {
                        if (E.GetPrediction(ETarget).HitChance >= HitChance.High)
                            E.Cast(ETarget);
                    }
                    else if (Player.HealthPercent > 40 && E.GetPrediction(ETarget).HitChance >= HitChance.High)
                        E.Cast(ETarget);
                }
            }
        }
        
        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            var t = target as AIHeroClient;

            if (t.IsValidTarget() && W.IsReady())
            {
                W.Cast();
                Orbwalker.ResetAutoAttack();
                if (target != null)
                    EloBuddy.Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
        }

        private static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            if (Player.IsDead || Player.IsRecalling() || MenuGUI.IsChatOpen || !sender.IsEnemy || !sender.IsValidTarget())
                return;

            if (e.EndPos.Distance(Player.ServerPosition) < Q.Range + sender.BoundingRadius &&
                e.EndPos.Distance(Player.ServerPosition) > Q.Range + sender.BoundingRadius - 220 && Q.IsReady())
            {
                if (e.Duration < 750)
                    Q.Cast();
                else
                    Core.DelayAction(() => Q.Cast(), e.Duration - 750);
            }

            if (e.StartPos.Distance(Player.ServerPosition) < E.Range && 
                e.EndPos.Distance(Player.ServerPosition) > E.Range && E.IsReady())
            {
                E.Cast(sender);
                Chat.Print("Dash E");
            }

        }

        private static void Interrupt_OnInterruptableTarget(AIHeroClient sender, SDK.Interrupt.InterruptableTargetEventArgs args)
        {
            if (Player.IsDead || Player.IsRecalling() || MenuGUI.IsChatOpen)
                return;

            if (sender.IsValidTarget(E.Range + sender.BoundingRadius) && E.IsReady())
            {
                E.Cast(sender);
            }
        }
    }
}
