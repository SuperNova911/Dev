using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public float CoolTime;
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
                new SC2Timer { ChampionName = "Gragas", Slot = SpellSlot.E, SpriteName = Resources.EzrealR_BIG },
                new SC2Timer { ChampionName = "Aatrox", Slot = SpellSlot.R, SpriteName = Resources.AatroxR_BIG },
                new SC2Timer { ChampionName = "Ahri", Slot = SpellSlot.R, SpriteName = Resources.AhriR_BIG },
                new SC2Timer { ChampionName = "Alistar", Slot = SpellSlot.R, SpriteName = Resources.AlistarR_BIG },
                new SC2Timer { ChampionName = "Amumu", Slot = SpellSlot.R, SpriteName = Resources.AmumuR_BIG },
                //new SC2Timer { ChampionName = "Anivia", Slot = SpellSlot.R, SpriteName = Resources.AniviaPassive_BIG },
                new SC2Timer { ChampionName = "Annie", Slot = SpellSlot.R, SpriteName = Resources.AnnieR_BIG },
                new SC2Timer { ChampionName = "Ashe", Slot = SpellSlot.R, SpriteName = Resources.AsheR, Global = true },
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
                new SC2Timer { ChampionName = "Ezreal", Slot = SpellSlot.R, SpriteName = Resources.EzrealR, Global = true },
                new SC2Timer { ChampionName = "Fiddlesticks", Slot = SpellSlot.R, SpriteName = Resources.FiddlesticksR_BIG },
                new SC2Timer { ChampionName = "Fiora", Slot = SpellSlot.R, SpriteName = Resources.FioraR_BIG },
                new SC2Timer { ChampionName = "Fizz", Slot = SpellSlot.R, SpriteName = Resources.FizzR_BIG },
                new SC2Timer { ChampionName = "Galio", Slot = SpellSlot.R, SpriteName = Resources.GalioR_BIG },
                new SC2Timer { ChampionName = "Gangplank", Slot = SpellSlot.R, SpriteName = Resources.GangplankR, Global = true },
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
                new SC2Timer { ChampionName = "Karthus", Slot = SpellSlot.R, SpriteName = Resources.KarthusR, Global = true },
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
                new SC2Timer { ChampionName = "Rengar", Slot = SpellSlot.R, SpriteName = Resources.RengarR, Global = true },
                new SC2Timer { ChampionName = "Riven", Slot = SpellSlot.R, SpriteName = Resources.RivenR_BIG },
                new SC2Timer { ChampionName = "Rumble", Slot = SpellSlot.R, SpriteName = Resources.RumbleR_BIG },
                new SC2Timer { ChampionName = "Sejuani", Slot = SpellSlot.R, SpriteName = Resources.SejuaniR_BIG },
                new SC2Timer { ChampionName = "Shaco", Slot = SpellSlot.R, SpriteName = Resources.ShacoR_BIG },
                new SC2Timer { ChampionName = "Shen", Slot = SpellSlot.R, SpriteName = Resources.ShenR, Global = true },
                new SC2Timer { ChampionName = "Shyvana", Slot = SpellSlot.R, SpriteName = Resources.ShyvanaR_BIG },
                new SC2Timer { ChampionName = "Singed", Slot = SpellSlot.R, SpriteName = Resources.SingedR_BIG },
                new SC2Timer { ChampionName = "Sion", Slot = SpellSlot.R, SpriteName = Resources.SionR, Global = true },
                new SC2Timer { ChampionName = "Sivir", Slot = SpellSlot.R, SpriteName = Resources.SivirR_BIG },
                new SC2Timer { ChampionName = "Skarner", Slot = SpellSlot.R, SpriteName = Resources.SkarnerR_BIG },
                new SC2Timer { ChampionName = "Sona", Slot = SpellSlot.R, SpriteName = Resources.SonaR_BIG },
                new SC2Timer { ChampionName = "Soraka", Slot = SpellSlot.R, SpriteName = Resources.SorakaR, Global = true },
                new SC2Timer { ChampionName = "Syndra", Slot = SpellSlot.R, SpriteName = Resources.SyndraR_BIG },
                new SC2Timer { ChampionName = "TahmKench", Slot = SpellSlot.R, SpriteName = Resources.TahmKenchR_BIG },
                new SC2Timer { ChampionName = "Talon", Slot = SpellSlot.R, SpriteName = Resources.TalonR_BIG },
                new SC2Timer { ChampionName = "Taric", Slot = SpellSlot.R, SpriteName = Resources.TaricR_BIG },
                new SC2Timer { ChampionName = "Thresh", Slot = SpellSlot.R, SpriteName = Resources.ThreshR_BIG },
                new SC2Timer { ChampionName = "Tristana", Slot = SpellSlot.R, SpriteName = Resources.TristanaR_BIG },
                new SC2Timer { ChampionName = "Trundle", Slot = SpellSlot.R, SpriteName = Resources.TrundleR_BIG },
                new SC2Timer { ChampionName = "Tryndamere", Slot = SpellSlot.R, SpriteName = Resources.TryndamereR_BIG },
                new SC2Timer { ChampionName = "TwistedFate", Slot = SpellSlot.R, SpriteName = Resources.TwistedFateR, Global = true },
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
                new SC2Timer { ChampionName = "Xerath", Slot = SpellSlot.R, SpriteName = Resources.XerathR, Global = true },
                new SC2Timer { ChampionName = "XinZhao", Slot = SpellSlot.R, SpriteName = Resources.XinZhaoR_BIG },
                new SC2Timer { ChampionName = "Yasuo", Slot = SpellSlot.R, SpriteName = Resources.YasuoR_BIG },
                new SC2Timer { ChampionName = "Yorick", Slot = SpellSlot.R, SpriteName = Resources.YorickR_BIG },
                new SC2Timer { ChampionName = "Zac", Slot = SpellSlot.R, SpriteName = Resources.ZacR_BIG },
                new SC2Timer { ChampionName = "Zed", Slot = SpellSlot.R, SpriteName = Resources.ZedR_BIG },
                new SC2Timer { ChampionName = "Ziggs", Slot = SpellSlot.R, SpriteName = Resources.ZiggsR, Global = true },
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


                new SC2Timer { SC2Type = SC2Type.Jungle, Name = "SRU_Red4.1.1", CoolTime = 300000, DisplayName = "Red Brambleback", SpriteName = Resources.Red_Brambleback_BIG },
                new SC2Timer { SC2Type = SC2Type.Jungle, Name = "SRU_Dragon_death_sound.troy", CoolTime = 360000, DisplayName = "Dragon", SpriteName = Resources.Dragon_BIG },
            };
        }
    }

    public class SC2Slot
    {
        public SC2Timer Timer;
        public int Slot;
    }

    public static class SC2SlotManager
    {
        public static List<SC2Slot> SC2SlotList = new List<SC2Slot>();

        public static void AddOnSlot(this SC2Timer sc2)
        {
            SC2SlotList.Add(new SC2Slot
            {
                Timer = sc2,
                Slot = SC2SlotList.Count,
            });
        }

        public static void RemoveOnSlot(this SC2Slot slot)
        {
            foreach (var list in SC2SlotList.Where(d => d.Slot > slot.Slot))
            {
                list.Slot--;
            }
            SC2SlotList.RemoveAll(d => d == slot);
        }
    }

    public static class SC2TimerManager
    {
        static SC2TimerManager()
        {
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
        }

        private static void Game_OnTick(EventArgs args)
        {
            foreach (var sc2 in Program.SC2TimerList.Where(d => d.Drawing == false && d.EndTime - Utility.TickCount < 10000))
            {
                sc2.AddOnSlot();
                sc2.Drawing = true;
            }

            foreach (var sc2 in SC2SlotManager.SC2SlotList.Where(d => d.Timer.Drawing && d.Timer.EndTime + 3000 < Utility.TickCount))
            {
                sc2.RemoveOnSlot();
                continue;
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            foreach (var sc2slot in SC2SlotManager.SC2SlotList.Where(d => d.Timer.EndTime - Utility.TickCount < 10000))
            {
                sc2slot.SC2HudSprite(10000);
            }
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var sc2slot in SC2SlotManager.SC2SlotList.Where(d => d.Timer.EndTime - Utility.TickCount < 10000))
            {
                sc2slot.SC2HudText(10000);
            }
        }
        
        public static void SC2HudSprite(this SC2Slot slot, int duration)
        {
            var sc2 = slot.Timer;
            var centerpos = new Vector2(Drawing.Width, Drawing.Height / 2) + new Vector2(-240, -slot.Slot * 90);
            var moveTime = 1000;

            if (sc2.EndTime - (duration - moveTime) >= Utility.TickCount)
            {
                var kappa = (float)(Math.Pow(sc2.EndTime - (duration - moveTime) - Utility.TickCount, 3) / Math.Pow(moveTime, 3) * 240);
                centerpos = centerpos + new Vector2(kappa, 0);
            }
            if (sc2.EndTime + 3000 - moveTime <= Utility.TickCount)
            {
                var kappa = 240 - (float)(Math.Pow(Utility.TickCount - (sc2.EndTime + 3000 - moveTime), 3) / Math.Pow(moveTime, 3) * 240);
                centerpos = centerpos + new Vector2(240 - kappa, 0);
            }

            var iconpos = centerpos + new Vector2(25, 25);

            Drawing.DrawLine(centerpos + new Vector2(0, 26), centerpos + new Vector2(240, 26), 82, System.Drawing.Color.Black);

            TextureDraw.DrawSC2Hud(sc2.Team, centerpos);
            TextureDraw.SpriteList[sc2.MenuCode].Draw(iconpos);
        }

        public static void SC2HudText(this SC2Slot slot, int duration)
        {
            var sc2 = slot.Timer;
            var centerpos = new Vector2(Drawing.Width, Drawing.Height / 2) + new Vector2(-240, -slot.Slot * 90);
            var moveTime = 1000;

            if (sc2.EndTime - (duration - moveTime) >= Utility.TickCount)
            {
                var kappa = (float)(Math.Pow(sc2.EndTime - (duration - moveTime) - Utility.TickCount, 3) / Math.Pow(moveTime, 3) * 240);
                centerpos = centerpos + new Vector2(kappa, 0);
            }
            if (sc2.EndTime + 3000 - moveTime <= Utility.TickCount)
            {
                var kappa = 240 - (float)(Math.Pow(Utility.TickCount - (sc2.EndTime + 3000 - moveTime), 3) / Math.Pow(moveTime, 3) * 240);
                centerpos = centerpos + new Vector2(240 - kappa, 0);
            }
            
            var namepos = centerpos + new Vector2(24, 1);
            var timerpos = centerpos + new Vector2(85, 21);
            
            var name = sc2.DisplayName;
            var remainTime = (sc2.EndTime - Utility.TickCount) / 1000f;

            var timer = remainTime >= 10 ? Math.Truncate(remainTime).ToString() : remainTime >= 0 ? remainTime.ToString("F1") :
                sc2.SC2Type == SC2Type.Jungle ? "Spawn" : "Ready";

            var barlength = (Utility.TickCount - (sc2.EndTime - duration)) / duration * 129f;
            if (barlength > 129)
                barlength = 129;
            var barpos = centerpos + new Vector2(89, 54);

            Drawing.DrawLine(barpos, barpos + new Vector2(barlength, 0), 7, System.Drawing.Color.DarkCyan);

            DrawManager.TestFont.DrawText(null, name, (int)(namepos).X, (int)(namepos).Y, SharpDX.Color.White);
            DrawManager.TestFont2.DrawText(null, timer, (int)(timerpos).X, (int)(timerpos).Y, SharpDX.Color.White);
        }

        public static void SC2TimerDetector(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsValid)
                return;
            
            var database = SC2TimerDatabase.Database.FirstOrDefault(d => 
            ((d.SC2Type == SC2Type.SummonerSpell && d.Name == args.SData.Name) ||
            (d.SC2Type == SC2Type.Spell && d.ChampionName == sender.BaseSkinName && d.Slot == args.Slot)));

            if (database != null)
            {
                var cooldown = sender.Spellbook.GetSpell(args.Slot).Cooldown * 1000;

                Program.SC2TimerList.Add(new SC2Timer
                {
                    SC2Type = database.SC2Type,
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
                Chat.Print(cooldown);
                return;
            }
        }

        public static void SC2JungleDetector(GameObject sender, EventArgs args)
        {
            var database = SC2TimerDatabase.Database.FirstOrDefault(d => d.SC2Type == SC2Type.Jungle && d.Name == sender.Name);

            if (database != null)
            {
                Program.SC2TimerList.Add(new SC2Timer
                {
                    SC2Type = database.SC2Type,
                    Team = Team.Neutral,
                    Name = database.Name,
                    MenuCode = "sc2" + database.Name,
                    DisplayName = database.DisplayName,
                    CoolTime = database.CoolTime,
                    StartTime = Utility.TickCount,
                    EndTime = database.CoolTime + Utility.TickCount + 3000,
                    Global = true,
                    SpriteName = database.SpriteName,
                });
            }
        }

        public static void SC2TimerRemover()
        {
            if (Program.SC2TimerList.Count > 0)
            {
                Program.SC2TimerList.RemoveAll(d => d.EndTime + 3500 < Utility.TickCount);
            }
        }
    }
}
