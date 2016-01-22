using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Linq;
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
            if (Player.Instance.IsDead || !Config.Menu["damageIndicator"].Cast<CheckBox>().CurrentValue)
                return;

            foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsHPBarRendered && e.VisibleOnScreen && e.IsValidTarget()))
            {
                var pos = new Vector2(enemy.HPBarPosition.X + _xOffset, enemy.HPBarPosition.Y + _yOffset);
                var fullbar = (_barLength) * (enemy.HealthPercent / 100);

                var damage = (_barLength) *
                                 ((enemy.RDamage() / enemy.MaxHealth) > 1
                                     ? 1
                                     : (enemy.RDamage() / enemy.MaxHealth));
                var fulldamage = (_barLength) *
                                 ((enemy.RDamage(5) / enemy.MaxHealth) > 1
                                     ? 1
                                     : (enemy.RDamage(5) / enemy.MaxHealth));

                UltimateOutPut result = enemy.GetResult();

                Line.DrawLine(
                    result.IsKillable ? Color.FromArgb(220, Color.Red) : Color.FromArgb(220, Color.DarkRed),
                    9f, 
                    new Vector2(pos.X, pos.Y),
                    new Vector2((float)(pos.X + (damage > fullbar ? fullbar : damage)), pos.Y));
                Line.DrawLine(Color.FromArgb(220, Color.Orange), 
                    9f,
                    new Vector2((float)(pos.X + (damage > fullbar ? fullbar : damage)), pos.Y),
                    new Vector2((float)(pos.X + (fulldamage > fullbar ? fullbar : fulldamage)), pos.Y));

                if (result.Unnecessary && Config.DrawMenu["drawText"].Cast<CheckBox>().CurrentValue && Config.Menu["drawing"].Cast<CheckBox>().CurrentValue)
                    Drawing.DrawText(enemy.Position.WorldToScreen() + new Vector2(-20, 10), Color.Orange, "No Ult", 10);
            }
        }
    }
}
