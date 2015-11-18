using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = ChallengerJinx.Config.Modes.Harass;

namespace ChallengerJinx.Mode
{
    public class Harass : ModeBase
    {
        public override bool ShouldBeExecute()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.SelectedTarget != null
                      && TargetSelector.SelectedTarget.Distance(Player.Instance) < (!EventManager.FishBoneActive
                       ? Player.Instance.GetAutoAttackRange() + EventManager.FishBoneBonus
                       : Player.Instance.GetAutoAttackRange()) + 300
                       ? TargetSelector.SelectedTarget
                       : TargetSelector.GetTarget((!EventManager.FishBoneActive
                       ? Player.Instance.GetAutoAttackRange() + EventManager.FishBoneBonus
                       : Player.Instance.GetAutoAttackRange()) + 300, DamageType.Physical);

            var targetW = TargetSelector.SelectedTarget != null
                       && TargetSelector.SelectedTarget.Distance(Player.Instance) < SpellManager.W.Range
                        ? TargetSelector.SelectedTarget
                        : TargetSelector.GetTarget(SpellManager.W.Range, DamageType.Physical);

            Orbwalker.ForcedTarget = null;

            //Q Normal
            if (target != null && Config.Modes.Harass.UseQ && EventManager.FishBoneActive)
            {
                if (target.Distance(Player.Instance) <= Player.Instance.GetAutoAttackRange(target) - EventManager.FishBoneBonus)
                {
                    SpellManager.Q.Cast();
                }
            }
            else if (target != null && Config.Modes.Combo.UseQ)
            {
                if (target.Distance(Player.Instance) > Player.Instance.GetAutoAttackRange(target))
                {
                    SpellManager.Q.Cast();
                }
            }

            //Q AOE
            if (target != null && Config.Modes.Combo.UseQSplash)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(
                    e => e.IsValidTarget(EventManager.MinigunRange(e) + EventManager.FishBoneBonus))
                    .OrderBy(TargetSelector.GetPriority).Where(enemy => enemy.CountEnemiesInRange(150) > 1
                    && (enemy.NetworkId == target.NetworkId || enemy.Distance(target) < 150)))
                {
                    if (!EventManager.FishBoneActive)
                    {
                        SpellManager.Q.Cast();
                    }
                    Orbwalker.ForcedTarget = enemy;

                    return;
                }
            }

            //W Normal
            if (targetW != null
                && Config.Modes.Combo.UseW
                && targetW.Distance(Player.Instance) > Config.Misc.MinWRange
                && targetW.IsValidTarget(SpellManager.W.Range))
            {
                SpellManager.W.Cast(targetW);
            }
        }
    }
}
