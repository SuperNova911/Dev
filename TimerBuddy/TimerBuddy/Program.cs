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
using Color = SharpDX.Color;

namespace TimerBuddy
{
    public static class Program
    {
        public static List<Spell> SpellList = new List<Spell>();
        
        static void Main(string[] args)
        {
            try
            {
                Loading.OnLoadingComplete += Loading_OnLoadingComplete;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE MAIN");
            }
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            try
            {
                Config.Initialize();

                Drawing.OnEndScene += Drawing_OnEndScene;
                Game.OnTick += Game_OnTick;
                Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
                Obj_AI_Base.OnBuffGain += Obj_AI_Base_OnBuffGain;
                Obj_AI_Base.OnBuffUpdate += Obj_AI_Base_OnBuffUpdate;
                Obj_AI_Base.OnBuffLose += Obj_AI_Base_OnBuffLose;
                GameObject.OnCreate += GameObject_OnCreate;
                GameObject.OnDelete += GameObject_OnDelete;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE INIT");
            }
        }

        private static void Obj_AI_Base_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            try
            {
                var hero = sender as AIHeroClient;

                if (hero == null || !args.Buff.Caster.IsValid)
                    return;

                var database = SpellDatabase.Database.FirstOrDefault(d => d.Name == args.Buff.DisplayName && d.Buff == true);

                if (database != null)
                {
                    var target = args.Buff.Caster as AIHeroClient;

                    SpellList.Add(new Spell
                    {
                        Target = target,
                        Caster = sender,
                        FullTime = args.Buff.EndTime * 1000 - args.Buff.StartTime * 1000,
                        EndTime = args.Buff.EndTime * 1000,
                        Name = database.Name,
                        SpellType = database.SpellType,
                        Team = sender.IsAlly ? Team.Ally : sender.IsEnemy ? Team.Enemy : Team.None,
                        Buff = database.Buff,
                    });

                    Chat.Print("Add " + args.Buff.DisplayName + " " + args.Buff.EndTime * 1000 + " " + args.Buff.Caster.Name, System.Drawing.Color.LawnGreen);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE BUFF_GAIN " + args.Buff.DisplayName);
            }
        }

        private static void Obj_AI_Base_OnBuffUpdate(Obj_AI_Base sender, Obj_AI_BaseBuffUpdateEventArgs args)
        {
            var hero = sender as AIHeroClient;

            if (hero == null || !args.Buff.Caster.IsValid)
                return;

            var buff = SpellList.FirstOrDefault(l => l.Name == args.Buff.Name && l.Caster == sender && l.Buff == true);

            buff.EndTime = args.Buff.EndTime * 1000;
        }

        private static void Obj_AI_Base_OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            try
            {
                var hero = sender as AIHeroClient;

                if (hero == null || !args.Buff.Caster.IsValid)
                    return;

                SpellList.RemoveAll(l => l.Name == args.Buff.DisplayName && l.Caster == args.Buff.Caster && l.Buff == true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE BUFF_LOSE " + args.Buff.DisplayName);
            }
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            try
            {
                if (!sender.IsValid)
                    return;
                
                if (sender.Name.Contains("Minion") || sender.GetType().Name == "MissileClient" || sender.Name.Contains("SRU") || sender.Name.Contains("FeelNoPain"))
                    return;

                var baseObject = sender as Obj_AI_Base;
                var objectName = baseObject == null ? sender.Name : baseObject.BaseSkinName;
                
                Console.WriteLine("Add\tType: {0} | Name: {1} | NetID: {2} | objectName: {3}", sender.GetType().Name, sender.Name, sender.NetworkId, objectName);

                var trapDatabase = SpellDatabase.Database.FirstOrDefault(d => d.Name == sender.Name && d.ObjectName == objectName && d.GameObject == true && d.SpellType == SpellType.Trap);

                if (trapDatabase != null)
                {
                    SpellList.Add(new Spell
                    {
                        SpellType = trapDatabase.SpellType,
                        Team = sender.IsAlly ? Team.Ally : sender.IsEnemy ? Team.Enemy : Team.None,
                        Slot = trapDatabase.Slot,
                        Caster = null,
                        CastPosition = sender.Position,
                        ChampionName = trapDatabase.ChampionName,
                        Name = sender.Name,
                        ObjectName = objectName,
                        MenuString = trapDatabase.MenuString,
                        EndTime = trapDatabase.EndTime + Utility.TickCount,
                        NetworkID = sender.NetworkId,
                        GameObject = trapDatabase.GameObject,
                        SkillShot = trapDatabase.SkillShot
                    });

                    Chat.Print("Add " + sender.Name + " " + objectName + " " + sender.NetworkId, System.Drawing.Color.LawnGreen);

                    return;
                }

                var database = SpellDatabase.Database.FirstOrDefault(d => d.Name == sender.Name && d.GameObject == true);

                if (database != null)
                {
                    SpellList.Add(new Spell
                    {
                        SpellType = database.SpellType,
                        Team = sender.IsAlly ? Team.Ally : sender.IsEnemy ? Team.Enemy : Team.None,
                        Slot = database.Slot,
                        Caster = null,
                        CastPosition = sender.Position,
                        ChampionName = database.ChampionName,
                        Name = sender.Name,
                        MenuString = database.MenuString,
                        EndTime = database.EndTime + Utility.TickCount,
                        NetworkID = sender.NetworkId,
                        GameObject = database.GameObject,
                        SkillShot = database.SkillShot
                    });

                    Chat.Print("Add " + sender.Name + " " + sender.NetworkId, System.Drawing.Color.LawnGreen);

                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE OnCreate");
            }
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            try
            {
                if (!sender.IsValid)
                    return;
                
                if (sender.Name.Contains("Minion") || sender.Name.Contains("NAV") || sender.Name.Contains("Odin") || sender.Name.Contains("Shopkeeper") || sender.Name.Contains("SRU") ||
                    sender.GetType().Name == "MissileClient" || sender.GetType().Name == "DrawFX" || sender.Name.Contains("empty.troy") || sender.Name.Contains("LevelProp")
                     || sender.Name.Contains("FeelNoPain") || sender.Name.Contains("LaserSight")) 
                    return;

                Console.WriteLine("Delete\tType: {0} | Name: {1} | NetID: {2}", sender.GetType().Name, sender.Name, sender.NetworkId);

                SpellList.RemoveAll(l => l.NetworkID == sender.NetworkId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE OnDelete");
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                Chat.Print(sender.BaseSkinName + " " + args.Slot + " | " + args.SData.Name);

                var database = SpellDatabase.Database.FirstOrDefault(d => (d.ChampionName == sender.BaseSkinName && d.Slot == args.Slot && d.GameObject == false && d.Buff == false) || (d.Name == args.SData.Name && d.SpellType != SpellType.Spell && d.Buff == false));

                if (database != null)
                {
                    SpellList.Add(new Spell
                    {
                        SpellType = database.SpellType,
                        Team = sender.IsAlly ? Team.Ally : sender.IsEnemy ? Team.Enemy : Team.None,
                        Slot = database.Slot,
                        Caster = sender,
                        CastPosition = args.End,
                        ChampionName = database.ChampionName,
                        Name = args.SData.Name,
                        MenuString = database.MenuString,
                        EndTime = database.EndTime + Utility.TickCount,
                        NetworkID = sender.NetworkId,
                        GameObject = database.GameObject,
                        SkillShot = database.SkillShot
                    });

                    Chat.Print("Add " + args.SData.Name + " " + sender.NetworkId, System.Drawing.Color.LawnGreen);

                    return;
                }

                if (args.SData.Name == "teleportcancel")
                {
                    var slist = SpellList.FirstOrDefault(l => l.Name == "summonerteleport" && l.Caster == sender);

                    if (slist != null)
                        CastCancel(slist);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE OnProcessSpellCast");
            }
        }

        private static void CastCancel(Spell list)
        {
            try
            {
                list.Cancel = true;
                list.EndTime = Utility.TickCount + 2000;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE CANCEL");
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            try
            {
                SpellList.RemoveAll(l => l.Buff == true ? l.EndTime < Game.Time * 1000 : l.EndTime < Utility.TickCount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE OnUpdate");
            }
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            try
            {
                if (Config.DebugMenu["c1"].Cast<CheckBox>().CurrentValue)
                    DrawManager.DrawLine();

                foreach (var list in SpellList.Where(l => l.Buff == true ? l.EndTime >= Game.Time * 1000f : l.EndTime >= Utility.TickCount))
                {
                    switch (list.SpellType)
                    {
                        case SpellType.SummonerSpell:
                            DrawManager.DrawSummoner(list);
                            break;
                        case SpellType.Item:
                            DrawManager.DrawItem(list);
                            break;
                        case SpellType.Spell:
                            DrawManager.DrawSpell(list);
                            break;
                        case SpellType.Trap:
                            DrawManager.DrawTrap(list);
                            break;
                        default:
                            Chat.Print("error CODE DRAW_TYPE");
                            break;
                    }
                }

                foreach (var list in SpellList.Where(l => l.Buff == true))
                {
                    DrawManager.DrawBuff(list);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW");
            }            
        }
    }
}
