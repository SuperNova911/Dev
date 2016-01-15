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

namespace DatDarius
{
    public static class Utility
    {
        public static bool HasSpellShield(this AIHeroClient unit)
        {
            return unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield);
        }

        public static Vector3 DirectionVector(AIHeroClient unit)
        {
            Vector3 Kappa;

            Kappa.X = (unit.Direction.X * (float)Math.Cos(90 * Math.PI / 180)) - (unit.Direction.Y * (float)Math.Sin(90 * Math.PI / 180));
            Kappa.Y = (unit.Direction.X * (float)Math.Sin(90 * Math.PI / 180)) - (unit.Direction.Y * (float)Math.Cos(90 * Math.PI / 180));
            Kappa.Z = 0;

            return Kappa;
        }
        
        public static Vector3 UnitVector(Vector3 vec3)
        {
            Vector3 Kappa;

            var length = Math.Sqrt(Math.Pow(vec3.X, 2) + Math.Pow(vec3.Y, 2) + Math.Pow(vec3.Z, 2));
            Kappa.X = vec3.X / (float)length;
            Kappa.Y = vec3.Y / (float)length;
            Kappa.Z = vec3.Z / (float)length;

            return Kappa;
        }

        public static Vector3 PositionPrediction(AIHeroClient unit, float sec)
        {
            if (!unit.IsMoving)
                return unit.ServerPosition;

            Vector3 Kappa;

            Kappa = unit.Position + UnitVector(DirectionVector(unit)) * unit.MoveSpeed * sec;

            return Kappa;
        }

        public static float MordeShield(this AIHeroClient unit)
        {
            if (unit.BaseSkinName == "Mordekaiser")
                return unit.Mana;
            else
                return 0;
        }

        public static int BuffCount(this Obj_AI_Base unit, string buffName)
        {
            return unit.HasBuff(buffName) ? unit.GetBuffCount(buffName) : 0;
        }

        public static float BuffRemainTime(this Obj_AI_Base unit, string buffName)
        {
            return unit.HasBuff(buffName)
                ? unit.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                  .Where(buff => buff.Name == buffName)
                  .Select(buff => buff.EndTime)
                  .FirstOrDefault() - Game.Time
                : 0;
        }
    }
}
