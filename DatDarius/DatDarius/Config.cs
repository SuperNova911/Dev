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
        public static Menu Menu, DrawMenu, DebugMenu, SpellMenu, OrbMenu;

        static Config()
        {
            Menu = MainMenu.AddMenu("Dat Darius", "Dat Darius");
            Menu.AddGroupLabel("Dat Darius");
            Menu.Add("damageIndicator", new CheckBox("Damage Indicator", true));
            Menu.Add("drawing", new CheckBox("Enable Drawing", true));
            Menu.Add("skin", new CheckBox("Skin Changer", false));
            Menu.Add("debug", new CheckBox("Enable Debug  (need F5)", true));
            Menu.Add("useItem", new CheckBox("Use Item", true));
            Menu.AddSeparator();
            Menu.AddGroupLabel("SkinChanger");
            var skin = Menu.Add("sID", new Slider("Skin", 0, 0, 5));
            var sID = new[] { "Classic", "Lord Darius", "Bioforge Darius", "Woad King Darius", "Dunkmaster Darius", "Academy Darius" };
            skin.DisplayName = sID[skin.CurrentValue];

            skin.OnValueChange +=
                delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = sID[changeArgs.NewValue];
                };
            Menu.AddSeparator();
            Menu.AddGroupLabel("Credit");
            Menu.AddLabel("Addons - by Tychus");
            Menu.AddLabel("DamageIndicator - by Fluxy");

            #region Orbwalk Menu
            OrbMenu = Menu.AddSubMenu("Orbwalk mode");
            OrbMenu.AddGroupLabel("Combo");
            OrbMenu.Add("useQcombo", new CheckBox("Use Q", true));
            OrbMenu.Add("useWcombo", new CheckBox("Use W", true));
            OrbMenu.Add("useEcombo", new CheckBox("Use E", true));
            OrbMenu.Add("useRcombo", new CheckBox("Use R", true));
            OrbMenu.AddSeparator();

            OrbMenu.AddGroupLabel("Harass");
            OrbMenu.Add("useQharass", new CheckBox("Use Q", false));
            OrbMenu.Add("useWharass", new CheckBox("Use W", true));
            OrbMenu.Add("useEharass", new CheckBox("Use E", false));
            OrbMenu.Add("useRharass", new CheckBox("Use R", false));
            OrbMenu.AddSeparator();
            
            OrbMenu.AddGroupLabel("Lane Clear");
            OrbMenu.Add("useQlaneclear", new CheckBox("Use Q", false));
            OrbMenu.Add("useWlaneclear", new CheckBox("Use W", true));
            OrbMenu.Add("laneMana", new Slider("Minimum mana %", 40, 0, 100));
            OrbMenu.AddSeparator();

            OrbMenu.AddGroupLabel("Jungle Clear");
            OrbMenu.Add("useQjungleclear", new CheckBox("Use Q", false));
            OrbMenu.Add("useWjungleclear", new CheckBox("Use W", true));
            OrbMenu.Add("jungleMana", new Slider("Minimum mana %", 40, 0, 100));
            OrbMenu.AddSeparator();
            #endregion

            #region Spell Menu
            SpellMenu = Menu.AddSubMenu("Spell settings");
            SpellMenu.AddGroupLabel("Q");
            SpellMenu.Add("moveAssist", new CheckBox("Movement assist", true));
            SpellMenu.AddGroupLabel("W");
            SpellMenu.Add("aaReset", new CheckBox("AA Reset", true));
            SpellMenu.AddGroupLabel("E");
            SpellMenu.Add("flashE", new KeyBind("Flash E", false, KeyBind.BindTypes.HoldActive, 'T'));
            SpellMenu.Add("dashE", new CheckBox("Dash E", true));
            SpellMenu.Add("interruptE", new CheckBox("Interrupt E", true));
            SpellMenu.Add("towerE", new CheckBox("Tower E", true));
            SpellMenu.AddGroupLabel("R");
            SpellMenu.Add("flashR", new KeyBind("Flash R", false, KeyBind.BindTypes.HoldActive, 'G'));
            SpellMenu.Add("autoR", new KeyBind("Enable Auto R", true, KeyBind.BindTypes.PressToggle, 'Z'));
            SpellMenu.Add("saveRMana", new CheckBox("Save mana for R", true));
            SpellMenu.Add("unneR", new CheckBox("Don't use if unnecessary", true));
            //SpellMenu.Add("dieR", new CheckBox("Use R before die", true));
            SpellMenu.AddGroupLabel("Ignite");
            SpellMenu.Add("useIgnite", new CheckBox("Use Ignite", true));
            SpellMenu.Add("1tick", new CheckBox("Instant Kill", true));
            SpellMenu.Add("igniteTick", new Slider("Ignite calculation tick", 4, 1, 5));    // 다이나믹 슬라이더
            #endregion
            
            #region Draw Menu
            DrawMenu = Menu.AddSubMenu("Drawing");
            DrawMenu.AddGroupLabel("Spell Range");
            DrawMenu.Add("drawQ", new CheckBox("Draw Q", true));
            DrawMenu.Add("drawE", new CheckBox("Draw E", true));
            DrawMenu.Add("drawR", new CheckBox("Draw R", false));
            DrawMenu.AddSeparator();
            DrawMenu.Add("drawFlashE", new CheckBox("Draw Flash E", false));
            DrawMenu.Add("drawFlashR", new CheckBox("Draw Flash R", false));
            DrawMenu.AddSeparator();
            DrawMenu.Add("drawText", new CheckBox("Draw Text", true));
            #endregion

            if (Menu["debug"].Cast<CheckBox>().CurrentValue)
            {
                #region Debug Menu
                DebugMenu = Menu.AddSubMenu("Debug", "Debug");
                DebugMenu.AddGroupLabel("Drawing");
                DebugMenu.Add("ePosPred", new CheckBox("E position prediction", true));
                DebugMenu.AddSeparator();
                DebugMenu.AddGroupLabel("HUD");
                DebugMenu.Add("hud", new CheckBox("Show hud", true));
                DebugMenu.AddSeparator();
                DebugMenu.Add("hudGeneral", new CheckBox("General properties", true));
                DebugMenu.Add("hudHealth", new CheckBox("Health properties", false));
                DebugMenu.Add("hudPrediction", new CheckBox("Prediction properties", false));
                DebugMenu.Add("hudDamage", new CheckBox("Damage properties", false));
                DebugMenu.Add("hudUltimateOutPut", new CheckBox("UltimateOutPut properties", false));
                DebugMenu.Add("hudTarget", new CheckBox("Target properties", false));
                #endregion
            }
        }

        public static void Initialize()
        {

        }
    }
}
