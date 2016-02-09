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
        public static Menu Menu, SpellMenu, DebugMenu, SummonerMenu, TrapMenu, ItemMenu, WardMenu, SC2Menu, MiscMenu;
        public static List<string> MenuChecker = new List<string>();
        

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

                #region Main Menu
                Menu = MainMenu.AddMenu("TimerBuddy", "TimerBuddy", "TimerBuddy - Life is about Timing!");
                Menu.AddGroupLabel("Welcome to TimerBuddy xD");
                Menu.AddLabel(string.Format("{0} datas Loaded", summonerList.Count + itemList.Count + trapList.Count + spellList.Count + wardList.Count));
                Menu.AddGroupLabel("General Settings");
                Menu.AddImportanceItem("minImportance", "Minimum Importance Level to draw: ");

                //Menu.AddGroupLabel("Spell Timer");
                Menu.AddCheckBox("sTimer", "Spell Timer", true);
                Menu.AddCheckBox("ssTimer", "Summoner Spell Timer", true);
                Menu.AddCheckBox("itemTimer", "Item Timer", true);
                Menu.AddBlank("mainBlank");
                Menu.AddCheckBox("trapTimer", "Trap Timer", true);
                Menu.AddCheckBox("wardTimer", "Ward Timer", true);
                Menu.AddSeparator();

                Menu.AddGroupLabel("Additional Features");
                Menu.AddCheckBox("blinkTracker", "Blink Tracker", true);
                Menu.AddCheckBox("cloneTracker", "Clone Tracker", false);
                Menu.AddSeparator();

                Menu.AddGroupLabel("Credits");
                Menu.AddLabel("Tychus   - Addon Developer");
                Menu.AddLabel("Hellsing - Dev-a-lot");
                Menu.AddLabel("and Developing forum buddies");
                #endregion

                #region SC2Menu
                SC2Menu = Menu.AddSubMenu("Notifications");
                SC2Menu.AddGroupLabel("Dragon, Baron Nashor Spawn Time");
                SC2Menu.AddCheckBox("jungleEnable", "Enable", true);
                SC2Menu.AddBlank("blank");
                SC2Menu.AddCheckBox("jungle", "Notifications when 10 seconds left", true);
                SC2Menu.AddCheckBox("jungle1min", "Notifications when 1 minute left", true);
                SC2Menu.AddSeparator();
                SC2Menu.AddGroupLabel("Spell Cooldown");
                SC2Menu.AddCheckBox("ult", "Ultimate (Near heros)", true);
                SC2Menu.AddCheckBox("globalUlt", "Global Ultimate", true);
                SC2Menu.AddCheckBox("ss", "Summoner Spell (Player Only)", true);
                SC2Menu.AddSeparator();
                SC2Menu.AddGroupLabel("Global Notification");
                foreach (var database in SC2TimerDatabase.Database.Where(d => heroName.Contains(d.ChampionName) && d.SC2Type == SC2Type.Spell))
                    SC2Menu.AddCheckBox("sc2global" + database.ChampionName, database.ChampionName + " " + database.Slot.ToString(), database.Global);
                SC2Menu.AddSeparator();
                SC2Menu.AddGroupLabel("Misc settings");
                //SC2Menu.AddSlider("duration", "Notifications duration time", 10, 2, 20);
                SC2Menu.AddSlider("maxSlot", "Maximum notifications number", 5, 2, 8);
                #endregion

                #region SpellMenu
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
                        SpellMenu.AddCheckBox(s.MenuCode + "onlyme", "Drawing only Player is " + s.ChampionName, s.OnlyMe);
                        SpellMenu.AddImportanceItem(s.MenuCode + "importance", "Importance Level: ", s.Importance.ToInt());
                        SpellMenu.AddDrawTypeItem(s.MenuCode + "drawtype", "Drawing Style: ", s.DrawType.ToInt());
                        SpellMenu.AddColorItem(s.MenuCode + "color");
                        SpellMenu.AddSeparator();
                    }
                }
                #endregion

                #region SummonerMenu
                if (summonerList.Count > 0)
                {
                    SummonerMenu = Menu.AddSubMenu("SummonerSpell List");
                    foreach (var t in summonerList)
                    {
                        if (MenuChecker.Contains(t.MenuCode))
                            continue;

                        MenuChecker.Add(t.MenuCode);

                        SummonerMenu.AddGroupLabel(t.MenuCode);
                        SummonerMenu.Add(t.MenuCode + "draw", new CheckBox("Draw"));
                        SummonerMenu.AddImportanceItem(t.MenuCode + "importance", "Importance Level: ", t.Importance.ToInt());
                        SummonerMenu.AddDrawTypeItem(t.MenuCode + "drawtype", "Drawing Style: ", t.DrawType.ToInt());
                        SummonerMenu.AddColorItem(t.MenuCode + "color");
                        SummonerMenu.AddSeparator();
                    }
                }
                #endregion

                #region ItemMenu
                if (itemList.Count > 0)
                {
                    ItemMenu = Menu.AddSubMenu("Item List");
                    foreach (var i in itemList)
                    {
                        ItemMenu.AddGroupLabel(i.MenuCode);
                        ItemMenu.AddCheckBox(i.MenuCode + "draw", "Draw", true);
                        ItemMenu.AddBlank(i.MenuCode + "blank");
                        ItemMenu.AddCheckBox(i.MenuCode + "ally", "Draw ally Item", true);
                        ItemMenu.AddCheckBox(i.MenuCode + "enemy", "Draw enemy Item", true);
                        ItemMenu.AddDrawTypeItem(i.MenuCode + "drawtype", "Drawing Style: ", i.DrawType.ToInt());
                        ItemMenu.AddColorItem(i.MenuCode + "color");
                        ItemMenu.AddSeparator();
                    }
                }
                #endregion

                #region TrapMenu
                if (trapList.Count > 0)
                {
                    TrapMenu = Menu.AddSubMenu("Trap List");

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
                    TrapMenu.AddGroupLabel("Misc");
                    TrapMenu.AddCheckBox("circleOnlyEnemy", "Draw circle only enemies trap", true);
                }
                #endregion

                #region WardMenu
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
                #endregion

                #region MiscMenu
                MiscMenu = Menu.AddSubMenu("Misc Settigns");
                MiscMenu.AddGroupLabel("Drawing");
                MiscMenu.AddCheckBox("error", "Show Error Message", true);
                #endregion

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
