using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
namespace ChallengerJinx
{
    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 1450, SkillShotType.Linear,  3300 60)
        }
    }
}
