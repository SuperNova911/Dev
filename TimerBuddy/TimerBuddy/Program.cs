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
        public static List<SpellCaster> CasterList = new List<SpellCaster>();
        public static List<WardCaster> WardCasterList = new List<WardCaster>();
        public static List<SC2Timer> SC2TimerList = new List<SC2Timer>();

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
                TextureDraw.Initialize();
                ObjectDetector.Initialize();
                //SpellDetector.Initialize();
                Debug.Initialize();
                LaneManager.Initialize();
                Drawing.OnDraw += Drawing_OnDraw;
                Drawing.OnEndScene += Drawing_OnEndScene;
                //Game.OnTick += Game_OnTick;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE INIT");
            }
        }
        
        private static void Game_OnTick(EventArgs args)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE ON_TICK");
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {

        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            try
            {
                var s1 = Config.DebugMenu["s1"].Cast<Slider>().CurrentValue;
                if (Config.DebugMenu["c1"].Cast<CheckBox>().CurrentValue)
                {
                    Drawing.DrawText(Game.CursorPos2D + new Vector2(-20, -15), System.Drawing.Color.White, ((int)Game.Time / 60) + ":" + ((int)Game.Time % 60), 10);
                }
                if (Config.DebugMenu["c2"].Cast<CheckBox>().CurrentValue)
                {
                    //Drawing.DrawLine(Game.CursorPos2D, Drawing.WorldToScreen(Player.Instance.Position), 3, System.Drawing.Color.Yellow);
                    Drawing.DrawLine(Drawing.WorldToScreen(Player.Instance.Position), Drawing.WorldToScreen(Player.Instance.Position), 3, System.Drawing.Color.Yellow);
                }
                if (Config.DebugMenu["c3"].Cast<CheckBox>().CurrentValue)
                {
                    Drawing.DrawText(Game.CursorPos2D + new Vector2(-20, -15), System.Drawing.Color.White, (Utility.TickCount).ToString(), 10);
                }
               

                Utility.CloneTracker();

                foreach (var list in SpellList.Where(l => l.Buff == true ? l.EndTime >= Game.Time : l.EndTime >= Utility.TickCount))
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
                        case SpellType.Blink:
                            DrawManager.DrawBlink(list);
                            break;
                        case SpellType.Ward:
                            DrawManager.DrawWard(list);
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
