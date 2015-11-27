using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = ChallengerBlitzcrank.Config.SpellSetting;

namespace ChallengerBlitzcrank.Mode
{
    public class Combo : ModeBase
    {
        private static bool SpellShield(AIHeroClient unit)
        {
            return unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield);
        }

        public override bool ShouldBeExecute()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var Qtarget = TargetSelector.GetTarget(Settings.Q.MaxrangeQ, DamageType.Magical);

            if (Settings.Q.ComboQ && Q.IsReady() &&
                Qtarget.IsValidTarget(Settings.Q.MaxrangeQ) &&
                Qtarget.Distance(Player.ServerPosition) > Settings.Q.MinrangeQ &&
                SpellShield(Qtarget))
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
                if (Rtarget.HasBuff("powerfistslow"))
                    R.Cast();
                if (Rtarget.HasBuff("rocketgrab2") && E.IsOnCooldown && Settings.E.ComboE)
                    R.Cast();
                if (Player.CountEnemiesInRange(R.Range) >= Settings.R.MinEnemyR)
                    R.Cast();
            }
        }
    }
}