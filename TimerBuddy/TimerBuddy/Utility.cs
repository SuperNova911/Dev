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

        public static float GetRemainTimeFloat(this Spell list)
        {
            try
            {
                if (list.Buff == true)
                    return list.BuffRemainTime();
                Spell database = null;
                if (list.SpellType == SpellType.Spell)
                    database = SpellDatabase.Database.FirstOrDefault(d => d.ChampionName == list.ChampionName && d.Slot == list.Slot);
                else
                    database = SpellDatabase.Database.FirstOrDefault(d => d.Name == list.Name);
                
                if (database == null)
                {
                    Chat.Print("error: CODE REMAIN_TIMER KAPPA " + list.Caster.BaseSkinName);
                    //return "Kappa";
                    return list.EndTime - TickCount >= 0 ? ((list.EndTime - TickCount)) : 0f;
                }

                return list.EndTime - TickCount >= 0 ? ((list.EndTime - TickCount)) : 0f;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE REMAIN_FLOAT " + list.Caster.BaseSkinName);
                return 777f;
            }
        }

        public static int TickCount
        {
            get
            {
                return Environment.TickCount & int.MaxValue;
            }
        }

        public static float BuffRemainTime(this Spell spell)
        {
            if (spell.Target.HasBuff(spell.Name))
            {
                if (spell.EndTime >= Game.Time * 1000)
                {
                    return spell.EndTime - Game.Time * 1000;
                }
                return 0;
            }
            return 0;

            /*
            return unit.HasBuff(buffName)
                ? unit.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                  .Where(buff => buff.Name == buffName)
                  .Select(buff => buff.EndTime)
                  .FirstOrDefault() - Game.Time
                : 0;*/
        }

        public static SharpDX.Color GetColor(this Spell spell)
        {
            return Config.SpellMenu.GetColor(spell.MenuString + "color");
        }

        public static System.Drawing.Color ConvertColor(SharpDX.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Spell GetDatabase(Spell spell)
        {
            try
            {
                if (spell.Buff == true)
                    return SpellDatabase.Database.FirstOrDefault(d => d.Name == spell.Name && d.Buff == true);

                switch (spell.SpellType)
                {
                    case SpellType.SummonerSpell:
                        return SpellDatabase.Database.FirstOrDefault(d => d.Name == spell.Name && d.SpellType == SpellType.SummonerSpell);
                    case SpellType.Trap:
                        return SpellDatabase.Database.FirstOrDefault(d => d.Name == spell.Name && d.ObjectName == spell.ObjectName && d.GameObject == true && d.SpellType == SpellType.Trap);
                    case SpellType.Item:
                        return SpellDatabase.Database.FirstOrDefault(d => d.Name == spell.Name && d.SpellType == SpellType.Item);
                    case SpellType.Spell:
                        if (spell.GameObject == true)
                            return SpellDatabase.Database.FirstOrDefault(d => d.Name == spell.Name && d.ChampionName == spell.ChampionName && d.GameObject == true && d.SpellType == SpellType.Spell);
                        else
                            return SpellDatabase.Database.FirstOrDefault(d => d.ChampionName == spell.ChampionName && d.Slot == spell.Slot && d.GameObject == false && d.SpellType == SpellType.Spell);
                }

                Chat.Print("error CODE GET_DATA " + spell.Name);
                return SpellDatabase.Database.FirstOrDefault(d => d.Name == spell.Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error CODE GET_DATA " + spell.Name);
                return SpellDatabase.Database.FirstOrDefault(d => d.Name == spell.Name);
            }
        }
    }
}
