using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using ChallengerBlitzcrank;
using ChallengerBlitzcrank.Mode;
using EloBuddy.SDK.Enumerations;

namespace ChallengerBlitzcrank
{
    public class Blitzcrank
    {
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

            Drawing.OnDraw += Drawing_OnDraw;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            Orbwalker.OnAttack += Orbwalker_OnAttack;
        }

        private static void Interrupter2_OnInterruptableTarget(AIHeroClient sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!sender.IsEnemy)
                return;

            if (Config.SpellSetting.Q.InterruptQ && SpellManager.Q.IsReady() && sender.IsValidTarget(Config.SpellSetting.Q.MaxrangeQ))
            {
                var predic = SpellManager.Q.GetPrediction(sender);

                if (predic.HitChance >= HitChance.Medium)
                {
                    SpellManager.Q.Cast(predic.CastPosition);
                }
            }

            if (Config.SpellSetting.R.InterruptR && SpellManager.R.IsReady() && sender.IsValidTarget(SpellManager.R.Range))
            {
                SpellManager.R.Cast();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Config.Drawing.DrawQ)
            {
                new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = Config.SpellSetting.Q.MaxrangeQ }.Draw(Player.Instance.Position);
            }
            if (Config.Drawing.DrawR)
            {
                new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = SpellManager.R.Range }.Draw(Player.Instance.Position);
            }
        }

        private static void Orbwalker_OnAttack(AttackableUnit target, EventArgs args)
        {
            var t = target as AIHeroClient;
            if (SpellManager.E.IsReady() && Config.SpellSetting.E.ComboE && t.IsValidTarget())
            {
                SpellManager.E.Cast();
            }
        }
    }
}
