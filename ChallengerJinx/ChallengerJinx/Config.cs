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

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useR;

                public static bool UseQ { get { return _useQ.CurrentValue; } }
                public static bool UseW { get { return _useW.CurrentValue; } }
                public static bool UseE { get { return _useE.CurrentValue; } }
                public static bool UseR { get { return _useR.CurrentValue; } }

                static Combo()
                {
                    Menu.AddGroupLabel("Combo");

                    _useQ = Menu.Add("comboUseQ", new CheckBox("Use Q"));
                    _useW = Menu.Add("comboUseW", new CheckBox("Use W"));
                    _useE = Menu.Add("comboUseE", new CheckBox("Use E"));
                    _useR = Menu.Add("comboUseR", new CheckBox("Use R"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly Slider _mana;

                public static bool UseQ { get { return _useQ.CurrentValue; } }
                public static bool UseW { get { return _useW.CurrentValue; } }
                public static int Mana { get { return _mana.CurrentValue; } }

                static Harass()
                {
                    Menu.AddGroupLabel("Harass");

                    _useQ = Menu.Add("harassUseQ", new CheckBox("Use Q"));
                    _useW = Menu.Add("harassUseW", new CheckBox("Use W"));
                    _mana = Menu.Add("harassMana", new Slider("Minimum mana in %", 30));
                }

                public static Initialize()
                {
                }
            }

            public static class LaneClear
            {
                private static readonly CheckBox _useQ;
                private static readonly Slider _mana;

                public static bool UseQ { get { return _useQ.CurrentValue; } }
                public static int Mana { get { return _mana.CurrentValue; } }

                static LaneClear()
                {
                    Menu.AddGroupLabel("LaneClear");

                    _useQ = Menu.Add("laneUseQ", new CheckBox("Use Q"));
                    _mana = Menu.Add("laneMana", new Slider("Minimum mana in %", 30));
                }

                public static Initialize()
                {
                }
            }

            public static class JungleClear
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly Slider _mana;

                public static bool UseQ { get { return _useQ.CurrentValue; } }
                public static bool UseW { get { return _useW.CurrentValue; } }
                public static int Mana { get { return _mana.CurrentValue; } }

                static JungleClear()
                {
                    Menu.AddGroupLabel("JungleClear");

                    _useQ = Menu.Add("jungleUseQ", new CheckBox("Use Q"));
                    _useW = Menu.Add("jungleUseW", new CheckBox("Use W"));
                    _mana = Menu.Add("jungleMana", new Slider("Minimum mana in %", 30));
                }

                public static Initialize()
                {
                }
            }

            public static class Flee
            {
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                
                public static bool UseW { get { return _useW.CurrentValue; } }
                public static bool UseE { get { return _useE.CurrentValue; } }

                static Flee()
                {
                    Menu.AddGroupLabel("Flee");

                    _useW = Menu.Add("fleeUseW", new CheckBox("Use W"));
                    _useE = Menu.Add("fleeUseE", new CheckBox("Use E"));
                }

                public static Initialize()
                {
                }
            }
        }

        public static class Drawing
        {
            private static Menu Menu 
        }
    }
}