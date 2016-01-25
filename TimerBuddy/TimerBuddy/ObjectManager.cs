using EloBuddy;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool Cancel = false;
        public bool Buff = false;
        public Color Color = Color.White;
    }

    internal static class SpellDatabase
    {
        public static readonly List<Spell> Database;

        static SpellDatabase()
        {
            Database = new List<Spell>
            {
                new Spell { SpellType = SpellType.SummonerSpell, Name = "summonerteleport", EndTime = 3500, MenuString = "Summoner Teleport" },
                new Spell { SpellType = SpellType.SummonerSpell, Name = "summonerbarrier", EndTime = 2000, MenuString = "Summoner Barrier" },
                new Spell { SpellType = SpellType.SummonerSpell, Name = "summonerhaste", EndTime = 10000, MenuString = "Summoner Haste" },
                new Spell { SpellType = SpellType.SummonerSpell, Name = "summonerheal", EndTime = 1000, MenuString = "Summoner Heal" },

                new Spell { SpellType = SpellType.Item, Name = "ZhonyasHourglass", EndTime = 2500, MenuString = "Zhonyas Hourglass" },
                new Spell { SpellType = SpellType.Item, GameObject = true, Name = "LifeAura.troy", EndTime = 4000, MenuString = "Guardian Angel" },

                //new Spell { SpellType = SpellType.Ward, GameObject = true, Name = "SightWard", EndTime = 60000, MenuString = "Warding Totem" },
                //new Spell { SpellType = SpellType.Ward, GameObject = true, Name = "VisionWard", ObjectName = "SightWard", EndTime = 150000, MenuString = "Sightstone" },
                //new Spell { SpellType = SpellType.Ward, GameObject = true, Name = "VisionWard", ObjectName = "VisionWard", EndTime = 0, MenuString = "Vision Ward" },

                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.R, GameObject = true, ChampionName = "Teemo", Name = "Noxious Trap", ObjectName = "TeemoMushroom", EndTime = 600000, MenuString = "Teemo Trap" },
                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.W, GameObject = true, ChampionName = "Nidalee", Name = "Noxious Trap", ObjectName = "NidaleeSpear", EndTime = 120000, MenuString = "Nidalee Trap" },
                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.W, GameObject = true, ChampionName = "Caitlyn", Name = "Cupcake Trap", EndTime = 90000, MenuString = "Caitlyn Trap" },
                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.E, GameObject = true, ChampionName = "Jinx", Name = "JinxEMine", ObjectName = "CaitlynTrap", EndTime = 5000, MenuString = "Jinx Mine" },
                new Spell { SpellType = SpellType.Trap, Slot = SpellSlot.W, GameObject = true, ChampionName = "Shaco", Name = "Jack In The Box", ObjectName = "ShacoBox", EndTime = 60000, MenuString = "Shaco Box" },


                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, GameObject = true, ChampionName = "Gragas", Name = "Gragas_Base_Q_Ally.troy", EndTime = 4000, MenuString = "Gragas Q" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, GameObject = true, ChampionName = "Gragas", Name = "Gragas_Base_Q_Enemy.troy", EndTime = 4000, MenuString = "Gragas Q" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Nasus", Name = "Nasus_Base_E_SpiritFire.troy", EndTime = 4500, MenuString = "Nasus E" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Lux", Name = "Lux_Base_E_tar_aoe_green.troy", EndTime = 5000, MenuString = "Lux E" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, GameObject = true, ChampionName = "Lux", Name = "Lux_Base_E_tar_aoe_red.troy", EndTime = 5000, MenuString = "Lux E" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "LeBlanc", Name = "LeBlanc_Base_W_return_indicator.troy", EndTime = 4000, MenuString = "LeBlanc W" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "LeBlanc", Name = "LeBlanc_Base_RW_return_indicator.troy", EndTime = 4000, MenuString = "LeBlanc W" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "MasterYi", Name = "MasterYi_Base_W_Buf.troy", EndTime = 4000, MenuString = "MasterYi W" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Akali", Name = "Akali_Base_smoke_bomb_tar_team_green.troy", EndTime = 8000, MenuString = "Akali W" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Akali", Name = "Akali_Base_smoke_bomb_tar_team_red.troy", EndTime = 8000, MenuString = "Akali W" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Fizz", Name = "OMNOMNOMNOMONOM", EndTime = 1500, MenuString = "Fizz R" },                         // Fizz R Kappa
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Nunu", Name = "Nunu_Base_R_indicator_blue.troy", EndTime = 3000, MenuString = "Nunu R" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Nunu", Name = "Nunu_Base_R_indicator_red.troy", EndTime = 3000, MenuString = "Nunu R" },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Shen", Name = "shen_Teleport_target_v2.troy", EndTime = 3000, MenuString = "Shen R" },
                //new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, GameObject = true, ChampionName = "Shen", Name = "ShenTeleport_v2.troy", EndTime = 3000, MenuString = "Shen R" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Graves", Name = "Graves_SmokeGrenade_Cloud_Team_Green.troy", EndTime = 4000, MenuString = "Graves W" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, GameObject = true, ChampionName = "Graves", Name = "Graves_SmokeGrenade_Cloud_Team_Red.troy", EndTime = 4000, MenuString = "Graves W" },
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
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Morgana", EndTime = 3000, MenuString = "Morgana R" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Vladimir", EndTime = 2000, MenuString = "Vladimir W" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "FiddleSticks", EndTime = 1500 + 250, MenuString = "FiddleSticks R" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Anivia", EndTime = 5000, SkillShot = true, MenuString = "Anivia W" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Veigar", EndTime = 1200, SkillShot = true, MenuString = "Veigar W" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Ekko", EndTime = 3000, SkillShot = true, MenuString = "Ekko W" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Urgot", EndTime = 1000, MenuString = "Urgot R" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Warwick", EndTime = 1800, MenuString = "Warwick R" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Tryndamere", EndTime = 5000, MenuString = "Tryndamere R" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.E, ChampionName = "Trundle", EndTime = 6000, SkillShot = true, MenuString = "Trundle E" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Kindred", EndTime = 4000, SkillShot = true, MenuString = "Kindred R" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Zyra", EndTime = 2000, SkillShot = true, MenuString = "Zyra R" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.R, ChampionName = "Shen", EndTime = 3000, SkillShot = true, MenuString = "Shen R" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Viktor", EndTime = 4000, SkillShot = true, MenuString = "Viktor W" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.Q, ChampionName = "Shaco", EndTime = 3500 + 250, MenuString = "Shaco Q" },
                new Spell { SpellType = SpellType.Spell, Slot = SpellSlot.W, ChampionName = "Leona", EndTime = 3000, MenuString = "Leona W" },

                //new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "SummonerBarrier", MenuString = "Summoner Barrier" },
                //new Spell { SpellType = SpellType.SummonerSpell, Buff = true, Name = "Teleport", MenuString = "Summoner Teleport" },
                //new Spell { SpellType = SpellType.Item, Buff = true, Name = "Zhonyas Ring", MenuString = "Zhonyas Hourglass" },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Rengar", Name = "RengarRBuff", MenuString = "Rengar R" },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Vladimir",Name = "VladimirHemoplagueDebuff", MenuString = "Vladimir R" },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Sion", Name = "SionWShieldStacks", MenuString = "Sion W" },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Leona", Name = "LeonaShieldOfDaybreak", MenuString = "Leona Q" },
                new Spell { SpellType = SpellType.Spell, Buff = true, ChampionName = "Lucian", Name = "LucianPassiveBuff", MenuString = "Lucian Passive" },
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
