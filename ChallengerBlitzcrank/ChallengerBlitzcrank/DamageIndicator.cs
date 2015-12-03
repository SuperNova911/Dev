using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace ChallengerBlitzcrank
{
    public class DamageIndicator
    {
        private readonly float _barLength = 104;
        private readonly float _xOffset = 2;
        private readonly float _yOffset = 9;
        public float CheckDistance = 2000;

        public DamageIndicator()
        {
            Drawing.OnEndScene += Drawing_OnDraw;
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            if (!Config.Drawing.DrawDamage || Player.Instance.IsDead) return;

            foreach (var aiHeroClient in EntityManager.Heroes.Enemies)
            {
                if (!aiHeroClient.IsHPBarRendered || !aiHeroClient.VisibleOnScreen) continue;

                var pos = new Vector2(aiHeroClient.HPBarPosition.X + _xOffset, aiHeroClient.HPBarPosition.Y + _yOffset);
                var fullbar = (_barLength) * (aiHeroClient.HealthPercent / 100);
                var damage = (_barLength) *
                                 ((DamageHandler.ComboDamage(aiHeroClient) / aiHeroClient.MaxHealth) > 1
                                     ? 1
                                     : (DamageHandler.ComboDamage(aiHeroClient) / aiHeroClient.MaxHealth));

                Line.DrawLine(Color.FromArgb(100, Color.Orange), 9f, new Vector2(pos.X, pos.Y),
                    new Vector2((float)(pos.X + (damage > fullbar ? fullbar : damage)), pos.Y));
                Line.DrawLine(Color.Black, 9,
                    new Vector2((float)(pos.X + (damage > fullbar ? fullbar : damage) - 2), pos.Y),
                    new Vector2((float)(pos.X + (damage > fullbar ? fullbar : damage) + 2), pos.Y));
            }
        }
    }
}
