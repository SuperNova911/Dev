using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerBuddy
{
    public static class Utility
    {
        /*public static string GetRemainTime(this Spell list)
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
                    return list.EndTime - TickCount >= 0 ? ((list.EndTime - TickCount) / 1000f).ToString("F1") : 0f.ToString("F1");
                }
                                
                if (database.EndTime > 10000)
                    return ((int)(list.EndTime - TickCount) / 1000 + 1).ToString();
                else
                    return list.EndTime - TickCount >= 0 ? ((list.EndTime - TickCount) / 1000f).ToString("F1") : 0f.ToString("F1");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE REMAIN_TIMER " + list.Caster.BaseSkinName);
                return "Kappa";
            }
        }*/

        public static string GetRemainTimeString(this Spell spell)
        {
            try
            {
                if (spell.GetFullTime() > 15000)
                    return ((int)(spell.GetRemainTime() / 1000f) + 1).ToString();
                return (spell.GetRemainTime() / 1000f).ToString("F1");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE REMAIN_STRING " + spell.Caster.BaseSkinName);
                return "KAPPA";
            }
        }

        /*public static float GetRemainTimeFloat(this Spell list)
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
        }*/

        public static float GetRemainTime(this Spell spell)
        {
            try
            {
                if (spell.Buff)
                    return spell.BuffRemainTime();
                
                var remainTime = (spell.EndTime - TickCount);

                return remainTime > 0 ? remainTime : 0f;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE REMAIN_TIME " + spell.Caster.BaseSkinName);
                return 4444;
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
            try
            {
                if (spell.Target.HasBuff(spell.Name))
                    if (spell.EndTime >= Game.Time)
                    {
                        var remainTime = spell.EndTime * 1000 - Game.Time * 1000;

                        return remainTime > 0 ? remainTime : 0f;
                    }
                return 0f;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error CODE BUFF_REMAIN " + spell.Name);
                return 0f;
            }
        }

        public static float GetFullTime(this Spell spell)
        {
            try
            {
                if (spell.Buff)
                    return spell.FullTime * 1000f;

                return spell.FullTime;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error CODE GET_FULL " + spell.Name);
                return 0f;
            }
        }

        public static SharpDX.Color GetColor(this Spell spell)
        {
            try
            {
                switch (spell.SpellType)
                {
                    case SpellType.SummonerSpell:
                        return Config.SummonerMenu.GetColor(spell.MenuString + "color");
                    case SpellType.Spell:
                        return Config.SpellMenu.GetColor(spell.MenuString + "color");
                    case SpellType.Trap:
                        return Config.TrapMenu.GetColor(spell.MenuString + "color");
                    case SpellType.Item:
                        return Config.ItemMenu.GetColor(spell.MenuString + "color");
                }
                return SharpDX.Color.White;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error CODE GET_COLOR " + spell.Name);
                return SharpDX.Color.White;
            }
        }

        public static System.Drawing.Color ConvertColor(SharpDX.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Spell GetDatabase(this Spell spell)
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
        
        public static Obj_AI_Base FIndCaster(this GameObject sender, Spell database)
        {
            try
            {
                if (database.GameObject == true)
                {
                    var name = database.ChampionName;

                    var heroList = EntityManager.Heroes.AllHeroes.Where(h => h.BaseSkinName == database.ChampionName).ToList();

                    if (heroList.Count == 1)
                    {
                        return heroList.First();
                    }

                    var caster = Program.CasterList.FirstOrDefault(c => c.Caster.BaseSkinName == database.ChampionName && c.Slot == database.Slot);

                    if (caster != null)
                        return caster.Caster;
                }
                return Player.Instance;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error CODE FIND_CASTER " + database.Name);
                return Player.Instance;
            }
        }

        public static string BaseObjectName(this GameObject sender)
        {
            var baseObject = sender as Obj_AI_Base;
            return baseObject == null ? sender.Name : baseObject.BaseSkinName;
        }

        public static void ShacoBoxActive(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.SData.Name == "ShacoBoxSpell")
            {
                var shacobox = Program.SpellList.FirstOrDefault(d => d.GameObject && sender.NetworkId == d.NetworkID && d.Cancel == false);

                if (shacobox != null)
                {
                    shacobox.Cancel = true;
                    shacobox.EndTime = 5000 + TickCount;
                }

                return;
            }
        }

        public static void CastCancel(Spell list)
        {
            try
            {
                list.Cancel = true;
                list.EndTime = TickCount + 2000;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE CANCEL");
            }
        }

        public static bool IsHero(this Obj_AI_Base unit)
        {
            if (unit.IsMinion || unit.IsMonster || unit.IsWard())
                return false;
            return true;
        }
    }
}
