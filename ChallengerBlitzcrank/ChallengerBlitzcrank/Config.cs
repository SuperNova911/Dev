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
            Menu.AddGroupLabel("Challenger BLitzcrank");
            Menu.AddLabel("Kappa");

            SpellSetting.Initialize();
            Mode.Initialize();
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
                public static CheckBox comboQ;
                public static CheckBox harassQ;
                public static CheckBox killstealQ;
                public static CheckBox interruptQ;
                public static CheckBox dashQ;
                public static CheckBox immobileQ;
                public static Slider hitchanceQ;
                public static Slider minrangeQ;
                public static Slider maxrangeQ;
                public static Slider _minHealthQ;

                public static bool ComboQ { get { return comboQ.CurrentValue; } }
                public static bool HarassQ { get { return harassQ.CurrentValue; } }
                public static bool KillstealQ { get { return killstealQ.CurrentValue; } }
                public static bool InterruptQ { get { return interruptQ.CurrentValue; } }
                public static bool DashQ { get { return dashQ.CurrentValue; } }
                public static bool ImmobileQ { get { return immobileQ.CurrentValue; } }
                public static int HitchanceQ { get { return hitchanceQ.CurrentValue; } }
                public static int MinrangeQ { get { return minrangeQ.CurrentValue; } }
                public static int MaxrangeQ { get { return maxrangeQ.CurrentValue; } }
                public static int MinHealthQ { get { return _minHealthQ.CurrentValue; } }

                static Q()
                {
                    Menu.AddGroupLabel("Q - Rocket Grab");

                    comboQ = Menu.Add("comboQ", new CheckBox("Combo", true));
                    harassQ = Menu.Add("harassQ", new CheckBox("Harass", true));
                    killstealQ = Menu.Add("killstealQ", new CheckBox("Kill Steal", true));
                    interruptQ = Menu.Add("interruptQ", new CheckBox("Interrupt Enemy", true));
                    dashQ = Menu.Add("dashQ", new CheckBox("Dashing Enemy", true));
                    immobileQ = Menu.Add("immobileQ", new CheckBox("Immobile Enemy", true));
                    hitchanceQ = Menu.Add("hitchanceQ", new Slider("Hit Chance", 3, 1, 3));
                    minrangeQ = Menu.Add("minrangeQ", new Slider("Minimum Range", 450, 0, 950));
                    maxrangeQ = Menu.Add("maxrangeQ", new Slider("Maximum Range", 950, 0, 950));
                    _minHealthQ = Menu.Add("_minManaQ", new Slider("Minimum Health % for AutoGrab", 10, 0, 100)); 
                }

                public static void Initialize()
                {
                }
            }

            public static class E
            {
                public static CheckBox comboE;
                public static CheckBox harassE;
                public static CheckBox interruptE;
                public static CheckBox killstealE;

                public static bool ComboE { get { return comboE.CurrentValue; } }
                public static bool HarassE { get { return harassE.CurrentValue; } }
                public static bool InterruptE { get { return interruptE.CurrentValue; } }
                public static bool KillstealE { get { return killstealE.CurrentValue; } }

                static E()
                {
                    Menu.AddGroupLabel("E - Power Fist");

                    comboE = Menu.Add("comboE", new CheckBox("Combo", true));
                    harassE = Menu.Add("harassE", new CheckBox("Harass", true));
                    interruptE = Menu.Add("interruptE", new CheckBox("Interrupt Enemy", true));
                    killstealE = Menu.Add("killstealE", new CheckBox("Killsteal Enemy", false));
                }

                public static void Initialize()
                {
                }
            }

            public static class R
            {
                public static CheckBox comboR;
                public static CheckBox harassR;
                public static CheckBox killstealR;
                public static CheckBox interruptR;
                public static Slider minEnemyR;

                public static bool ComboR { get { return comboR.CurrentValue; } }
                public static bool HarassR { get { return harassR.CurrentValue; } }
                public static bool KillstealR { get { return killstealR.CurrentValue; } }
                public static bool InterruptR { get { return interruptR.CurrentValue; } }
                public static int MinEnemyR { get { return minEnemyR.CurrentValue; } }

                static R()
                {
                    Menu.AddGroupLabel("R - Static Field");

                    comboR = Menu.Add("comboR", new CheckBox("Combo", true));
                    harassR = Menu.Add("harassR", new CheckBox("Harass", false));
                    killstealR = Menu.Add("killstealR", new CheckBox("Interrupt Enemy", true));
                    interruptR = Menu.Add("interruptR", new CheckBox("Killsteal Enemy", true));
                    minEnemyR = Menu.Add("minEnemyR", new Slider("Minimum Enemies In Range", 2, 1, 5));
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class Mode
        {
            public static Menu Menu { get; set; }

            public static Slider grabMode;

            public static int GrabMode { get { return grabMode.CurrentValue; } }

            static Mode()
            {
                Menu = Config.Menu.AddSubMenu("Grab Mode");

                Menu.AddGroupLabel("Grab Mode");
                Menu.AddLabel("1 = Don't Grab, 2 = Normal Grab, 3 = Auto Grab");

                var num = 1;
                foreach (var enemy in EntityManager.Heroes.Enemies)
                {
                    Chat.Print(num);
                    num++;
                    grabMode = Menu.Add("grabMode" + enemy.ChampionName, new Slider(enemy.ChampionName, 2, 1, 3));
                }

            }

            public static void Initialize()
            {
            }
        }

        public static class Drawing
        {
            public static Menu Menu { get; set; }

            public static CheckBox drawQ;
            public static CheckBox drawR;
            public static CheckBox drawTarget;
            public static CheckBox drawHitchance;
            public static CheckBox drawDamage;

            public static bool DrawQ { get { return drawQ.CurrentValue; } }
            public static bool DrawR { get { return drawR.CurrentValue; } }
            public static bool DrawTarget { get { return drawTarget.CurrentValue; } }
            public static bool DrawHitchance { get { return drawHitchance.CurrentValue; } }
            public static bool DrawDamage { get { return drawDamage.CurrentValue; } }

            static Drawing()
            {
                Menu = Config.Menu.AddSubMenu("Drawing");

                Menu.AddGroupLabel("Spell");
                drawQ = Menu.Add("drawQ", new CheckBox("Q Range", true));
                drawR = Menu.Add("drawR", new CheckBox("R Range", false));

                Menu.AddGroupLabel("Misc");
                drawTarget = Menu.Add("DrawTarget", new CheckBox("Mark Q Target", true));
                drawHitchance = Menu.Add("drawHitchance", new CheckBox("Hitchance", false));
                drawDamage = Menu.Add("drawDamage", new CheckBox("Damage Indicator", true));
            }

            public static void Initialize()
            {
            }
        }
    }
}
