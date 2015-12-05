using AimBuddy.SDK;
using EloBuddy;
using System.Linq;

namespace AimBuddy
{
	public static class Utility
    {
		public static string[] HitchanceNameArray = { "Low", "Medium", "High", "Very High", "Only Immobile" };
		public static HitChance[] HitchanceArray = { HitChance.Low, HitChance.Medium, HitChance.High, HitChance.VeryHigh, HitChance.Immobile };

		public static bool IsImmobilizeBuff(BuffType type)
		{
			return type == BuffType.Snare || type == BuffType.Stun || type == BuffType.Charm || type == BuffType.Knockup || type == BuffType.Suppression;
		}

		public static bool IsImmobileTarget(AIHeroClient target)
		{
			return target.Buffs.Count(p => IsImmobilizeBuff(p.Type)) > 0 || target.IsChannelingImportantSpell();
		}

		public static bool IsActive(this Spell s)
		{
			return ObjectManager.Player.Spellbook.GetSpell(s.Slot).ToggleState == 2;
		}
	}
}
