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
        static Geometry.Polygon.Circle DashCircle; //don't use new, just what I wrote

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
            Dash.OnDash += Dash_OnDash;
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

            if (DashCircle != null)
                DashCircle.Draw(Color.Yellow);
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

        private static void Orbwalker_OnAttack(AttackableUnit target, EventArgs args)
        {
            var t = target as AIHeroClient;
            if (SpellManager.E.IsReady() && Config.SpellSetting.E.ComboE && t.IsValidTarget())
            {
                SpellManager.E.Cast();
            }
        }

        private static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            DashCircle = new Geometry.Polygon.Circle(/*center*/ e.EndPos, /*radius*/ 70);
            Core.DelayAction(() => DashCircle = null, e.Duration); //I don't know if Duration is in miliseconds, you need to print it when doing a test

            if (!sender.IsEnemy || sender.HasBuff("powerfistslow"))
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
