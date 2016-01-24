using EloBuddy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerBuddy
{
    public static class Utility
    {
        public static string GetRemainTime(this Spell list)
        {
            try
            {
                Spell database = null;
                if (list.SpellType == SpellType.Spell)
                    database = SpellDatabase.Database.FirstOrDefault(d => d.ChampionName == list.ChampionName && d.Slot == list.Slot);
                else
                    database = SpellDatabase.Database.FirstOrDefault(d => d.Name == list.Name);

                if (database == null)
                {
                    //Chat.Print("error: CODE REMAIN_TIMER KAPPA " + list.Caster.BaseSkinName);
                    //return "Kappa";
                    return list.EndTime - TickCount >= 0 ? ((list.EndTime - TickCount) / 1000f).ToString("F2") : 0f.ToString("F2");
                }
                                
                if (database.EndTime > 10000)
                    return ((list.EndTime - TickCount) / 1000 + 1).ToString();
                else
                    return list.EndTime - TickCount >= 0 ? ((list.EndTime - TickCount) / 1000f).ToString("F2") : 0f.ToString("F2");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE REMAIN_TIMER " + list.Caster.BaseSkinName);
                return "Kappa";
            }
        }

        public static int TickCount
        {
            get
            {
                return Environment.TickCount & int.MaxValue;
            }
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

        public static SharpDX.Color GetColor(this Spell spell)
        {
            return Config.SpellMenu.GetColor(spell.MenuString + "color");
        }
    }
}
