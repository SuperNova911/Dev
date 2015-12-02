using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ChallengerBlitzcrank
{
    public static class Config
    {
        private const string MenuName = "Blitzcrank";
        public static Menu Menu { get; private set; }

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, "blitzMenu");
            Menu.AddGroupLabel("Challenger Blitzcrank");
            //Menu.AddGroupLabel("");
            //Menu.AddLabel("");

            SpellSetting.Initialize();
            Drawing.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class SpellSetting
        {
            public static Menu Menu;

            static SpellSetting()
            {
                Menu = Config.Menu.AddSubMenu("Spell", "spell");

                Q.Initialize();
                Menu.AddSeparator();

                E.Initialize();
                Menu.AddSeparator();

                R.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Q
            {
                public static CheckBox _comboQ;
                public static CheckBox _harassQ;
                public static CheckBox _focusQ;
                public static CheckBox _killstealQ;
                public static CheckBox _interruptQ;
                public static CheckBox _dashQ;
                public static CheckBox _immobileQ;
                public static Slider _hitchanceQ;
                public static Slider _minrangeQ;
                public static Slider _maxrangeQ;
                public static Slider _minHealthQ;

                public static bool ComboQ { get { return _comboQ.CurrentValue; } }
                public static bool HarassQ { get { return _harassQ.CurrentValue; } }
                public static bool FocusQ { get { return _focusQ.CurrentValue; } }
                public static bool KillstealQ { get { return _killstealQ.CurrentValue; } }
                public static bool InterruptQ { get { return _interruptQ.CurrentValue; } }
                public static bool DashQ { get { return _dashQ.CurrentValue; } }
                public static bool ImmobileQ { get { return _immobileQ.CurrentValue; } }
                public static int HitchanceQ { get { return _hitchanceQ.CurrentValue; } }
                public static int MinrangeQ { get { return _minrangeQ.CurrentValue; } }
                public static int MaxrangeQ { get { return _maxrangeQ.CurrentValue; } }
                public static int MinHealthQ { get { return _minHealthQ.CurrentValue; } }

                static Q()
                {
                    Menu.AddGroupLabel("Q - Rocket Grab");

                    Menu.AddLabel("Orbwalk");
                    _comboQ = Menu.Add("comboQ", new CheckBox("Combo", true));
                    _harassQ = Menu.Add("harassQ", new CheckBox("Harass", true));
                    _focusQ = Menu.Add("focusQ", new CheckBox("Force Grab Selected Target", true));
                    Menu.AddSeparator();
                    Menu.AddLabel("Auto Grab");
                    _killstealQ = Menu.Add("killstealQ", new CheckBox("Kill Secure", true));
                    _dashQ = Menu.Add("dashQ", new CheckBox("Dashing Enemy", true));
                    _interruptQ = Menu.Add("interruptQ", new CheckBox("Interrupt Enemy", true));
                    _immobileQ = Menu.Add("immobileQ", new CheckBox("Immobile Enemy", true));
                    Menu.AddSeparator();
                    Menu.AddLabel("Settings");
                    _hitchanceQ = Menu.Add("hitchanceQ", new Slider("Hitchance", 3, 1, 3));
                    _minrangeQ = Menu.Add("minrangeQ", new Slider("Minimum Range", 450, 0, 950));
                    _maxrangeQ = Menu.Add("maxrangeQ", new Slider("Maximum Range", 950, 0, 950));
                    _minHealthQ = Menu.Add("minHealthQ", new Slider("Minimum Health % for AutoGrab", 10, 0, 100));
                }

                public static void Initialize()
                {
                }
            }

            public static class E
            {
                public static CheckBox _comboE;
                public static CheckBox _harassE;
                public static CheckBox _interruptE;
                public static CheckBox _killstealE;
                public static CheckBox _autoE;
                public static CheckBox _aaResetE;

                public static bool ComboE { get { return _comboE.CurrentValue; } }
                public static bool HarassE { get { return _harassE.CurrentValue; } }
                public static bool InterruptE { get { return _interruptE.CurrentValue; } }
                public static bool KillstealE { get { return _killstealE.CurrentValue; } }
                public static bool AutoE { get { return _autoE.CurrentValue; } }
                public static bool AAResetE { get { return _aaResetE.CurrentValue; } }

                static E()
                {
                    Menu.AddGroupLabel("E - Power Fist");

                    Menu.AddLabel("Orbwalk");
                    _comboE = Menu.Add("comboE", new CheckBox("Combo", true));
                    _harassE = Menu.Add("harassE", new CheckBox("Harass", true));
                    Menu.AddLabel("Settings");
                    _interruptE = Menu.Add("interruptE", new CheckBox("Interrupt Enemy", true));
                    _killstealE = Menu.Add("killstealE", new CheckBox("Killsteal Enemy", false));
                    _autoE = Menu.Add("autoE", new CheckBox("Auto E (Before AA)", true));
                    _aaResetE = Menu.Add("aaResetE", new CheckBox("AA Reset (After AA)", false));
                }

                public static void Initialize()
                {
                }
            }

            public static class R
            {
                public static CheckBox _comboR;
                public static CheckBox _harassR;
                public static CheckBox _killstealR;
                public static CheckBox _interruptR;
                public static CheckBox _gapcloseR;
                public static Slider _minEnemyR;

                public static bool ComboR { get { return _comboR.CurrentValue; } }
                public static bool HarassR { get { return _harassR.CurrentValue; } }
                public static bool KillstealR { get { return _killstealR.CurrentValue; } }
                public static bool InterruptR { get { return _interruptR.CurrentValue; } }
                public static bool GapcloseR { get { return _gapcloseR.CurrentValue; } }
                public static int MinEnemyR { get { return _minEnemyR.CurrentValue; } }

                static R()
                {
                    Menu.AddGroupLabel("R - Static Field");

                    Menu.AddLabel("Orbwalk");
                    _comboR = Menu.Add("comboR", new CheckBox("Combo", true));
                    _harassR = Menu.Add("harassR", new CheckBox("Harass", false));
                    Menu.AddLabel("Settings");
                    _killstealR = Menu.Add("killstealR", new CheckBox("Interrupt Enemy", true));
                    _interruptR = Menu.Add("interruptR", new CheckBox("Killsteal Enemy", true));
                    _gapcloseR = Menu.Add("gapcloseR", new CheckBox("On Gapcloser", false));
                    _minEnemyR = Menu.Add("minEnemyR", new Slider("Minimum Enemies In Range", 2, 1, 5));
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class Drawing
        {
            public static Menu Menu { get; set; }

            public static CheckBox _drawQ;
            public static CheckBox _drawR;
            public static CheckBox _smartDrawing;
            public static CheckBox _drawTarget;
            public static CheckBox _drawHitchance;
            public static CheckBox _drawDamage;

            public static bool DrawQ { get { return _drawQ.CurrentValue; } }
            public static bool DrawR { get { return _drawR.CurrentValue; } }
            public static bool SmartDrawing { get { return _smartDrawing.CurrentValue; } }
            public static bool DrawTarget { get { return _drawTarget.CurrentValue; } }
            public static bool DrawHitchance { get { return _drawHitchance.CurrentValue; } }
            public static bool DrawDamage { get { return _drawDamage.CurrentValue; } }

            static Drawing()
            {
                Menu = Config.Menu.AddSubMenu("Drawing");

                Menu.AddGroupLabel("Spell");
                _drawQ = Menu.Add("drawQ", new CheckBox("Q Range", true));
                _drawR = Menu.Add("drawR", new CheckBox("R Range", false));
                Menu.AddSeparator();
                _smartDrawing = Menu.Add("smartDrawing", new CheckBox("Smart Drawing", true));
                Menu.AddLabel("Green = Ready");
                Menu.AddLabel("Orange = On CoolDown");
                Menu.AddLabel("Red = Too Low Health % for AutoGrab");
                Menu.AddSeparator();

                Menu.AddGroupLabel("Misc");
                _drawTarget = Menu.Add("DrawTarget", new CheckBox("Mark Q Target", true));
                _drawHitchance = Menu.Add("drawHitchance", new CheckBox("Hitchance", false));
                _drawDamage = Menu.Add("drawDamage", new CheckBox("Damage Indicator", true));
            }

            public static void Initialize()
            {
            }
        }
    }
}
