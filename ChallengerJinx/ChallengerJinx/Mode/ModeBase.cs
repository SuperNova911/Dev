using EloBuddy;
using EloBuddy.SDK;

namespace ChallengerJinx.Mode
{
    public abstract class ModeBase
    {
        private static readonly AIHeroClient Player = EloBuddy.Player.Instance;

        private Spell.Active Q { get { return SpellManager.Q; } }
        private Spell.Skillshot W { get { return SpellManager.W; } }
        private Spell.Skillshot E { get { return SpellManager.E; } }
        private Spell.Skillshot R { get { return SpellManager.R; } }

        public abstract bool ShouldBeExecute();

        public abstract void Execute();
    }
}
