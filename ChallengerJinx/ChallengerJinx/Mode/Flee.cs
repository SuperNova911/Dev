using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = ChallengerJinx.Config.Modes.Flee;

namespace ChallengerJinx.Mode
{
    public class Flee : ModeBase
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
