using EloBuddy;
using EloBuddy.SDK;

namespace DatDarius
{
    public class HealHandler
    {
        public class Potion
        {
            /// <summary>
            /// 대상이 포션이 있는지 확인.
            /// </summary>
            /// <param name="unit">대상.</param>
            /// <returns><c>0</c> 포션 없음, <c>1</c> Health Portion, <c>2</c> Biscuit, <c>3</c> Refillable Portion, <c>4</c> Hunter's Portion, <c>5</c> Corrupting Portion.</returns>
            public static int HasPotion(AIHeroClient unit)
            {
                if (Item.HasItem(ItemId.Health_Potion, unit))
                    return 1;
                else if (Item.HasItem(ItemId.Rejuvenation_Bead, unit))
                    return 2;
                else if (Item.HasItem(2031, unit))
                    return 3;
                else if (Item.HasItem(2032, unit))
                    return 4;
                else if (Item.HasItem(2033, unit))
                    return 5;
                else
                    return 0;
            }

            /// <summary>
            /// 대상이 포션으로 체력을 회복하고 있는지 확인.
            /// </summary>
            /// <param name="unit">대상.</param>
            /// <returns>true == 회복 중.</returns>
            public static bool IsHealing(AIHeroClient unit)
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

            /// <summary>
            /// 포션을 가진 대상이 포션으로 초당 얼마의 체력을 회복할수 있는지 계산.
            /// </summary>
            /// <param name="unit">대상.</param>
            /// <returns>초당 회복량.</returns>
            public static double HealTick(AIHeroClient unit)
            {
                switch (HasPotion(unit))
                {
                    case 0:
                        return 0;
                    case 1:
                        return unit.HasBuff("RegenerationPotion") ? 0 : 150 / 15;
                    case 2:
                        return unit.HasBuff("ItemMiniRegenPotion") ? 0 : (150 / 15) + 20;
                    case 3:
                        return unit.HasBuff("ItemCrystalFlask") ? 0 : 125 / 12;
                    case 4:
                        return unit.HasBuff("ItemCrystalFlaskJungle") ? 0 : 60 / 8;
                    case 5:
                        return unit.HasBuff("ItemDarkCrystalFlask") ? 0 : 150 / 12;
                }
                return 0;
            }

            /// <summary>
            /// 대상이 몇 초간 얼마나 체력을 회복할지 계산.
            /// </summary>
            /// <param name="unit">대상.</param>
            /// <param name="second">시간(s).</param>
            /// <returns>몇 초간 얻는 회복량.</returns>
            public static double GetHeal(AIHeroClient unit, int second)
            {
                if (unit.HasBuff("RegenerationPotion"))
                {
                    if (unit.GetBuff("RegenerationPotion").EndTime - Game.Time < second)
                        return (150 / 15) * unit.GetBuff("RegenerationPotion").EndTime - Game.Time;
                    else
                        return (150 / 15) * second;
                }
                else if (unit.HasBuff("ItemMiniRegenPotion"))
                {
                    if (unit.GetBuff("ItemMiniRegenPotion").EndTime - Game.Time < second)
                        return (150 / 15) * unit.GetBuff("ItemMiniRegenPotion").EndTime - Game.Time;
                    else
                        return (150 / 15) * second;
                }
                else if (unit.HasBuff("ItemCrystalFlask"))
                {
                    if (unit.GetBuff("ItemCrystalFlask").EndTime - Game.Time < second)
                        return (125 / 12) * unit.GetBuff("ItemCrystalFlask").EndTime - Game.Time;
                    else
                        return (125 / 12) * second;
                }
                else if (unit.HasBuff("ItemCrystalFlaskJungle"))
                {
                    if (unit.GetBuff("ItemCrystalFlaskJungle").EndTime - Game.Time < second)
                        return (60 / 8) * unit.GetBuff("ItemCrystalFlaskJungle").EndTime - Game.Time;
                    else
                        return (60 / 8) * second;

                }
                else if (unit.HasBuff("ItemDarkCrystalFlask"))
                {
                    if (unit.GetBuff("ItemDarkCrystalFlask").EndTime - Game.Time < second)
                        return (150 / 12) * unit.GetBuff("ItemDarkCrystalFlask").EndTime - Game.Time;
                    else
                        return (150 / 12) * second;

                }
                else
                    return 0;
            }
        }
    }
}
