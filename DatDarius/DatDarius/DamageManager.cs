using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace DatDarius
{
    public static class DamageManager
    {
        public static double RDamage(this AIHeroClient target)
        {
            if (!SpellManager.R.IsLearned)
                return 0;

            return (new double[] { 100, 200, 300 }[SpellManager.R.Level - 1] + 0.75 * Player.Instance.FlatPhysicalDamageMod) * (1 + 0.2 * target.BuffCount("dariushemo"));
        }

        public static double RDamage(this AIHeroClient target, int stack)
        {
            if (!SpellManager.R.IsLearned)
                return 0;

            return (new double[] { 100, 200, 300 }[SpellManager.R.Level - 1] + 0.75 * Player.Instance.FlatPhysicalDamageMod) * (1 + 0.2 * stack);
        }

        public static int IgniteDamage(int second = 5)
        {
            return (10 + 4 * Player.Instance.Level) * second;
        }

        public static float PassiveDamage(this AIHeroClient target)
        {
            float damagePerSec = (9 + Player.Instance.Level + 0.3f * Player.Instance.FlatPhysicalDamageMod) / 5;

            return Damage.CalculateDamageOnUnit(Player.Instance, target, DamageType.Physical, damagePerSec * target.BuffCount("dariushemo") * target.BuffRemainTime("dariushemo"), false, false);
        }

        public static float PassiveDamage(this AIHeroClient target, int second)
        {
            float damagePerSec = (9 + Player.Instance.Level + 0.3f * Player.Instance.FlatPhysicalDamageMod) / 5;

            return Damage.CalculateDamageOnUnit(Player.Instance, target, DamageType.Physical, damagePerSec * target.BuffCount("dariushemo") * target.BuffRemainTime("dariushemo") > second ? second : target.BuffRemainTime("dariushemo"), false, false);
        }

        public static float PassiveDamage(this AIHeroClient target, int stack, int second)
        {
            float damage = ((9 + Player.Instance.Level + 0.3f * Player.Instance.FlatPhysicalDamageMod) / 5) * stack * second;

            return Damage.CalculateDamageOnUnit(Player.Instance, target, DamageType.Physical, damage, false, false);
        }
    }
}
