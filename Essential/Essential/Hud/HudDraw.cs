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

namespace Essential.Hud
{
    internal class HudDraw
    {
        public static Menu HudMenu;

        public static void Init()
        {
            var menu = Program.Menu;

            HudMenu = menu.AddSubMenu("HUD");

            HudMenu.AddGroupLabel("WaterMark");
            HudMenu.Add("watermark", new CheckBox("Draw EloBuddy WaterMark", true));
            HudMenu.AddLabel("(Note: F5 Required)");
        }

        internal static bool watermark()
        {
            var watermark = HudMenu["watermark"].Cast<CheckBox>().CurrentValue;
            return watermark;
        }
    }
}
