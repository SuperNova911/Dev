using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
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
                Console.WriteLine(e);
                Chat.Print("error CODE GET_DRAW_TYPE " + spell.Name);
                return DrawType.Default;
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
                Console.WriteLine(e);
                Chat.Print("error CODE GET_COLOR " + spell.Name);
                return Color.White;
            }
        }

        public static System.Drawing.Color ConvertColor(this Color color)
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
                Chat.Print("FINDING CASTER....", System.Drawing.Color.LightBlue);

                Obj_AI_Base result;

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
                        Chat.Print("Caster " + caster.Caster.BaseSkinName, System.Drawing.Color.Red);
                        result = caster.Caster;
                        Program.CasterList.Remove(caster);
                        return caster.Caster;
                    }
                }
                result = EntityManager.Heroes.AllHeroes.Where(d => d.BaseSkinName == database.ChampionName).OrderBy(d => d.Distance(sender.Position)).FirstOrDefault();
                if (result != null)
                {
                    Chat.Print("Caster Near " + result.BaseSkinName, System.Drawing.Color.Purple);
                    return result;
                }

                Chat.Print("Failed to find caster " + database.Name);
                Chat.Print("Caster " + Player.Instance.BaseSkinName, System.Drawing.Color.Red);
                return Player.Instance;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error CODE FIND_CASTER " + database.Name);
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

                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE FIND_CASTER_WARD2 " + sender.Name, Color.LightBlue);
                return Player.Instance;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE FIND_CASTER_WARD " + sender.Name, Color.LightBlue);
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

        public static Vector3 KappaRoss(this Spell spell)
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

        public static void CloneTracker()
        {
            try
            {
                if (Config.Menu.CheckboxValue("cloneTracker"))
                {
                    foreach (var enemy in EntityManager.Heroes.AllHeroes.Where(d => !d.IsDead && d.VisibleOnScreen &&
                    (d.BaseSkinName == "Leblanc" || d.BaseSkinName == "Shaco" || d.BaseSkinName == "MonkeyKing" || d.BaseSkinName == "Yorick")))
                    {
                        new Circle { Color = System.Drawing.Color.Gold, Radius = enemy.BoundingRadius, BorderWidth = 4 }.Draw(enemy.Position);
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

        public static void AddCheckBox(this Menu menu, string uid, string displayName, bool defaultvalue)
        {
            try
            {
                menu.Add(uid, new CheckBox(displayName, defaultvalue));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE ADD_CHECKBOX " + displayName, Color.LightBlue);
            }
        }

        public static void AddSlider(this Menu menu, string uid, string displayName, int a, int b, int c)
        {
            try
            {
                menu.Add(uid, new Slider(displayName, a, b, c));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE ADD_SLIDER " + displayName, Color.LightBlue);
            }
        }

        public static void AddBlank(this Menu menu, string uid)
        {
            try
            {
                menu.Add(uid, new CheckBox("Blank"));
                menu[uid].Cast<CheckBox>().IsVisible = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE ADD_BLANK " + uid, Color.LightBlue);
            }
        }

        public static bool CheckboxValue(this Menu menu, string uid)
        {
            try
            {
                if (menu[uid].Cast<CheckBox>().CurrentValue)
                    return true;

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE CHECKBOX_VALUE " + uid, Color.LightBlue);
                return false;
            }
        }

        public static int SliderValue(this Menu menu, string uid)
        {
            try
            {
                return menu[uid].Cast<Slider>().CurrentValue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE SLIDER_VALUE " + uid, Color.LightBlue);
                return 0;
            }
        }

        public static int ComboBoxValue(this Menu menu, string uid)
        {
            try
            {
                return menu[uid].Cast<ComboBox>().CurrentValue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE COMBOBOX_VALUE " + uid, Color.LightBlue);
                return 0;
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
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_TYPE_TO_INT " + type.ToString(), Color.LightBlue);
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
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE IMPORTANCE_TO_INT " + type.ToString(), Color.LightBlue);
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
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE IMPORTANCE_TO_INT " + color.ToString(), Color.LightBlue);
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
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE GET_TEAM " + unit.BaseSkinName, Color.LightBlue);
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
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE GET_MENU_CODE2 " + sc2.SpriteName.ToString(), Color.LightBlue);
                return "sc2" + sc2.DisplayName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE GET_MENU_CODE " + sc2.SpriteName.ToString(), Color.LightBlue);
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
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE GET_MENU_CODE " + sc2.SpriteName.ToString(), Color.LightBlue);
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
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE YELLOW_TRINKET_REMAIN_TIME " + unit.BaseSkinName, Color.LightBlue);
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
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE HAS_SMITE " + unit.BaseSkinName, Color.LightBlue);
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
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE CLOCK_STYLE " + seconds, Color.LightBlue);
                return seconds.ToString();
            }
        }
    }
}
