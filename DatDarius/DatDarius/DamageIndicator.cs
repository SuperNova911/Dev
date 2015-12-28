using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace DatDarius
{
    public class DamageIndicator
    {
        private readonly float _barLength = 104;
        private readonly float _xOffset = 2;
        private readonly float _yOffset = 9;

        public DamageIndicator()
        {
            Drawing.OnEndScene += Drawing_OnDraw;
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            if (Player.Instance.IsDead)
                return;

            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                if (!enemy.IsHPBarRendered || !enemy.VisibleOnScreen)
                    continue;

                var pos = new Vector2(enemy.HPBarPosition.X + _xOffset, enemy.HPBarPosition.Y + _yOffset);
                var fullbar = (_barLength) * (enemy.HealthPercent / 100);
                var damage = (_barLength) *
                                 ((DamageHandler.RDamage(enemy) / enemy.MaxHealth) > 1
                                     ? 1
                                     : (DamageHandler.RDamage(enemy) / enemy.MaxHealth));

                Line.DrawLine(Color.FromArgb(100, Color.Orange), 9f, new Vector2(pos.X, pos.Y),
                    new Vector2((float)(pos.X + (damage > fullbar ? fullbar : damage)), pos.Y));
                //Line.DrawLine(Color.Black, 9,
                //new Vector2((float)(pos.X + (damage > fullbar ? fullbar : damage) - 2), pos.Y),
                //new Vector2((float)(pos.X + (damage > fullbar ? fullbar : damage) + 2), pos.Y));
            }
        }
    }
}
