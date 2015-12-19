using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using System.Linq;
using Color = System.Drawing.Color;

namespace Path_Tracker
{
    public class Program
    {
        public static Menu Menu;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Menu = MainMenu.AddMenu("Path Tracker", "Path Tracker");
            Menu.AddGroupLabel("Drawing");
            Menu.Add("me", new CheckBox("My Path", true));
            Menu.Add("ally", new CheckBox("Ally Path", false));
            Menu.Add("enemy", new CheckBox("Enemy Path", true));
            Menu.AddSeparator();
            Menu.Add("eta", new CheckBox("Estimated time of arrival", true));
            Menu.Add("name", new CheckBox("Champion Name", true));
            Menu.Add("thick", new Slider("Line Thickness", 2, 1, 5));

            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var ETA = Menu["eta"].Cast<CheckBox>().CurrentValue;
            var Name = Menu["name"].Cast<CheckBox>().CurrentValue;
            var Thickness = Menu["thick"].Cast<Slider>().CurrentValue;

            if (Menu["me"].Cast<CheckBox>().CurrentValue)
            {
                DrawPath(Player.Instance, Thickness, Color.LawnGreen);

                if (ETA && Player.Instance.Path.Length > 1)
                {
                    Drawing.DrawText(Player.Instance.Path[Player.Instance.Path.Length - 1].WorldToScreen(), Color.NavajoWhite, GetETA(Player.Instance), 10);
                }
            }

            if (Menu["ally"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var ally in EntityManager.Heroes.Allies.Where(a => !a.IsMe && a.IsValid))
                {
                    DrawPath(ally, Thickness, Color.Orange);

                    if (ally.Path.Length > 1)
                    {
                        if (Name)
                        {
                            Drawing.DrawText(ally.Path[ally.Path.Length - 1].WorldToScreen(), Color.LightSkyBlue, ally.BaseSkinName, 10);
                        }
                        if (ETA)
                        {
                            Drawing.DrawText(ally.Path[ally.Path.Length - 1].WorldToScreen() + new Vector2(0, 20), Color.NavajoWhite, GetETA(ally), 10);
                        }
                    }
                }
            }

            if (Menu["enemy"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsValid))
                {
                    DrawPath(enemy, Thickness, Color.Red);

                    if (enemy.Path.Length > 1)
                    {
                        if (Name)
                        {
                            Drawing.DrawText(enemy.Path[enemy.Path.Length - 1].WorldToScreen(), Color.LightSkyBlue, enemy.BaseSkinName, 10);
                        }
                        if (ETA)
                        {
                            Drawing.DrawText(enemy.Path[enemy.Path.Length - 1].WorldToScreen() + new Vector2(0, 20), Color.NavajoWhite, GetETA(enemy), 10);
                        }
                    }
                }
            }
        }

        public static void DrawPath(AIHeroClient unit, int thickness, Color color)
        {
            if (unit.Path[0].Distance(unit.Path[unit.Path.Length - 1]) > 1000 && unit.Path.Length > 1)
            {
                Drawing.DrawLine(unit.Position.WorldToScreen(), Drawing.WorldToScreen(unit.Path[unit.Path.Length - 1]), thickness, color);
            }
            else
            {
                for (var i = 1; unit.Path.Length > i; i++)
                {
                    if (i < 2)
                        Drawing.DrawLine(unit.Position.WorldToScreen(), Drawing.WorldToScreen(unit.Path[i]), thickness, color);
                    else
                    {
                        Drawing.DrawLine(Drawing.WorldToScreen(unit.Path[i - 1]), Drawing.WorldToScreen(unit.Path[i]), thickness, color);
                    }
                }
            }
        }

        public static string GetETA(AIHeroClient unit)
        {
            float Distance = 0;

            if (unit.Path.Length > 1)
            {
                for (var i = 1; unit.Path.Length > i; i++)
                {
                    Distance += unit.Path[i - 1].Distance(unit.Path[i]);
                }
            }

            return (Distance / unit.MoveSpeed).ToString("F2");
        }
    }
}
