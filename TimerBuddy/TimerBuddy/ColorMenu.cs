using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TimerBuddy
{
    public static class ColorMenu
    {
        static Font TestFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 30));

        private static readonly Color[] Colors =
        {
            Color.White, Color.WhiteSmoke, Color.Gainsboro, Color.LightGray, Color.DarkGray,
            Color.DimGray, Color.Black, Color.DarkRed, Color.Firebrick, Color.Brown,
            Color.Crimson, Color.Red, Color.OrangeRed, Color.DarkOrange, Color.Orange,
            Color.Goldenrod, Color.Gold, Color.Yellow, Color.Olive, Color.YellowGreen,
            Color.OliveDrab, Color.Green, Color.ForestGreen, Color.LimeGreen, Color.Lime,
            Color.LawnGreen, Color.Chartreuse, Color.MediumSpringGreen, Color.SpringGreen, Color.Cyan,
            Color.Aqua, Color.Aquamarine, Color.DeepSkyBlue, Color.SkyBlue, Color.LightSkyBlue,
            Color.DodgerBlue, Color.RoyalBlue, Color.Blue, Color.MediumBlue, Color.DarkBlue,
            Color.DarkSlateBlue, Color.MediumSlateBlue, Color.MediumPurple, Color.BlueViolet, Color.DarkOrchid,
            Color.Purple, Color.Fuchsia, Color.Magenta, Color.DeepPink, Color.Violet,
            Color.HotPink
        };

        private static readonly string[] ColorsName =
        {
            "White", "WhiteSmoke", "Gainsboro", "LightGray", "DarkGray", "DimGray", "Black", 
            "DarkRed", "Firebrick", "Brown", "Crimson", "Red", "OrangeRed",
            "DarkOrange", "Orange", "Goldenrod", "Gold", "Yellow", "Olive", "YellowGreen",
            "OliveDrab", "Green", "ForestGreen", "LimeGreen", "Lime", "LawnGreen",
            "Chartreuse", "MediumSpringGreen", "SpringGreen", "Cyan", "Aqua", "Aquamarine",
            "DeepSkyBlue", "SkyBlue", "LightSkyBlue", "DodgerBlue", "RoyalBlue",
            "Blue", "MediumBlue", "DarkBlue", "DarkSlateBlue", "MediumSlateBlue",
            "MediumPurple", "BlueViolet", "DarkOrchid", "Purple", "Fuchsia", "Magenta",
            "DeepPink", "Violet", "HotPink"
        };

        public static void AddColorItem(this Menu menu, string uniqueId, int defaultColour = 0)
        {
            var a = menu.Add(uniqueId, new Slider("Color Picker: ", defaultColour, 0, Colors.Count() - 1));
            a.DisplayName = "Color Picker: " + ColorsName[a.CurrentValue];
            a.OnValueChange += delegate 
            {
                var t = 2000 + Utility.TickCount;
                a.DisplayName = "Color Picker: " + ColorsName[a.CurrentValue];
                var color = menu.GetColor(uniqueId).ConvertColor();
                Drawing.OnEndScene += delegate
                {
                    if (t >= Utility.TickCount)
                    {
                        Drawing.DrawLine(new Vector2(200, 195), new Vector2(200, 305), 110, System.Drawing.Color.Black);
                        Drawing.DrawLine(new Vector2(200, 200), new Vector2(200, 300), 100, color);
                        TestFont.DrawText(null, "Kappa123", 115, 320, menu.GetColor(uniqueId));
                    }
                };
            };            
        }

        public static Color GetColor(this Menu m, string id)
        {
            var number = m[id].Cast<Slider>();
            if (number != null)
            {
                return Colors[number.CurrentValue];
            }
            return Color.White;
        }


        private static readonly DrawType[] DrawTypeList =
        {
            DrawType.Default, DrawType.HPLine, DrawType.Number, DrawType.NumberLine
        };

        private static readonly string[] DrawTypeName =
        {
            "Default", "Under HP bar", "Timer at position", "Timer and TimeBar at position"
        };

        public static void AddDrawTypeItem(this Menu menu, string uniqueId, int defaultLevel = 0)
        {
            var a = menu.Add(uniqueId, new ComboBox("Drawing Style: ", defaultLevel, DrawTypeName));
        }

        public static DrawType GetDrawType(this Menu m, string id)
        {
            var number = m[id].Cast<ComboBox>();
            if (number != null)
            {
                return DrawTypeList[number.CurrentValue];
            }
            return DrawType.Default;
        }


        private static readonly Importance[] ImportanceList =
        {
            Importance.Low, Importance.Medium, Importance.High
        };

        private static readonly string[] ImportanceName =
        {
            "Low", "Medium", "High"
        };

        public static void AddImportanceItem(this Menu menu, string uniqueId, int defaultLevel = 1)
        {
            var a = menu.Add(uniqueId, new ComboBox("Importance Level: ", defaultLevel, ImportanceName));
        }

        public static Importance GetImportance(this Menu m, string id)
        {
            var number = m[id].Cast<ComboBox>();
            if (number != null)
            {
                return ImportanceList[number.CurrentValue];
            }
            return Importance.Medium;
        }
    }
}
