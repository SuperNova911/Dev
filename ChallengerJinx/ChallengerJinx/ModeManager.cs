using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using ChallengerJinx.Mode;

namespace ChallengerJinx
{
    public class ModeManager
    {
        private static List<ModeBase> AvailableModes { get; set; }

        public static void Initialize()
        {
            AvailableModes = new List<ModeBase>
            {
                new Combo(),
                new Harass(),
                new LaneClear(),
                new JungleClear(),
                new LastHit(),
                new flee(),
                new PermaActive()
            };
            Game.OnTick += Ontick;
        }

        private static void Ontick(EventArgs args)
        {
            AvailableModes.ForEach(mode =>
                {
                    if (mode.ShouldBeExecute())
                    {
                        mode.Execute();
                    }
                });
        }
    }
}
