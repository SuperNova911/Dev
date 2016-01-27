﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using SharpDX;
using TimerBuddy.Properties;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using Sprite = EloBuddy.SDK.Rendering.Sprite;

namespace TimerBuddy
{
    public enum Team
    {
        Ally, Enemy, None
    }

    public enum Priority
    {
        Kappa
    }

    public enum SpellType
    {
        SummonerSpell, Item, Trap, Spell, Ward
    }

    public class Spell
    {
        public SpellType SpellType;
        public Team Team = Team.None;
        public SpellSlot Slot;
        public Obj_AI_Base Caster;
        public Obj_AI_Base Target;
        public Vector3 CastPosition;
        public string ChampionName;
        public string Name;
        public string ObjectName;
        public string MenuString;
        public float FullTime;
        public float EndTime;
        public int NetworkID;
        public bool GameObject = false;
        public bool SkillShot = false;
        public bool Targeting = false;
        public bool Cancel = false;
        public bool Buff = false;
        public Color Color = Color.White;
        public Bitmap SpriteName;
    }

    internal static class SpellDatabase
    {
        public static readonly List<Spell> Database;

        static SpellDatabase()
        {
            Database = new List<Spell>
            {
                //new Spell { SpellType = SpellType.SummonerSpell, Name = "summonerteleport", EndTime = 3500, MenuString = "Summoner Teleport", SpriteName = Resources.Teleport },
                //new Spell { SpellType = SpellType.SummonerSpell, Name = "summonerbarrier", EndTime = 2000, MenuString = "Summoner Barrier", SpriteName = Resources.Barrier },
                //new Spell { SpellType = SpellType.SummonerSpell, Name = "summonerhaste", EndTime = 10000, MenuString = "Summoner Haste", SpriteName = Resources.Haste },
                new Spell { SpellType = SpellType.SummonerSpell, Name = "summonerheal", EndTime = 1000, MenuString = "Summoner Heal", SpriteName = Resources.Heal },
                new Spell { SpellType = SpellType.SummonerSpell, Name = "summonerboost", EndTime = 3000, MenuString = "Summoner Cleanse", SpriteName = Resources.Cleanse },

                new Spell { SpellType = SpellType.Item, Name = "ZhonyasHourglass", EndTime = 2500, MenuString = "Zhonyas Hourglass", SpriteName = Resources.Zhonya_s_Hourglass },
                new Spell { SpellType = SpellType.Item, Name = "shurelyascrest", EndTime = 3000, MenuString = "Talisman of Ascension", SpriteName = Resources.Talisman_of_Ascension },
                new Spell { SpellType = SpellType.Item, GameObject = true, Name = "LifeAura.troy", EndTime = 4000, MenuString = "Guardian Angel", SpriteName = Resources.Guardian_Ange },
                new Spell { SpellType = SpellType.Item, GameObject = true, Name = "TrinketLensLvl1Audio.troy", EndTime = 3500, MenuString = "Sweeping Lens (Trinket)", SpriteName = Resources.Sweeping_Lens__Trinket_ },

                //new Spell { SpellType = SpellType.Ward, GameObject = true, Name = "SightWard", EndTime = 60000, MenuString = "Warding Totem" },
                //new Spell { SpellType = SpellType.Ward, GameObject = true, Name = "VisionWard", ObjectName = "SightWard", EndTime = 150000, MenuString = "Sightstone" },
                //new Spell { SpellType = SpellType.Ward, GameObject = true, Name = "VisionWard", ObjectName = "VisionWard", EndTime = 0, MenuString = "Vision Ward" },

                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.R, GameObject = true, ChampionName = "Teemo", Name = "Noxious Trap", ObjectName = "TeemoMushroom", EndTime = 600000, MenuString = "Teemo Trap", SpriteName = Resources.TeemoR },
                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.W, GameObject = true, ChampionName = "Nidalee", Name = "Noxious Trap", ObjectName = "NidaleeSpear", EndTime = 120000, MenuString = "Nidalee Trap", SpriteName = Resources.NidaleeHumanW },
                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.W, GameObject = true, ChampionName = "Caitlyn", Name = "Cupcake Trap", EndTime = 90000, MenuString = "Caitlyn Trap", SpriteName = Resources.CaitlynW },
                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.E, GameObject = true, ChampionName = "Jinx", Name = "JinxEMine", ObjectName = "CaitlynTrap", EndTime = 5000, MenuString = "Jinx Mine", SpriteName = Resources.JinxE },
                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.W, GameObject = true, ChampionName = "Shaco", Name = "Jack In The Box", ObjectName = "ShacoBox", EndTime = 60000, MenuString = "Shaco Box", SpriteName = Resources.ShacoW },

                
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Gangplank", Name = "GangplankE", EndTime = 60000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, GameObject = true, ChampionName = "Gragas", Name = "Gragas_Base_Q_Ally.troy", EndTime = 4000, MenuString = "Gragas Q", SpriteName = Resources.GragasQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, GameObject = true, ChampionName = "Gragas", Name = "Gragas_Base_Q_Enemy.troy", EndTime = 4000, MenuString = "Gragas Q", SpriteName = Resources.GragasQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Graves", Name = "Graves_SmokeGrenade_Cloud_Team_Green.troy", EndTime = 4000, MenuString = "Graves W", SpriteName = Resources.GravesW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Graves", Name = "Graves_SmokeGrenade_Cloud_Team_Red.troy", EndTime = 4000, MenuString = "Graves W", SpriteName = Resources.GravesW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Nasus", Name = "Nasus_Base_E_SpiritFire.troy", EndTime = 4500, MenuString = "Nasus E", SpriteName = Resources.NasusE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Nunu", Name = "Nunu_Base_R_indicator_blue.troy", EndTime = 3000, MenuString = "Nunu R", SpriteName = Resources.NunuR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Nunu", Name = "Nunu_Base_R_indicator_red.troy", EndTime = 3000, MenuString = "Nunu R", SpriteName = Resources.NunuR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Lux", Name = "Lux_Base_E_tar_aoe_green.troy", EndTime = 5000, MenuString = "Lux E", SpriteName = Resources.LuxE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Lux", Name = "Lux_Base_E_tar_aoe_red.troy", EndTime = 5000, MenuString = "Lux E", SpriteName = Resources.LuxE },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Rumble", Name = "", EndTime = 5000, MenuString = "Rumble R", SpriteName = Resources.RumbleR },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "RekSai", Name = "", EndTime = 600000, MenuString = "RekSai E", SpriteName = Resources.RekSaiE },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "RekSai", Name = "", EndTime = 12000, MenuString = "RekSai E", SpriteName = Resources.RekSaiE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "LeBlanc", Name = "LeBlanc_Base_W_return_indicator.troy", EndTime = 4000, MenuString = "LeBlanc W", SpriteName = Resources.LeBlancW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "LeBlanc", Name = "LeBlanc_Base_RW_return_indicator.troy", EndTime = 4000, MenuString = "LeBlanc W", SpriteName = Resources.LeBlancW },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Malzahar", Name = "", EndTime = 2500, MenuString = "Malzahar R", SpriteName = Resources.MalzaharR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "MissFortune", Name = "MissFortune_Base_E_Unit_Tar_green.troy", EndTime = 2500, MenuString = "MissFortune E", SpriteName = Resources.MissFortuneE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "MissFortune", Name = "MissFortune_Base_E_Unit_Tar_red.troy", EndTime = 2500, MenuString = "MissFortune E", SpriteName = Resources.MissFortuneE },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Bard", Name = "", EndTime = 10000, MenuString = "Bard W", SpriteName = Resources.BardW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Veigar", Name = "Veigar_Base_E_cage_green.troy", EndTime = 3000, MenuString = "Veigar E", SpriteName = Resources.VeigarE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Veigar", Name = "Veigar_Base_E_cage_red.troy", EndTime = 3000, MenuString = "Veigar E", SpriteName = Resources.VeigarE },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Braum", Name = "", EndTime = 4000, MenuString = "Braum R", SpriteName = Resources.BraumR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Viktor", Name = "Viktor_ChaosStorm_green.troy", EndTime = 7000, MenuString = "Viktor R", SpriteName = Resources.ViktorR },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Unknown, GameObject = true, ChampionName = "Skarner", Name = "", EndTime = 2500, MenuString = "Skarner Passive", SpriteName = Resources.SkarnerP },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Thresh", Name = "", EndTime = 6000, MenuString = "Thresh W", SpriteName = Resources.ThreshW },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Thresh", Name = "", EndTime = 4000, MenuString = "Thresh R", SpriteName = Resources.ThreshR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Akali", Name = "Akali_Base_smoke_bomb_tar_team_green.troy", EndTime = 8000, MenuString = "Akali W", SpriteName = Resources.AkaliW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Akali", Name = "Akali_Base_smoke_bomb_tar_team_red.troy", EndTime = 8000, MenuString = "Akali W", SpriteName = Resources.AkaliW },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Unknown, GameObject = true, ChampionName = "Anivia", Name = "", EndTime = 6000, MenuString = "Anivia Egg", SpriteName = Resources.AniviaPassive },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Unknown, GameObject = true, ChampionName = "Illaoi", Name = "Illaoi", EndTime = 60000 },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Jarvan", Name = "", EndTime = 3500, MenuString = "Jarvan R", SpriteName = Resources.JarvanR },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Unknown, GameObject = true, ChampionName = "Zyra", Name = "", EndTime = 2000, MenuString = "Zyra Passive", SpriteName = Resources.ZyraP },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, GameObject = true, ChampionName = "Zyra", Name = "", EndTime = 10000, MenuString = "Zyra Q", SpriteName = Resources.ZyraQ },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Zyra", Name = "", EndTime = 30000, MenuString = "Zyra W", SpriteName = Resources.ZyraW },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Zyra", Name = "", EndTime = 10000, MenuString = "Zyra E", SpriteName = Resources.ZyraE },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Unknown, GameObject = true, ChampionName = "Zac", Name = "", EndTime = 8000, MenuString = "Zac Passive", SpriteName = Resources.ZacP },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, GameObject = true, ChampionName = "Janna", Name = "", EndTime = 3000, MenuString = "Janna Q", SpriteName = Resources.JannaQ },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Zed", Name = "", EndTime = 4000, MenuString = "Zed W", SpriteName = Resources.ZedW },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Zed", Name = "", EndTime = 6000, MenuString = "Zed R", SpriteName = Resources.ZedR },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Jayce", Name = "", EndTime = 4000, MenuString = "Jayce E Cannon", SpriteName = Resources.JayceCannonE },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Ziggs", Name = "", EndTime = 4000, MenuString = "Ziggs W", SpriteName = Resources.ZiggsW },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, GameObject = true, ChampionName = "Zilean", Name = "", EndTime = 3000, MenuString = "Zilean Q", SpriteName = Resources.ZileanQ },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, GameObject = true, ChampionName = "Karma", Name = "", EndTime = 1500, MenuString = "Karma Q", SpriteName = Resources.KarmaQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Corki", Name = "Corki_Base_W_tar.troy", EndTime = 2000, MenuString = "Corki W", SpriteName = Resources.CorkiW },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Corki", Name = "", EndTime = 5000, MenuString = "Corki W", SpriteName = Resources.CorkiW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Fizz", Name = "Fizz_Ring_Green.troy", EndTime = 1500, MenuString = "Fizz R", SpriteName = Resources.FizzR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Fizz", Name = "Fizz_Ring_Red.troy", EndTime = 1500, MenuString = "Fizz R", SpriteName = Resources.FizzR },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, GameObject = true, ChampionName = "Heimerdinger", Name = "", EndTime = 8000, MenuString = "Heimerdinger Q", SpriteName = Resources.HeimerdingerQ },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, GameObject = true, ChampionName = "Heimerdinger", Name = "", EndTime = 8000, MenuString = "Heimerdinger Q", SpriteName = Resources.HeimerdingerQ },


                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "MasterYi", Name = "MasterYi_Base_W_Buf.troy", EndTime = 4000, MenuString = "MasterYi W", SpriteName = Resources.MasterYiW },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Shen", Name = "shen_Teleport_target_v2.troy", EndTime = 3000, MenuString = "Shen R", SpriteName = Resources. },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Shen", Name = "ShenTeleport_v2.troy", EndTime = 3000, MenuString = "Shen R", SpriteName = Resources. },
                
                
                /*
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Rumble", Name = "RumbleR", EndTime = 5000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "miss", Name = "MissR", EndTime = 3000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Unknown, GameObject = true, ChampionName = "Anivia", Name = "AniviaEgg", EndTime = 6000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Zed", Name = "zedR", EndTime = 3000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "twist", Name = "twistR", EndTime = 3000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Zilean", Name = "ZileanR", EndTime = 3000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Ziggs", Name = "Ziggs", EndTime = 4000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Janna", Name = "JannaR", EndTime = 3000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Zyra", Name = "ZyraW", EndTime = 30000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Unknown, GameObject = true, ChampionName = "Zyra", Name = "Zyra", EndTime = 1000 },
                */
                
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Garen", EndTime = 2000, MenuString = "Garen W", SpriteName = Resources.GarenW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Gangplank", EndTime = 2000, SkillShot = true, MenuString = "Gangplank R", SpriteName = Resources.GangplankR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Nasus", EndTime = 15000, MenuString = "Nasus R", SpriteName = Resources.NasusR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Nocturne", EndTime = 4000, MenuString = "Nocturne R", SpriteName = Resources.NocturneR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Rammus", EndTime = 6000, MenuString = "Rammus W", SpriteName = Resources.RammusW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Rammus", EndTime = 8000, MenuString = "Rammus R", SpriteName = Resources.RammusR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, ChampionName = "Rumble", EndTime = 3000, MenuString = "Rumble Q", SpriteName = Resources.RumbleQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Rumble", EndTime = 2000, MenuString = "Rumble W", SpriteName = Resources.RumbleW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Renekton", EndTime = 15000, MenuString = "Renekton R", SpriteName = Resources.RenektonR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Leona", EndTime = 3000, MenuString = "Leona W", SpriteName = Resources.LeonaW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Lulu", EndTime = 7000, MenuString = "Lulu R", SpriteName = Resources.LuluR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "MasterYi", EndTime = 5000, MenuString = "MasterYi E", SpriteName = Resources.MasterYiE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Malzahar", EndTime = 4000, SkillShot = true, MenuString = "Malzahar W", SpriteName = Resources.MalzaharW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Malphite", EndTime = 6000, MenuString = "Malphite W", SpriteName = Resources.MalphiteW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Morgana", EndTime = 5000, SkillShot = true, MenuString = "Morgana W", SpriteName = Resources.MorganaW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Morgana", EndTime = 3000, MenuString = "Morgana R", SpriteName = Resources.MorganaR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "DrMundo", EndTime = 12000, MenuString = "DrMundo R", SpriteName = Resources.DrMundoR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "MissFortune", EndTime = 3000, MenuString = "MissFortune W", SpriteName = Resources.MissFortuneW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Varus", EndTime = 4000 + 250, MenuString = "Varus E", SpriteName = Resources.VarusE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Veigar", EndTime = 1200, SkillShot = true, MenuString = "Veigar W", SpriteName = Resources.VeigarW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Volibear", EndTime = 12000, MenuString = "Volibear R", SpriteName = Resources.VolibearR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Brand", EndTime = 500, SkillShot = true, MenuString = "Brand W", SpriteName = Resources.BrandW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Vladimir", EndTime = 2000, MenuString = "Vladimir W", SpriteName = Resources.VladimirW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Viktor", EndTime = 4000, SkillShot = true, MenuString = "Viktor W", SpriteName = Resources.ViktorW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, ChampionName = "Shaco", EndTime = 3500 + 250, MenuString = "Shaco Q", SpriteName = Resources.ShacoQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Soraka", EndTime = 1500, SkillShot = true, MenuString = "Soraka E", SpriteName = Resources.SorakaE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, ChampionName = "Swain", EndTime = 3000, MenuString = "Swain Q", SpriteName = Resources.SwainQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Swain", EndTime = 875, MenuString = "Swain W", SpriteName = Resources.SwainW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Swain", EndTime = 4000, MenuString = "Swain E", SpriteName = Resources.SwainE },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Shen", EndTime = 3000, SkillShot = true, MenuString = "Shen R", SpriteName = Resources.ShenR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Skarner", EndTime = 1750, MenuString = "Skarner R", SpriteName = Resources.SkarnerR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "XinZhao", EndTime = 5000, MenuString = "XinZhao W", SpriteName = Resources.XinZhaoW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Singed", EndTime = 5000, MenuString = "Singed W", SpriteName = Resources.SingedW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Singed", EndTime = 25000, MenuString = "Singed R", SpriteName = Resources.SingedR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Aatrox", EndTime = 12000, MenuString = "Aatrox R", SpriteName = Resources.AatroxR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Alistar", EndTime = 7000, MenuString = "Alistar R", SpriteName = Resources.AlistarR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Annie", EndTime = 5000, MenuString = "Annie E", SpriteName = Resources.AnnieE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Anivia", EndTime = 5000, SkillShot = true, MenuString = "Anivia W", SpriteName = Resources.AniviaW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, ChampionName = "Ashe", EndTime = 4000, MenuString = "Ashe Q", SpriteName = Resources.AsheQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Yasuo", EndTime = 4000, SkillShot = true, MenuString = "Yasuo W", SpriteName = Resources.YasuoW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Yasuo", EndTime = 15000, MenuString = "Yasuo R", SpriteName = Resources.YasuoR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Ekko", EndTime = 3000, SkillShot = true, MenuString = "Ekko W", SpriteName = Resources.EkkoW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "MonkeyKing", EndTime = 1500, MenuString = "Wukong W", SpriteName = Resources.WukongW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "MonkeyKing", EndTime = 4000, MenuString = "Wukong E", SpriteName = Resources.WukongE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "MonkeyKing", EndTime = 4000, MenuString = "Wukong R", SpriteName = Resources.WukongR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Olaf", EndTime = 6000, MenuString = "Olaf W", SpriteName = Resources.OlafW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Olaf", EndTime = 6000, MenuString = "Olaf R", SpriteName = Resources.OlafR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, ChampionName = "Udyr", EndTime = 5000, MenuString = "Udyr Q", SpriteName = Resources.UdyrQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Udyr", EndTime = 5000, MenuString = "Udyr W", SpriteName = Resources.UdyrW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Udyr", EndTime = 5000, MenuString = "Udyr R", SpriteName = Resources.UdyrR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Urgot", EndTime = 1000, MenuString = "Urgot R", SpriteName = Resources.UrgotR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Irelia", EndTime = 6000, MenuString = "Irelia W", SpriteName = Resources.IreliaW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Evelynn", EndTime = 3000, MenuString = "Evelynn W", SpriteName = Resources.EvelynnW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Ezreal", EndTime = 1000, MenuString = "Ezreal R", SpriteName = Resources.EzrealR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Illaoi", EndTime = 8000, MenuString = "Illaoi R", SpriteName = Resources.IllaoiR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Zyra", EndTime = 2000, SkillShot = true, MenuString = "Zyra R", SpriteName = Resources.ZyraR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Jax", EndTime = 8000, MenuString = "Jax R", SpriteName = Resources.JaxR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Xerath", EndTime = 500, SkillShot = true, MenuString = "Xerath W", SpriteName = Resources.XerathW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Ziggs", EndTime = 10000, SkillShot = true, MenuString = "Ziggs E", SpriteName = Resources.ZiggsE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Zilean", EndTime = 2500, MenuString = "Zilean E", SpriteName = Resources.ZileanE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Zilean", EndTime = 5000, MenuString = "Zilean R", SpriteName = Resources.ZileanR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Karma", EndTime = 1500, MenuString = "Karma E", SpriteName = Resources.KarmaE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Karthus", EndTime = 5000, SkillShot = true, MenuString = "Karthus W", SpriteName = Resources.KarthusW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Cassiopeia", EndTime = 7000, SkillShot = true, MenuString = "Cassiopeia W", SpriteName = Resources.CassiopeiaW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, ChampionName = "Caitlyn", EndTime = 500, MenuString = "Caitlyn Q", SpriteName = Resources.CaitlynQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Caitlyn", EndTime = 1000, MenuString = "Caitlyn R", SpriteName = Resources.CaitlynR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Kayle", EndTime = 2500, MenuString = "Kayle W", SpriteName = Resources.KayleW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Kayle", EndTime = 10000, MenuString = "Kayle E", SpriteName = Resources.KayleE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "KogMaw", EndTime = 6000, MenuString = "KogMaw W", SpriteName = Resources.KogMawW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Corki", EndTime = 4000, MenuString = "Corki E", SpriteName = Resources.CorkiE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Quinn", EndTime = 2000, MenuString = "Quinn W", SpriteName = Resources.QuinnW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Quinn", EndTime = 2000, MenuString = "Quinn R", SpriteName = Resources.QuinnR },   //가능하면 버프로 옮기기
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Kindred", EndTime = 4000, SkillShot = true, MenuString = "Kindred R", SpriteName = Resources.KindredR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Taric", EndTime = 10000, MenuString = "Taric R", SpriteName = Resources.TaricR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Talon", EndTime = 3000, MenuString = "Talon E", SpriteName = Resources.TalonE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Trundle", EndTime = 8000, SkillShot = true, MenuString = "Trundle W", SpriteName = Resources.TrundleW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Trundle", EndTime = 6000, SkillShot = true, MenuString = "Trundle E", SpriteName = Resources.TrundleE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, ChampionName = "Tristana", EndTime = 5000, MenuString = "Tristana Q", SpriteName = Resources.TristanaQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Tryndamere", EndTime = 5000, MenuString = "Tryndamere R", SpriteName = Resources.TryndamereR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Twitch", EndTime = 5000, MenuString = "Twitch R", SpriteName = Resources.TwitchR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Teemo", EndTime = 3000, MenuString = "Teemo W", SpriteName = Resources.TeemoW },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Pantheon", EndTime = 3000, SkillShot = true, MenuString = "Pantheon R", SpriteName = Resources.PantheonR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "FiddleSticks", EndTime = 1500 + 250, MenuString = "FiddleSticks R", SpriteName = Resources.FiddlesticksR },    //가능하면 버프로 옮기기
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Fizz", EndTime = 6000, MenuString = "Fizz W", SpriteName = Resources.FizzW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Hecarim", EndTime = 4000, MenuString = "Hecarim W", SpriteName = Resources.HecarimW },




                //new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "SummonerBarrier", MenuString = "Summoner Barrier" },
                //new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "Teleport", MenuString = "Summoner Teleport" },
                //new Spell { SpellType = SpellType.Item, Buff = true, Name = "Zhonyas Ring", MenuString = "Zhonyas Hourglass" },
                new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "Recall", MenuString = "Recall", SpriteName = Resources.Recall },
                new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "S4SpawnLockSpeed", MenuString = "Happy", SpriteName = Resources.Haste },
                new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "SRHomeguardSpeed", MenuString = "Happy2", SpriteName = Resources.Haste },
                new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "SummonerBarrier", MenuString = "Summoner Barrier", SpriteName = Resources.Barrier },
                new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "Haste", MenuString = "Summoner Haste", SpriteName = Resources.Haste },
                new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "Teleport", MenuString = "Summoner Teleport", SpriteName = Resources.Teleport },
                new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "SummonerIgnite", MenuString = "Summoner Ignite", SpriteName = Resources.Ignite },
                new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "SummonerExhaustDebuff", MenuString = "Summoner Exhaust", SpriteName = Resources.Exhaust },

                new Spell { SpellType = SpellType.Item, Buff = true, Name = "HealthBomb", MenuString = "Face of the Mountain", SpriteName = Resources.Face_of_the_Mountain },
                new Spell { SpellType = SpellType.Item, Buff = true, Name = "IronStylusBuff", MenuString = "Locket of the Iron Solari", SpriteName = Resources.Locket_of_the_Iron_Solari },
                new Spell { SpellType = SpellType.Item, Buff = true, Name = "SpectralFury", MenuString = "Youmuu's Ghostblade", SpriteName = Resources.Youmuu_s_Ghostblade },
                new Spell { SpellType = SpellType.Item, Buff = true, Name = "Health Potion", MenuString = "Health Potion", SpriteName = Resources.Health_Potion },
                new Spell { SpellType = SpellType.Item, Buff = true, Name = "ItemMiniRegenPotion", MenuString = "Biscuit", SpriteName = Resources.Total_Biscuit_of_Rejuvenation },
                new Spell { SpellType = SpellType.Item, Buff = true, Name = "ItemCrystalFlask", MenuString = "Refillable_Potion", SpriteName = Resources.Refillable_Potion },
                new Spell { SpellType = SpellType.Item, Buff = true, Name = "ItemCrystalFlaskJungle", MenuString = "Hunter's Potion", SpriteName = Resources.Hunter_s_Potion },
                new Spell { SpellType = SpellType.Item, Buff = true, Name = "ItemDarkCrystalFlask", MenuString = "Corrupting Potion", SpriteName = Resources.Corrupting_Potion },
                new Spell { SpellType = SpellType.Item, Buff = true, Name = "ElixirOfWrath", MenuString = "Elixir_of_Wrath", SpriteName = Resources.Elixir_of_Wrath },
                new Spell { SpellType = SpellType.Item, Buff = true, Name = "ElixirOfSorcery", MenuString = "Elixir of Sorcery", SpriteName = Resources.Elixir_of_Sorcery },
                new Spell { SpellType = SpellType.Item, Buff = true, Name = "ElixirOfIron", MenuString = "Elixir of Iron", SpriteName = Resources.Elixir_of_Iron },


                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Garen", Name = "GarenQBuff", MenuString = "Garen Q", SpriteName = Resources.GarenQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Garen", Name = "GarenE", MenuString = "Garen E", SpriteName = Resources.GarenE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Galio", Name = "GalioBulwark", MenuString = "Galio W", SpriteName = Resources. },   이미지만 추가
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Galio", Name = "", MenuString = "Galio R", SpriteName = Resources. },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Gangplank", Name = "", MenuString = "Gangplank R Buff", SpriteName = Resources.GangplankBuff },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Gangplank", Name = "", MenuString = "Gangplank R Buff", SpriteName = Resources.GangplankDeBuff },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Gragas", Name = "GragasWAttackBuff", MenuString = "Gragas W", SpriteName = Resources.GragasW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Graves", Name = "GravesEGrit", MenuString = "Graves E", SpriteName = Resources.GravesE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Gnar", Name = "gnartransformsoon", MenuString = "Gnar Transform", SpriteName = Resources.GnarTransformSoon },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Gnar", Name = "GnarWBuff", MenuString = "Gnar W", SpriteName = Resources.GnarWBuff },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Nami", Name = "", MenuString = "Nami Passive", SpriteName = Resources.NamiPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Nami", Name = "", MenuString = "Nami E", SpriteName = Resources.NamiE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Nasus", Name = "NasusW", MenuString = "Nasus W", SpriteName = Resources.NasusW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Nautilus", Name = "", MenuString = "Nautilus W", SpriteName = Resources.NautilusW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Nocturne", Name = "", MenuString = "Nocturne Q", SpriteName = Resources.NocturneQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Nocturne", Name = "", MenuString = "Nocturne W", SpriteName = Resources.NocturneW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Nocturne", Name = "", MenuString = "Nocturne E", SpriteName = Resources.NocturneE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Nunu", Name = "", MenuString = "Nunu Q", SpriteName = Resources.NunuQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Nunu", Name = "Blood Boil", MenuString = "Nunu W", SpriteName = Resources.NunuW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Nunu", Name = "IceBlast", MenuString = "Nunu E", SpriteName = Resources.NunuE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Nidalee", Name = "NidaleePassiveHunted", MenuString = "Nidalee Passive", SpriteName = Resources.NidaleeP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Nidalee", Name = "PrimalSurge", MenuString = "Nidalee E", SpriteName = Resources.NidaleeHumanE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Darius", Name = "DariusHemo", MenuString = "Darius Passive", SpriteName = Resources.DariusPassive },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Darius", Name = "DariusExecuteMulticast", MenuString = "Darius R", SpriteName = Resources.DariusR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Diana", Name = "DianaPassiveMarker", MenuString = "Diana Passive", SpriteName = Resources.DianaP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Diana", Name = "DianaMoonlight", MenuString = "Diana Q", SpriteName = Resources.DianaQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Diana", Name = "DianaOrbs", MenuString = "Diana W", SpriteName = Resources.DianaW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Draven", Name = "", MenuString = "Draven Q", SpriteName = Resources.DravenQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Draven", Name = "", MenuString = "Draven W", SpriteName = Resources.DravenW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ryze", Name = "RyzePassiveStack", MenuString = "Ryze Passive", SpriteName = Resources.RyzeP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ryze", Name = "RyzePassiveCharged", MenuString = "Ryze Passive", SpriteName = Resources.RyzeP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ryze", Name = "RyzeR", MenuString = "Ryze R", SpriteName = Resources.RyzeR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Rammus", Name = "PowerBall", MenuString = "Rammus Q", SpriteName = Resources.RammusQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Lux", Name = "LuxIlluminatingFraulein", MenuString = "Lux Passive", SpriteName = Resources.LuxP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Lux", Name = "LuxShield", MenuString = "Lux W", SpriteName = Resources.LuxW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Leona", Name = "LeonaShieldOfDaybreak", MenuString = "Leona Q", SpriteName = Resources.LeonaQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "RekSai", Name = "", MenuString = "RekSai Q", SpriteName = Resources.RekSaiQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Rengar", Name = "", MenuString = "Rengar Q", SpriteName = Resources.RengarQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Rengar", Name = "", MenuString = "Rengar W", SpriteName = Resources.RengarW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Rengar", Name = "RengarRBuff", MenuString = "Rengar R", SpriteName = Resources.RengarR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Lucian", Name = "LucianPassiveBuff", MenuString = "Lucian Passive", SpriteName = Resources.LucianP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Lucian", Name = "LucianWBuff", MenuString = "Lucian W", SpriteName = Resources.LucianW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Lucian", Name = "lucianwdebuff", MenuString = "Lucian W", SpriteName = Resources.LucianW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Lucian", Name = "", MenuString = "Lucian R", SpriteName = Resources.LucianR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Lulu", Name = "", MenuString = "Lulu W", SpriteName = Resources.LuluW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Lulu", Name = "", MenuString = "Lulu E", SpriteName = Resources.LuluE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "LeBlanc", Name = "", MenuString = "LeBlanc P", SpriteName = Resources.LeBlancP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "LeBlanc", Name = "", MenuString = "LeBlanc Q", SpriteName = Resources.LeBlancQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "LeBlanc", Name = "", MenuString = "LeBlanc E", SpriteName = Resources.LeBlancE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "LeeSin", Name = "BlindMonkSonicWave", MenuString = "LeeSin Q", SpriteName = Resources.LeeSinQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "LeeSin", Name = "BlindMonkSafeguard", MenuString = "LeeSin W", SpriteName = Resources.LeeSinW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "LeeSin", Name = "BlindMonkIronWill", MenuString = "LeeSin W2", SpriteName = Resources.LeeSinWW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "LeeSin", Name = "BlindMonkTempest", MenuString = "LeeSin E", SpriteName = Resources.LeeSinE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "LeeSin", Name = "BlindMonkCripple", MenuString = "LeeSin E2", SpriteName = Resources.LeeSinEE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Riven", Name = "", MenuString = "Riven Passive", SpriteName = Resources.RivenP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Riven", Name = "", MenuString = "Riven Q", SpriteName = Resources.RivenQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Riven", Name = "", MenuString = "Riven E", SpriteName = Resources.RivenE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Riven", Name = "", MenuString = "Riven R", SpriteName = Resources.RivenR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Lissandra", Name = "", MenuString = "Lissandra R", SpriteName = Resources.LissandraR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "MasterYi", Name = "", MenuString = "MasterYi W", SpriteName = Resources.MasterYiW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "MasterYi", Name = "", MenuString = "MasterYi R", SpriteName = Resources.MasterYiR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Malzahar", Name = "AlZaharMaleficVisions", MenuString = "Malzahar E", SpriteName = Resources.MalzaharE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Malzahar", Name = "AlZaharNetherGrasp", MenuString = "Malzahar R", SpriteName = Resources.MalzaharR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Malzahar", Name = "", MenuString = "Malzahar R", SpriteName = Resources.MalzaharR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Malphite", Name = "", MenuString = "Malphite P", SpriteName = Resources.MalphiteP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Malphite", Name = "", MenuString = "Malphite Q", SpriteName = Resources.MalphiteQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Malphite", Name = "", MenuString = "Malphite E", SpriteName = Resources.MalphiteE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Mordekaiser", Name = "", MenuString = "Mordekaiser W", SpriteName = Resources.MordekaiserW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Mordekaiser", Name = "", MenuString = "Mordekaiser E", SpriteName = Resources.MordekaiserE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Mordekaiser", Name = "", MenuString = "Mordekaiser R", SpriteName = Resources.MordekaiserR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Morgana", Name = "Black Shield", MenuString = "Morgana E", SpriteName = Resources.MorganaE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "MissFortune", Name = "MissFortuneViciousStrikes", MenuString = "MissFortune W", SpriteName = Resources.MissFortuneW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "MissFortune", Name = "MissFortuneBulletSound", MenuString = "MissFortune R", SpriteName = Resources.MissFortuneR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Bard", Name = "", MenuString = "Bard P", SpriteName = Resources.BardPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Bard", Name = "", MenuString = "Bard Q", SpriteName = Resources.BardQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Varus", Name = "", MenuString = "Varus Passive", SpriteName = Resources.VarusP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Varus", Name = "", MenuString = "Varus Q", SpriteName = Resources.VarusQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Varus", Name = "", MenuString = "Varus W", SpriteName = Resources.VarusW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Varus", Name = "", MenuString = "Varus R", SpriteName = Resources.VarusR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vi", Name = "", MenuString = "Vi Passive", SpriteName = Resources.ViP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vi", Name = "", MenuString = "Vi Q", SpriteName = Resources.ViQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vi", Name = "", MenuString = "Vi W", SpriteName = Resources.ViW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vi", Name = "", MenuString = "Vi R", SpriteName = Resources.ViR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vayne", Name = "", MenuString = "Vayne Q", SpriteName = Resources.VayneQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vayne", Name = "", MenuString = "Vayne W", SpriteName = Resources.VayneW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vayne", Name = "", MenuString = "Vayne E", SpriteName = Resources.VayneE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vayne", Name = "", MenuString = "Vayne R", SpriteName = Resources.VayneR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vayne", Name = "", MenuString = "Vayne R", SpriteName = Resources.VayneR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Velkoz", Name = "", MenuString = "Velkoz Passive", SpriteName = Resources.VelkozP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Velkoz", Name = "", MenuString = "Velkoz R", SpriteName = Resources.VelkozR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Volibear", Name = "", MenuString = "Volibear Passive", SpriteName = Resources.VolibearP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Volibear", Name = "", MenuString = "Volibear Q", SpriteName = Resources.VolibearQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Volibear", Name = "", MenuString = "Volibear W", SpriteName = Resources.VolibearW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Volibear", Name = "", MenuString = "Volibear E", SpriteName = Resources.VolibearE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Braum", Name = "", MenuString = "Braum Passive", SpriteName = Resources.BraumPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Braum", Name = "", MenuString = "Braum Q", SpriteName = Resources.BraumQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Braum", Name = "", MenuString = "Braum W", SpriteName = Resources.BraumW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Braum", Name = "", MenuString = "Braum E", SpriteName = Resources.BraumE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Brand", Name = "", MenuString = "Brand Passive", SpriteName = Resources.BrandPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vladimir", Name = "", MenuString = "Vladimir E", SpriteName = Resources.VladimirE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vladimir",Name = "VladimirHemoplagueDebuff", MenuString = "Vladimir R", SpriteName = Resources.VladimirR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Blitzcrank",Name = "", MenuString = "Blitzcrank Passive", SpriteName = Resources.BlitzcrankPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Blitzcrank",Name = "", MenuString = "Blitzcrank W", SpriteName = Resources.BlitzcrankW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Blitzcrank",Name = "", MenuString = "Blitzcrank E", SpriteName = Resources.BlitzcrankE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Blitzcrank",Name = "", MenuString = "Blitzcrank R", SpriteName = Resources.BlitzcrankR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Viktor",Name = "ViktorPowerTransferReturn", MenuString = "Viktor Q", SpriteName = Resources.ViktorQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Viktor",Name = "ViktorPowerTransferReturn", MenuString = "Viktor Q", SpriteName = Resources.ViktorQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Viktor",Name = "", MenuString = "Viktor Q Evolution", SpriteName = Resources.ViktorQQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Poppy",Name = "poppypassivecooldown", MenuString = "Poppy Passive", SpriteName = Resources.PoppyP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Poppy",Name = "PoppyPassiveShield", MenuString = "Poppy Passive", SpriteName = Resources.PoppyP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Poppy",Name = "PoppyWZone", MenuString = "Poppy W", SpriteName = Resources.PoppyW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Poppy",Name = "PoppyR", MenuString = "Poppy R", SpriteName = Resources.PoppyR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sion",Name = "SionPassiveSpeed", MenuString = "Sion Passive", SpriteName = Resources.SionP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sion",Name = "SionQ", MenuString = "Sion Q", SpriteName = Resources.SionQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sion", Name = "SionWShieldStacks", MenuString = "Sion W", SpriteName = Resources.SionW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sion", Name = "SionR", MenuString = "Sion R", SpriteName = Resources.SionR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Shaco", Name = "Two Shiv Poison", MenuString = "Shaco E", SpriteName = Resources.ShacoE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sejuani", Name = "", MenuString = "Sejuani Passive", SpriteName = Resources.SejuaniP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sejuani", Name = "", MenuString = "Sejuani Q", SpriteName = Resources.SejuaniQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sejuani", Name = "", MenuString = "Sejuani W", SpriteName = Resources.SejuaniW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sejuani", Name = "", MenuString = "Sejuani W", SpriteName = Resources.SejuaniE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sejuani", Name = "", MenuString = "Sejuani W", SpriteName = Resources.SejuaniR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sona", Name = "SonaQProcAttacker", MenuString = "Sona Q", SpriteName = Resources.SonaQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sona", Name = "SonaWShield", MenuString = "Sona W", SpriteName = Resources.SonaW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sona", Name = "SonaEZone", MenuString = "Sona E", SpriteName = Resources.SonaE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sona", Name = "SonaR", MenuString = "Sona R", SpriteName = Resources.SonaR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Shen", Name = "Shen Vorpal Star", MenuString = "Shen Q", SpriteName = Resources.ShenQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Shen", Name = "Shen Feint Buff", MenuString = "Shen W", SpriteName = Resources.ShenW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Shen", Name = "", MenuString = "Shen R", SpriteName = Resources.ShenR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Shyvana", Name = "", MenuString = "Shyvana W", SpriteName = Resources.ShyvanaW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Shyvana", Name = "", MenuString = "Shyvana E", SpriteName = Resources.ShyvanaE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Skarner", Name = "", MenuString = "Skarner Q", SpriteName = Resources.SkarnerQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Skarner", Name = "", MenuString = "Skarner W", SpriteName = Resources.SkarnerW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Skarner", Name = "", MenuString = "Skarner E", SpriteName = Resources.SkarnerE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sivir", Name = "", MenuString = "Sivir Passive", SpriteName = Resources.SivirP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sivir", Name = "", MenuString = "Sivir E", SpriteName = Resources.SivirE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sivir", Name = "", MenuString = "Sivir R", SpriteName = Resources.SivirR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "XinZhao", Name = "XenZhaoComboTarget", MenuString = "XinZhao Q", SpriteName = Resources.XinZhaoQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "XinZhao", Name = "", MenuString = "XinZhao R", SpriteName = Resources.XinZhaoR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Singed", Name = "", MenuString = "Singed Q", SpriteName = Resources.SingedQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Thresh", Name = "", MenuString = "Thresh Q", SpriteName = Resources.ThreshQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ahri", Name = "", MenuString = "Ahri W", SpriteName = Resources.AhriW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ahri", Name = "", MenuString = "Ahri R", SpriteName = Resources.AhriR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Amumu", Name = "", MenuString = "Amumu Passive", SpriteName = Resources.AmumuPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Akali", Name = "", MenuString = "Akali Passive", SpriteName = Resources.AkaliPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Alistar", Name = "", MenuString = "Alistar Passive", SpriteName = Resources.AlistarPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ashe", Name = "", MenuString = "Ashe Q", SpriteName = Resources.AsheQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Yasuo", Name = "", MenuString = "Yasuo Q", SpriteName = Resources.YasuoQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ekko", Name = "", MenuString = "Ekko Passive", SpriteName = Resources.EkkoPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ekko", Name = "", MenuString = "Ekko W", SpriteName = Resources.EkkoW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ekko", Name = "", MenuString = "Ekko E", SpriteName = Resources.EkkoE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Elise", Name = "", MenuString = "Elise W", SpriteName = Resources.EliseSpiderW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Elise", Name = "", MenuString = "Elise E", SpriteName = Resources.EliseSpiderEInitial },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Orianna", Name = "OrianaRedactShield", MenuString = "Orianna W", SpriteName = Resources.OriannaW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Orianna", Name = "OrianaDissonanceAlly", MenuString = "Orianna E", SpriteName = Resources.OriannaE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Orianna", Name = "OrianaDissonanceEnemy", MenuString = "Orianna E", SpriteName = Resources.OriannaE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Udyr", Name = "", MenuString = "Udyr Passive", SpriteName = Resources.UdyrP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Udyr", Name = "", MenuString = "Udyr E", SpriteName = Resources.UdyrE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Urgot", Name = "", MenuString = "Urgot Passive", SpriteName = Resources.UrgotP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Urgot", Name = "", MenuString = "Urgot W", SpriteName = Resources.UrgotW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Urgot", Name = "", MenuString = "Urgot E", SpriteName = Resources.UrgotE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Warwick", Name = "", MenuString = "Warwick Passive", SpriteName = Resources.WarwickP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Warwick", Name = "", MenuString = "Warwick W", SpriteName = Resources.WarwickW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Warwick", Name = "", MenuString = "Warwick R", SpriteName = Resources.WarwickR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Irelia", Name = "", MenuString = "Irelia R", SpriteName = Resources.IreliaRR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Evelynn", Name = "", MenuString = "Evelynn Passive", SpriteName = Resources.EvelynnPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Evelynn", Name = "", MenuString = "Evelynn E", SpriteName = Resources.EvelynnE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Evelynn", Name = "", MenuString = "Evelynn R", SpriteName = Resources.EvelynnR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ezreal", Name = "", MenuString = "Ezreal Passive", SpriteName = Resources.EzrealPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ezreal", Name = "", MenuString = "Ezreal W", SpriteName = Resources.EzrealW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jarvan", Name = "", MenuString = "Jarvan Passive", SpriteName = Resources.JarvanPassvie },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jarvan", Name = "", MenuString = "Jarvan Q", SpriteName = Resources.JarvanQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jarvan", Name = "", MenuString = "Jarvan W", SpriteName = Resources.JarvanW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jarvan", Name = "", MenuString = "Jarvan E", SpriteName = Resources.JarvanE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Janna", Name = "Eye Of The Storm", MenuString = "Janna E", SpriteName = Resources.JannaE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Janna", Name = "Reap The Whirlwind", MenuString = "Janna R", SpriteName = Resources.JannaR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jax", Name = "", MenuString = "Jax Passive", SpriteName = Resources.JaxPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jax", Name = "", MenuString = "Jax W", SpriteName = Resources.JaxW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jax", Name = "", MenuString = "Jax E", SpriteName = Resources.JaxE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Zed", Name = "", MenuString = "Zed R", SpriteName = Resources.ZedR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Xerath", Name = "", MenuString = "Xerath Passive", SpriteName = Resources.XerathP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Xerath", Name = "", MenuString = "Xerath Q", SpriteName = Resources.XerathQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Xerath", Name = "", MenuString = "Xerath R", SpriteName = Resources.XerathR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jayce", Name = "", MenuString = "Jayce Passive", SpriteName = Resources.JaycePassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jayce", Name = "", MenuString = "Jayce W Hammer", SpriteName = Resources.JayceHammerW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jayce", Name = "", MenuString = "Jayce W Cannon", SpriteName = Resources.JayceCannonW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jayce", Name = "", MenuString = "Jayce R Hammer", SpriteName = Resources.JayceHammerR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jayce", Name = "", MenuString = "Jayce R Cannon", SpriteName = Resources.JayceCannonR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ziggs", Name = "", MenuString = "Ziggs Passive", SpriteName = Resources.ZiggsP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jinx", Name = "", MenuString = "Jinx Passive", SpriteName = Resources.JinxPassive },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Jinx", Name = "", MenuString = "Jinx Q", SpriteName = Resources.JinxQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Karma", Name = "", MenuString = "Karma W", SpriteName = Resources.KarmaW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kassadin", Name = "", MenuString = "Kassadin Q", SpriteName = Resources.KassadinQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kassadin", Name = "", MenuString = "Kassadin W", SpriteName = Resources.KassadinW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kassadin", Name = "", MenuString = "Kassadin R", SpriteName = Resources.KassadinR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Karthus", Name = "", MenuString = "Karthus Passive", SpriteName = Resources.KarthusP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Karthus", Name = "", MenuString = "Karthus W", SpriteName = Resources.KarthusW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Karthus", Name = "", MenuString = "Karthus R", SpriteName = Resources.KarthusR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Cassiopeia", Name = "", MenuString = "Cassiopeia Q", SpriteName = Resources.CassiopeiaQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Cassiopeia", Name = "", MenuString = "Cassiopeia W", SpriteName = Resources.CassiopeiaW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Khazix", Name = "", MenuString = "Khazix R", SpriteName = Resources.KhazixR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Katarina", Name = "", MenuString = "Katarina Passive", SpriteName = Resources.KatarinaP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Katarina", Name = "", MenuString = "Katarina Q", SpriteName = Resources.KatarinaQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Katarina", Name = "", MenuString = "Katarina W", SpriteName = Resources.KatarinaW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Katarina", Name = "", MenuString = "Katarina E", SpriteName = Resources.KatarinaE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Katarina", Name = "", MenuString = "Katarina R", SpriteName = Resources.KatarinaR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kalista", Name = "", MenuString = "Kalista W", SpriteName = Resources.KalistaW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kalista", Name = "", MenuString = "Kalista E", SpriteName = Resources.KalistaE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kennen", Name = "", MenuString = "Kennen Passive", SpriteName = Resources.KennenP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kennen", Name = "KennenLightningRush", MenuString = "Kennen E", SpriteName = Resources.KennenE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kennen", Name = "KennenShurikenStorm", MenuString = "Kennen R", SpriteName = Resources.KennenR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kayle", Name = "", MenuString = "Kayle Passive", SpriteName = Resources.KayleP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kayle", Name = "", MenuString = "Kayle Q", SpriteName = Resources.KayleQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kayle", Name = "", MenuString = "Kayle R", SpriteName = Resources.KayleR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "KogMaw", Name = "", MenuString = "KogMaw Passive", SpriteName = Resources.KogMawP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "KogMaw", Name = "", MenuString = "KogMaw Q", SpriteName = Resources.KogMawQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "KogMaw", Name = "", MenuString = "KogMaw E", SpriteName = Resources.KogMawE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "KogMaw", Name = "", MenuString = "KogMaw R", SpriteName = Resources.KogMawR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Corki", Name = "CorkiLoaded", MenuString = "Corki Package", SpriteName = Resources.CorkiBoom },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Corki", Name = "", MenuString = "Corki Q", SpriteName = Resources.CorkiQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Quinn", Name = "", MenuString = "Quinn Passive", SpriteName = Resources.QuinnP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Taric", Name = "TaricGemcraftBuff", MenuString = "Taric Passive", SpriteName = Resources.TaricP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Taric", Name = "Shatter", MenuString = "Taric W", SpriteName = Resources.TaricW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Talon", Name = "", MenuString = "Talon Q", SpriteName = Resources.TalonQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Talon", Name = "", MenuString = "Talon Q", SpriteName = Resources.TalonQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Talon", Name = "", MenuString = "Talon R", SpriteName = Resources.TalonR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "TahmKench", Name = "", MenuString = "TahmKench Passive", SpriteName = Resources.TahmKenchP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "TahmKench", Name = "", MenuString = "TahmKench W", SpriteName = Resources.TahmKenchW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "TahmKench", Name = "", MenuString = "TahmKench E", SpriteName = Resources.TahmKenchE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "TahmKench", Name = "", MenuString = "TahmKench R", SpriteName = Resources.TahmKenchR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Trundle", Name = "TrundleQDebuff", MenuString = "Trundle Q", SpriteName = Resources.TrundleQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Trundle", Name = "TrundleCircleSlow", MenuString = "Trundle E", SpriteName = Resources.TrundleE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Trundle", Name = "", MenuString = "Trundle R", SpriteName = Resources.TrundleR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Tristana", Name = "", MenuString = "Tristana E", SpriteName = Resources.TristanaE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Tryndamere", Name = "", MenuString = "Tryndamere W", SpriteName = Resources.TryndamereW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "TwistedFate", Name = "", MenuString = "TwistedFate W", SpriteName = Resources.TwistedFateW },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "TwistedFate", Name = "", MenuString = "TwistedFate R", SpriteName = Resources.TwistedFateR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "TwistedFate", Name = "", MenuString = "TwistedFate R", SpriteName = Resources.TwistedFateR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Twitch", Name = "", MenuString = "Twitch Passive", SpriteName = Resources.TwitchP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Twitch", Name = "", MenuString = "Twitch Q", SpriteName = Resources.TwitchQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Teemo", Name = "", MenuString = "Teemo Passive", SpriteName = Resources.TeemoP },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Teemo", Name = "", MenuString = "Teemo Passive", SpriteName = Resources.TeemoP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Pantheon", Name = "PantheonESound", MenuString = "Pantheon E", SpriteName = Resources.PantheonE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Pantheon", Name = "PantheonR", MenuString = "Pantheon R", SpriteName = Resources.PantheonR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "FiddleSticks", Name = "", MenuString = "Fiddlesticks Passive", SpriteName = Resources.FiddlesticksPassive },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "FiddleSticks", Name = "Drain", MenuString = "FiddleSticks W", SpriteName = Resources.FiddlesticksW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Fiora", Name = "FioraQ", MenuString = "Fiora Q", SpriteName = Resources.FioraQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Fiora", Name = "FioraW", MenuString = "Fiora W", SpriteName = Resources.FioraW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Fiora", Name = "FioraE", MenuString = "Fiora E", SpriteName = Resources.FioraE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Fiora", Name = "FioraE2", MenuString = "Fiora E", SpriteName = Resources.FioraE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Fiora", Name = "FioraRMark", MenuString = "Fiora R", SpriteName = Resources.FioraR },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Hecarim", Name = "", MenuString = "Hecarim Q", SpriteName = Resources.HecarimQ },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Hecarim", Name = "", MenuString = "Hecarim E", SpriteName = Resources.HecarimE },
            };
        }
    }

    
    /*
    public class Timer
    {
        public ObjType type;

        public Team team = Team.None;

        public Obj_AI_Base caster;

        public Vector3 castPosition;

        public string name;

        public float endTime;

        public bool cancel;

        public SpellSlot slot;

        public bool skillshot = false;
    }

    public class TimerInfo
    {
        public ObjType type;

        public string name;

        public float endTime;

        public SpellSlot slot;

        public string championName;

        public bool skillshot = false;
    }

    public class GameObj
    {
        public ObjType type;

        public Team team = Team.None;

        public Vector3 position;

        public float endTime;

        public string name;
        
        public int networkID;
    }

    public class GameObjInfo
    {
        public ObjType type;

        public string name;
        
        public float endTime;
    }*/
    
    public class BuffInfo
    {
        public string buffname;
    }
}
