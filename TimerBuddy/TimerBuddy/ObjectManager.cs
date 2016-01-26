using System;
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

    /*public enum ObjType
    {
        Teleport, Item, Spell, Trap
    }*/

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
                new Spell { SpellType = SpellType.Item, GameObject = true, Name = "TrinketLensLvl1Audio.troy", EndTime = 6000, MenuString = "Sweeping Lens (Trinket)", SpriteName = Resources.Sweeping_Lens__Trinket_ },

                //new Spell { SpellType = SpellType.Ward, GameObject = true, Name = "SightWard", EndTime = 60000, MenuString = "Warding Totem" },
                //new Spell { SpellType = SpellType.Ward, GameObject = true, Name = "VisionWard", ObjectName = "SightWard", EndTime = 150000, MenuString = "Sightstone" },
                //new Spell { SpellType = SpellType.Ward, GameObject = true, Name = "VisionWard", ObjectName = "VisionWard", EndTime = 0, MenuString = "Vision Ward" },

                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.R, GameObject = true, ChampionName = "Teemo", Name = "Noxious Trap", ObjectName = "TeemoMushroom", EndTime = 600000, MenuString = "Teemo Trap", SpriteName = Resources.TeemoR },
                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.W, GameObject = true, ChampionName = "Nidalee", Name = "Noxious Trap", ObjectName = "NidaleeSpear", EndTime = 120000, MenuString = "Nidalee Trap", SpriteName = Resources.NidaleeHumanW },
                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.W, GameObject = true, ChampionName = "Caitlyn", Name = "Cupcake Trap", EndTime = 90000, MenuString = "Caitlyn Trap", SpriteName = Resources.CaitlynW },
                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.E, GameObject = true, ChampionName = "Jinx", Name = "JinxEMine", ObjectName = "CaitlynTrap", EndTime = 5000, MenuString = "Jinx Mine", SpriteName = Resources.JinxE },
                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.W, GameObject = true, ChampionName = "Shaco", Name = "Jack In The Box", ObjectName = "ShacoBox", EndTime = 60000, MenuString = "Shaco Box", SpriteName = Resources.ShacoW },
                
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, GameObject = true, ChampionName = "Gragas", Name = "Gragas_Base_Q_Ally.troy", EndTime = 4000, MenuString = "Gragas Q", SpriteName = Resources.GragasQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, GameObject = true, ChampionName = "Gragas", Name = "Gragas_Base_Q_Enemy.troy", EndTime = 4000, MenuString = "Gragas Q", SpriteName = Resources.GragasQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Nasus", Name = "Nasus_Base_E_SpiritFire.troy", EndTime = 4500, MenuString = "Nasus E", SpriteName = Resources.NasusE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Lux", Name = "Lux_Base_E_tar_aoe_green.troy", EndTime = 5000, MenuString = "Lux E", SpriteName = Resources.LuxE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Lux", Name = "Lux_Base_E_tar_aoe_red.troy", EndTime = 5000, MenuString = "Lux E", SpriteName = Resources.LuxE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "LeBlanc", Name = "LeBlanc_Base_W_return_indicator.troy", EndTime = 4000, MenuString = "LeBlanc W", SpriteName = Resources.LeBlancW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "LeBlanc", Name = "LeBlanc_Base_RW_return_indicator.troy", EndTime = 4000, MenuString = "LeBlanc W", SpriteName = Resources.LeBlancW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "MasterYi", Name = "MasterYi_Base_W_Buf.troy", EndTime = 4000, MenuString = "MasterYi W", SpriteName = Resources.MasterYiW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Akali", Name = "Akali_Base_smoke_bomb_tar_team_green.troy", EndTime = 8000, MenuString = "Akali W", SpriteName = Resources.AkaliW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Akali", Name = "Akali_Base_smoke_bomb_tar_team_red.troy", EndTime = 8000, MenuString = "Akali W", SpriteName = Resources.AkaliW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Fizz", Name = "Fizz_Ring_Green.troy", EndTime = 1500, MenuString = "Fizz R", SpriteName = Resources.FizzR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Fizz", Name = "Fizz_Ring_Red.troy", EndTime = 1500, MenuString = "Fizz R", SpriteName = Resources.FizzR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Nunu", Name = "Nunu_Base_R_indicator_blue.troy", EndTime = 3000, MenuString = "Nunu R", SpriteName = Resources.NunuR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Nunu", Name = "Nunu_Base_R_indicator_red.troy", EndTime = 3000, MenuString = "Nunu R", SpriteName = Resources.NunuR },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Shen", Name = "shen_Teleport_target_v2.troy", EndTime = 3000, MenuString = "Shen R", SpriteName = Resources. },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Shen", Name = "ShenTeleport_v2.troy", EndTime = 3000, MenuString = "Shen R", SpriteName = Resources. },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Graves", Name = "Graves_SmokeGrenade_Cloud_Team_Green.troy", EndTime = 4000, MenuString = "Graves W", SpriteName = Resources.GravesW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Graves", Name = "Graves_SmokeGrenade_Cloud_Team_Red.troy", EndTime = 4000, MenuString = "Graves W", SpriteName = Resources.GravesW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "MissFortune", Name = "MissFortune_Base_E_Unit_Tar_green.troy", EndTime = 2500, MenuString = "MissFortune E", SpriteName = Resources.MissFortuneE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "MissFortune", Name = "MissFortune_Base_E_Unit_Tar_red.troy", EndTime = 2500, MenuString = "MissFortune E", SpriteName = Resources.MissFortuneE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Corki", Name = "Corki_Base_W_tar.troy", EndTime = 2000, MenuString = "Corki W", SpriteName = Resources.CorkiW },
                /*
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Gangplank", Name = "GangplankE", EndTime = 60000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Rumble", Name = "RumbleR", EndTime = 5000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Lissandra", Name = "LissandraR", EndTime = 2500 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Malzahar", Name = "MalzaharW", EndTime = 5000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "miss", Name = "MissR", EndTime = 3000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Unknown, GameObject = true, ChampionName = "Anivia", Name = "AniviaEgg", EndTime = 6000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Zed", Name = "zedR", EndTime = 3000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "twist", Name = "twistR", EndTime = 3000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Zilean", Name = "ZileanR", EndTime = 3000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Ziggs", Name = "Ziggs", EndTime = 4000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Janna", Name = "JannaR", EndTime = 3000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Zyra", Name = "ZyraW", EndTime = 30000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Unknown, GameObject = true, ChampionName = "Zyra", Name = "Zyra", EndTime = 1000 },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Unknown, GameObject = true, ChampionName = "Illaoi", Name = "Illaoi", EndTime = 60000 },
                */
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Morgana", EndTime = 3000, MenuString = "Morgana R", SpriteName = Resources.MorganaR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Vladimir", EndTime = 2000, MenuString = "Vladimir W", SpriteName = Resources.VladimirW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "FiddleSticks", EndTime = 1500 + 250, MenuString = "FiddleSticks R", SpriteName = Resources.FiddlesticksR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Anivia", EndTime = 5000, SkillShot = true, MenuString = "Anivia W", SpriteName = Resources.AniviaW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Veigar", EndTime = 1200, SkillShot = true, MenuString = "Veigar W", SpriteName = Resources.VeigarW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Ekko", EndTime = 3000, SkillShot = true, MenuString = "Ekko W", SpriteName = Resources.EkkoW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Urgot", EndTime = 1000, MenuString = "Urgot R", SpriteName = Resources.UrgotR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Warwick", EndTime = 1800, MenuString = "Warwick R", SpriteName = Resources.WarwickR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Tryndamere", EndTime = 5000, MenuString = "Tryndamere R", SpriteName = Resources.TryndamereR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Trundle", EndTime = 6000, SkillShot = true, MenuString = "Trundle E", SpriteName = Resources.TrundleE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Kindred", EndTime = 4000, SkillShot = true, MenuString = "Kindred R", SpriteName = Resources.KindredR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Zyra", EndTime = 2000, SkillShot = true, MenuString = "Zyra R", SpriteName = Resources.ZyraR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Shen", EndTime = 3000, SkillShot = true, MenuString = "Shen R", SpriteName = Resources.ShenR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Viktor", EndTime = 4000, SkillShot = true, MenuString = "Viktor W", SpriteName = Resources.ViktorW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, ChampionName = "Shaco", EndTime = 3500 + 250, MenuString = "Shaco Q", SpriteName = Resources.ShacoQ },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Leona", EndTime = 3000, MenuString = "Leona W", SpriteName = Resources.LeonaW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Corki", EndTime = 4000, MenuString = "Corki E", SpriteName = Resources.CorkiE },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "XinZhao", EndTime = 5000, MenuString = "XinZhao W", SpriteName = Resources.XinZhaoW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Garen", EndTime = 2000, MenuString = "Garen W", SpriteName = Resources.GarenW },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Taric", EndTime = 10000, MenuString = "Taric R", SpriteName = Resources.TaricR },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Fizz", EndTime = 6000, MenuString = "Fizz W", SpriteName = Resources.FizzW },

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
                new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "SummonerExhaustDeBuff", MenuString = "Summoner Exhaust", SpriteName = Resources.Exhaust },

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

                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Rengar", Name = "RengarRBuff", MenuString = "Rengar R", SpriteName = Resources.RengarR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vladimir",Name = "VladimirHemoplagueDebuff", MenuString = "Vladimir R", SpriteName = Resources.VladimirR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sion", Name = "SionWShieldStacks", MenuString = "Sion W", SpriteName = Resources.SionW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Leona", Name = "LeonaShieldOfDaybreak", MenuString = "Leona Q", SpriteName = Resources.LeonaQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Lucian", Name = "LucianPassiveBuff", MenuString = "Lucian Passive", SpriteName = Resources.LucianP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "MissFortune", Name = "MissFortuneViciousStrikes", MenuString = "MissFortune W", SpriteName = Resources.MissFortuneW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "MissFortune", Name = "MissFortuneBulletSound", MenuString = "MissFortune R", SpriteName = Resources.MissFortuneR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Janna", Name = "Eye Of The Storm", MenuString = "Janna E", SpriteName = Resources.JannaE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Janna", Name = "Reap The Whirlwind", MenuString = "Janna R", SpriteName = Resources.JannaR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kennen", Name = "KennenShurikenStorm", MenuString = "Kennen R", SpriteName = Resources.KennenR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Kennen", Name = "KennenLightningRush", MenuString = "Kennen E", SpriteName = Resources.KennenE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Corki", Name = "CorkiLoaded", MenuString = "Corki Package", SpriteName = Resources.CorkiBoom },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "XinZhao", Name = "XenZhaoComboTarget", MenuString = "XinZhao Q", SpriteName = Resources.XinZhaoQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Diana", Name = "DianaOrbs", MenuString = "Diana W", SpriteName = Resources.DianaW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Garen", Name = "GarenQBuff", MenuString = "Garen Q", SpriteName = Resources.GarenQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Garen", Name = "GarenE", MenuString = "Garen E", SpriteName = Resources.GarenE },
                //new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Gragas", Name = "GragasWAttackBuff", MenuString = "Gragas W", SpriteName = Resources.Recall }, 왜 버그가 있는지 모르겠다
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Graves", Name = "GravesEGrit", MenuString = "Graves E", SpriteName = Resources.GravesE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Gnar", Name = "GnarWBuff", MenuString = "Gnar W", SpriteName = Resources.GnarWBuff },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Gnar", Name = "gnartransformsoon", MenuString = "Gnar Transform", SpriteName = Resources.GnarTransformSoon },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "FiddleSticks", Name = "Drain", MenuString = "FiddleSticks W", SpriteName = Resources.FiddlesticksW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Orianna", Name = "OrianaRedactShield", MenuString = "Orianna W", SpriteName = Resources.OriannaW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Orianna", Name = "OrianaDissonanceAlly", MenuString = "Orianna E", SpriteName = Resources.OriannaE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Orianna", Name = "OrianaDissonanceEnemy", MenuString = "Orianna E", SpriteName = Resources.OriannaE },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ryze", Name = "RyzeR", MenuString = "Ryze R", SpriteName = Resources.RyzeR },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ryze", Name = "RyzePassiveStack", MenuString = "Ryze Passive", SpriteName = Resources.RyzeP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Ryze", Name = "RyzePassiveCharged", MenuString = "Ryze Passive", SpriteName = Resources.RyzeP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Taric", Name = "TaricGemcraftBuff", MenuString = "Taric Passive", SpriteName = Resources.TaricP },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Taric", Name = "Shatter", MenuString = "Taric W", SpriteName = Resources.TaricW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Shen", Name = "Shen Vorpal Star", MenuString = "Shen Q", SpriteName = Resources.ShenQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Shen", Name = "Shen Feint Buff", MenuString = "Shen W", SpriteName = Resources.ShenW },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Trundle", Name = "TrundleQDebuff", MenuString = "Trundle Q", SpriteName = Resources.TrundleQ },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Trundle", Name = "TrundleCircleSlow", MenuString = "Trundle E", SpriteName = Resources.TrundleE },
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
