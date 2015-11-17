using EloBuddy;
using EloBuddy.SDK;

namespace ChallengerJinx.Mode
{
    public abstract class ModeBase
    {
        private static readonly AIHeroClient Player = EloBuddy.Player.Instance;

        private Spell.Active Q { get { return SpellManager.Q} }
    }
}
