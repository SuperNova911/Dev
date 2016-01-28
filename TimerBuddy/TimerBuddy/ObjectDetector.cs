﻿using EloBuddy;
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
            Obj_AI_Base.OnBuffGain += Obj_AI_Base_OnBuffGain;
            Obj_AI_Base.OnBuffLose += Obj_AI_Base_OnBuffLose;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            Game.OnTick += Game_OnTick;
        }
        
        private static void Obj_AI_Base_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            try
            {
                if (!sender.IsValid || !args.Buff.Caster.IsValid)
                    return;

                var database = SpellDatabase.Database.FirstOrDefault(d => d.Buff && (d.Name == args.Buff.DisplayName || d.Name == args.Buff.Name));

                if (database != null)
                {
                    Program.SpellList.Add(new Spell
                    {
                        SpellType = database.SpellType,
                        Team = sender.IsAlly ? Team.Ally : sender.IsEnemy ? Team.Enemy : Team.None,
                        Caster = sender,
                        Target = sender,
                        CastPosition = sender.Position,
                        ChampionName = database.ChampionName,
                        Name = database.Name,
                        MenuString = database.MenuString,
                        FullTime = args.Buff.EndTime - args.Buff.StartTime,
                        EndTime = args.Buff.EndTime,
                        NetworkID = sender.NetworkId,
                        Buff = database.Buff,
                        Color = database.Color,
                        SpriteName = database.SpriteName,
                    });

                    Chat.Print("Buff " + args.Buff.DisplayName + sender.BaseSkinName, Color.LawnGreen);

                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE BUFF_GAIN " + args.Buff.DisplayName, Color.Blue);
            }
        }

        private static void Obj_AI_Base_OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            try
            {
                if (!sender.IsValid || !args.Buff.Caster.IsValid)
                    return;

                Program.SpellList.RemoveAll(d => d.Buff && d.Target == sender && (d.Name == args.Buff.DisplayName || d.Name == args.Buff.Name));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE BUFF_LOSE " + args.Buff.DisplayName, Color.Blue);
            }
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            try
            {
                if (!sender.IsValid)
                    return;

                var database = SpellDatabase.Database.FirstOrDefault(d => d.GameObject && d.SpellType == SpellType.Trap
                ? d.Name == sender.Name && d.ObjectName == sender.BaseObjectName()
                : d.Name == sender.Name);

                if (database != null)
                {
                    var caster = sender.FIndCaster(database);

                    Program.SpellList.Add(new Spell
                    {
                        SpellType = database.SpellType,
                        Team = caster.IsAlly ? Team.Ally : caster.IsEnemy ? Team.Enemy : Team.None,
                        Slot = database.Slot,
                        Caster = caster,
                        CastPosition = sender.Position,
                        ChampionName = database.ChampionName,
                        Name = database.Name,
                        ObjectName = sender.BaseObjectName(),
                        MenuString = database.MenuString,
                        FullTime = database.EndTime,
                        EndTime = database.EndTime + Utility.TickCount,
                        NetworkID = sender.NetworkId,
                        GameObject = database.GameObject,
                        Color = database.Color,
                        SpriteName = database.SpriteName,
                    });

                    Chat.Print("GameObject " + sender.Name + " " + sender.FIndCaster(database).BaseSkinName, Color.LawnGreen);

                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE ON_CREATE " + sender.Name, Color.Blue);
            }
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            try
            {
                if (!sender.IsValid)
                    return;

                Program.SpellList.RemoveAll(d => d.GameObject && d.NetworkID == sender.NetworkId/* && d.Name == sender.Name && d.ObjectName == sender.BaseObjectName()*/);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE ON_DELETE " + sender.Name, Color.Blue);
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
                ((d.SpellType == SpellType.SummonerSpell || d.SpellType == SpellType.Item) && d.Name == args.SData.Name));

                if (database != null)
                {
                    var target = args.Target as Obj_AI_Base;
                    if (target == null)
                        target = Player.Instance;

                    Program.SpellList.Add(new Spell
                    {
                        SpellType = database.SpellType,
                        Team = sender.IsAlly ? Team.Ally : sender.IsEnemy ? Team.Enemy : Team.None,
                        Slot = database.Slot,
                        Caster = sender,
                        Target = target,
                        CastPosition = database.SkillShot ? args.End : target.Position,
                        MenuString = database.MenuString,
                        FullTime = database.EndTime,
                        EndTime = database.EndTime + Utility.TickCount,
                        NetworkID = sender.NetworkId,
                        SkillShot = database.SkillShot,
                        Color = database.Color,
                        SpriteName = database.SpriteName,
                    });

                    Chat.Print("Spell " + sender.BaseSkinName + " " + args.Slot + " | " + args.SData.Name, Color.LawnGreen);

                    return;
                }

                var objDatabase = SpellDatabase.Database.FirstOrDefault(d => d.GameObject && d.SpellType == SpellType.Spell &&
                d.ChampionName == sender.BaseSkinName && d.Slot == args.Slot);

                if (objDatabase != null)
                {
                    Program.CasterList.Add(new SpellCaster
                    {
                        Caster = sender,
                        Slot = objDatabase.Slot,
                        EndTime = 2000 + Utility.TickCount
                    });

                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE ON_SPELLCAST " + sender.BaseSkinName + " " + args.Slot, Color.Blue);
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

                foreach (var hero in EntityManager.Heroes.AllHeroes.Where(h => h.IsValid() && h.VisibleOnScreen))
                {
                    foreach (var buff in hero.Buffs.Where(b => b.IsValid()))
                    {
                        var bufflist = Program.SpellList.FirstOrDefault(d => d.Buff && d.Name == buff.DisplayName);

                        if (bufflist != null)
                            bufflist.EndTime = buff.EndTime;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE ON_TICK", Color.Blue);
            }
        }

        public static void Initialize()
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE OBJECT_DETECTOR", Color.Blue);
            }
        }
    }
}