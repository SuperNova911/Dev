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
        private static Spell.Active Q = SpellManager.Q;
        private static Spell.Active W = SpellManager.W;
        private static Spell.Skillshot E = SpellManager.E;
        private static Spell.Targeted R = SpellManager.R;
        private static Spell.Skillshot Flash = SpellManager.Flash;
        private static Spell.Targeted Ignite = SpellManager.Ignite;

        private static AIHeroClient Player = ObjectManager.Player;
        public static AIHeroClient ETarget = null;

        public static Item Randuin;
        public static Item RavenousHydra;
        public static Item TitanicHydra;

        public static DamageIndicator Indicator;

        public static bool Attacking = false;

        
        public static int SaveRMana()
        {
            if (Player.Level < 5)
                return 0;
            
            if (Config.SpellMenu["saveRMana"].Cast<CheckBox>().CurrentValue)
                return R.Level == 3 ? 0 :100;

            return 0;
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

            Config.Initialize();
            Debug.Initialize();

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnAttack += Orbwalker_OnAttack;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Dash.OnDash += Dash_OnDash;
            SDK.Interrupt.OnInterruptableTarget += Interrupt_OnInterruptableTarget;

            //Indicator = new DamageIndicator();

            Chat.Print("Dat Darius Loaded", Color.OrangeRed);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;

            if (Config.DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue && Player.VisibleOnScreen)
                new Circle
                {
                    Color = Q.IsReady() ? Color.LawnGreen : Color.Red,
                    BorderWidth = 2,
                    Radius = Q.Range
                }.Draw(Player.Position);

            if (Config.DrawMenu["drawE"].Cast<CheckBox>().CurrentValue && Player.VisibleOnScreen)
            {
                if (ETarget.IsValidTarget())
                    new Geometry.Polygon.Sector(Player.Position, Utility.PositionPrediction(ETarget, 0.25f), SpellManager.eAngle, E.Range)
                        .Draw(ETarget.ServerPosition.Distance(Player.ServerPosition) < E.Range ? Color.Red : Color.Orange);
                else
                    new Geometry.Polygon.Sector(Player.Position, Game.CursorPos, SpellManager.eAngle, E.Range).Draw(Color.LawnGreen);
            }

            if (Config.DrawMenu["drawR"].Cast<CheckBox>().CurrentValue && Player.VisibleOnScreen)
                new Circle
                {
                    Color = R.IsReady() ? Color.LawnGreen : Color.Red,
                    BorderWidth = 2,
                    Radius = R.Range
                }.Draw(Player.Position);

            if (Config.DrawMenu["drawFlashE"].Cast<CheckBox>().CurrentValue && Player.VisibleOnScreen && E.IsReady() && Flash.IsReady())
                new Circle
                {
                    Color = Color.Orange,
                    BorderWidth = 2,
                    Radius = E.Range + Flash.Range
                }.Draw(Player.Position);

            if (Config.DrawMenu["drawFlashR"].Cast<CheckBox>().CurrentValue && Player.VisibleOnScreen && R.IsReady() && Flash.IsReady())
                new Circle
                {
                    Color = Color.Red,
                    BorderWidth = 2,
                    Radius = R.Range + Flash.Range
                }.Draw(Player.Position);

            #region Debug
            if (false)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.VisibleOnScreen))
                {
                    var i = 0;
                    const int step = 20;

                    var Damage = new Dictionary<string, object>
                            {
                                { "Passive Stack", enemy.BuffCount("dariushemo") },
                                { "Passive Remain Time", enemy.BuffRemainTime("dariushemo") },
                                { "Passive Damage", enemy.PassiveDamage() },
                                { "R Min Damage", enemy.RDamage(1) },
                                { "R Max Damage", enemy.RDamage(5) },
                                { "Passive + Ignite", enemy.PassiveDamage() + DamageManager.IgniteDamage() },
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

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.IsDead || MenuGUI.IsChatOpen)
                return;

            AutoUlt();
            KillSecure();
            TowerE();


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
                var ComboQ = Config.Menu["useQcombo"].Cast<CheckBox>().CurrentValue;
                var Qtarget = TargetSelector.GetTarget(1000, DamageType.Physical);

                if (Qtarget.IsValidTarget() && Q.IsReady() && ComboQ)
                {
                    CastQ(Qtarget);
                }

                var ComboE = Config.Menu["useEcombo"].Cast<CheckBox>().CurrentValue;
                ETarget = TargetSelector.GetTarget(E.Range + 150, DamageType.Physical);
                if (ETarget.IsValidTarget() && E.IsReady() && ComboE)
                {
                    Chat.Print("ComboE");
                    CastE(ETarget);
                }


                Chat.Print(ETarget.RDamage());
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
            if (!R.IsReady())
                return;

            foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(R.Range)))
            {
                UltLogic(enemy);
            }
        }

        public static void KillSecure()
        {
            if (Player.IsDead)
                return;

            if (Ignite.IsReady())
            {
                var IgniteTarget = EntityManager.Heroes.Enemies.FirstOrDefault(t => DamageManager.IgniteDamage() >= t.Health);

                if (!IgniteTarget.IsValidTarget(Ignite.Range + Flash.Range))
                    return;

                // Damage Prediction
                var time = Config.SpellMenu["igniteTick"].Cast<Slider>().CurrentValue;
                var heal = IgniteTarget.GetHeal(time);

                if (heal == 0)
                {
                    if (DamageManager.IgniteDamage(1) * time >= IgniteTarget.Health
                        - IgniteTarget.PassiveDamage()
                        + (IgniteTarget.HPRegenRate * time) / 2
                        + IgniteTarget.HealTick(time - 1))
                        Ignite.Cast(IgniteTarget);
                }
                else if (heal > 0)
                {
                    if (DamageManager.IgniteDamage(1) * time >= IgniteTarget.Health
                        - IgniteTarget.PassiveDamage()
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
        
        public static void CastE(AIHeroClient target)
        {
            /*
            if (!Player.IsFacing(target) && target.IsFacing(Player))
                return;
                */

            var distance1 = Utility.PositionPrediction(target, 0.25f).Distance(Player.ServerPosition);
            var distance2 = target.ServerPosition.Distance(Player.ServerPosition);

            if (target.IsMoving && 450 < distance1 && distance1 < E.Range)
                E.Cast(Utility.PositionPrediction(target, 0.25f));
            else if (450 < distance2 && distance2 < E.Range)
                E.Cast(target.ServerPosition);
        }

        public static void UltLogic(AIHeroClient target)
        {
            UltimateOutPut result = target.GetResult();

            if (result.Unnecessary)
                return;

            if (result.IsKillable)
            {
                if (result.IsInRange)
                    R.Cast(target);

                if (result.IsInFlashRange && Config.SpellMenu["flashR"].Cast<KeyBind>().CurrentValue)
                {
                    Flash.Cast(target.ServerPosition);
                    R.Cast(target);
                }
            }

            if (result.LetItGo && result.LanePhase && !Attacking && false)
            {
                R.Cast(target);
            }
        }

        public static void TowerE()
        {
            if (!Config.SpellMenu["towerE"].Cast<CheckBox>().CurrentValue)
                return;

            ETarget = TargetSelector.GetTarget(E.Range + 150, DamageType.Physical);

            if (E.IsReady() && ETarget.IsValidTarget())
            {
                Obj_AI_Turret Turret = EntityManager.Turrets.Allies.FirstOrDefault(t => t.Distance(Player.ServerPosition) < 1000);

                if (!Turret.IsDead && Player.ServerPosition.Distance(Turret) < Turret.AttackRange - 50)
                {
                    if (Player.Health > ETarget.Health)
                        CastE(ETarget);
                    else if (Player.HealthPercent > 40)
                        CastE(ETarget);
                }
            }
        }

        private static void Orbwalker_OnAttack(AttackableUnit target, EventArgs args)
        {
            Attacking = true;
            var delay = Player.AttackCastDelay * 1000;
            Core.DelayAction(() => Attacking = false, (int)delay);
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!Config.SpellMenu["aaReset"].Cast<CheckBox>().CurrentValue)
                return;

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
            if (!Config.SpellMenu["dashE"].Cast<CheckBox>().CurrentValue)
                return;

            if (Player.IsDead || Player.IsRecalling() || MenuGUI.IsChatOpen || !sender.IsEnemy || !sender.IsValidTarget() || sender.IsZombie)
                return;

            /*
            if (e.EndPos.Distance(Player.ServerPosition) < Q.Range + sender.BoundingRadius &&
                e.EndPos.Distance(Player.ServerPosition) > Q.Range + sender.BoundingRadius - 220 && Q.IsReady())
            {
                if (e.Duration < 750)
                    Q.Cast();
                else
                    Core.DelayAction(() => Q.Cast(), e.Duration - 750);
            }
            */

            if (e.StartPos.Distance(Player.ServerPosition) < E.Range &&
                e.EndPos.Distance(Player.ServerPosition) > E.Range && E.IsReady())
                E.Cast(sender);

        }

        private static void Interrupt_OnInterruptableTarget(AIHeroClient sender, SDK.Interrupt.InterruptableTargetEventArgs args)
        {
            if (!Config.SpellMenu["interruptE"].Cast<CheckBox>().CurrentValue)
                return;

            if (Player.IsDead || Player.IsRecalling() || MenuGUI.IsChatOpen || !sender.IsEnemy || !sender.IsValidTarget() || sender.IsZombie)
                return;

            if (sender.IsValidTarget(E.Range) && E.IsReady())
                E.Cast(sender.ServerPosition);
        }
    }
}
