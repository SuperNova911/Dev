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

            Mode.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            private static readonly Menu Menu;

            static Modes()
            {
                Menu = Config.Menu.AddSubMenu("Modes", "modes");

                Combo.Initialize();
                Menu.AddSeparator();

                Harass.Initialize();
                Menu.AddSeparator();

                LaneClear.Initialize();
                Menu.AddSeparator();

                JungleClear.Initialize();
                Menu.AddSeparator();

                Flee.Initialize();
            }

            public static void Initialize()
            {
            }


        }
    }
}