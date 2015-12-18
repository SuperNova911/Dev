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
        /// <param name="target">대상.</param>
        /// <returns>R 데미지.</returns>
        public static double RDamage(Obj_AI_Base target)
        {
            if (!SpellManager.R.IsReady())
                return 0;

            var BuffCount = target.HasBuff("dariushemo") ? target.GetBuffCount("dariushemo") : 0;
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

        public static float PassiveDamage(Obj_AI_Base target)
        {
            var BuffCount = target.HasBuff("dariushemo") ? target.GetBuffCount("dariushemo") : 0;
            var BuffRemainTime = target.HasBuff("dariushemo") ? target.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                    .Where(buff => buff.Name == "dariushemo")
                    .Select(buff => buff.EndTime)
                    .FirstOrDefault() - Game.Time
                    : 0;
            var TickDamage = (float)(((9 + Player.Instance.Level) + (0.3 * Player.Instance.FlatPhysicalDamageMod)) / 5);

            return Damage.CalculateDamageOnUnit(Player.Instance, target, DamageType.Physical, (TickDamage * BuffRemainTime) * BuffCount, false, false);
        }
    }
}
