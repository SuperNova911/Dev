using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Enumerations;

namespace ChallengerBlitzcrank
{
    public class Blitzcrank
    {
        static Geometry.Polygon.Circle DashCircle; //don't use new, just what I wrote

        private static bool SpellShield(AIHeroClient unit)
        {
            return unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield);
        }
        private static bool SpellShield(Obj_AI_Base unit)
        {
            return unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield);
        }

        private static bool CC(AIHeroClient unit)
        {
            return unit.HasBuffOfType(BuffType.Charm) || unit.HasBuffOfType(BuffType.Fear)
                || unit.HasBuffOfType(BuffType.Snare) || unit.HasBuffOfType(BuffType.Slow)
                || unit.HasBuffOfType(BuffType.Taunt) || unit.HasBuffOfType(BuffType.Stun)
                || unit.HasBuffOfType(BuffType.Knockup) || unit.HasBuffOfType(BuffType.Suppression);
        }

        static Blitzcrank()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        static void Main(string[] args)
        {
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Blitzcrank")
                return;

            Config.Initialize();
            ModeManager.Initialize();

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            Orbwalker.OnAttack += Orbwalker_OnAttack;
            Dash.OnDash += Dash_OnDash;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead || Player.Instance.IsRecalling())
                return;

            KillSteal();
            AutoGrab();
            Immobile();
        }

        private static void KillSteal()
        {
            if (Config.SpellSetting.Q.KillstealQ && SpellManager.Q.IsReady())
            {
                var Qtarget = ObjectManager.Get<AIHeroClient>().FirstOrDefault(e => e.IsEnemy);

                if (Qtarget.IsValidTarget(Config.SpellSetting.Q.MaxrangeQ) &&
                    Player.Instance.GetSpellDamage(Qtarget, SpellSlot.Q) > Qtarget.Health &&
                    !SpellShield(Qtarget))
                {
                    var predic = SpellManager.Q.GetPrediction(Qtarget);
                    SpellManager.Q.Cast(predic.CastPosition);
                }
            }

            if (Config.SpellSetting.R.KillstealR && SpellManager.R.IsReady())
            {
                var Rtarget = ObjectManager.Get<AIHeroClient>().FirstOrDefault(e => e.IsEnemy);

                if (Rtarget.IsValidTarget(SpellManager.R.Range) &&
                    Player.Instance.GetSpellDamage(Rtarget, SpellSlot.R) > Rtarget.Health &&
                    !SpellShield(Rtarget))
                {
                    SpellManager.R.Cast();
                }
            }
        }

        private static void AutoGrab()
        {

        }

        private static void Immobile()
        {
            var Qtarget = TargetSelector.GetTarget(Config.SpellSetting.Q.MaxrangeQ, DamageType.Magical);

            if (Qtarget == null || SpellShield(Qtarget) || Config.SpellSetting.Q.MinHealthQ > Player.Instance.HealthPercent)
                return;

            if (Config.SpellSetting.Q.ImmobileQ && SpellManager.Q.IsReady() &&
                Qtarget.IsValidTarget(Config.SpellSetting.Q.MaxrangeQ) &&
                Qtarget.Distance(Player.Instance.ServerPosition) > Config.SpellSetting.Q.MinrangeQ &&
                CC(Qtarget))
            {
                SpellManager.Q.Cast(Qtarget);
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.Instance.IsDead || Player.Instance.IsRecalling())
                return;

            if (Config.Drawing.DrawQ)
            {
                new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = Config.SpellSetting.Q.MaxrangeQ }.Draw(Player.Instance.Position);
            }
            if (Config.Drawing.DrawR)
            {
                new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = SpellManager.R.Range }.Draw(Player.Instance.Position);
            }

            if (DashCircle != null)
                DashCircle.Draw(Color.Yellow);
        }

        private static void Interrupter2_OnInterruptableTarget(AIHeroClient sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (Player.Instance.IsDead || Player.Instance.IsRecalling())
                return;

            if (!sender.IsEnemy || SpellShield(sender) || Config.SpellSetting.Q.MinHealthQ > Player.Instance.HealthPercent)
                return;

            if (Config.SpellSetting.Q.InterruptQ && SpellManager.Q.IsReady() &&
                sender.IsValidTarget(Config.SpellSetting.Q.MaxrangeQ))
            {
                var predic = SpellManager.Q.GetPrediction(sender);

                if (predic.HitChance >= HitChance.Medium)
                {
                    SpellManager.Q.Cast(predic.CastPosition);
                }
            }

            if (Config.SpellSetting.R.InterruptR && SpellManager.R.IsReady() &&
                sender.IsValidTarget(SpellManager.R.Range) &&
                !sender.HasBuffOfType(BuffType.SpellImmunity) && !sender.HasBuffOfType(BuffType.SpellShield))
            {
                SpellManager.R.Cast();
            }
        }

        private static void Orbwalker_OnAttack(AttackableUnit target, EventArgs args)
        {
            var t = target as AIHeroClient;

            if (SpellShield(t))
                return;

            if (SpellManager.E.IsReady() && Config.SpellSetting.E.ComboE && t.IsValidTarget())
            {
                SpellManager.E.Cast();
            }
        }

        private static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            if (Player.Instance.IsDead || Player.Instance.IsRecalling())
                return;
            DashCircle = new Geometry.Polygon.Circle(/*center*/ e.EndPos, /*radius*/ 70);
            Core.DelayAction(() => DashCircle = null, e.Duration); //I don't know if Duration is in miliseconds, you need to print it when doing a test

            if (!sender.IsEnemy || sender.HasBuff("powerfistslow") || SpellShield(sender) || Config.SpellSetting.Q.MinHealthQ > Player.Instance.HealthPercent)
                return;

            if (e.EndPos.Distance(Player.Instance.Position) < Config.SpellSetting.Q.MinrangeQ)
            {
                Chat.Print("Too Close");
            }
            else if (e.EndPos.Distance(Player.Instance.Position) > Config.SpellSetting.Q.MaxrangeQ)
            {
                Chat.Print("Too Far");
            }
            else if (e.EndPos.Distance(Player.Instance.Position) > Config.SpellSetting.Q.MinrangeQ &&
                     e.EndPos.Distance(Player.Instance.Position) < Config.SpellSetting.Q.MaxrangeQ)
            {
                Chat.Print("9ood");
                SpellManager.Q.Cast(e.EndPos);
            }
        }
    }
}
