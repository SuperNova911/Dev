using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ChallengerJinx.Mode;

namespace ChallengerJinx
{
    public static class Config
    {
        private const string MenuName = "Jinx";
        public static Menu Menu { get; private set; }

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, "jinxMenu");
            Menu.AddGroupLabel("Challenger Jinx");
            Menu.AddLabel("Kappa");

            
        }
    }
}
