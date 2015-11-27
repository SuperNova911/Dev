using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using ChallengerBlitzcrank.Mode;

namespace ChallengerBlitzcrank
{
    public static class ModeManager
    {
        public static List<ModeBase> AvailableModes { get; set; }

        public static void Initialize()
        {
            AvailableModes = new List<ModeBase>
            {
                new Combo(),
                new Harass(),
                new PermaActive()
            };
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead || Player.Instance.IsRecalling())
                return;

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
