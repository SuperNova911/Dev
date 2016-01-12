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
    public class UltimateOutPut
    {
        public bool Targetable = false;
        
        public bool IsKillable = false;

        public bool LetItGo = false;

        public bool InRange = false;

        public bool InFlashRange = false;

        public bool Alone = false;

        public bool UnnecessaryR = false;
    }

    public class Ultimate
    {
        public static UltimateOutPut GetResult(AIHeroClient unit)
        {
            UltimateOutPut result = null;

            var Health = unit.Health;
            var Shield = unit.AllShield + unit.MordeShield();
            double HPRegenRate = unit.HPRegenRate;
            double PotionHeal = 0;  //이 부분 힐 매니저에서 새로 짜기
            if (unit.IsHealing())
                    PotionHeal = HealHandler.GetHeal(unit, 3);
            if (unit.HasPotion() != 0 && !unit.IsHealing())
                PotionHeal = HealHandler.HealTick(unit) * 3;

            int Stack = unit.BuffCount("dariushemo");

            if (unit.IsValidTarget(SpellManager.R.Range + SpellManager.Flash.Range) && !unit.HasSpellShield() && !unit.HasBuff("kindredrnodeathbuff"))
                result.Targetable = true;
            if (Player.Instance.CountEnemiesInRange(1500) == 1)
                result.Alone = true;
            if (Player.Instance.HealthPercent >= 75 && unit.HealthPercent <= 25 && result.Alone && SpellManager.R.Level < 3)
                result.UnnecessaryR = true;

            if (unit.RDamage() > Health + Shield + HPRegenRate)
            {
                if (unit.IsValidTarget(SpellManager.R.Range))
                    result.InRange = true;
                else if (unit.IsValidTarget(SpellManager.R.Range + SpellManager.Flash.Range))
                    result.InFlashRange = true;
            }
            else if (unit.RDamage() + unit.PassiveDamage(Stack == 5 ? Stack : Stack + 1, 3) > Health + Shield + (unit.FlatHPRegenMod * 3) + PotionHeal)
                result.LetItGo = true;

            return result;
        }
    }
}
