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
        public static Menu Menu, DrawMenu, SpellMenu;
        public static List<string> MenuChecker = new List<string>();

        static Config()
        {
            try
            {
                Menu = MainMenu.AddMenu("TimerBuddy", "TimerBuddy");
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

                var hero = EntityManager.Heroes.AllHeroes;
                var heroName = hero.Select(h => h.BaseSkinName).ToArray();
                var teleportList = SpellDatabase.Database.Where(i => i.SpellType == SpellType.Teleport).ToList();
                var itemList = SpellDatabase.Database.Where(i => i.SpellType == SpellType.Item).ToList();
                var trapList = SpellDatabase.Database.Where(t => heroName.Contains(t.ChampionName) && t.SpellType == SpellType.Trap).ToList();
                var spellList = SpellDatabase.Database.Where(s => heroName.Contains(s.ChampionName) && s.SpellType == SpellType.Spell).ToList();
                
                SpellMenu = Menu.AddSubMenu("Timer List");
                SpellMenu.AddLabel(string.Format("Timer datas Loaded {0}", teleportList.Count + itemList.Count + trapList.Count + spellList.Count));

                foreach (var t in teleportList)
                {
                    SpellMenu.AddGroupLabel(t.MenuString);
                    SpellMenu.Add(t.MenuString + "ally", new CheckBox("Ally"));
                    SpellMenu.Add(t.MenuString + "enemy", new CheckBox("Enemy"));
                    SpellMenu.Add(t.MenuString + "draw", new CheckBox("Draw"));
                    SpellMenu.AddColorItem(t.MenuString + "color");
                    SpellMenu.AddSeparator();
                }
                foreach (var i in itemList)
                {
                    SpellMenu.AddGroupLabel(i.MenuString);
                    SpellMenu.Add(i.MenuString + "ally", new CheckBox("Ally"));
                    SpellMenu.Add(i.MenuString + "enemy", new CheckBox("Enemy"));
                    SpellMenu.Add(i.MenuString + "draw", new CheckBox("Draw"));
                    SpellMenu.AddColorItem(i.MenuString + "color");
                    SpellMenu.AddSeparator();
                }
                foreach (var t in trapList)
                {
                    SpellMenu.AddGroupLabel(t.MenuString);
                    SpellMenu.Add(t.MenuString + "ally", new CheckBox("Ally"));
                    SpellMenu.Add(t.MenuString + "enemy", new CheckBox("Enemy"));
                    SpellMenu.Add(t.MenuString + "draw", new CheckBox("Draw"));
                    SpellMenu.Add(t.MenuString + "drawCircle", new CheckBox("Draw Circle"));
                    SpellMenu.AddColorItem(t.MenuString + "color");
                    SpellMenu.AddSeparator();
                }
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE MENU");
            }
        }

        private static void ColorValue_OnValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            throw new NotImplementedException();
        }

        public static void Initialize()
        {

        }
    }
}
