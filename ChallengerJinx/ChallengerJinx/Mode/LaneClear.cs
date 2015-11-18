using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = ChallengerJinx.Config.Modes.LaneClear;

namespace ChallengerJinx.Mode
{
    public class LaneClear : ModeBase
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
