using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

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

        }
    }
}
