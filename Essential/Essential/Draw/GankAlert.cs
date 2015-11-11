using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace Essential.Draw
{
    internal class GankAlert
    {
        public static Menu GankMenu;
        private readonly IDictionary<int, Champion> _championInfoById = new Dictionary<int, Champion>();

        public static void Init()
        {
            var menu = Program.Menu;

            GankMenu = menu.AddSubMenu("GankAlert");

            GankMenu.AddGroupLabel("Enemy");
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                GankMenu.Add(enemy.ChampionName, new CheckBox(enemy.ChampionName));
            }
            GankMenu.Add("ememySmite", new CheckBox("Warn Jungler Only (Smite)"));

            GankMenu.AddGroupLabel("Ally");
            foreach (var ally in EntityManager.Heroes.Allies)
            {
                GankMenu.Add(ally.ChampionName, new CheckBox(ally.ChampionName));
            }
            GankMenu.Add("allySmite", new CheckBox("Warn Jungler Only (Smite)"));

            GankMenu.AddGroupLabel("Draw");
            GankMenu.Add("range", new Slider("Alert Range", 3000, 500, 5000));
            GankMenu.Add("cooldown", new Slider("Trigger Cooldown", 10, 0, 60));
            GankMenu.Add("duration", new Slider("Line Duration", 10, 0, 20));
            GankMenu.Add("champName", new CheckBox("Show Champion Name", true));
            GankMenu.Add("minimapLine", new CheckBox("Draw on Minimap", false));
            GankMenu.Add("ping", new CheckBox("Danger Ping (Local)", false));
        }
    }

    internal class Champion
    {
        private static int index = 0;

        private readonly AIHeroClient hero;
        private readonly bool ally;
        private int textOffSet;

        private event EventHandler OnRange;

        private bool visable;
        private float distance;
        private float lastEnter;
        private float lineStart;
        private int lineWidth;

        public Champion(AIHeroClient _hero, bool _ally)
        {
            index++;
            textOffSet = index * 50;
            hero = _hero;
            ally = _ally;

            
        }
    }
}
