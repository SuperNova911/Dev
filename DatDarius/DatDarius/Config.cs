using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System.Collections.Generic;
using DatDarius.Example.Menu.Controls;
using DatDarius.Example.Menu.Interfaces;

namespace DatDarius
{
    public class Config
    {
        public static Menu Menu, DrawMenu, DebugMenu, UltMenu, SpellMenu;

        public static readonly string[] AvailableModes =
            {
                "Smart targetting",
                "Obvious scripting",
                "Near mouse",
                "On key press (auto)",
                "On key press (near mouse)"
            };

        static Config()
        {
            Menu = MainMenu.AddMenu("Dat Darius", "Dat Darius");

            #region Orbwalke Menu
            Menu.AddGroupLabel("Combo");
            Menu.Add("useQcombo", new CheckBox("Use Q", false));
            Menu.Add("useWcombo", new CheckBox("Use W", true));
            Menu.Add("useEcombo", new CheckBox("Use E", true));
            Menu.Add("useRcombo", new CheckBox("Use R", true));
            Menu.Add("IgniteTime", new Slider("Ignite Tick", 3, 1, 5));
            Menu.AddSeparator();

            Menu.AddGroupLabel("Harass");
            Menu.Add("useQharass", new CheckBox("Use Q", false));
            Menu.Add("useWharass", new CheckBox("Use W", true));
            Menu.Add("useEharass", new CheckBox("Use E", false));
            Menu.Add("useRharass", new CheckBox("Use R", false));
            Menu.AddSeparator();

            Menu.AddGroupLabel("LastHit");
            Menu.Add("useWlasthit", new CheckBox("Use W", true));
            Menu.AddSeparator();

            Menu.AddGroupLabel("Lane/Jungle Clear");
            Menu.Add("useQlaneclear", new CheckBox("Use Q", false));
            Menu.Add("useWlaneclear", new CheckBox("Use W", true));
            Menu.AddSeparator();

            Menu.AddGroupLabel("Flee");
            Menu.Add("useQflee", new CheckBox("Use Q", true));
            Menu.Add("useWflee", new CheckBox("Use W", true));
            Menu.AddSeparator();
            #endregion
            /*
            #region Ultimate Menu
            UltMenu = Menu.AddSubMenu("Ultimate");
            
            Slider logic = new Slider("Mode: " + AvailableModes[0], 0, 0, AvailableModes.Length - 1);
            logic.OnValueChange += delegate { logic.DisplayName = "Mode: " + AvailableModes[logic.CurrentValue]; };
            UltMenu.Add("mode", logic);
            UltMenu.AddLabel("Available modes:");
            for (var i = 0; i < AvailableModes.Length; i++)
            {
                UltMenu.AddLabel(string.Format("  - {0}: {1}", i, AvailableModes[i]));
            }

            UltMenu.Add("", new Slider("오차범위", 0, 0, 50));

            UltMenu.AddGroupLabel("Misc");
            UltMenu.Add("flashR", new CheckBox("Flash R", true));
            UltMenu.Add("dieR", new CheckBox("Use R before die", true));
            #endregion
    */
            #region Spell Menu
            SpellMenu = Menu.AddSubMenu("Spell");
            SpellMenu.AddGroupLabel("Q");
            SpellMenu.Add("moveAssist", new CheckBox("Movement assist", true));
            SpellMenu.AddGroupLabel("W");
            SpellMenu.Add("aaReset", new CheckBox("AA Reset", true));
            SpellMenu.AddGroupLabel("E");
            SpellMenu.Add("flashE", new KeyBind("Flash E", false, KeyBind.BindTypes.HoldActive, 'T'));
            SpellMenu.Add("dashE", new CheckBox("Use E on dashing enemy", true));
            SpellMenu.Add("interruptE", new CheckBox("Interrupt enemy", true));
            SpellMenu.Add("towerE", new CheckBox("Tower E", true));
            SpellMenu.AddGroupLabel("R");
            SpellMenu.Add("autoR", new KeyBind("Auto R", true, KeyBind.BindTypes.PressToggle, 'Z'));
            SpellMenu.Add("flashR", new KeyBind("Flash R", false, KeyBind.BindTypes.HoldActive, 'G'));
            SpellMenu.Add("dieR", new CheckBox("Use R before die", true));
            SpellMenu.Add("freeR", new CheckBox("Use R before end free R buff", true));
            SpellMenu.Add("saveRMana", new CheckBox("Save mana for R", true));
            SpellMenu.Add("오차", new Slider("오차범위", 0, 0, 50));
            SpellMenu.AddGroupLabel("Ignite");
            SpellMenu.Add("1tick", new CheckBox("Instant Kill", true));
            SpellMenu.Add("igniteTick", new Slider("Ignite calculation tick", 3, 1, 5));
            #endregion

            #region Draw Menu
            DrawMenu = Menu.AddSubMenu("Draw");
            DrawMenu.AddGroupLabel("Spell Range");
            DrawMenu.Add("drawQ", new CheckBox("Draw Q", true));
            DrawMenu.Add("drawE", new CheckBox("Draw E", true));
            DrawMenu.Add("drawR", new CheckBox("Draw R", false));
            DrawMenu.AddGroupLabel("Misc");
            DrawMenu.Add("drawFlashE", new CheckBox("Draw Flash E", true));
            DrawMenu.Add("drawFlashR", new CheckBox("Draw Flash R", true));
            #endregion

            #region Debug Menu
            DebugMenu = Menu.AddSubMenu("Debug", "Debug");
            DebugMenu.AddGroupLabel("Drawing");
            DebugMenu.Add("ePosPred", new CheckBox("E position prediction", true));
            DebugMenu.AddGroupLabel("HUD");
            DebugMenu.Add("hud", new CheckBox("Show hud", true));
            DebugMenu.Add("hudGeneral", new CheckBox("General properties", true));
            DebugMenu.Add("hudHealth", new CheckBox("Health properties", false));
            DebugMenu.Add("hudPrediction", new CheckBox("Prediction properties", false));
            DebugMenu.Add("hudDamage", new CheckBox("Damage properties", false));

            //DebugMenu.Add("", new CheckBox("", ));
            #endregion
        }

        public static void Initialize()
        {

        }
    }
}
