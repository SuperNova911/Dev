using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
//using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Linq;

namespace TimerBuddy
{
    public static class Utility
    {
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
                e.ErrorMessage("REMAIN_TIME_STRING", spell.Caster.BaseSkinName);
                return "KAPPA";
            }
        }

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
                e.ErrorMessage("REMAIN_TIME", spell.Caster.BaseSkinName);
                return 4444;
            }
        }
        
        public static float TickCount
        {
            get
            {
                //return Environment.TickCount & int.MaxValue;
                return Game.Time * 1000;
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
                e.ErrorMessage("BUFF_REMAIN_TIME", spell.Name);
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
                e.ErrorMessage("GET_FULL_TIME", spell.Name);
                return 0f;
            }
        }

        public static DrawType GetDrawType(this Spell spell)
        {
            try
            {
                switch (spell.SpellType)
                {
                    case SpellType.Item:
                        return Config.ItemMenu.GetDrawType(spell.MenuCode + "drawtype");

                    case SpellType.Spell:
                        return Config.SpellMenu.GetDrawType(spell.MenuCode + "drawtype");

                    case SpellType.SummonerSpell:
                        return Config.SummonerMenu.GetDrawType(spell.MenuCode + "drawtype");
                }

                return DrawType.Default;
            }
            catch (Exception e)
            {
                e.ErrorMessage("GET_DRAW_TYPE", spell.Name);
                return DrawType.Default;
            }
        }

        public static Importance GetImportance(this Spell spell)
        {
            try
            {
                switch (spell.SpellType)
                {

                    case SpellType.Spell:
                        return Config.SpellMenu.GetImportance(spell.MenuCode + "importance");

                    case SpellType.SummonerSpell:
                        return Config.SummonerMenu.GetImportance(spell.MenuCode + "importance");
                }

                return Importance.Medium;
            }
            catch (Exception e)
            {
                e.ErrorMessage("GET_IMPORTANCE", spell.Name);
                return Importance.Medium;
            }
        }

        public static Color GetColor(this Spell spell)
        {
            try
            {
                switch (spell.SpellType)
                {
                    case SpellType.SummonerSpell:
                        return Config.SummonerMenu.GetColor(spell.MenuCode + "color");
                    case SpellType.Spell:
                        return Config.SpellMenu.GetColor(spell.MenuCode + "color");
                    case SpellType.Trap:
                        return Config.TrapMenu.GetColor(spell.MenuCode + "color");
                    case SpellType.Item:
                        return Config.ItemMenu.GetColor(spell.MenuCode + "color");
                    case SpellType.Ward:
                        return Config.WardMenu.GetColor(spell.MenuCode + "color");
                }
                return Color.White;
            }
            catch (Exception e)
            {
                e.ErrorMessage("GET_COLOR", spell.Name);
                return Color.White;
            }
        }

        public static System.Drawing.Color ConvertColor(this Color color)
        {
            try
            {
                return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            }
            catch (Exception e)
            {
                e.ErrorMessage("CONVERT_COLOR", color.ToString());
                return System.Drawing.Color.White;
            }
        }
        
        public static Obj_AI_Base FIndCaster(this GameObject sender, Spell database)
        {
            try
            {
                Obj_AI_Base result;

                if (database.SpellType == SpellType.Item)
                {
                    result = EntityManager.Heroes.AllHeroes.OrderBy(d => d.Position.Distance(sender.Position)).FirstOrDefault();
                    if (result != null)
                    {
                        return result;
                    }
                }

                if (database.SpellType == SpellType.SummonerSpell)
                {
                    var caster = Program.CasterList.FirstOrDefault(c => c.SpellType == SpellType.SummonerSpell);
                    if (caster != null)
                    {
                        result = caster.Caster;
                        Program.CasterList.Remove(caster);
                        return caster.Caster;
                    }
                }

                if (database.GameObject == true)
                {
                    var heroList = EntityManager.Heroes.AllHeroes.Where(h => h.BaseSkinName == database.ChampionName).ToList();

                    if (heroList.Count == 1)
                    {
                        result = heroList.First();
                        Program.CasterList.Remove(Program.CasterList.FirstOrDefault(d => d.Caster.BaseSkinName == database.ChampionName && d.Slot == database.Slot));
                        return result;
                    }

                    var caster = Program.CasterList.FirstOrDefault(c => c.Caster.BaseSkinName == database.ChampionName && c.Slot == database.Slot);

                    if (caster != null)
                    {
                        result = caster.Caster;
                        Program.CasterList.Remove(caster);
                        return caster.Caster;
                    }
                }
                result = EntityManager.Heroes.AllHeroes.Where(d => d.BaseSkinName == database.ChampionName).OrderBy(d => d.Distance(sender.Position)).FirstOrDefault();
                if (result != null)
                {
                    return result;
                }
                
                return Player.Instance;
            }
            catch (Exception e)
            {
                e.ErrorMessage("FIND_CASTER", database.Name);
                return Player.Instance;
            }
        }

        public static Obj_AI_Base FindCasterWard(this GameObject sender, Spell database)
        {
            try
            {
                WardCaster caster;
                switch (database.ObjectName)
                {
                    case "YellowTrinket":
                        caster = Program.WardCasterList.FirstOrDefault(d => d.Name == "TrinketTotemLvl1");

                        if (caster != null)
                            return caster.Caster;
                        break;
                    case "SightWard":
                        caster = Program.WardCasterList.FirstOrDefault(d => d.Name == "ItemGhostWard");

                        if (caster != null)
                            return caster.Caster;
                        break;
                    case "BlueTrinket":
                        caster = Program.WardCasterList.FirstOrDefault(d => d.Name == "TrinketOrbLvl3");

                        if (caster != null)
                            return caster.Caster;
                        break;
                    case "VisionWard":
                        caster = Program.WardCasterList.FirstOrDefault(d => d.Name == "VisionWard");

                        if (caster != null)
                            return caster.Caster;
                        break;
                }

                if (sender.Team.IsAlly())
                {
                    var hero = EntityManager.Heroes.Allies.OrderBy(d => d.Distance(sender.Position)).First();

                    if (hero != null)
                        return hero;
                }

                if (sender.Team.IsEnemy())
                {
                    var hero = EntityManager.Heroes.Enemies.OrderBy(d => d.Distance(sender.Position)).First();

                    if (hero != null)
                        return hero;
                }
                
                return Player.Instance;
            }
            catch (Exception e)
            {
                e.ErrorMessage("FIND_CASTER_WARD", sender.Name);
                return Player.Instance;
            }
        }

        public static string BaseObjectName(this GameObject sender)
        {
            try
            {
                var baseObject = sender as Obj_AI_Base;
                return baseObject == null ? sender.Name : baseObject.BaseSkinName;
            }
            catch (Exception e)
            {
                e.ErrorMessage("BASE_OBJECT_NAME");
                return sender.Name;
            }
        }

        public static void ShacoBoxActive(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
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
            catch (Exception e)
            {
                e.ErrorMessage("SHACO_BOX_ACTIVE");
            }
        }
        
        public static bool IsHero(this Obj_AI_Base unit)
        {
            try
            {
                if (unit.IsMinion || unit.IsMonster || unit.IsWard())
                    return false;
                return true;
            }
            catch (Exception e)
            {
                e.ErrorMessage("IS_HERO", unit.BaseSkinName);
                return false;
            }
        }

        public static Vector3 KappaRoss(this Spell spell)
        {
            try
            {
                Vector3 Kappa;
                Vector3 vec3 = spell.CastPosition - spell.StartPosition;
                var length = Math.Sqrt(Math.Pow(vec3.X, 2) + Math.Pow(vec3.Y, 2));

                if (length > spell.Range)
                {
                    Kappa.X = vec3.X / (float)length;
                    Kappa.Y = vec3.Y / (float)length;
                    Kappa.Z = vec3.Z / (float)length;

                    return Kappa * spell.Range + spell.StartPosition;
                }

                return spell.CastPosition;
            }
            catch (Exception e)
            {
                e.ErrorMessage("KAPPA_ROSS", spell.Name);
                return spell.CastPosition;
            }
        }

        public static void CloneTracker()
        {
            try
            {
                if (Config.Menu.CheckboxValue("cloneTracker"))
                {
                    foreach (var enemy in EntityManager.Heroes.AllHeroes.Where(d => !d.IsDead && d.VisibleOnScreen &&
                    (d.BaseSkinName == "Leblanc" || d.BaseSkinName == "Shaco" || d.BaseSkinName == "MonkeyKing" || d.BaseSkinName == "Yorick")))
                    {
                        // Kappa new Circle { Color = System.Drawing.Color.Gold, Radius = enemy.BoundingRadius, BorderWidth = 4 }.Draw(enemy.Position);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE CLONE_TRACKER", Color.Cyan);
                Chat.Print("Disable Clone Tracker, Please report bugs with CODE", Color.Gold);
                Config.Menu["cloneTracker"].Cast<CheckBox>().CurrentValue = false;
            }
        }

        public static int ToInt(this DrawType type)
        {
            try
            {
                switch (type)
                {
                    case DrawType.Default:
                        return 0;
                    case DrawType.HPLine:
                        return 1;
                    case DrawType.Number:
                        return 2;
                    case DrawType.NumberLine:
                        return 3;
                }
                return 0;
            }
            catch (Exception e)
            {
                e.ErrorMessage("TO_INT_DRAWTYPE", type.ToString());
                return 0;
            }
        }

        public static int ToInt(this Importance type)
        {
            try
            {
                switch (type)
                {
                    case Importance.Low:
                        return 0;
                    case Importance.Medium:
                        return 1;
                    case Importance.High:
                        return 2;
                    case Importance.VeryHigh:
                        return 3;
                }
                return 1;
            }
            catch (Exception e)
            {
                e.ErrorMessage("TO_INT_IMPORTANCE", type.ToString());
                return 1;
            }
        }

        public static int ToInt(this Color color)
        {
            try
            {
                if (color == Color.HotPink)
                    return 51;
                if (color == Color.Yellow)
                    return 17;
                if (color == Color.LawnGreen)
                    return 25;
                if (color == Color.SkyBlue)
                    return 33;

                return 0;
            }
            catch (Exception e)
            {
                e.ErrorMessage("TO_INT_COLOR", color.ToString());
                return 0;
            }
        }

        public static Team GetTeam(this Obj_AI_Base unit)
        {
            try
            {
                return unit.IsAlly ? Team.Ally : unit.IsEnemy ? Team.Enemy : unit.IsMonster ? Team.Neutral : Team.None;
            }
            catch (Exception e)
            {
                e.ErrorMessage("GET_TEAM", unit.BaseSkinName);
                return 0;
            }
        }

        public static string GetMenuCode(this SC2Timer sc2)
        {
            try
            {
                switch (sc2.SC2Type)
                {
                    case SC2Type.Spell:
                        return "sc2" + sc2.ChampionName + sc2.Slot.ToString();
                    case SC2Type.SummonerSpell:
                        return "sc2" + sc2.DisplayName;
                    case SC2Type.Jungle:
                        return "sc2" + sc2.DisplayName;
                }
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE GET_MENU_CODE2 " + sc2.SpriteName.ToString(), Color.SpringGreen);
                return "sc2" + sc2.DisplayName;
            }
            catch (Exception e)
            {
                e.ErrorMessage("GET_MENU_CODE", sc2.SpriteName.ToString());
                return "sc2" + sc2.DisplayName;
            }
        }

        public static string GetDisplayName(this SC2Timer sc2)
        {
            try
            {
                switch (sc2.SC2Type)
                {
                    case SC2Type.Spell:
                        return sc2.ChampionName + " " + sc2.Slot.ToString();
                }
                return sc2.DisplayName;
            }
            catch (Exception e)
            {
                e.ErrorMessage("GET_DISPLAY_NAME", sc2.SpriteName.ToString());
                return sc2.DisplayName;
            }
        }

        public static float YellowTrinketRemaintime(this Obj_AI_Base unit)
        {
            try
            {
                var Kappa = unit as AIHeroClient;
                return 60000f + 3.5f * (Kappa.Level - 1) * 1000f;
            }
            catch (Exception e)
            {
                e.ErrorMessage("YELLOW_TRINKET_REMAIN_TIME");
                return 60000f + 3.5f * (Player.Instance.Level - 1) * 1000f;
            }
        }
        
        public static bool HasSmite(this AIHeroClient unit)
        {
            try
            {
                if (unit.Spellbook.Spells.Where(s => s.Name.Contains("summonersmite")).Any())
                    return true;

                return false;
            }
            catch (Exception e)
            {
                e.ErrorMessage("HAS_SMITE", unit.BaseSkinName);
                return false;
            }
        }

        public static string ClockStyle(this float seconds)
        {
            try
            {
                return seconds < 0 ? null : TimeSpan.FromSeconds(seconds).ToString(@"m\:ss");
            }
            catch (Exception e)
            {
                e.ErrorMessage("CLOCK_STYLE", seconds.ToString());
                return TimeSpan.FromSeconds(seconds).ToString(@"m\:ss");
            }
        }

        public static void ErrorMessage(this Exception e, string CODE, string moreInfo = "")
        {
            try
            {
                if (!Config.MiscMenu.CheckboxValue("error"))
                    return;

                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR: </font>CODE " + CODE + " " + moreInfo, System.Drawing.Color.SpringGreen);
            }
            catch (Exception f)
            {
                Console.WriteLine(f);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE ERROR_MESSAGE", Color.SpringGreen);
                Chat.Print("WTF Kappa Kappa Kappa", Color.Gold);
            }
        }
    }
}
