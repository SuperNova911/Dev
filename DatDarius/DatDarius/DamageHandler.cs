using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace DatDarius
{
    public class DamageHandler
    {
        /// <summary>
        /// R 데미지 계산.
        /// </summary>
        /// <param name="unit">대상.</param>
        /// <returns>R 데미지.</returns>
        public static double RDamage(Obj_AI_Base unit)
        {
            if (!SpellManager.R.IsReady())
                return 0;

            var BuffCount = unit.HasBuff("dariushemo") ? unit.GetBuffCount("dariushemo") : 0;
            var Damage = (new double[] { 100, 200, 300 }[SpellManager.R.Level - 1] + (0.75 * ObjectManager.Player.FlatPhysicalDamageMod)) * (1 + 0.2 * BuffCount);

            return Damage;
        }

        /// <summary>
        /// 점화 데미지 계산.
        /// </summary>
        /// <param name="TotalDamage">true = 총 데미지, false = 1초 데미지.</param>
        /// <returns>점화 1초 또는 총 데미지.</returns>
        public static int IgniteDamage(bool TotalDamage)
        {
            return TotalDamage ? 50 + (20 * Player.Instance.Level) : 50 + (20 * Player.Instance.Level) / 5;
        }

        /// <summary>
        /// 패시브 데미지 계산.
        /// </summary>
        /// <param name="unit">대상.</param>
        /// <returns>앞으로 들어갈 패시브 데미지.</returns>
        public static float PassiveDamage(Obj_AI_Base unit)
        {
            var BuffCount = unit.HasBuff("dariushemo") ? unit.GetBuffCount("dariushemo") : 0;
            var BuffRemainTime = unit.HasBuff("dariushemo") ? unit.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                    .Where(buff => buff.Name == "dariushemo")
                    .Select(buff => buff.EndTime)
                    .FirstOrDefault() - Game.Time
                    : 0;
            var TickDamage = (float)(((9 + Player.Instance.Level) + (0.3 * Player.Instance.FlatPhysicalDamageMod)) / 5);

            return Damage.CalculateDamageOnUnit(Player.Instance, unit, DamageType.Physical, (TickDamage * BuffRemainTime) * BuffCount, false, false);
        }

        public static float PassiveDamage(Obj_AI_Base unit, int stack, int second)
        {
            var damage = (float)((((9 + Player.Instance.Level) + (0.3 * Player.Instance.FlatPhysicalDamageMod)) / 5) * stack * second) ;

            return Damage.CalculateDamageOnUnit(Player.Instance, unit, DamageType.Physical, damage, false, false);
        }

        public static int Ultimate(AIHeroClient unit)
        {
            var Health = unit.Health;
            var Shield = unit.AllShield + MoShield(unit);
            double HPRegenRate = unit.HPRegenRate;

            if (!unit.IsValidTarget(SpellManager.R.Range + SpellManager.Flash.Range) || Darius.HasSpellShield(unit) || unit.HasBuff("kindredrnodeathbuff"))
                return 0;

            if (RDamage(unit) > Health + Shield + HPRegenRate)
            {
                if (unit.IsValidTarget(SpellManager.R.Range))
                    return 1;
                else if (unit.IsValidTarget(SpellManager.R.Range + SpellManager.Flash.Range))
                    return 2;
            }


            return 0;
        }

        public static float MoShield(AIHeroClient unit)
        {
            if (unit.BaseSkinName == "Mordekaiser")
                return unit.Mana;
            else
                return 0;
        }
    }
}
