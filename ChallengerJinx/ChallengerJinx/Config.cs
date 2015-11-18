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

            Modes.Initialize();

            Misc.Initialize();
            Drawing.Initialize();
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

                LastHit.Initialize();
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
                private static readonly CheckBox _useQSplash;

                public static bool UseQ { get { return _useQ.CurrentValue; } }
                public static bool UseW { get { return _useW.CurrentValue; } }
                public static bool UseE { get { return _useE.CurrentValue; } }
                public static bool UseR { get { return _useR.CurrentValue; } }
                public static bool UseQSplash { get { return _useQSplash.CurrentValue; } }

                static Combo()
                {
                    Menu.AddGroupLabel("Combo");

                    _useQ = Menu.Add("comboUseQ", new CheckBox("Use Q"));
                    _useW = Menu.Add("comboUseW", new CheckBox("Use W"));
                    _useE = Menu.Add("comboUseE", new CheckBox("Use E"));
                    _useR = Menu.Add("comboUseR", new CheckBox("Use R"));
                    _useQSplash = Menu.Add("comboUseQSplash", new CheckBox("Use Q Splash"));
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

                public static void Initialize()
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

                public static void Initialize()
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

                public static void Initialize()
                {
                }
            }

            public static class LastHit
            {

                public static void Initialize()
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

                public static void Initialize()
                {
                }
            }
        }

        public static class Misc
        {
            private static Menu Menu { get; set; }

            private static readonly CheckBox _gapcloser;
            private static readonly Slider _minWRange;

            public static bool GapCloser { get { return _gapcloser.CurrentValue; } }
            public static int MinWRange { get { return _minWRange.CurrentValue; } }

            static Misc()
            {
                Menu = Config.Menu.AddSubMenu("Misc");

                Menu.AddGroupLabel("Misc Features");

                _gapcloser = Menu.Add("gapcloser", new CheckBox("Use E on Gapcloser"));
                _minWRange = Menu.Add("minWRange", new Slider("Minimum W Range", 600, 250, 1450));
            }

            public static void Initialize()
            {
            }
        }

        public static class Drawing
        {
            private static Menu Menu { get; set; }
            
            private static readonly CheckBox _drawAA;
            private static readonly CheckBox _drawQ;
            private static readonly CheckBox _drawW;
            private static readonly CheckBox _drawE;
            private static readonly CheckBox _drawR;

            private static readonly CheckBox _healthbar;
            private static readonly CheckBox _percent;

            public static bool DrawAA { get { return _drawAA.CurrentValue; } }
            public static bool DrawQ { get { return _drawQ.CurrentValue; } }
            public static bool DrawW { get { return _drawW.CurrentValue; } }
            public static bool DrawE { get { return _drawE.CurrentValue; } }
            public static bool DrawR { get { return _drawR.CurrentValue; } }
            public static bool Healthbar { get { return _healthbar.CurrentValue; } }
            public static bool Percent { get { return _percent.CurrentValue; } }

            static Drawing()
            {
                Menu = Config.Menu.AddSubMenu("Drawing");

                Menu.AddGroupLabel("Spell Ranges");
                _drawAA = Menu.Add("drawAA", new CheckBox("AA Range"));
                _drawQ = Menu.Add("drawQ", new CheckBox("Q Range"));
                _drawW = Menu.Add("drawW", new CheckBox("W Range"));
                _drawE = Menu.Add("drawE", new CheckBox("E Range"));
                _drawR = Menu.Add("drawR", new CheckBox("R Range"));

                Menu.AddGroupLabel("Damage Indicators (Ult)");
                _healthbar = Menu.Add("healthbar", new CheckBox("Healthbar Overlay"));
                _percent = Menu.Add("percent", new CheckBox("Damage Percent Info"));
            }

            public static void Initialize()
            {
            }
        }
    }
}