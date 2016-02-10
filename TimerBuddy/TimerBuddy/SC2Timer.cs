using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TimerBuddy.Properties;

namespace TimerBuddy
{
    public enum SC2Type
    {
        Spell, SummonerSpell, Jungle
    }

    public class SC2Timer
    {
        public SpellSlot Slot;
        public SC2Type SC2Type = SC2Type.Spell;
        public Team Team;
        public Obj_AI_Base Caster;
        public string ChampionName;
        public string Name;
        public string MenuCode;
        public string DisplayName;
        public float FullTime;
        public float StartTime;
        public float EndTime;
        public int Number;
        public bool GameObject = false;
        public bool Cancel = false;
        public bool Global = false;
        public bool Drawing = false;
        public Bitmap SpriteName;
    }

    internal static class SC2TimerDatabase
    {
        public static readonly List<SC2Timer> Database;

        static SC2TimerDatabase()
        {
            Database = new List<SC2Timer>
            {
                new SC2Timer { ChampionName = "Aatrox", Slot = SpellSlot.R, SpriteName = Resources.AatroxR_BIG },
                new SC2Timer { ChampionName = "Ahri", Slot = SpellSlot.R, SpriteName = Resources.AhriR_BIG },
                new SC2Timer { ChampionName = "Alistar", Slot = SpellSlot.R, SpriteName = Resources.AlistarR_BIG },
                new SC2Timer { ChampionName = "Amumu", Slot = SpellSlot.R, SpriteName = Resources.AmumuR_BIG },
                //new SC2Timer { ChampionName = "Anivia", Slot = SpellSlot.R, SpriteName = Resources.AniviaPassive_BIG },
                new SC2Timer { ChampionName = "Annie", Slot = SpellSlot.R, SpriteName = Resources.AnnieR_BIG },
                new SC2Timer { ChampionName = "Ashe", Slot = SpellSlot.R, SpriteName = Resources.AsheR_BIG, Global = true },
                new SC2Timer { ChampionName = "Azir", Slot = SpellSlot.R, SpriteName = Resources.AzirR_BIG },
                new SC2Timer { ChampionName = "Bard", Slot = SpellSlot.R, SpriteName = Resources.BardR_BIG },
                new SC2Timer { ChampionName = "Brand", Slot = SpellSlot.R, SpriteName = Resources.BrandR_BIG },
                new SC2Timer { ChampionName = "Braum", Slot = SpellSlot.R, SpriteName = Resources.BraumR_BIG },
                new SC2Timer { ChampionName = "Caitlyn", Slot = SpellSlot.R, SpriteName = Resources.CaitlynR_BIG },
                new SC2Timer { ChampionName = "Cassiopeia", Slot = SpellSlot.R, SpriteName = Resources.CassiopeiaR_BIG },
                new SC2Timer { ChampionName = "Darius", Slot = SpellSlot.R, SpriteName = Resources.DariusR_BIG },
                new SC2Timer { ChampionName = "DrMundo", Slot = SpellSlot.R, SpriteName = Resources.DrMundoR_BIG },
                new SC2Timer { ChampionName = "Draven", Slot = SpellSlot.R, SpriteName = Resources.DravenR_BIG },
                new SC2Timer { ChampionName = "Ekko", Slot = SpellSlot.R, SpriteName = Resources.EkkoR_BIG },
                new SC2Timer { ChampionName = "Evelynn", Slot = SpellSlot.R, SpriteName = Resources.EvelynnR_BIG },
                new SC2Timer { ChampionName = "Ezreal", Slot = SpellSlot.R, SpriteName = Resources.EzrealR_BIG, Global = true },
                new SC2Timer { ChampionName = "Fiddlesticks", Slot = SpellSlot.R, SpriteName = Resources.FiddlesticksR_BIG },
                new SC2Timer { ChampionName = "Fiora", Slot = SpellSlot.R, SpriteName = Resources.FioraR_BIG },
                new SC2Timer { ChampionName = "Fizz", Slot = SpellSlot.R, SpriteName = Resources.FizzR_BIG },
                new SC2Timer { ChampionName = "Galio", Slot = SpellSlot.R, SpriteName = Resources.GalioR_BIG },
                new SC2Timer { ChampionName = "Gangplank", Slot = SpellSlot.R, SpriteName = Resources.GangplankR_BIG, Global = true },
                new SC2Timer { ChampionName = "Garen", Slot = SpellSlot.R, SpriteName = Resources.GarenR_BIG },
                new SC2Timer { ChampionName = "Gnar", Slot = SpellSlot.R, SpriteName = Resources.GnarR_BIG },
                new SC2Timer { ChampionName = "Gragas", Slot = SpellSlot.R, SpriteName = Resources.GragasR_BIG },
                new SC2Timer { ChampionName = "Graves", Slot = SpellSlot.R, SpriteName = Resources.GravesR_BIG },
                new SC2Timer { ChampionName = "Hecarim", Slot = SpellSlot.R, SpriteName = Resources.HecarimR_BIG },
                new SC2Timer { ChampionName = "Illaoi", Slot = SpellSlot.R, SpriteName = Resources.IllaoiR_BIG },
                new SC2Timer { ChampionName = "Irelia", Slot = SpellSlot.R, SpriteName = Resources.IreliaR_BIG },
                new SC2Timer { ChampionName = "Janna", Slot = SpellSlot.R, SpriteName = Resources.JannaR_BIG },
                new SC2Timer { ChampionName = "JarvanIV", Slot = SpellSlot.R, SpriteName = Resources.JarvanR_BIG },
                new SC2Timer { ChampionName = "Jax", Slot = SpellSlot.R, SpriteName = Resources.JaxR_BIG },
                new SC2Timer { ChampionName = "Jhin", Slot = SpellSlot.R, SpriteName = Resources.JhinR_BIG },
                new SC2Timer { ChampionName = "Jinx", Slot = SpellSlot.R, SpriteName = Resources.JinxR_BIG },
                new SC2Timer { ChampionName = "Kalista", Slot = SpellSlot.R, SpriteName = Resources.KalistaR_BIG },
                new SC2Timer { ChampionName = "Karthus", Slot = SpellSlot.R, SpriteName = Resources.KarthusR_BIG, Global = true },
                new SC2Timer { ChampionName = "Katarina", Slot = SpellSlot.R, SpriteName = Resources.KatarinaR_BIG },
                new SC2Timer { ChampionName = "Kayle", Slot = SpellSlot.R, SpriteName = Resources.KayleR_BIG },
                new SC2Timer { ChampionName = "Kennen", Slot = SpellSlot.R, SpriteName = Resources.KennenR_BIG },
                new SC2Timer { ChampionName = "Kindred", Slot = SpellSlot.R, SpriteName = Resources.KindredR_BIG },
                new SC2Timer { ChampionName = "LeeSin", Slot = SpellSlot.R, SpriteName = Resources.LeeSinR_BIG },
                new SC2Timer { ChampionName = "Leona", Slot = SpellSlot.R, SpriteName = Resources.LeonaR_BIG },
                new SC2Timer { ChampionName = "Lissandra", Slot = SpellSlot.R, SpriteName = Resources.LissandraR_BIG },
                new SC2Timer { ChampionName = "Lucian", Slot = SpellSlot.R, SpriteName = Resources.LucianR_BIG },
                new SC2Timer { ChampionName = "Lulu", Slot = SpellSlot.R, SpriteName = Resources.LuluR_BIG },
                new SC2Timer { ChampionName = "Lux", Slot = SpellSlot.R, SpriteName = Resources.LuxR_BIG },
                new SC2Timer { ChampionName = "Malphite", Slot = SpellSlot.R, SpriteName = Resources.MalphiteR_BIG },
                new SC2Timer { ChampionName = "Malzahar", Slot = SpellSlot.R, SpriteName = Resources.MalzaharR_BIG },
                new SC2Timer { ChampionName = "MasterYi", Slot = SpellSlot.R, SpriteName = Resources.MasterYiR_BIG },
                new SC2Timer { ChampionName = "MissFortune", Slot = SpellSlot.R, SpriteName = Resources.MissFortuneR_BIG },
                new SC2Timer { ChampionName = "Mordekaiser", Slot = SpellSlot.R, SpriteName = Resources.MordekaiserR_BIG },
                new SC2Timer { ChampionName = "Morgana", Slot = SpellSlot.R, SpriteName = Resources.MorganaR_BIG },
                new SC2Timer { ChampionName = "Nami", Slot = SpellSlot.R, SpriteName = Resources.NamiR_BIG },
                new SC2Timer { ChampionName = "Nasus", Slot = SpellSlot.R, SpriteName = Resources.NasusR_BIG },
                new SC2Timer { ChampionName = "Nautilus", Slot = SpellSlot.R, SpriteName = Resources.NautilusR_BIG },
                new SC2Timer { ChampionName = "Nocturne", Slot = SpellSlot.R, SpriteName = Resources.NocturneR_BIG },
                new SC2Timer { ChampionName = "Nunu", Slot = SpellSlot.R, SpriteName = Resources.NunuR_BIG },
                new SC2Timer { ChampionName = "Olaf", Slot = SpellSlot.R, SpriteName = Resources.OlafR_BIG },
                new SC2Timer { ChampionName = "Orianna", Slot = SpellSlot.R, SpriteName = Resources.OriannaR_BIG },
                new SC2Timer { ChampionName = "Pantheon", Slot = SpellSlot.R, SpriteName = Resources.PantheonR_BIG },
                new SC2Timer { ChampionName = "Poppy", Slot = SpellSlot.R, SpriteName = Resources.PoppyR_BIG },
                new SC2Timer { ChampionName = "Rammus", Slot = SpellSlot.R, SpriteName = Resources.RammusR_BIG },
                new SC2Timer { ChampionName = "RekSai", Slot = SpellSlot.R, SpriteName = Resources.RekSaiR_BIG },
                new SC2Timer { ChampionName = "Renekton", Slot = SpellSlot.R, SpriteName = Resources.RenektonR_BIG },
                new SC2Timer { ChampionName = "Rengar", Slot = SpellSlot.R, SpriteName = Resources.RengarR_BIG, Global = true },
                new SC2Timer { ChampionName = "Riven", Slot = SpellSlot.R, SpriteName = Resources.RivenR_BIG },
                new SC2Timer { ChampionName = "Rumble", Slot = SpellSlot.R, SpriteName = Resources.RumbleR_BIG },
                new SC2Timer { ChampionName = "Sejuani", Slot = SpellSlot.R, SpriteName = Resources.SejuaniR_BIG },
                new SC2Timer { ChampionName = "Shaco", Slot = SpellSlot.R, SpriteName = Resources.ShacoR_BIG },
                new SC2Timer { ChampionName = "Shen", Slot = SpellSlot.R, SpriteName = Resources.ShenR_BIG, Global = true },
                new SC2Timer { ChampionName = "Shyvana", Slot = SpellSlot.R, SpriteName = Resources.ShyvanaR_BIG },
                new SC2Timer { ChampionName = "Singed", Slot = SpellSlot.R, SpriteName = Resources.SingedR_BIG },
                new SC2Timer { ChampionName = "Sion", Slot = SpellSlot.R, SpriteName = Resources.SionR_BIG, Global = true },
                new SC2Timer { ChampionName = "Sivir", Slot = SpellSlot.R, SpriteName = Resources.SivirR_BIG },
                new SC2Timer { ChampionName = "Skarner", Slot = SpellSlot.R, SpriteName = Resources.SkarnerR_BIG },
                new SC2Timer { ChampionName = "Sona", Slot = SpellSlot.R, SpriteName = Resources.SonaR_BIG },
                new SC2Timer { ChampionName = "Soraka", Slot = SpellSlot.R, SpriteName = Resources.SorakaR_BIG, Global = true },
                new SC2Timer { ChampionName = "Syndra", Slot = SpellSlot.R, SpriteName = Resources.SyndraR_BIG },
                new SC2Timer { ChampionName = "TahmKench", Slot = SpellSlot.R, SpriteName = Resources.TahmKenchR_BIG },
                new SC2Timer { ChampionName = "Talon", Slot = SpellSlot.R, SpriteName = Resources.TalonR_BIG },
                new SC2Timer { ChampionName = "Taric", Slot = SpellSlot.R, SpriteName = Resources.TaricR_BIG },
                new SC2Timer { ChampionName = "Thresh", Slot = SpellSlot.R, SpriteName = Resources.ThreshR_BIG },
                new SC2Timer { ChampionName = "Tristana", Slot = SpellSlot.R, SpriteName = Resources.TristanaR_BIG },
                new SC2Timer { ChampionName = "Trundle", Slot = SpellSlot.R, SpriteName = Resources.TrundleR_BIG },
                new SC2Timer { ChampionName = "Tryndamere", Slot = SpellSlot.R, SpriteName = Resources.TryndamereR_BIG },
                new SC2Timer { ChampionName = "TwistedFate", Slot = SpellSlot.R, SpriteName = Resources.TwistedFateR_BIG, Global = true },
                new SC2Timer { ChampionName = "Twitch", Slot = SpellSlot.R, SpriteName = Resources.TwitchR_BIG },
                new SC2Timer { ChampionName = "Urgot", Slot = SpellSlot.R, SpriteName = Resources.UrgotR_BIG },
                new SC2Timer { ChampionName = "Varus", Slot = SpellSlot.R, SpriteName = Resources.VarusR_BIG },
                new SC2Timer { ChampionName = "Vayne", Slot = SpellSlot.R, SpriteName = Resources.VayneR_BIG },
                new SC2Timer { ChampionName = "Veigar", Slot = SpellSlot.R, SpriteName = Resources.VeigarR_BIG },
                new SC2Timer { ChampionName = "Velkoz", Slot = SpellSlot.R, SpriteName = Resources.VelkozR_BIG },
                new SC2Timer { ChampionName = "Vi", Slot = SpellSlot.R, SpriteName = Resources.ViR_BIG },
                new SC2Timer { ChampionName = "Viktor", Slot = SpellSlot.R, SpriteName = Resources.ViktorR_BIG },
                new SC2Timer { ChampionName = "Vladimir", Slot = SpellSlot.R, SpriteName = Resources.VladimirR_BIG },
                //new SC2Timer { ChampionName = "Volibear", Slot = SpellSlot.R, SpriteName = Resources.VolibearP_BIG },
                new SC2Timer { ChampionName = "Volibear", Slot = SpellSlot.R, SpriteName = Resources.VolibearR_BIG },
                new SC2Timer { ChampionName = "Warwick", Slot = SpellSlot.R, SpriteName = Resources.WarwickR_BIG },
                new SC2Timer { ChampionName = "Wukong", Slot = SpellSlot.R, SpriteName = Resources.WukongR_BIG },
                new SC2Timer { ChampionName = "Xerath", Slot = SpellSlot.R, SpriteName = Resources.XerathR_BIG, Global = true },
                new SC2Timer { ChampionName = "XinZhao", Slot = SpellSlot.R, SpriteName = Resources.XinZhaoR_BIG },
                new SC2Timer { ChampionName = "Yasuo", Slot = SpellSlot.R, SpriteName = Resources.YasuoR_BIG },
                new SC2Timer { ChampionName = "Yorick", Slot = SpellSlot.R, SpriteName = Resources.YorickR_BIG },
                new SC2Timer { ChampionName = "Zac", Slot = SpellSlot.R, SpriteName = Resources.ZacR_BIG },
                new SC2Timer { ChampionName = "Zed", Slot = SpellSlot.R, SpriteName = Resources.ZedR_BIG },
                new SC2Timer { ChampionName = "Ziggs", Slot = SpellSlot.R, SpriteName = Resources.ZiggsR_BIG, Global = true },
                new SC2Timer { ChampionName = "Zilean", Slot = SpellSlot.R, SpriteName = Resources.ZileanR_BIG },
                new SC2Timer { ChampionName = "Zyra", Slot = SpellSlot.R, SpriteName = Resources.ZyraR_BIG },

                new SC2Timer { SC2Type = SC2Type.SummonerSpell, Name = "summonerteleport", DisplayName = "Summoner Teleport", SpriteName = Resources.Teleport_BIG },
                new SC2Timer { SC2Type = SC2Type.SummonerSpell, Name = "summonerheal", DisplayName = "Summoner Heal", SpriteName = Resources.Heal_BIG },
                new SC2Timer { SC2Type = SC2Type.SummonerSpell, Name = "summonerboost", DisplayName = "Summoner Cleanse", SpriteName = Resources.Cleanse_BIG },
                new SC2Timer { SC2Type = SC2Type.SummonerSpell, Name = "summonerbarrier", DisplayName = "Summoner Barrier", SpriteName = Resources.Barrier_BIG },
                new SC2Timer { SC2Type = SC2Type.SummonerSpell, Name = "summonerhaste", DisplayName = "Summoner Haste", SpriteName = Resources.Haste_BIG },
                new SC2Timer { SC2Type = SC2Type.SummonerSpell, Name = "summonerdot", DisplayName = "Summoner Ignite", SpriteName = Resources.Ignite_BIG },
                new SC2Timer { SC2Type = SC2Type.SummonerSpell, Name = "summonerflash", DisplayName = "Summoner Flash", SpriteName = Resources.Flash_BIG },
                new SC2Timer { SC2Type = SC2Type.SummonerSpell, Name = "summonerexhaust", DisplayName = "Summoner Exhaust", SpriteName = Resources.Exhaust_BIG },
                new SC2Timer { SC2Type = SC2Type.SummonerSpell, Name = "summonermana", DisplayName = "Summoner Clarity", SpriteName = Resources.Clarity_BIG },


                //new SC2Timer { SC2Type = SC2Type.Jungle, Name = "SRU_Red", FullTime = 300000, DisplayName = "Red Brambleback", SpriteName = Resources.Red_Brambleback_BIG },
                //new SC2Timer { SC2Type = SC2Type.Jungle, Name = "SRU_Blue", FullTime = 300000, DisplayName = "Blue Sentinel", SpriteName = Resources.Blue_Sentinel_BIG },
                new SC2Timer { SC2Type = SC2Type.Jungle, Name = "SRU_JungleBuff_Dragon_Activation_Buf.troy", FullTime = 360000, DisplayName = "Dragon", SpriteName = Resources.Dragon_BIG },
                new SC2Timer { SC2Type = SC2Type.Jungle, Name = "SRU_JungleBuff_Baron_SiegeMin_Cas.troy", FullTime = 420000, DisplayName = "Baron Nashor", SpriteName = Resources.Baron_Nashor_BIG },

                //Add     Type: Obj_GeneralParticleEmitter | Name: SRU_JungleBuff_Dragon_Activation_Buf_avatar.troy | NetID: 1073749625 | objectName: SRU_JungleBuff_Dragon_Activation_Buf_avatar.troy
                //Add     Type: Obj_GeneralParticleEmitter | Name: SRU_JungleBuff_Dragon_Activation_Buf.troy | NetID: 1073749624 | objectName: SRU_JungleBuff_Dragon_Activation_Buf.troy
                //Delete  Type: Obj_GeneralParticleEmitter | Name: SRU_JungleBuff_Dragon_Activation_Buf.troy
                //Delete  Type: Obj_GeneralParticleEmitter | Name: SRU_JungleBuff_Dragon_Activation_Buf_avatar.troy
                //SRU_JungleBuff_Baron_SiegeMin_Cas.troy
                //
            };
        }
    }

    public class SC2Slot
    {
        public SC2Timer Timer;
        public int Slot;
        public float Duration;
        public float StartTime;
    }

    public static class SC2SlotManager
    {
        public static List<SC2Slot> SC2SlotList = new List<SC2Slot>();

        public static void AddOnSlot(this SC2Timer sc2, int duration)
        {
            try
            {
                SC2SlotList.Add(new SC2Slot
                {
                    Timer = sc2,
                    Slot = SC2SlotList.Count,
                    Duration = duration,
                    StartTime = Utility.TickCount,
                });
            }
            catch (Exception e)
            {
                e.ErrorMessage("ADD_ON_SLOT", sc2.DisplayName);
            }
        }

        public static void RemoveOnSlot(this SC2Slot slot)
        {
            try
            {
                foreach (var list in SC2SlotList.Where(d => d.Slot > slot.Slot))
                {
                    list.Slot--;
                }
                slot.Timer.Drawing = false;
                SC2SlotList.Remove(slot);
            }
            catch (Exception e)
            {
                e.ErrorMessage("REMOVE_ON_SLOT", slot.Timer.DisplayName);
            }
        }
    }

    public class HeroList
    {
        public Obj_AI_Base Hero;
        public float Endtime;
    }

    public static class SC2TimerManager
    {
        public static List<HeroList> HeroList = new List<HeroList>();

        static SC2TimerManager()
        {
            try
            {
                Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
                GameObject.OnCreate += GameObject_OnCreate;
                Game.OnTick += Game_OnTick;
                Drawing.OnDraw += Drawing_OnDraw;
                Drawing.OnEndScene += Drawing_OnEndScene;
            }
            catch (Exception e)
            {
                e.ErrorMessage("SC2TIMER_MANAGER_INIT");
            }            
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            try
            {
                if (!sender.IsValid)
                    return;

                SC2JungleDetector(sender, args);
            }
            catch (Exception e)
            {
                e.ErrorMessage("SC2TIMER_ONDELETE", sender.Name);
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                Core.DelayAction(() => SC2TimerDetector(sender, args), 1000);
            }
            catch (Exception e)
            {
                e.ErrorMessage("SC2TIMER_SPELLCAST", args.SData.Name);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            try
            {
                ListControl();
                HeroListManager();
                SC2TimerRemover();
            }
            catch (Exception e)
            {
                e.ErrorMessage("SC2TIMER_ONTICK");
            }
        }

        private static void ListControl()
        {
            try
            {
                foreach (var sc2 in Program.SC2TimerList.Where(d => d.Drawing == false && d.ShowHudCheck() && d.CheckArea()))
                {
                    sc2.Drawing = true;
                    var duration = sc2.SC2Type == SC2Type.Jungle ? sc2.OneMin() ? 5000 : 10000 : 10000;
                    sc2.AddOnSlot(duration);
                }

                var removeTarget = SC2SlotManager.SC2SlotList.FirstOrDefault(d => d.Timer.Drawing == true && d.StartTime + d.Duration + 3000 < Utility.TickCount);

                if (removeTarget != null)
                    removeTarget.RemoveOnSlot();
            }
            catch (Exception e)
            {
                e.ErrorMessage("LIST_CONTROL");
            }
        }

        private static bool ShowHudCheck(this SC2Timer sc2)
        {
            try
            {
                float time = sc2.EndTime - Utility.TickCount;
                bool menu1 = true;
                bool menu2 = true;
                int timer1 = 10000;
                int timer2 = 61000;


                switch (sc2.SC2Type)
                {
                    case SC2Type.Jungle:
                        menu1 = Config.SC2Menu.CheckboxValue("jungle");
                        menu2 = Config.SC2Menu.CheckboxValue("jungle1min");
                        timer1 = 10000;
                        timer2 = 61000;

                        if (timer1 - 2000 < time && time <= timer1 && menu1)  // 10초 전
                            return true;

                        if (timer2 - 2000 < time && time <= timer2 && menu2)  // 1분 전
                            return true;
                        break;

                    case SC2Type.Spell:
                        menu1 = Config.SC2Menu.CheckboxValue("ult");
                        timer1 = 10000;

                        if (timer1 - 2000 < time && time <= timer1 && menu1)  // 10초 전
                            return true;
                        break;

                    case SC2Type.SummonerSpell:
                        menu1 = Config.SC2Menu.CheckboxValue("ss");
                        timer1 = 10000;

                        if (timer1 - 2000 < time && time <= timer1 && menu1)  // 10초 전
                            return true;

                        break;
                }

                return false;
            }
            catch (Exception e)
            {
                e.ErrorMessage("SHOW_HUD_CHECK", sc2.DisplayName);
                return false;
            }
        }

        private static bool OneMin(this SC2Timer sc2)
        {
            try
            {
                if (sc2.SC2Type == SC2Type.Jungle)
                {
                    float time = sc2.EndTime - Utility.TickCount;
                    int timer = 61000;

                    if (timer - 2000 < time && time < timer)
                        return true;
                }

                return false;
            }
            catch (Exception e)
            {
                e.ErrorMessage("ONE_MIN", sc2.DisplayName);
                return false;
            }
        }
        
        private static bool CheckArea(this SC2Timer sc2)
        {
            try
            {
                bool menuMe = true;
                bool menuSS = Config.SC2Menu.CheckboxValue("ss");
                bool menuJungle = Config.SC2Menu.CheckboxValue("jungleEnable");

                if (sc2.SC2Type == SC2Type.SummonerSpell && menuSS)
                    return true;

                if (sc2.SC2Type == SC2Type.Jungle && menuJungle)
                    return true;

                if (sc2.SC2Type == SC2Type.Spell)
                {
                    bool menuGlobal = Config.SC2Menu.CheckboxValue("sc2global" + sc2.ChampionName);

                    if (sc2.Caster.IsMe && menuMe)
                        return true;

                    if (sc2.Global && menuGlobal)
                        return true;

                    if (HeroList.FirstOrDefault(d => d.Hero == sc2.Caster) != null)
                        return true;
                }

                return false;
            }
            catch (Exception e)
            {
                e.ErrorMessage("CHECK_AREA", sc2.Caster.BaseSkinName);
                return false;
            }
        }

        private static void HeroListManager()
        {
            try
            {
                if (HeroList.Count < 4)
                {
                    foreach (var hero in EntityManager.Heroes.AllHeroes.Where(d => d.Position.Distance(Player.Instance.Position) < 3500).OrderBy(d => d.Distance(Player.Instance)))
                    {
                        int ally = 0;
                        int enemy = 0;
                        foreach (var a in HeroList.Where(d => d.Hero.IsAlly))
                            ally++;
                        foreach (var a in HeroList.Where(d => d.Hero.IsEnemy))
                            enemy++;

                        var database = HeroList.FirstOrDefault(d => d.Hero == hero);
                        if (database == null && HeroList.Count < 4 && ((hero.IsAlly && ally < 2) || (hero.IsEnemy && enemy < 2)))
                            HeroList.Add(new HeroList
                            {
                                Hero = hero,
                                Endtime = 60000 + Utility.TickCount
                            });
                    }
                }

                var herolist = HeroList.FirstOrDefault(d => d.Hero.Distance(Player.Instance) > 3500 && d.Endtime < Utility.TickCount);

                if (herolist != null)
                {
                    HeroList.Remove(herolist);
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("HERO_LIST_MANAGER");
            }
        }
        
        private static void Drawing_OnDraw(EventArgs args)
        {
            try
            {
                var maxSlot = Config.SC2Menu.SliderValue("maxSlot");

                foreach (var sc2slot in SC2SlotManager.SC2SlotList.Where(d => d.Slot < maxSlot))
                {
                    sc2slot.SC2HudSprite();
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("SC2TIMER_ONDRAW");
            }
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            try
            {
                var maxSlot = Config.SC2Menu.SliderValue("maxSlot");

                foreach (var sc2slot in SC2SlotManager.SC2SlotList.Where(d => d.Slot < maxSlot))
                {
                    sc2slot.SC2HudText();
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("SC2TIMER_ONEND");
            }
        }

        private static void SC2HudSprite(this SC2Slot slot)
        {
            try
            {
                var sc2 = slot.Timer;
                var centerpos = new Vector2(Drawing.Width, Drawing.Height / 2) + new Vector2(-240, -slot.Slot * 90);
                var moveTime = 1000;

                if (slot.StartTime + moveTime >= Utility.TickCount)
                {
                    var kappa = (float)(Math.Pow(slot.StartTime + moveTime - Utility.TickCount, 3) / Math.Pow(moveTime, 3) * 240);
                    centerpos = centerpos + new Vector2(kappa, 0);
                }
                if (slot.StartTime + slot.Duration + 3000 - moveTime <= Utility.TickCount)
                {
                    var kappa = 240 - (float)(Math.Pow(Utility.TickCount - (slot.StartTime + slot.Duration + 3000 - moveTime), 3) / Math.Pow(moveTime, 3) * 240);
                    centerpos = centerpos + new Vector2(240 - kappa, 0);
                }


                Drawing.DrawLine(centerpos + new Vector2(0, 41), centerpos + new Vector2(240, 41), 82, System.Drawing.Color.Black);

                TextureDraw.DrawSC2Hud(sc2, centerpos);
            }
            catch (Exception e)
            {
                e.ErrorMessage("SC2_HUD_SPRITE", slot.Timer.DisplayName);
            }
        }

        private static void SC2HudText(this SC2Slot slot)
        {
            try
            {
                var sc2 = slot.Timer;
                var centerpos = new Vector2(Drawing.Width, Drawing.Height / 2) + new Vector2(-240, -slot.Slot * 90);
                var moveTime = 1000f;

                if (slot.StartTime + moveTime >= Utility.TickCount)
                {
                    var kappa = (float)(Math.Pow(slot.StartTime + moveTime - Utility.TickCount, 3) / Math.Pow(moveTime, 3) * 240);
                    centerpos = centerpos + new Vector2(kappa, 0);
                }
                if (slot.StartTime + slot.Duration + 3000 - moveTime <= Utility.TickCount)
                {
                    var kappa = 240 - (float)(Math.Pow(Utility.TickCount - (slot.StartTime + slot.Duration + 3000 - moveTime), 3) / Math.Pow(moveTime, 3) * 240);
                    centerpos = centerpos + new Vector2(240 - kappa, 0);
                }

                var namepos = centerpos + new Vector2(24, 1);
                var timerpos = centerpos + new Vector2(85, 21);

                var name = sc2.DisplayName;
                var remainTime = (sc2.EndTime - Utility.TickCount) / 1000f;

                var timer = remainTime >= 10 ? Math.Truncate(remainTime).ToString() : remainTime >= 0 ? remainTime.ToString("F1") :
                    sc2.SC2Type == SC2Type.Jungle ? "Spawn" : "Ready";
                var barlength = (sc2.EndTime - Utility.TickCount) <= slot.Duration
                    ? (Utility.TickCount - slot.StartTime) / slot.Duration * 129
                    : (Utility.TickCount - sc2.StartTime) / sc2.FullTime * 129;

                if (barlength > 129)
                    barlength = 129;
                var barpos = centerpos + new Vector2(89, 69);
                Drawing.DrawLine(barpos, barpos + new Vector2(barlength, 0), 7,
                sc2.Caster != null && sc2.Caster.IsMe ? System.Drawing.Color.Lime : sc2.Team == Team.Ally ? System.Drawing.Color.DarkCyan : sc2.Team == Team.Enemy ? System.Drawing.Color.Red : System.Drawing.Color.Orange);

                var iconpos = centerpos + new Vector2(25, 25);
                TextureDraw.SpriteList[sc2.GetMenuCode()].Draw(iconpos);

                DrawManager.SC2Font.DrawText(null, name, (int)(namepos).X, (int)(namepos).Y, SharpDX.Color.White);
                DrawManager.SC2Font2.DrawText(null, timer, (int)(timerpos).X, (int)(timerpos).Y, SharpDX.Color.White);

            }
            catch (Exception e)
            {
                e.ErrorMessage("SC2_HUD_TEXT", slot.Timer.DisplayName);
            }
        }

        private static void SC2TimerDetector(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (!sender.IsValid)
                    return;

                var check = Program.SC2TimerList.FirstOrDefault(d => d.Caster == sender && d.ChampionName == sender.BaseSkinName && d.Slot == args.Slot);

                if (check != null)
                {
                    var cooldown = (sender.Spellbook.GetSpell(args.Slot).CooldownExpires - Game.Time) * 1000f;

                    check.FullTime = cooldown;
                    check.StartTime = Utility.TickCount;
                    check.StartTime = Utility.TickCount;
                    check.EndTime = cooldown + Utility.TickCount;

                    return;
                }


                var database = SC2TimerDatabase.Database.FirstOrDefault(d =>
                ((d.SC2Type == SC2Type.SummonerSpell && d.Name == args.SData.Name) ||
                (d.SC2Type == SC2Type.Spell && d.ChampionName == sender.BaseSkinName && d.Slot == args.Slot)));

                if (database != null)
                {
                    var cooldown = (sender.Spellbook.GetSpell(args.Slot).CooldownExpires - Game.Time) * 1000f;

                    Program.SC2TimerList.Add(new SC2Timer
                    {
                        SC2Type = database.SC2Type,
                        Slot = args.Slot,
                        Team = sender.GetTeam(),
                        Caster = sender,
                        ChampionName = sender.BaseSkinName,
                        Name = args.SData.Name,
                        MenuCode = database.GetMenuCode(),
                        DisplayName = database.GetDisplayName(),
                        FullTime = cooldown,
                        StartTime = Utility.TickCount,
                        EndTime = cooldown + Utility.TickCount,
                        Cancel = false,
                        Global = database.Global,
                        SpriteName = database.SpriteName,
                    });

                    return;
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("SC2TIMER_DETECTOR", args.SData.Name);
            }
        }

        private static void SC2JungleDetector(GameObject sender, EventArgs args)
        {
            try
            {
                if (!Config.SC2Menu.CheckboxValue("jungleEnable"))
                    return;

                var check = Program.SC2TimerList.FirstOrDefault(d => d.SC2Type == SC2Type.Jungle && d.Name == sender.Name);

                if (check != null)
                {
                    return;
                }

                var database = SC2TimerDatabase.Database.FirstOrDefault(d => d.SC2Type == SC2Type.Jungle && d.Name == sender.Name);

                if (database != null)
                {
                    Program.SC2TimerList.Add(new SC2Timer
                    {
                        SC2Type = database.SC2Type,
                        Team = Team.Neutral,
                        Name = database.Name,
                        MenuCode = database.GetMenuCode(),
                        DisplayName = database.DisplayName,
                        StartTime = Utility.TickCount,
                        FullTime = database.FullTime,

                        EndTime = database.FullTime + Utility.TickCount,
                        Global = true,
                        SpriteName = database.SpriteName,
                    });

                    return;
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("SC2TIMER_JUNGLE_DETECTOR", sender.Name);
            }
        }

        private static void SC2TimerRemover()
        {
            try
            {
                if (Program.SC2TimerList.Count > 0)
                {
                    Program.SC2TimerList.RemoveAll(d => d.EndTime + 3500 < Utility.TickCount);
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("SC2TIMER_REMOVER");
            }
        }

        public static void Initialize()
        {
            try
            {

            }
            catch (Exception e)
            {
                e.ErrorMessage("SC2TIMER_INIT");
            }
        }
    }
}
