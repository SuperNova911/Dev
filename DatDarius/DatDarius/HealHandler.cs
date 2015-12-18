using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			/// <param name="target">대상.</param>
			/// <returns><c>0</c> 포션 없음, <c>1</c> Health Portion, <c>2</c> Biscuit, <c>3</c> Refillable Portion, <c>4</c> Hunter's Portion, <c>5</c> Corrupting Portion.</returns>
			public static int HasPotion(AIHeroClient target)
			{
				if (Item.HasItem(ItemId.Health_Potion, target))
					return 1;
				else if (Item.HasItem(ItemId.Rejuvenation_Bead, target))
					return 2;
				else if (Item.HasItem(2031, target))
					return 3;
				else if (Item.HasItem(2032, target))
					return 4;
				else if (Item.HasItem(2033, target))
					return 5;
				else
					return 0;
			}

			/// <summary>
			/// 포션을 가진 대상이 포션으로 초당 얼마의 체력을 회복할수 있는지 계산.
			/// </summary>
			/// <param name="target">대상.</param>
			/// <returns>초당 회복량.</returns>
			public static double HealTick(AIHeroClient target)
			{
				switch (HasPotion(target))
				{
					case 0:
						return 0;
					case 1:
						return target.HasBuff("RegenerationPotion") ? 0 : 150 / 15;
					case 2:
						return target.HasBuff("ItemMiniRegenPotion") ? 0 : 150 / 15;
					case 3:
						return target.HasBuff("ItemCrystalFlask") ? 0 : 125 / 12;
					case 4:
						return target.HasBuff("ItemCrystalFlaskJungle") ? 0 : 60 / 8;
					case 5:
						return target.HasBuff("ItemDarkCrystalFlask") ? 0 : 150 / 12;
				}
				return 0;
			}

			/// <summary>
			/// 대상이 몇 초간 얼마나 체력을 회복할지 계산.
			/// </summary>
			/// <param name="target">대상.</param>
			/// <param name="second">시간(s).</param>
			/// <returns>몇 초간 얻는 회복량.</returns>
			public static double GetHeal(AIHeroClient target, int second)
			{
				if (target.HasBuff("RegenerationPotion"))
				{
					if (target.GetBuff("RegenerationPotion").EndTime - Game.Time < second)
						return (150 / 15) * target.GetBuff("RegenerationPotion").EndTime - Game.Time;
					else
						return (150 / 15) * second;
				}
				else if (target.HasBuff("ItemMiniRegenPotion"))
				{
					if (target.GetBuff("ItemMiniRegenPotion").EndTime - Game.Time < second)
						return (150 / 15) * target.GetBuff("ItemMiniRegenPotion").EndTime - Game.Time;
					else
						return (150 / 15) * second;
				}
				else if (target.HasBuff("ItemCrystalFlask"))
				{
					if (target.GetBuff("ItemCrystalFlask").EndTime - Game.Time < second)
						return (125 / 12) * target.GetBuff("ItemCrystalFlask").EndTime - Game.Time;
					else
						return (125 / 12) * second;
				}
				else if (target.HasBuff("ItemCrystalFlaskJungle"))
				{
					if (target.GetBuff("ItemCrystalFlaskJungle").EndTime - Game.Time < second)
						return (60 / 8) * target.GetBuff("ItemCrystalFlaskJungle").EndTime - Game.Time;
					else
						return (60 / 8) * second;

				}
				else if (target.HasBuff("ItemDarkCrystalFlask"))
				{
					if (target.GetBuff("ItemDarkCrystalFlask").EndTime - Game.Time < second)
						return (150 / 12) * target.GetBuff("ItemDarkCrystalFlask").EndTime - Game.Time;
					else
						return (150 / 12) * second;

				}
				else
					return 0;
			}
		}
	}
}
