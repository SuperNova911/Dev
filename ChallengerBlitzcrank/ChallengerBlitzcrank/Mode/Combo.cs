using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using SharpDX;
using Color = System.Drawing.Color;
using Settings = ChallengerBlitzcrank.Config.SpellSetting;
using ChallengerBlitzcrank;

namespace ChallengerBlitzcrank.Mode
{
    public class Combo : ModeBase
    {
        public override bool ShouldBeExecute()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var Qtarget = TargetSelector.GetTarget(Settings.Q.MaxrangeQ, DamageType.Magical);

            if (Settings.Q.ComboQ && Q.IsReady() &&
                Qtarget.IsValidTarget(Settings.Q.MaxrangeQ) &&
                Qtarget.Distance(Player.ServerPosition) > Settings.Q.MinrangeQ)
            {
                var predic = Q.GetPrediction(Qtarget);

                if (predic.HitChance > (HitChance)Settings.Q.HitchanceQ + 2)
                {
                    Q.Cast(predic.CastPosition);
                }
            }

            var Etarget = TargetSelector.GetTarget(Settings.Q.MaxrangeQ, DamageType.Physical);

            if (Settings.E.ComboE && E.IsReady() &&
                Etarget.IsValidTarget(Settings.Q.MaxrangeQ))
            {
                if (Etarget.HasBuff("rocketgrab2"))
                {
                    E.Cast();
                }
            }

            var Rtarget = TargetSelector.GetTarget(SpellManager.R.Range, DamageType.Magical);

            if (Settings.R.ComboR && R.IsReady() && Rtarget.IsValidTarget(R.Range))
            {
                if (Settings.E.ComboE)
                {
                    if (E.IsOnCooldown)
                        R.Cast();
                }
                else
                {
                    if (Rtarget.HasBuffOfType(BuffType.Knockup))
                        R.Cast();
                }
            }
        }
    }
}