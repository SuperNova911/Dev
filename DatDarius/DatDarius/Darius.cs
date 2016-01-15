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
        private static Item Randuin;
        private static Item RavenousHydra;
        private static Item TitanicHydra;

        private static AIHeroClient Player = ObjectManager.Player;
        private static AIHeroClient Qtarget = null;
        private static AIHeroClient ETarget = null;

        private static DamageIndicator Indicator;

        public static bool PlayerIsAttacking = false;

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
            Orbwalker.OnAttack += Orbwalker_OnAttack;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Interrupt.OnInterruptableTarget += Interrupt_OnInterruptableTarget;
            Dash.OnDash += Dash_OnDash;

            Chat.Print("Dat Darius Loaded", Color.LawnGreen);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
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
                if (ETarget.IsValid)
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
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.IsDead)
                return;

            Qtarget = TargetSelector.GetTarget(Q.Range + 150, DamageType.Physical);
            ETarget = TargetSelector.GetTarget(E.Range + 150, DamageType.Physical);

            Logic.AutoIgnite();
            Logic.AutoUlt();

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
                case Orbwalker.ActiveModes.JungleClear:
                    Mode.JungleClear();
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
                if (Config.Menu["useQcombo"].Cast<CheckBox>().CurrentValue)
                {

                }

                if (Config.Menu["useWcombo"].Cast<CheckBox>().CurrentValue)
                {

                }

                if (Config.Menu["useEcombo"].Cast<CheckBox>().CurrentValue)
                {
                    Logic.CastE();
                }

                if (Config.Menu["useRcombo"].Cast<CheckBox>().CurrentValue)
                {
                    Logic.CastR();
                }
            }

            public static void Harass()
            {
                if (Config.Menu["useQharass"].Cast<CheckBox>().CurrentValue)
                {

                }

                if (Config.Menu["useWharass"].Cast<CheckBox>().CurrentValue)
                {

                }

                if (Config.Menu["useEharass"].Cast<CheckBox>().CurrentValue)
                {
                    Logic.CastE();
                }

                if (Config.Menu["useRharass"].Cast<CheckBox>().CurrentValue)
                {
                    Logic.CastR();
                }
            }

            public static void LastHit()
            {

            }

            public static void LaneClear()
            {

            }

            public static void JungleClear()
            {

            }

            public static void Flee()
            {

            }
        }

        public class Logic
        {
            public static void CastQ()
            {
                if (!Q.IsReady() || !Qtarget.IsValidTarget())
                    return;

                if (Config.Menu["saveRMana"].Cast<CheckBox>().CurrentValue && Player.Mana - SpellManager.QMana() <= SpellManager.RMana())
                    return;

                if (PlayerIsAttacking)
                    return;

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
                if (!E.IsReady() || !ETarget.IsValidTarget())
                    return;

                if (Config.Menu["saveRMana"].Cast<CheckBox>().CurrentValue && Player.Mana - SpellManager.QMana() <= SpellManager.RMana())
                    return;

                if (ETarget.IsMoving)
                {
                    var distance = Utility.PositionPrediction(ETarget, 0.25f).Distance(Player.ServerPosition);

                    if (450 < distance && distance < E.Range)
                        E.Cast(Utility.PositionPrediction(ETarget, 0.25f));
                }
                else
                {
                    var distance = ETarget.ServerPosition.Distance(Player.ServerPosition);

                    if (450 < distance && distance < E.Range)
                        E.Cast(ETarget.ServerPosition);
                }
            }

            public static void CastR()
            {
                if (!R.IsReady())
                    return;

                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget()))
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

            public static void AutoUlt()
            {
                if (!R.IsReady())
                    return;

                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget()))
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
                if (Ignite == null || !Ignite.IsReady())
                    return;

                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget()))
                {
                    if (Config.Menu["1tick"].Cast<CheckBox>().CurrentValue)
                        if (DamageManager.IgniteDamage(1) > enemy.Health + enemy.AllShield && enemy.IsValidTarget(Ignite.Range) &&
                            enemy.PassiveDamage() / 2 < enemy.Health + enemy.AllShield + enemy.HPRegenRate * enemy.BuffRemainTime("dariushemo"))
                            Ignite.Cast(enemy);

                    int tick = Config.Menu["igniteTick"].Cast<Slider>().CurrentValue;

                    if (DamageManager.IgniteDamage(tick) + enemy.PassiveDamage(tick) > enemy.Health + enemy.AllShield + enemy.HPRegenRate(tick, true) * 0.6f)
                        Ignite.Cast(enemy);
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
            if (!Config.SpellMenu["aaReset"].Cast<CheckBox>().CurrentValue || target == null)
                return;

            if (Config.Menu["saveRMana"].Cast<CheckBox>().CurrentValue && Player.Mana - SpellManager.WMana() <= SpellManager.RMana())
                return;

            if (target.IsValidTarget() && W.IsReady())
            {
                W.Cast();
                Orbwalker.ResetAutoAttack();
                EloBuddy.Player.IssueOrder(GameObjectOrder.AttackUnit, target);
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
            if (!Config.SpellMenu["dashE"].Cast<CheckBox>().CurrentValue || Player.IsDead || Player.IsRecalling() || !sender.IsEnemy)
                return;

            if (Config.Menu["saveRMana"].Cast<CheckBox>().CurrentValue && Player.Mana - SpellManager.EMana() <= SpellManager.RMana())
                return;

            if (sender.IsValidTarget() && E.IsReady())
                E.Cast(sender);
        }
    }
}
