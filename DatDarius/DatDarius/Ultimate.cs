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
        public bool IsKillable = false;

        public bool LetItGo = false;

        public bool IsInRange = false;

        public bool IsInFlashRange = false;

        public bool IsAlone = false;

        public bool Unnecessary = false;

        public bool LanePhase = false;
    }

    public static class Ultimate
    {
        public static UltimateOutPut GetResult(this AIHeroClient unit)
        {
            UltimateOutPut result = null;

            float Health = unit.Health;
            float Shield = unit.AllShield + unit.MordeShield();
            float HPRegenRate = unit.HPRegenRate;
            float PotionHealAmount = unit.PotionHeal(3);
            int Stack = unit.BuffCount("dariushemo");
            int 오차 = Config.SpellMenu["오차"].Cast<Slider>().CurrentValue;

            if (!unit.IsValidTarget(SpellManager.R.Range + SpellManager.Flash.Range) && unit.HasSpellShield() && unit.HasBuff("kindredrnodeathbuff"))
                return result;

            if (Player.Instance.CountEnemiesInRange(1500) == 1)
                result.IsAlone = true;

            if (Player.Instance.CountAlliesInRange(1500) == 1 && Player.Instance.CountEnemiesInRange(1500) == 1)
                result.LanePhase = true;

            if (Player.Instance.HealthPercent >= 75 && unit.HealthPercent <= 25 && result.IsAlone && SpellManager.R.Level < 3)
                result.Unnecessary = true;

            if (unit.RDamage() > Health + Shield + HPRegenRate + 오차)
            {
                result.IsKillable = true;

                if (unit.IsValidTarget(SpellManager.R.Range))
                    result.IsInRange = true;

                if (unit.IsValidTarget(SpellManager.R.Range + SpellManager.Flash.Range))
                    result.IsInFlashRange = true;
            }

            if (unit.RDamage() + unit.PassiveDamage(Stack == 5 ? Stack : Stack + 1, 3) > Health + Shield + PotionHealAmount + (HPRegenRate - unit.GetHeal(1)) * 3 && result.IsAlone)
                result.LetItGo = true;
            
            return result;
        }

        public static float PotionHeal(this AIHeroClient unit, int second)
        {
            if (unit.IsUsingPotion())
                return unit.GetHeal(second);

            if (unit.HasPotion() != Potion.NoPotion)
                return unit.HealTick(second);

            return 0;
        }
    }
}
