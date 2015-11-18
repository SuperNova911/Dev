using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;

namespace ChallengerJinx
{
    public class Jinx
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Jinx")
            {
                return;
            }

            Config.Initialize();
            ModeManager.Initialize();

            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            if (Config.Drawing.DrawAA)
            {
                new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = 575 }.Draw(Player.Instance.Position);
            }
            if (Config.Drawing.DrawQ)
            {
                if (SpellManager.Q.Cast())
                {
                    new Circle { Color = Color.Purple, BorderWidth = 4, Radius = 525 }.Draw(Player.Instance.Position);
                }
                else
                {
                    new Circle { Color = Color.Purple, BorderWidth = 4, Radius = SpellManager.Q.Range }.Draw(Player.Instance.Position);
                }
            }
            if (Config.Drawing.DrawW)
            {
                new Circle { Color = Color.Pink, BorderWidth = 4, Radius = 1450 }.Draw(Player.Instance.Position);
            }
            if (Config.Drawing.DrawE)
            {
                new Circle { Color = Color.Orange, BorderWidth = 4, Radius = 900 }.Draw(Player.Instance.Position);
            }
            if (Config.Drawing.DrawR)
            {
                new Circle { Color = Color.LightSkyBlue, BorderWidth = 4, Radius = 3000 }.Draw(Player.Instance.Position);
            }
        }
    }
}
