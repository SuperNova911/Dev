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
        static Font TeleportFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 17));
        static Font TrapFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 15));

        public static List<Timer> TimerList = new List<Timer>();
        public static List<GameObj> ObjList = new List<GameObj>();

        public static TimerInfo[] TimerInfoList = new TimerInfo[]
        {
            new TimerInfo { type = ObjType.Teleport, name = "summonerteleport", endTime = 3500 },
            new TimerInfo { type = ObjType.Item, name = "ZhonyasHourglass", endTime = 2500 },
            //new TimerInfo { type = ObjType.Spell, name = "SoulShackles", endTime = 3000 },
            //new TimerInfo { type = ObjType.Spell, name = "VladimirSanguinePool", endTime = 2000 },
            new TimerInfo { type = ObjType.Spell, championName = "Morgana", slot = SpellSlot.R, endTime = 3000 },
            new TimerInfo { type = ObjType.Spell, championName = "Vladimir", slot = SpellSlot.W, endTime = 2000 },
            new TimerInfo { type = ObjType.Spell, championName = "FiddleStick", slot = SpellSlot.R, endTime = 1500 },
            new TimerInfo { type = ObjType.Spell, championName = "Anivia", slot = SpellSlot.W, endTime = 5000 },
            new TimerInfo { type = ObjType.Spell, championName = "Veigar", slot = SpellSlot.W, endTime = 1200 },
            new TimerInfo { type = ObjType.Spell, championName = "Ekko", slot = SpellSlot.W, endTime = 3000 },
            new TimerInfo { type = ObjType.Spell, championName = "Urgot", slot = SpellSlot.R, endTime = 1000 },
            new TimerInfo { type = ObjType.Spell, championName = "Warwick", slot = SpellSlot.R, endTime = 1800 },
            new TimerInfo { type = ObjType.Spell, championName = "FiddleStick", slot = SpellSlot.R, endTime = 1500 },
            new TimerInfo { type = ObjType.Spell, championName = "FiddleStick", slot = SpellSlot.R, endTime = 1500 },
            new TimerInfo { type = ObjType.Spell, championName = "FiddleStick", slot = SpellSlot.R, endTime = 1500 },
            new TimerInfo { type = ObjType.Spell, championName = "FiddleStick", slot = SpellSlot.R, endTime = 1500 },
            new TimerInfo { type = ObjType.Spell, championName = "FiddleStick", slot = SpellSlot.R, endTime = 1500 },
        };
        public static GameObjInfo[] GameObjInfoList = new GameObjInfo[]
        {
            new GameObjInfo { type = ObjType.Trap, name = "Noxious Trap", endTime = 300000 },
            //new GameObjInfo { type = ObjType.Trap, name = "Noxious Trap", endTime = 120000 },           //Nidalee W
            new GameObjInfo { type = ObjType.Trap, name = "Cupcake Trap", endTime = 90000 },
            new GameObjInfo { type = ObjType.Trap, name = "JinxEMine", endTime = 5000 },
            new GameObjInfo { type = ObjType.Trap, name = "Jack In The Box", endTime = 60000 },
            
            new GameObjInfo { type = ObjType.Spell, name = "Gragas_Base_Q_Ally.troy", deleteName = "Gragas_Base_Q_End.troy", endTime = 4000 },
            new GameObjInfo { type = ObjType.Spell, name = "Nasus_E_Green_Ring.troy", endTime = 4500 },
            new GameObjInfo { type = ObjType.Spell, name = "Lux_Base_E_tar_aoe_green.troy", endTime = 5000 },
            new GameObjInfo { type = ObjType.Spell, name = "LeBlanc_Base_W_return_indicator.troy", endTime = 4000 },
            new GameObjInfo { type = ObjType.Spell, name = "LeBlanc_Base_RW_return_indicator.troy", endTime = 4000 },
            new GameObjInfo { type = ObjType.Spell, name = "MasterYi_Base_W_Buf.troy", endTime = 4000 },
            new GameObjInfo { type = ObjType.Spell, name = "Akali_Base_smoke_bomb_tar_team_green.troy", endTime = 8000 },
            new GameObjInfo { type = ObjType.Spell, name = "OMNOMNOMNOMONOM", endTime = 1500 },     // Fizz R Kappa
            new GameObjInfo { type = ObjType.Spell, name = "Nunu_Base_R_indicator_blue.troy", endTime = 3000 },

            new GameObjInfo { type = ObjType.Spell, name = "GangplankE", endTime = 60000 },
            new GameObjInfo { type = ObjType.Spell, name = "GravesW", endTime = 4000 },
            new GameObjInfo { type = ObjType.Spell, name = "NasusE", endTime = 5000 },
            new GameObjInfo { type = ObjType.Spell, name = "RumbleR", endTime = 5000 },
            new GameObjInfo { type = ObjType.Spell, name = "LissandraR", endTime = 2500 },
            new GameObjInfo { type = ObjType.Spell, name = "MalzaharW", endTime = 5000 },
            new GameObjInfo { type = ObjType.Spell, name = "MissR", endTime = 3000 },
            new GameObjInfo { type = ObjType.Spell, name = "AniviaEgg", endTime = 6000 },
            new GameObjInfo { type = ObjType.Spell, name = "zedR", endTime = 3000 },
            new GameObjInfo { type = ObjType.Spell, name = "twistR", endTime = 3000 },
            new GameObjInfo { type = ObjType.Spell, name = "trindaR", endTime = 3000 }, 
            new GameObjInfo { type = ObjType.Spell, name = "EkkoW", endTime = 3000 },
            new GameObjInfo { type = ObjType.Spell, name = "EkkoW", endTime = 3000 },
        };

        public static Menu Menu;

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
                Menu = MainMenu.AddMenu("TimerBuddy", "TimerBuddy");
                Menu.AddGroupLabel("TrackList");
                Menu.Add("teleport", new CheckBox("Teleport", true));
                Menu.Add("trap", new CheckBox("Trap", true));
                Menu.Add("spell", new CheckBox("Spell", true));
                Menu.Add("item", new CheckBox("Item", true));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE MENU");
            }

            try
            {
                Drawing.OnEndScene += Drawing_OnEndScene;
                Game.OnUpdate += Game_OnUpdate;
                Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
                GameObject.OnCreate += GameObject_OnCreate;
                GameObject.OnDelete += GameObject_OnDelete;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE INIT");
            }
        }
        
        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            try
            {
                if (!sender.IsValid)
                    return;
                
                if (sender.Name.Contains("Minion") || sender.GetType().Name == "MissileClient" || sender.Name.Contains("SRU"))
                    return;

                var baseObject = sender as Obj_AI_Base;
                var objectName = baseObject == null ? sender.Name : baseObject.BaseSkinName;

                if (sender.Name.Contains("Minion"))
                    return;
                
                Console.WriteLine("Add\tType: {0} | Name: {1} | NetID: {2} | objectName: {3}", sender.GetType().Name, sender.Name, sender.NetworkId, objectName);

                var list = GameObjInfoList.FirstOrDefault(l => l.name == sender.Name);

                if (list != null)
                {
                    ObjList.Add(new GameObj
                    {
                        team = sender.IsAlly ? Team.Ally : Team.Enemy,
                        type = list.type,
                        position = sender.Position,
                        endTime = TickCount + list.endTime,
                        name = sender.Name,
                        networkID = sender.NetworkId
                    });

                    Chat.Print("Add " + sender.Name + sender.NetworkId);
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
                
                if (sender.Name.Contains("Minion") || sender.Name.Contains("Odin") || sender.Name.Contains("Shopkeeper") || sender.Name.Contains("SRU") ||
                    sender.GetType().Name == "MissileClient" || sender.GetType().Name == "DrawFX")
                    return;

                ObjList.RemoveAll(l => l.networkID == sender.NetworkId);

                Console.WriteLine("Delete\tType: {0} | Name: {1} | NetID: {2}", sender.GetType().Name, sender.Name, sender.NetworkId);
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
                Chat.Print(args.SData.Name + " | " + sender.BaseSkinName + args.Slot);

                var list = TimerInfoList.FirstOrDefault(s => s.championName == sender.BaseSkinName && s.slot == args.Slot);

                if (list != null)
                {
                    TimerList.Add(new Timer
                    {
                        type = list.type,
                        team = sender.IsAlly ? Team.Ally : Team.Enemy,
                        caster = sender,
                        castPosition = args.End,
                        name = args.SData.Name,
                        endTime = TickCount + list.endTime,
                        slot = args.Slot
                    });

                    return;
                }

                if (args.SData.Name == "teleportcancel")
                {
                    var tlist = TimerList.FirstOrDefault(l => l.name == "summonerteleport" && sender == l.caster);

                    if (tlist != null)
                    {
                        tlist.cancel = true;
                        tlist.endTime = TickCount + 2000;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE OnProcessSpellCast");
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            try
            {
                TimerList.RemoveAll(l => l.endTime < Environment.TickCount);
                ObjList.RemoveAll(l => l.endTime < Environment.TickCount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE OnUpdate");
            }
        }

        public static void DrawBuff()
        {
            foreach (var hero in EntityManager.Heroes.AllHeroes.Where(h => h.IsValidTarget() && h.VisibleOnScreen))
            {
                if (hero.HasBuff("VladimirHemoplagueDebuff"))
                    DrawText(hero.BuffRemainTime("VladimirHemoplagueDebuff").ToString("F2"), hero.Position, Color.White, ObjType.Spell);

                if (hero.HasBuff("RengarRBuff"))
                    DrawText(hero.BuffRemainTime("RengarRBuff").ToString("F2"), hero.Position, Color.White, ObjType.Spell);
            }
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            try
            {
                //DrawText(Player.Instance.BaseSkinName, Game.CursorPos + new Vector3(-60, 10, 0), Color.White, ObjType.Teleport);
                //DrawText("Canceled", Game.CursorPos + new Vector3(-70, 65, 0), Color.Red, ObjType.Teleport);

                DrawBuff();

                foreach (var list in TimerList.Where(l => l.endTime > Environment.TickCount))
                {
                    switch (list.type)
                    {
                        case ObjType.Teleport:
                            DrawTeleport(list);
                            break;
                        case ObjType.Item:
                            DrawItem(list);
                            break;
                        case ObjType.Spell:
                            DrawSpell(list);
                            break;
                    }
                }

                foreach (var list in ObjList.Where(l =>l.endTime > Environment.TickCount))
                {
                    switch (list.type)
                    {
                        case ObjType.Trap:
                            DrawTrap(list);
                            break;
                        case ObjType.Spell:
                            DrawSpell(list);
                            break;
                        default:
                            Chat.Print("Fappa");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW");
            }            
        }

        public static void DrawTeleport(Timer list)
        {
            try
            {
                if (!list.cancel)
                {
                    DrawText(list.caster.BaseSkinName, list.castPosition + new Vector3(-60, 10, 0), Color.White, list.type);
                    DrawText(list.GetRemainTime(), list.castPosition + new Vector3(-30, 65, 0), Color.LawnGreen, list.type);
                }
                else
                {
                    DrawText(list.caster.BaseSkinName, list.castPosition + new Vector3(-60, 10, 0), Color.White, list.type);
                    DrawText("Canceled", list.castPosition + new Vector3(-70, 65, 0), Color.Red, list.type);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_TP " + list.caster.BaseSkinName);
            }
        }

        public static void DrawItem(Timer list)
        {
            try
            {
                DrawText(list.GetRemainTime(), list.castPosition + new Vector3(-15, 10, 0), Color.White, list.type);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_ITEM " + list.caster.BaseSkinName);
            }
        }

        public static void DrawTrap(GameObj list)
        {
            try
            {
                DrawText(list.GetRemainTime(), list.position + new Vector3(-20, 0, 0), Color.White, ObjType.Trap);

                if (list.team == Team.Enemy)
                    new Circle
                    {
                        Color = System.Drawing.Color.Red,
                        BorderWidth = 4,
                        Radius = 50,
                    }.Draw(list.position);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_TRAP " + list.name);
            }
        }

        public static void DrawSpell(GameObj list)
        {
            try
            {
                DrawText(list.GetRemainTime(), list.position + new Vector3(-40, -30, 0), Color.White, list.type);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_ITEM " + list.name);
            }
        }

        public static void DrawSpell(Timer list)
        {
            try
            {
                DrawText(list.GetRemainTime(), list.caster.Position + new Vector3(-40, -30, 0), Color.White, list.type);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_ITEM " + list.name);
            }
        }

        public static void DrawText(string text, Vector3 position, Color color, ObjType type)
        {
            try
            {
                switch (type)
                {
                    case ObjType.Trap:
                        TrapFont.DrawText(null,
                        text,
                        (int)Drawing.WorldToScreen(position).X,
                        (int)Drawing.WorldToScreen(position).Y,
                        color);
                        break;
                    case ObjType.Teleport:
                        TeleportFont.DrawText(null,
                        text,
                        (int)Drawing.WorldToScreen(position).X,
                        (int)Drawing.WorldToScreen(position).Y,
                        color);
                        break;
                    case ObjType.Item:
                        TeleportFont.DrawText(null,
                        text,
                        (int)Drawing.WorldToScreen(position).X,
                        (int)Drawing.WorldToScreen(position).Y,
                        color);
                        break;
                    case ObjType.Spell:
                        TeleportFont.DrawText(null,
                        text,
                        (int)Drawing.WorldToScreen(position).X,
                        (int)Drawing.WorldToScreen(position).Y,
                        color);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_TEXT");
            }
        }

        public static string GetRemainTime(this Timer list)
        {
            try
            {
                if (list.type == ObjType.Spell)
                {
                    var sdata = TimerInfoList.FirstOrDefault(d => d.championName == list.caster.BaseSkinName && d.slot == list.slot);

                    if (sdata == null)
                    {
                        Chat.Print("error: CODE REMAIN_TIMER_SPELL KAPPA " + list.caster.BaseSkinName);
                        return "Kappa";
                    }

                    if (sdata.endTime <= 10000)
                        return ((list.endTime - TickCount) / 1000f).ToString("F2");
                    else
                        return ((int)(list.endTime - TickCount) / 1000 + 1).ToString();
                }

                var data = TimerInfoList.FirstOrDefault(d => list.name == d.name);

                if (data == null)
                {
                    Chat.Print("error: CODE REMAIN_TIMER KAPPA " + list.caster.BaseSkinName);
                    return "Kappa";
                }

                if (data.endTime <= 10000)
                    return ((list.endTime - TickCount) / 1000f).ToString("F2");
                else
                    return ((int)(list.endTime - TickCount) / 1000 + 1).ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE REMAIN_TIMER " + list.caster.BaseSkinName);
                return "Kappa";
            }
        }

        public static string GetRemainTime(this GameObj list)
        {
            try
            {
                var data = GameObjInfoList.FirstOrDefault(d => list.name == d.name);

                if (data == null)
                {
                    Chat.Print("error: CODE REMAIN_OBJ KAPPA " + list.name);
                    return "Kappa";
                }

                if (data.endTime <= 10000)
                    return ((list.endTime - TickCount) / 1000f).ToString("F2");
                else
                    return ((int)(list.endTime - TickCount) / 1000 + 1).ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE REMAIN_OBJ " + list.name);
                return "Kappa";
            }
        }

        public static int TickCount
        {
            get
            {
                return Environment.TickCount & int.MaxValue;
            }
        }

        public static float BuffRemainTime(this Obj_AI_Base unit, string buffName)
        {
            return unit.HasBuff(buffName)
                ? unit.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                  .Where(buff => buff.Name == buffName)
                  .Select(buff => buff.EndTime)
                  .FirstOrDefault() - Game.Time
                : 0;
        }
    }
}
