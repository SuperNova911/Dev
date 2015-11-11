using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace Essential.Draw
{
    internal class GankAlert
    {
        public static Menu GankMenu;

        public static void Init()
        {
            var menu = Program.Menu;

            GankMenu = menu.AddSubMenu("GankAlert");

            GankMenu.AddGroupLabel("Ally");
            GankMenu.Add("allySmite", new CheckBox("Warn Jungler (Smite)"));
            GankMenu.AddGroupLabel("Enemy");
        }
    }
}
