using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using Color = System.Drawing.Color;
using Settings = ChallengerJinx.Config.Modes.Combo;

namespace ChallengerJinx.Mode
{
    public class Combo : ModeBase
    {
        public override bool ShouldBeExecute()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {

            var targetW = TargetSelector.SelectedTarget != null &&
                         TargetSelector.SelectedTarget.Distance(Player.Instance) < SpellManager.W.Range
                ? TargetSelector.SelectedTarget
                : TargetSelector.GetTarget(SpellManager.W.Range, DamageType.Physical);


            var target = TargetSelector.SelectedTarget != null &&
                         TargetSelector.SelectedTarget.Distance(Player.Instance) < (!EventManager.FishBoneActive ? Player.Instance.GetAutoAttackRange() + EventManager.FishBoneBonus : Player.Instance.GetAutoAttackRange()) + 300
                ? TargetSelector.SelectedTarget
                : TargetSelector.GetTarget((!EventManager.FishBoneActive ? Player.Instance.GetAutoAttackRange() + EventManager.FishBoneBonus : Player.Instance.GetAutoAttackRange()) + 300, DamageType.Physical);
            var rtarget = TargetSelector.GetTarget(3000, DamageType.Physical);

            Orbwalker.ForcedTarget = null;

            if (Orbwalker.IsAutoAttacking) return;

            // E on immobile

            if (target != null && Config.Modes.Combo.UseE && (target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Stun)))
            {
                SpellManager.E.Cast(target);
            }

            if (rtarget != null)
            {
                // W/R KS
                var wPred = SpellManager.W.GetPrediction(rtarget);

                if (Config.Modes.Combo.UseW &&
                    wPred.HitChance >= HitChance.Medium && SpellManager.W.IsReady() && rtarget.IsValidTarget(SpellManager.W.Range) &&
                    Damages.WDamage(target) >= rtarget.Health)
                {
                    SpellManager.W.Cast(rtarget);
                }
                else if (Config.Modes.Combo.UseR && rtarget != null &&
                         rtarget.Distance(Player.Instance) > EventManager.MinigunRange(target) + EventManager.FishBoneBonus &&
                         rtarget.IsKillableByR())
                {
                    SpellManager.R.Cast(rtarget);
                }
            }

            // W out of range
            if (targetW != null && Config.Modes.Combo.UseW && SpellManager.W.IsReady() && targetW.Distance(Player.Instance) > Player.Instance.GetAutoAttackRange(targetW) &&
                targetW.IsValidTarget(SpellManager.W.Range))
            {
                SpellManager.W.Cast(targetW);
            }

            if (target != null && Config.Modes.Combo.UseQSplash)
            {
                // Aoe Logic
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(
                    a => a.IsValidTarget(EventManager.MinigunRange(a) + EventManager.FishBoneBonus))
                    .OrderBy(TargetSelector.GetPriority).Where(enemy => enemy.CountEnemiesInRange(150) > 1 && (enemy.NetworkId == target.NetworkId || enemy.Distance(target) < 150)))
                {
                    if (!EventManager.FishBoneActive)
                    {
                        SpellManager.Q.Cast();
                    }
                    Orbwalker.ForcedTarget = enemy;
                    return;
                }
            }

            // Regular Q Logic
            if (target != null && Config.Modes.Combo.UseQ && EventManager.FishBoneActive)
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
        }
/*        {
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

            var targetE = TargetSelector.GetTarget(SpellManager.E.Range, DamageType.Magical);

            Orbwalker.ForcedTarget = null;

            //Q Normal
            if (target != null && Config.Modes.Combo.UseQ && EventManager.FishBoneActive)
            {
                if (target.Distance(Player.Instance) <= Player.Instance.GetAutoAttackRange(target) - EventManager.FishBoneBonus)
                {
                    SpellManager.Q.Cast();
                    Chat.Print("Combo Q off", Color.Red);
                }
            }
            else if (target != null && Config.Modes.Combo.UseQ)
            {
                if (target.Distance(Player.Instance) > Player.Instance.GetAutoAttackRange(target))
                {
                    SpellManager.Q.Cast();
                    Chat.Print("Combo Q on", Color.Green);
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

            //E on CC
            if (target != null
                && Config.Modes.Combo.UseE
                && SpellManager.E.IsReady()
                && target.IsValidTarget(SpellManager.E.Range))
            {
                if (target.HasBuffOfType(BuffType.Fear)
                    || target.HasBuffOfType(BuffType.Slow)
                    || target.HasBuffOfType(BuffType.Snare)
                    || target.HasBuffOfType(BuffType.Stun)
                    || target.HasBuffOfType(BuffType.Taunt)
                    || target.HasBuff("zhonyasringshield")
                    || target.HasBuff("Recall"))
                {
                    SpellManager.E.Cast(target);
                }
            }
        }*/
    }
}
