using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Linq;

namespace TimerBuddy
{
    public static class MenuUtlility
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
            try
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
            catch (Exception e)
            {
                e.ErrorMessage("ADD_COLOR_ITEM" + uniqueId);
            }    
        }

        public static Color GetColor(this Menu m, string id)
        {
            try
            {
                var number = m[id].Cast<Slider>();
                if (number != null)
                {
                    return Colors[number.CurrentValue];
                }
                return Color.White;
            }
            catch (Exception e)
            {
                e.ErrorMessage("GET_COLOR", id);
                return Color.White;
            }
        }


        private static readonly DrawType[] DrawTypeList =
        {
            DrawType.Default, DrawType.HPLine, DrawType.Number, DrawType.NumberLine
        };

        private static readonly string[] DrawTypeName =
        {
            "Default", "Simple Line under HP bar", "Timer at position", "Timer and TimeBar at position"
        };

        public static void AddDrawTypeItem(this Menu menu, string uniqueId, string showName, int defaultLevel = 0)
        {
            try
            {
                var a = menu.Add(uniqueId, new ComboBox(showName, defaultLevel, DrawTypeName));
            }
            catch (Exception e)
            {
                e.ErrorMessage("ADD_DRAWTYPE_ITEM", uniqueId);
            }
        }

        public static DrawType GetDrawType(this Menu m, string id)
        {
            try
            {
                var number = m[id].Cast<ComboBox>();
                if (number != null)
                {
                    return DrawTypeList[number.CurrentValue];
                }
                return DrawType.Default;
            }
            catch (Exception e)
            {
                e.ErrorMessage("GET_DRAWTYPE", id);
                return DrawType.Default;
            }
        }
        

        private static readonly Importance[] ImportanceList =
        {
            Importance.Low, Importance.Medium, Importance.High, Importance.VeryHigh
        };

        private static readonly string[] ImportanceName =
        {
            "Low", "Medium", "High", "Very High"
        };

        public static void AddImportanceItem(this Menu menu, string uniqueId, string showName, int defaultLevel = 1)
        {
            try
            {
                var a = menu.Add(uniqueId, new ComboBox(showName, defaultLevel, ImportanceName));
            }
            catch (Exception e)
            {
                e.ErrorMessage("ADD_IMPORTANCE_ITEM", uniqueId);
            }
        }

        public static Importance GetImportance(this Menu m, string id)
        {
            try
            {
                var number = m[id].Cast<ComboBox>();
                if (number != null)
                {
                    return ImportanceList[number.CurrentValue];
                }
                return Importance.Medium;
            }
            catch (Exception e)
            {
                e.ErrorMessage("GET_IMPORTANCE", id);
                return Importance.Medium;

            }
        }



        public static void AddCheckBox(this Menu menu, string uid, string displayName, bool defaultvalue)
        {
            try
            {
                menu.Add(uid, new CheckBox(displayName, defaultvalue));
            }
            catch (Exception e)
            {
                e.ErrorMessage("ADD_CHECKBOX", uid + displayName);
            }
        }

        public static void AddSlider(this Menu menu, string uid, string displayName, int a, int b, int c)
        {
            try
            {
                menu.Add(uid, new Slider(displayName, a, b, c));
            }
            catch (Exception e)
            {
                e.ErrorMessage("ADD_SLIDER", uid + displayName);
            }
        }

        public static void AddBlank(this Menu menu, string uid)
        {
            try
            {
                menu.Add(uid, new CheckBox("Blank"));
                menu[uid].Cast<CheckBox>().IsVisible = false;
            }
            catch (Exception e)
            {
                e.ErrorMessage("ADD_BLANK", menu.DisplayName + uid);
            }
        }

        public static bool CheckboxValue(this Menu menu, string uid)
        {
            try
            {
                if (menu[uid].Cast<CheckBox>().CurrentValue)
                    return true;

                return false;
            }
            catch (Exception e)
            {
                e.ErrorMessage("CHECKBOX_VALUE", uid);
                return false;
            }
        }

        public static int SliderValue(this Menu menu, string uid)
        {
            try
            {
                return menu[uid].Cast<Slider>().CurrentValue;
            }
            catch (Exception e)
            {
                e.ErrorMessage("SLIDER_VALUE", uid);
                return 0;
            }
        }

        public static int ComboBoxValue(this Menu menu, string uid)
        {
            try
            {
                return menu[uid].Cast<ComboBox>().CurrentValue;
            }
            catch (Exception e)
            {
                e.ErrorMessage("COMBOBOX_VALUE", uid);
                return 0;
            }
        }

    }
}
