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
    public enum UltRange
    {
        OutOfRange, RRange, FlashRRange
    }

    public class UltimateOutPut
    {
        public bool IsKillable = false;

        public bool LetItGo = false;
        
        public bool IsAlone = false;

        public bool Unnecessary = false;

        public bool LanePhase = false;

        public UltRange Range = UltRange.OutOfRange;
    }

    public static class Ultimate
    {
        public static UltimateOutPut GetResult(this AIHeroClient unit)
        {
            UltimateOutPut result = new UltimateOutPut();

            float Health = unit.Health;
            float Shield = unit.AllShield + unit.MordeShield();
            int Stack = unit.BuffCount("dariushemo");

            if (Player.Instance.HealthPercent >= 75 && unit.HealthPercent <= 25 && result.IsAlone && SpellManager.R.Level < 3 && Config.SpellMenu["unneR"].Cast<CheckBox>().CurrentValue)
                result.Unnecessary = true;

            if (!unit.IsValidTarget(SpellManager.R.Range + SpellManager.Flash.Range) && unit.HasSpellShield() && unit.HasBuff("kindredrnodeathbuff"))
                return result;

            if (Player.Instance.CountEnemiesInRange(1500) == 1)
                result.IsAlone = true;

            if (Player.Instance.CountAlliesInRange(1500) == 1 && Player.Instance.CountEnemiesInRange(1500) == 1)
                result.LanePhase = true;

            if (unit.RDamage() > Health + Shield + unit.HPRegenRate)
            {
                result.IsKillable = true;

                if (unit.IsValidTarget(SpellManager.R.Range + SpellManager.Flash.Range) && unit.ServerPosition.Distance(Player.Instance.ServerPosition) > SpellManager.R.Range)
                    result.Range = UltRange.FlashRRange;

                if (unit.IsValidTarget(SpellManager.R.Range))
                    result.Range = UltRange.RRange;
            }

            int sec = 4;

            if (unit.RDamage() + unit.PassiveDamage(Stack == 5 ? Stack : Stack + 1, sec) + unit.IgniteDamage() > Health + Shield + unit.HPRegenRate(sec, true) && result.IsAlone)
                result.LetItGo = true;
            
            return result;
        }
    }
}
