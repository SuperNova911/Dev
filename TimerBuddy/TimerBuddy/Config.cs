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
        public static Menu Menu, DrawMenu, SpellMenu, DebugMenu, SummonerMenu, TrapMenu, ItemMenu;
        public static List<string> MenuChecker = new List<string>();

        static Config()
        {
            try
            {
                var hero = EntityManager.Heroes.AllHeroes;
                var heroName = hero.Select(h => h.BaseSkinName).ToArray();
                var summonerList = SpellDatabase.Database.Where(i => i.SpellType == SpellType.SummonerSpell).ToList();
                var itemList = SpellDatabase.Database.Where(i => i.SpellType == SpellType.Item).ToList();
                var trapList = SpellDatabase.Database.Where(t => heroName.Contains(t.ChampionName) && t.SpellType == SpellType.Trap).ToList();
                var spellList = SpellDatabase.Database.Where(s => heroName.Contains(s.ChampionName) && s.SpellType == SpellType.Spell).ToList();
                

                Menu = MainMenu.AddMenu("TimerBuddy", "TimerBuddy");
                Menu.AddLabel(string.Format("Timer datas Loaded {0}", summonerList.Count + itemList.Count + trapList.Count + spellList.Count));
                Menu.AddGroupLabel("TrackList");
                Menu.Add("teleport", new CheckBox("Teleport", true));
                Menu.Add("trap", new CheckBox("Trap", true));
                Menu.Add("spell", new CheckBox("Spell", true));
                Menu.Add("item", new CheckBox("Item", true));

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
                        if (MenuChecker.Contains(s.MenuString))
                            continue;

                        MenuChecker.Add(s.MenuString);

                        SpellMenu.AddGroupLabel(s.MenuString);
                        SpellMenu.Add(s.MenuString + "ally", new CheckBox("Ally"));
                        SpellMenu.Add(s.MenuString + "enemy", new CheckBox("Enemy"));
                        SpellMenu.Add(s.MenuString + "draw", new CheckBox("Draw"));
                        SpellMenu.AddColorItem(s.MenuString + "color");
                        SpellMenu.AddSeparator();
                    }
                }

                if (itemList.Count > 0)
                {
                    SummonerMenu = Menu.AddSubMenu("SummonerSpell List");
                    foreach (var t in summonerList)
                    {
                        SummonerMenu.AddGroupLabel(t.MenuString);
                        SummonerMenu.Add(t.MenuString + "ally", new CheckBox("Ally"));
                        SummonerMenu.Add(t.MenuString + "enemy", new CheckBox("Enemy"));
                        SummonerMenu.Add(t.MenuString + "draw", new CheckBox("Draw"));
                        SummonerMenu.AddColorItem(t.MenuString + "color");
                        SummonerMenu.AddSeparator();
                    }
                }

                if (itemList.Count > 0)
                {
                    ItemMenu = Menu.AddSubMenu("Item List");
                    foreach (var i in itemList)
                    {
                        ItemMenu.AddGroupLabel(i.MenuString);
                        ItemMenu.Add(i.MenuString + "ally", new CheckBox("Ally"));
                        ItemMenu.Add(i.MenuString + "enemy", new CheckBox("Enemy"));
                        ItemMenu.Add(i.MenuString + "draw", new CheckBox("Draw"));
                        ItemMenu.AddColorItem(i.MenuString + "color");
                        ItemMenu.AddSeparator();
                    }
                }

                if (trapList.Count > 0)
                {
                    TrapMenu = Menu.AddSubMenu("Trap List");
                    foreach (var t in trapList)
                    {
                        TrapMenu.AddGroupLabel(t.MenuString);
                        TrapMenu.Add(t.MenuString + "ally", new CheckBox("Ally"));
                        TrapMenu.Add(t.MenuString + "enemy", new CheckBox("Enemy"));
                        TrapMenu.Add(t.MenuString + "draw", new CheckBox("Draw"));
                        TrapMenu.Add(t.MenuString + "drawCircle", new CheckBox("Draw Circle"));
                        TrapMenu.AddColorItem(t.MenuString + "color");
                        TrapMenu.AddSeparator();
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
