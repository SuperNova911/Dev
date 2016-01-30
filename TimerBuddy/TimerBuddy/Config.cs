using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TimerBuddy
{
    internal class Config
    {
        public static Menu Menu, DrawMenu, SpellMenu, DebugMenu, SummonerMenu, TrapMenu, ItemMenu, WardMenu;
        public static List<string> MenuChecker = new List<string>();

        private static readonly Slider _importance;
        public static int ImportanceLevel
        {
            get { return _importance.CurrentValue; }
        }

        public static readonly string[] ImportanceLevelList =
            {
                "Low",
                "Midium",
                "High",
            };

        static Config()
        {
            try
            {
                var hero = EntityManager.Heroes.AllHeroes;
                var heroName = hero.Select(h => h.BaseSkinName).ToArray();
                var summonerList = SpellDatabase.Database.Where(i => i.SpellType == SpellType.SummonerSpell).ToList();
                var itemList = SpellDatabase.Database.Where(i => i.SpellType == SpellType.Item).ToList();
                var wardList = SpellDatabase.Database.Where(i => i.SpellType == SpellType.Ward).ToList();
                var trapList = SpellDatabase.Database.Where(t => heroName.Contains(t.ChampionName) && t.SpellType == SpellType.Trap).ToList();
                var spellList = SpellDatabase.Database.Where(s => heroName.Contains(s.ChampionName) && s.SpellType == SpellType.Spell).ToList();


                Menu = MainMenu.AddMenu("TimerBuddy", "TimerBuddy");
                //Menu.AddLabel(string.Format("Timer datas Loaded {0}", summonerList.Count + itemList.Count + trapList.Count + spellList.Count + wardList.Count));
                Menu.AddGroupLabel("Spell Timer");

                Menu.AddCheckBox("trackally", "Ally", true);
                Menu.AddCheckBox("trackenemy", "Enemy", true);
                Menu.AddSlider("maxList", "Maximum number of tracking list", 3, 1, 10);
                _importance = new Slider("Minimum Importance Level: " + ImportanceLevelList[1], 1, 0, ImportanceLevelList.Length - 1);
                _importance.OnValueChange += delegate { _importance.DisplayName = "Minimum Importance Level: " + ImportanceLevelList[_importance.CurrentValue]; };
                Menu.Add("mode", _importance);
                Menu.AddSeparator();
                

                Menu.AddCheckBox("spellTracker", "Spell Timer", true);
                Menu.AddCheckBox("wardTracker", "Ward Timer", true);
                Menu.AddCheckBox("cloneTracker", "Clone Tracker", true);
                Menu.AddCheckBox("blinkTracker", "Blink Tracker", true);

                

                DrawMenu = Menu.AddSubMenu("Drawing");
                DrawMenu.AddGroupLabel("Trap");
                DrawMenu.Add("trapAlly", new CheckBox("Ally Trap", true));
                DrawMenu.Add("trapEnemy", new CheckBox("Enemy Trap", true));
                DrawMenu.Add("trapTimer", new CheckBox("Timer", true));

                if (spellList.Count > 0)
                {
                    SpellMenu = Menu.AddSubMenu("Spell List");
                    foreach (var s in spellList)
                    {
                        if (MenuChecker.Contains(s.MenuCode))
                            continue;

                        MenuChecker.Add(s.MenuCode);

                        SpellMenu.AddGroupLabel(s.MenuCode);
                        SpellMenu.AddCheckBox(s.MenuCode + "draw", "Draw", true);
                        SpellMenu.AddCheckBox(s.MenuCode + "onlyme", "Draw when " + s.ChampionName + " is me", s.OnlyMe);
                        SpellMenu.AddImportanceItem(s.MenuCode + "importance", s.Importance.ToInt());
                        SpellMenu.AddDrawTypeItem(s.MenuCode + "drawtype", s.DrawType.ToInt());
                        SpellMenu.AddColorItem(s.MenuCode + "color");
                        SpellMenu.AddSeparator();
                    }
                }

                if (itemList.Count > 0)
                {
                    SummonerMenu = Menu.AddSubMenu("SummonerSpell List");
                    foreach (var t in summonerList)
                    {
                        if (MenuChecker.Contains(t.MenuCode))
                            continue;

                        MenuChecker.Add(t.MenuCode);

                        SummonerMenu.AddGroupLabel(t.MenuCode);
                        SummonerMenu.Add(t.MenuCode + "draw", new CheckBox("Draw"));
                        SummonerMenu.AddImportanceItem(t.MenuCode + "importance", t.Importance.ToInt());
                        SummonerMenu.AddColorItem(t.MenuCode + "color");
                        SummonerMenu.AddSeparator();
                    }
                }

                if (itemList.Count > 0)
                {
                    ItemMenu = Menu.AddSubMenu("Item List");
                    foreach (var i in itemList)
                    {
                        ItemMenu.AddGroupLabel(i.MenuCode);
                        ItemMenu.AddCheckBox(i.MenuCode + "draw", "Draw", true);
                        ItemMenu.AddCheckBox(i.MenuCode + "ally", "Draw ally Item", true);
                        ItemMenu.AddCheckBox(i.MenuCode + "enemy", "Draw enemy Item", true);
                        ItemMenu.AddColorItem(i.MenuCode + "color");
                        ItemMenu.AddDrawTypeItem(i.MenuCode + "drawtype", i.DrawType.ToInt());
                        ItemMenu.AddSeparator();
                    }
                }

                if (trapList.Count > 0)
                {
                    TrapMenu = Menu.AddSubMenu("Trap List");
                    TrapMenu.AddGroupLabel("Trap Manager");
                    TrapMenu.AddCheckBox("trapEnable", "Enable", true);
                    TrapMenu.AddCheckBox("circleOnlyEnemy", "Draw circle only enemies trap", true);

                    foreach (var t in trapList)
                    {
                        TrapMenu.AddGroupLabel(t.MenuCode);
                        TrapMenu.AddCheckBox(t.MenuCode + "draw", "Draw", true);
                        TrapMenu.AddCheckBox(t.MenuCode + "ally", "Draw ally trap", true);
                        TrapMenu.AddCheckBox(t.MenuCode + "drawCircle", "Draw circle", true);
                        TrapMenu.AddCheckBox(t.MenuCode + "enemy", "Draw enemy trap", true);
                        TrapMenu.AddColorItem(t.MenuCode + "color", 11);
                        TrapMenu.AddSeparator();
                    }
                }

                if (wardList.Count > 0)
                {
                    WardMenu = Menu.AddSubMenu("Ward List");
                    foreach (var w in wardList)
                    {
                        WardMenu.AddGroupLabel(w.MenuCode);
                        WardMenu.AddCheckBox(w.MenuCode + "draw", "Draw", true);
                        WardMenu.AddCheckBox(w.MenuCode + "ally", "Draw ally ward", true);
                        WardMenu.AddCheckBox(w.MenuCode + "drawCircle", "Draw circle", true);
                        WardMenu.AddCheckBox(w.MenuCode + "enemy", "Draw enemy ward", true);
                        WardMenu.AddColorItem(w.MenuCode + "color", w.Color.ToInt());
                        WardMenu.AddSeparator();
                    }
                }

                DebugMenu = Menu.AddSubMenu("Debug");
                DebugMenu.Add("s1", new Slider("Slider 1", 0, 0, 200));
                DebugMenu.Add("s2", new Slider("Slider 2", 0, 0, 200));
                DebugMenu.Add("s3", new Slider("Slider 3", 0, 0, 200));
                DebugMenu.Add("s4", new Slider("Slider 4", 0, 0, 200));
                DebugMenu.Add("s5", new Slider("Slider 5", 0, 0, 200));
                DebugMenu.Add("c1", new CheckBox("CheckBox 1"));
                DebugMenu.Add("c2", new CheckBox("CheckBox 2"));
                DebugMenu.Add("c3", new CheckBox("CheckBox 3"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE MENU");
            }
        }
        
        public static void Initialize()
        {

        }
    }
}
