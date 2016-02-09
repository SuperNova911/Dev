using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using Color = System.Drawing.Color;

namespace TimerBuddy
{
    public class ObjectDetector
    {
        static ObjectDetector()
        {
            try
            {
                Obj_AI_Base.OnBuffGain += Obj_AI_Base_OnBuffGain;
                Obj_AI_Base.OnBuffLose += Obj_AI_Base_OnBuffLose;
                Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
                GameObject.OnCreate += GameObject_OnCreate;
                GameObject.OnDelete += GameObject_OnDelete;
                Game.OnTick += Game_OnTick;
            }
            catch (Exception e)
            {
                e.ErrorMessage("OBJECT_DETECTOR_INIT2");
            }
        }
        
        private static void Obj_AI_Base_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            try
            {
                if (!sender.IsValid || !args.Buff.Caster.IsValid || !sender.IsHero())
                    return;

                var database = SpellDatabase.Database.FirstOrDefault(d => d.Buff && (d.Name == args.Buff.DisplayName || d.Name == args.Buff.Name));
                
                if (database != null)
                {
                    if ((database.SpellType == SpellType.Spell && !Config.Menu.CheckboxValue("sTimer")) ||
                    (database.SpellType == SpellType.SummonerSpell && !Config.Menu.CheckboxValue("ssTimer")) ||
                    (database.SpellType == SpellType.Item && !Config.Menu.CheckboxValue("itemTimer")))
                        return;

                    Program.SpellList.Add(new Spell
                    {
                        SpellType = database.SpellType,
                        Team = sender.IsAlly ? Team.Ally : sender.IsEnemy ? Team.Enemy : Team.None,
                        DrawType = database.DrawType,
                        Importance = database.Importance,
                        Caster = sender,
                        Target = sender,
                        CastPosition = sender.Position,
                        ChampionName = database.ChampionName,
                        Name = database.Name,
                        MenuCode = database.MenuCode,
                        FullTime = args.Buff.EndTime - args.Buff.StartTime,
                        EndTime = args.Buff.EndTime,
                        NetworkID = sender.NetworkId,
                        Buff = database.Buff,
                        OnlyMe = database.OnlyMe,
                        Color = database.Color,
                        SpriteName = database.SpriteName,
                    });

                    return;
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("BUFF_GAIN", args.Buff.DisplayName);
            }
        }

        private static void Obj_AI_Base_OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            try
            {
                if (!sender.IsValid || !args.Buff.Caster.IsValid || !sender.IsHero())
                    return;

                Program.SpellList.RemoveAll(d => d.Buff && d.Target == sender && (d.Name == args.Buff.DisplayName || d.Name == args.Buff.Name));
                DrawManager.Line.RemoveAll(d => d.Buff && d.Target == sender && (d.Name == args.Buff.DisplayName || d.Name == args.Buff.Name));
                DrawManager.Timer.RemoveAll(d => d.Buff && d.Target == sender && (d.Name == args.Buff.DisplayName || d.Name == args.Buff.Name));
                DrawManager.TimerLine.RemoveAll(d => d.Buff && d.Target == sender && (d.Name == args.Buff.DisplayName || d.Name == args.Buff.Name));
            }
            catch (Exception e)
            {
                e.ErrorMessage("BUFF_LOSE", args.Buff.DisplayName);
            }
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            try
            {
                if (!sender.IsValid)
                    return;

                WardDetector(sender, args);

                var database = SpellDatabase.Database.FirstOrDefault(d => d.GameObject && d.SpellType != SpellType.Ward && (d.ObjectName != null
                ? d.Name == sender.Name && d.ObjectName == sender.BaseObjectName()
                : sender.Name.Contains(d.Name)));
                
                if (database != null)
                {
                    if ((database.SpellType == SpellType.Spell && !Config.Menu.CheckboxValue("sTimer")) ||
                        (database.SpellType == SpellType.SummonerSpell && !Config.Menu.CheckboxValue("ssTimer")) ||
                        (database.SpellType == SpellType.Item && !Config.Menu.CheckboxValue("itemTimer")) ||
                        (database.SpellType == SpellType.Trap && !Config.Menu.CheckboxValue("trapTimer")))
                        return;

                    var caster = sender.FIndCaster(database);

                    Program.SpellList.Add(new Spell
                    {
                        SpellType = database.SpellType,
                        Team = caster.IsAlly ? Team.Ally : caster.IsEnemy ? Team.Enemy : Team.None,
                        Slot = database.Slot,
                        DrawType = database.DrawType,
                        Importance = database.Importance,
                        Caster = caster,
                        Object = sender,
                        CastPosition = sender.Position,
                        ChampionName = database.ChampionName != null ? database.ChampionName : caster.BaseSkinName,
                        Name = database.Name,
                        ObjectName = database.ObjectName,
                        MenuCode = database.MenuCode,
                        FullTime = database.EndTime,
                        EndTime = database.EndTime + Utility.TickCount,
                        NetworkID = sender.NetworkId,
                        GameObject = database.GameObject,
                        OnlyMe = database.OnlyMe,
                        Teleport = database.Teleport,
                        Color = database.Color,
                        SpriteName = database.SpriteName,
                    });

                    return;
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("ON_CREATE", sender.Name);
            }
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            try
            {
                if (!sender.IsValid)
                    return;

                Program.SpellList.RemoveAll(d => d.GameObject && d.NetworkID == sender.NetworkId/* && d.Name == sender.Name && d.ObjectName == sender.BaseObjectName()*/);
                DrawManager.Line.RemoveAll(d => d.GameObject && d.NetworkID == sender.NetworkId);
                DrawManager.Timer.RemoveAll(d => d.GameObject && d.NetworkID == sender.NetworkId);
                DrawManager.TimerLine.RemoveAll(d => d.GameObject && d.NetworkID == sender.NetworkId);
            }
            catch (Exception e)
            {
                e.ErrorMessage("ON_DELETE", sender.Name);
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (!sender.IsValid)
                    return;

                Utility.ShacoBoxActive(sender, args);
                
                var database = SpellDatabase.Database.FirstOrDefault(d => d.GameObject == false && d.Buff == false && 
                (d.SpellType == SpellType.Spell && d.ChampionName == sender.BaseSkinName && d.Slot == args.Slot) ||
                ((d.SpellType == SpellType.Item || d.SpellType == SpellType.Blink) && d.Name == args.SData.Name));
                
                if (database != null)
                {
                    if ((database.SpellType == SpellType.Spell && !Config.Menu.CheckboxValue("sTimer")) ||
                        (database.SpellType == SpellType.Item && !Config.Menu.CheckboxValue("itemTimer")) ||
                        (database.SpellType == SpellType.Blink && !Config.Menu.CheckboxValue("blinkTracker")))
                        return;

                    var target = args.Target as Obj_AI_Base;
                    if (target == null)
                        target = sender;

                    Program.SpellList.Add(new Spell
                    {
                        SpellType = database.SpellType,
                        Team = sender.IsAlly ? Team.Ally : sender.IsEnemy ? Team.Enemy : Team.None,
                        Slot = database.Slot,
                        DrawType = database.DrawType,
                        Importance = database.Importance,
                        Caster = sender,
                        Target = target,
                        StartPosition = args.Start,
                        CastPosition = database.SkillShot ? args.End : target.Position,
                        MenuCode = database.MenuCode,
                        FullTime = database.EndTime,
                        EndTime = database.EndTime + Utility.TickCount,
                        NetworkID = sender.NetworkId,
                        SkillShot = database.SkillShot,
                        Range = database.Range,
                        OnlyMe = database.OnlyMe,
                        Color = database.Color,
                        SpriteName = database.SpriteName,
                    });

                    return;
                }

                var spellDatabase = SpellDatabase.Database.FirstOrDefault(d => 
                (d.GameObject && d.SpellType == SpellType.Spell && d.ChampionName == sender.BaseSkinName && d.Slot == args.Slot) ||
                (d.GameObject && d.SpellType == SpellType.SummonerSpell && args.SData.Name == "summonerteleport"));

                if (spellDatabase != null)
                {
                    Program.CasterList.Add(new SpellCaster
                    {
                        Caster = sender,
                        SpellType = spellDatabase.SpellType,
                        Slot = spellDatabase.Slot,
                        EndTime = 10000 + Utility.TickCount,
                    });

                    if (spellDatabase.SpellType == SpellType.SummonerSpell)
                        Program.CasterList.Add(new SpellCaster
                        {
                            Caster = sender,
                            SpellType = spellDatabase.SpellType,
                            Slot = spellDatabase.Slot,
                            EndTime = 10000 + Utility.TickCount,
                        });

                    return;
                }

                var wardDatabase = SpellDatabase.Database.FirstOrDefault(d => d.GameObject && d.SpellType == SpellType.Ward && d.Name == args.SData.Name);

                if (wardDatabase != null)
                {
                    Program.WardCasterList.Add(new WardCaster
                    {
                        Caster = sender,
                        Name = wardDatabase.Name,
                        EndTime = 2000 + Utility.TickCount,
                    });

                    return;
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("OBJECT_DETECTOR_SPELLCAST", sender.BaseSkinName + " " + args.Slot);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            try
            {
                if (Program.SpellList.Count > 0)
                    Program.SpellList.RemoveAll(d => d.Buff
                    ? d.EndTime < Game.Time
                    : d.EndTime < Utility.TickCount);

                if (Program.CasterList.Count > 0)
                    Program.CasterList.RemoveAll(d => d.EndTime < Utility.TickCount);
                if (Program.WardCasterList.Count > 0)
                    Program.WardCasterList.RemoveAll(d => d.EndTime < Utility.TickCount);
                
                foreach (var hero in EntityManager.Heroes.AllHeroes.Where(h => h.IsValid() && h.VisibleOnScreen))
                {
                    foreach (var buff in hero.Buffs.Where(b => b.IsValid()))
                    {
                        var bufflist = Program.SpellList.FirstOrDefault(d => d.Buff && d.Name == buff.DisplayName && d.Caster.BaseSkinName == hero.BaseSkinName);

                        if (bufflist != null)
                            bufflist.EndTime = buff.EndTime;
                    }
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("OBJECT_DETECTOR_ONTICK");
            }
        }

        public static void Initialize()
        {
            try
            {

            }
            catch (Exception e)
            {
                e.ErrorMessage("OBJECT_DETECTOR_INIT");
            }
        }

        private static void WardDetector(GameObject sender, EventArgs args)
        {
            try
            {
                if (!Config.Menu.CheckboxValue("wardTimer"))
                    return;

                var database = SpellDatabase.Database.FirstOrDefault(d => d.GameObject && d.SpellType == SpellType.Ward &&
                d.ObjectName == sender.BaseObjectName());
                
                if (database != null)
                {
                    var caster = sender.FindCasterWard(database);
                    bool limited = (sender.BaseObjectName() == "YellowTrinket" || sender.BaseObjectName() == "SightWard");

                    Core.DelayAction(() =>
                    Program.SpellList.Add(new Spell
                    {
                        SpellType = database.SpellType,
                        Team = caster.IsAlly ? Team.Ally : caster.IsEnemy ? Team.Enemy : Team.None,  //sender.Team.IsAlly() ? Team.Ally : sender.Team.IsEnemy() ? Team.Enemy : Team.None,
                        Object = sender,
                        DrawType = database.DrawType,
                        Caster = caster,
                        StartPosition = caster.Position,
                        CastPosition = sender.Position,
                        ChampionName = caster.BaseSkinName,
                        Name = database.Name,
                        ObjectName = database.ObjectName,
                        MenuCode = database.MenuCode,
                        FullTime = limited ? (sender as Obj_AI_Base).Mana * 1000f : database.EndTime,
                        EndTime = limited ? (sender as Obj_AI_Base).Mana * 1000f + Utility.TickCount - 500 : database.EndTime + Utility.TickCount - 500,
                        NetworkID = sender.NetworkId,
                        GameObject = database.GameObject,
                        Color = database.Color,
                        SpriteName = database.SpriteName,
                    })
                    , database.ObjectName == "YellowTrinket" ? 500 : 0);

                    return;
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("WARD_DETECTOR");
            }
        }
    }
}
