using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace DatDarius
{
	public static class SpellManager
	{
		public static Spell.Active Q { get; private set; }
		public static Spell.Active W { get; private set; }
		public static Spell.Skillshot E { get; private set; }
		public static Spell.Targeted R { get; private set; }

		public static List<Spell.SpellBase> AllSpells { get; private set; }

		static SpellManager()
		{
			Q = new Spell.Active(SpellSlot.Q, 425);
			W = new Spell.Active(SpellSlot.W, 200);
			E = new Spell.Skillshot(SpellSlot.E, 550, SkillShotType.Cone, 250, int.MaxValue, 225);
			R = new Spell.Targeted(SpellSlot.R, 460);
		}
	}
}