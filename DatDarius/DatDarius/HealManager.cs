using EloBuddy;
using EloBuddy.SDK;

namespace DatDarius
{
    public enum Potion
    {
        NoPotion, HealthPotion, Biscuit, RefillablePotion, HuntersPotion, CorruptingPotion
    }

    public static class HealManager
    {
        public static Potion HasPotion(this AIHeroClient unit)
        {
            if (Item.HasItem(ItemId.Health_Potion, unit))
                return Potion.HealthPotion;
            else if (Item.HasItem(ItemId.Rejuvenation_Bead, unit))
                return Potion.Biscuit;
            else if (Item.HasItem(2031, unit))
                return Potion.RefillablePotion;
            else if (Item.HasItem(2032, unit))
                return Potion.HuntersPotion;
            else if (Item.HasItem(2033, unit))
                return Potion.CorruptingPotion;
            else
                return Potion.NoPotion;
        }

        public static bool IsUsingPotion(this AIHeroClient unit)
        {
            if (unit.HasBuff("RegenerationPotion") ||
                unit.HasBuff("ItemMiniRegenPotion") ||
                unit.HasBuff("ItemCrystalFlask") ||
                unit.HasBuff("ItemCrystalFlaskJungle") ||
                unit.HasBuff("ItemDarkCrystalFlask"))
                return true;
            else
                return false;
        }

        public static float HealTick(this AIHeroClient unit, int second)
        {
            switch (unit.HasPotion())
            {
                case Potion.NoPotion:
                    return 0;
                case Potion.HealthPotion:
                    return 150 / 15 * second;
                case Potion.Biscuit:
                    return 150 / 15 * second + 20;
                case Potion.RefillablePotion:
                    return 125 / 12 * second;
                case Potion.HuntersPotion:
                    return 60 / 8 * second;
                case Potion.CorruptingPotion:
                    return 150 / 12 * second;
                default:
                    return 0;
            }
        }
        
        public static float GetHeal(this AIHeroClient unit, int second)
        {
            if (unit.HasBuff("RegenerationPotion"))
            {
                if (unit.BuffRemainTime("RegenerationPotion") < second)
                    return 150 / 15 * unit.BuffRemainTime("RegenerationPotion");
                else
                    return 150 / 15 * second;
            }
            else if (unit.HasBuff("ItemMiniRegenPotion"))
            {
                if (unit.BuffRemainTime("ItemMiniRegenPotion") < second)
                    return 150 / 15 * unit.BuffRemainTime("ItemMiniRegenPotion");
                else
                    return 150 / 15 * second;
            }
            else if (unit.HasBuff("ItemCrystalFlask"))
            {
                if (unit.BuffRemainTime("ItemCrystalFlask") < second)
                    return 125 / 12 * unit.BuffRemainTime("ItemCrystalFlask");
                else
                    return 125 / 12 * second;
            }
            else if (unit.HasBuff("ItemCrystalFlaskJungle"))
            {
                if (unit.BuffRemainTime("ItemCrystalFlaskJungle") < second)
                    return 60 / 8 * unit.BuffRemainTime("ItemCrystalFlaskJungle");
                else
                    return 60 / 8 * second;
            }
            else if (unit.HasBuff("ItemDarkCrystalFlask"))
            {
                if (unit.BuffRemainTime("ItemDarkCrystalFlask") < second)
                    return 150 / 12 * unit.BuffRemainTime("ItemDarkCrystalFlask");
                else
                    return 150 / 12 * second;
            }
            else
                return 0;
        }

        public static float HPRegenRate(this AIHeroClient unit, int second, bool potionPrediction)
        {
            if (unit.IsUsingPotion())
                return unit.HPRegenRate * second + unit.GetHeal(second);

            if (potionPrediction)
                return unit.HPRegenRate * second + unit.HealTick(second);

            return unit.HasBuff("SummonerIgnite") ? unit.HPRegenRate * second * 0.6f : unit.HPRegenRate * second;
        }
    }
}
