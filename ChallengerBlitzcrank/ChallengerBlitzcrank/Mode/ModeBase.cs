using EloBuddy;
using EloBuddy.SDK;

namespace ChallengerBlitzcrank.Mode
{
    public abstract class ModeBase
    {
        public static AIHeroClient Player = EloBuddy.Player.Instance;

        public Spell.Skillshot Q { get { return SpellManager.Q; } }
        public Spell.Active W { get { return SpellManager.W; } }
        public Spell.Active E { get { return SpellManager.E; } }
        public Spell.Active R { get { return SpellManager.R; } }

        public abstract bool ShouldBeExecute();

        public abstract void Execute();
    }
}
