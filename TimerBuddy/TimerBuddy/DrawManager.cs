using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace TimerBuddy
{
    public class TimerSlot
    {
        public Obj_AI_Base hero;
        public int Slot = 0;
    }

    public static class DrawManager
    {
        public static Font TeleportFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 17));
        public static Font SpellFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 17));
        public static Font TrapFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 13));
        public static Font WardFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 11));
        public static Font LineFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 10));
        public static Font TestFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Arial", 15));
        public static Font TestFont2 = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Arial", 30));
        public static Font TrapFont2 = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 17));

        public static List<Spell> Line = new List<Spell>();
        public static List<Spell> Timer = new List<Spell>();
        public static List<Spell> TimerLine = new List<Spell>();

        public static List<TimerSlot> TimerSlot = new List<TimerSlot>();

        public static bool DrawWardFix = false;
        public static bool DrawTrapFix = false;
        public static bool DrawBlinkFix = false;

        static DrawManager()
        {
            ClearTimerSlot();

            Game.OnTick += Game_OnTick;
            Drawing.OnEndScene += Drawing_OnEndScene;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Line.Count > 0)
                Line.RemoveAll(d => d.EndTime < (d.Buff ? Game.Time : Utility.TickCount));
            if (Timer.Count > 0)
                Timer.RemoveAll(d => d.EndTime < (d.Buff ? Game.Time : Utility.TickCount));
            if (TimerLine.Count > 0)
                TimerLine.RemoveAll(d => d.EndTime < (d.Buff ? Game.Time : Utility.TickCount));
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var spell in Program.SpellList.Where(d => d.EndTime >= (d.Buff ? Game.Time : Utility.TickCount)))
                DrawAssign(spell);

            LineManager();
            TimerManager();
            TimerLineManager();
        }

        private static void ClearTimerSlot()
        {
            try
            {
                TimerSlot.Clear();

                foreach (var hero in EntityManager.Heroes.AllHeroes)
                {
                    TimerSlot.Add(new TimerSlot
                    {
                        hero = hero,
                        Slot = 0,
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE CLEAR_TIMER_SLOT", Color.LightBlue);
            }
        }

        public static void DrawAssign(Spell spell)
        {
            try
            {
                if (spell.SpellType == SpellType.Spell && !Config.Menu.CheckboxValue("sTimer"))
                    return;

                if (spell.SpellType == SpellType.SummonerSpell && !Config.Menu.CheckboxValue("ssTimer"))
                    return;

                if (spell.SpellType == SpellType.Item && !Config.Menu.CheckboxValue("itemTimer"))
                    return;

                if (spell.SpellType == SpellType.Spell && Config.SpellMenu.CheckboxValue(spell.MenuCode + "onlyme") && Player.Instance.BaseSkinName != spell.ChampionName)
                    return;

                if (spell.SpellType == SpellType.Spell && !Config.SpellMenu.CheckboxValue(spell.MenuCode + "draw"))
                    return;

                if (spell.SpellType == SpellType.SummonerSpell && !Config.SummonerMenu.CheckboxValue(spell.MenuCode + "draw"))
                    return;

                if (spell.SpellType == SpellType.Item && !Config.ItemMenu.CheckboxValue(spell.MenuCode + "draw"))
                    return;

                if (spell.SpellType == SpellType.Item && !Config.ItemMenu.CheckboxValue(spell.MenuCode + "ally") && spell.Team == Team.Ally)
                    return;

                if (spell.SpellType == SpellType.Item && !Config.ItemMenu.CheckboxValue(spell.MenuCode + "enemy") && spell.Team == Team.Enemy)
                    return;

                switch (spell.SpellType)
                {
                    case SpellType.Blink:
                        if (Config.Menu.CheckboxValue("blinkTracker"))
                            DrawBlink(spell);
                        return;

                    case SpellType.Trap:
                        if (Config.Menu.CheckboxValue("trapTimer"))
                            DrawTrap(spell);
                        return;

                    case SpellType.Ward:
                        if (Config.Menu.CheckboxValue("wardTimer"))
                            DrawWard(spell);
                        return;
                }

                if (Line.Contains(spell) || Timer.Contains(spell) || TimerLine.Contains(spell))
                    return;
                
                switch (spell.GetDrawType())
                {
                    case DrawType.Default:
                        if (spell.GameObject)
                        {
                            TimerLine.Add(spell);
                            return;
                        }
                        if (spell.SkillShot)
                        {
                            TimerLine.Add(spell);
                            return;
                        }
                        Line.Add(spell);
                        return;

                    case DrawType.HPLine:
                        Line.Add(spell);
                        return;

                    case DrawType.Number:
                        Timer.Add(spell);
                        return;

                    case DrawType.NumberLine:
                        TimerLine.Add(spell);
                        return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_ASSIGN " + spell.Name + " " + spell.DrawType.ToString(), Color.LightBlue);
            }
        }

        private static void LineManager()
        {
            try
            {
                //int maxLine = 3;
                int minImportance = Config.Menu.ComboBoxValue("minImportance");

                if (minImportance <= 3)
                {
                    foreach (var spell in Line.Where(d => d.Importance == Importance.VeryHigh && d.EndTime >= (d.Buff ? Game.Time : Utility.TickCount)))
                    {
                        DrawLine(spell);
                    }
                }

                if (minImportance <= 2)
                {
                    foreach (var spell in Line.Where(d => d.Importance == Importance.High && d.EndTime >= (d.Buff ? Game.Time : Utility.TickCount)))
                    {
                        DrawLine(spell);
                    }
                }

                if (minImportance <= 1)
                {
                    foreach (var spell in Line.Where(d => d.Importance == Importance.Medium && d.EndTime >= (d.Buff ? Game.Time : Utility.TickCount)))
                    {
                        DrawLine(spell);
                    }
                }

                if (minImportance <= 0)
                {
                    foreach (var spell in Line.Where(d => d.Importance == Importance.Low && d.EndTime >= (d.Buff ? Game.Time : Utility.TickCount)))
                    {
                        DrawLine(spell);
                    }
                }

                /*
                foreach (var hero in EntityManager.Heroes.AllHeroes.Where(d => d.IsValid && d.IsVisible))
                {
                    var low = Line.Where(d => d.Importance == Importance.Low && d.Caster == hero);
                    var medium = Line.Where(d => d.Importance == Importance.Medium && d.Caster == hero);
                    var high = Line.Where(d => d.Importance == Importance.High && d.Caster == hero);
                    var veryhigh = Line.Where(d => d.Importance == Importance.VeryHigh && d.Caster == hero);
                    var slot = TimerSlot.FirstOrDefault(d => d.hero == hero);

                    var database = Line.FirstOrDefault(d => d.Caster == hero && d.EndTime < (d.Buff ? Game.Time : Utility.TickCount));

                    if (database != null)
                    {
                        if (database.Drawing == true)
                        {
                            database.Drawing = false;
                            slot.Slot--;
                        }

                        Line.Remove(database);
                    }

                    Drawing.DrawText(Drawing.WorldToScreen(Player.Instance.Position) + new Vector2(0, 0), Color.Orange, low.Count().ToString() + " " + (low == null).ToString(), 10);
                    Drawing.DrawText(Drawing.WorldToScreen(Player.Instance.Position) + new Vector2(0, 20), Color.Orange, medium.Count().ToString() + " " + (medium == null).ToString(), 10);
                    Drawing.DrawText(Drawing.WorldToScreen(Player.Instance.Position) + new Vector2(0, 40), Color.Orange, high.Count().ToString() + " " + (high == null).ToString(), 10);
                    Drawing.DrawText(Drawing.WorldToScreen(Player.Instance.Position) + new Vector2(0, 60), Color.Orange, veryhigh.Count().ToString() + " " + (veryhigh == null).ToString(), 10);

                    if (veryhigh != null && minImportance <= 3)
                    {
                        foreach (var spell in veryhigh.Where(d => d.Drawing == false))
                        {
                            if (slot.Slot >= maxLine)
                                continue;

                            DrawLine(spell);
                            spell.Drawing = true;
                            slot.Slot++;
                        }
                    }

                    if (high != null && minImportance <= 2)
                    {
                        foreach (var spell in high.Where(d => d.Drawing == false))
                        {
                            if (slot.Slot >= maxLine)
                                continue;

                            DrawLine(spell);
                            spell.Drawing = true;
                            slot.Slot++;
                        }
                    }

                    if (medium != null && minImportance <= 1)
                    {
                        foreach (var spell in medium)
                        {
                            if (slot.Slot >= maxLine)
                                continue;

                            DrawLine(spell);
                            spell.Drawing = true;
                            slot.Slot++;
                        }
                    }

                    if (low != null && minImportance <= 0)
                    {
                        foreach (var spell in low.Where(d => d.Drawing == false))
                        {
                            if (slot.Slot >= maxLine)
                                continue;

                            DrawLine(spell);
                            spell.Drawing = true;
                            slot.Slot++;
                        }
                    }
                }*/
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE LINE_MANAGER", Color.LightBlue);
            }
        }

        public static void TimerManager()
        {
            try
            {
                foreach (var list in Timer.Where(d => d.EndTime >= (d.Buff ? Game.Time : Utility.TickCount)))
                {
                    DrawTimer(list);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE TIMER_MANAGER", Color.LightBlue);
            }
        }

        public static void TimerLineManager()
        {
            try
            {
                foreach (var list in TimerLine.Where(d => d.EndTime >= (d.Buff ? Game.Time : Utility.TickCount)))
                {
                    DrawTimerLine(list);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE TIMER_LINE_MANAGER", Color.LightBlue);
            }
        }

        public static void DrawLine(Spell spell)
        {
            try
            {
                Obj_AI_Base hero = spell.Caster;

                if (!hero.VisibleOnScreen || !hero.IsHPBarRendered || !hero.IsHero())
                    return;

                Vector2 mainpos = hero.HPBarPosition;
                Vector2 startpos = hero.IsMe ? mainpos + new Vector2(25, 25) : mainpos + new Vector2(3, 32);

                float length = spell.GetRemainTime() / spell.GetFullTime() * 100f;
                Vector2 endpos = startpos + new Vector2(length, 0);
                Vector2 endpos2 = endpos + new Vector2(0, 6);

                Color lineColor = spell.GetColor().ConvertColor();
                SharpDX.Color textColor = spell.GetColor();

                Drawing.DrawLine(startpos, endpos, 1f, lineColor);
                Drawing.DrawLine(endpos, endpos2, 1f, lineColor);

                Vector2 textpos = endpos2 + new Vector2(10, 3);
                //Drawing.DrawText(textpos, textColor, spell.GetRemainTimeString(), 10);
                LineFont.DrawText(null, spell.GetRemainTimeString(), (int)textpos.X, (int)textpos.Y, textColor);
                Vector2 spritepos = textpos + new Vector2(-18, 0);
                TextureDraw.DrawSprite(spritepos, spell);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_LINE " + spell.Caster.BaseSkinName + " " + spell.Name, Color.LightBlue);
            }
        }

        public static void DrawTimer(Spell spell)
        {
            try
            {
                string text = spell.GetRemainTimeString();
                Vector2 position;
                if (spell.GameObject)
                    position = Drawing.WorldToScreen(spell.Object.Position);
                else if (spell.SkillShot)
                    position = Drawing.WorldToScreen(spell.CastPosition);
                else
                    position = Drawing.WorldToScreen(spell.Target.Position);
                position += new Vector2(-15, 0);
                SharpDX.Color color = spell.GetColor();
                
                SpellFont.DrawText(null, text, (int)position.X, (int)position.Y, color);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_TIMER " + spell.Caster.BaseSkinName + " " + spell.Name, Color.LightBlue);
            }
        }

        public static void DrawTimerLine(Spell spell)
        {
            try
            {
                Vector2 centerpos = Drawing.WorldToScreen(spell.GameObject ? spell.Object.Position : spell.SkillShot ? spell.CastPosition : spell.Target.Position) + new Vector2(0, 25);
                
                float remain = spell.GetRemainTime();
                float full = spell.GetFullTime();
                bool dynamic = full >= 3000 ? true : false;

                float length = dynamic ? remain / full * 70f : remain / full * 55f;

                if (spell.GetFullTime() >= 3500 && full - remain <= 500)
                {
                    float length2 = (full - remain) / 500f * length;
                    length = length2;
                }

                string text = spell.GetRemainTimeString();
                Vector2 textpos = centerpos + new Vector2(-15, -13);
                SharpDX.Color color = spell.GetColor();
                SpellFont.DrawText(null, text, (int)textpos.X, (int)textpos.Y, color);

                Color barColor = spell.Team == Team.Ally ? Color.LawnGreen : spell.Team == Team.Enemy ? Color.Red : Color.Orange;

                Vector2 linepos = centerpos + new Vector2(0, 15);
                Vector2 linestart = linepos - new Vector2(length, 0);
                Vector2 lineend = linepos + new Vector2(length, 0);

                Drawing.DrawLine(linestart - new Vector2(1, 0), lineend + new Vector2(1, 0), 6, Color.Black);
                Drawing.DrawLine(linestart, lineend, 4, barColor);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_TIMER_LINE " + spell.Caster.BaseSkinName + " " + spell.Name, Color.LightBlue);
            }
        }

        public static void DrawTrap(Spell spell)
        {
            try
            {
                if (!Config.TrapMenu.CheckboxValue(spell.MenuCode + "draw"))
                    return;

                if (!Config.TrapMenu.CheckboxValue(spell.MenuCode + "ally") && spell.Team == Team.Ally)
                    return;

                if (!Config.TrapMenu.CheckboxValue(spell.MenuCode + "enemy") && spell.Team == Team.Enemy)
                    return;

                string text = (spell.GetRemainTime() / 1000).ClockStyle();
                Vector2 position = Drawing.WorldToScreen(spell.Object.Position) + new Vector2(-15, 0);
                SharpDX.Color color = spell.GetColor();

                TrapFont.DrawText(null, text, (int)position.X, (int)position.Y, color);

                if (Config.TrapMenu.CheckboxValue(spell.MenuCode + "drawCircle") && DrawTrapFix == false)
                {
                    if (spell.Team == Team.Ally && Config.TrapMenu.CheckboxValue("circleOnlyEnemy"))
                        return;

                    Circle.Draw(spell.GetColor(), spell.Object.BoundingRadius, 4, spell.Object.Position);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_TRAP " + spell.Caster.BaseSkinName + " " + spell.Name, Color.Cyan);
                if (DrawTrapFix == false)
                {
                    Chat.Print("Temporarily Fix Trap Timer", Color.Gold);
                    DrawTrapFix = true;
                }
                else
                {
                    Chat.Print("Fixing failed, Disable Trap Timer, Please report bugs with CODE", Color.Gold);
                    Config.Menu["trapTimer"].Cast<CheckBox>().CurrentValue = false;
                }
            }
        }

        public static void DrawBlink(Spell spell)
        {
            try
            {
                Vector3 startpos = spell.StartPosition;
                Vector3 endpos = spell.KappaRoss();

                Drawing.DrawLine(Drawing.WorldToScreen(startpos), Drawing.WorldToScreen(endpos), 2, spell.Color.ConvertColor());
                Drawing.DrawText(Drawing.WorldToScreen(endpos) + new Vector2(-20, 15), Color.White, spell.Caster.BaseSkinName, 10);
                if (DrawBlinkFix == false)
                    new Circle { Color = spell.Color.ConvertColor(), Radius = 30f, BorderWidth = 1 }.Draw(endpos);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_BLINK " + spell.Caster.BaseSkinName, Color.LightBlue);
                if (DrawBlinkFix == false)
                {
                    Chat.Print("Temporarily Fix Blink Tracker", Color.Gold);
                    DrawBlinkFix = true;
                }
                else
                {
                    Chat.Print("Fixing failed, Disable Blink Tracker, Please report bugs with CODE", Color.Gold);
                    Config.Menu["blinkTracker"].Cast<CheckBox>().CurrentValue = false;
                }
            }
        }

        public static void DrawWard(Spell spell)
        {
            try
            {
                if (!Config.WardMenu.CheckboxValue(spell.MenuCode + "draw"))
                    return;

                if (!Config.WardMenu.CheckboxValue(spell.MenuCode + "ally") && spell.Team == Team.Ally)
                    return;

                if (!Config.WardMenu.CheckboxValue(spell.MenuCode + "enemy") && spell.Team == Team.Enemy)
                    return;

                if (spell.FullTime != 77777777)
                {
                    string text = (spell.GetRemainTime() / 1000).ClockStyle();
                    Vector2 position = Drawing.WorldToScreen(spell.Object.Position) + new Vector2(-12, 0);
                    SharpDX.Color color = spell.GetColor();

                    WardFont.DrawText(null, text, (int)position.X, (int)position.Y, color);
                }

                if (Config.WardMenu.CheckboxValue(spell.MenuCode + "drawCircle") && DrawWardFix == false)
                {
                    Circle.Draw(spell.GetColor(), 50, 4, spell.Object.Position);
                    //new Circle { Color = spell.Color.ConvertColor(), Radius = 50f, BorderWidth = 2 }.Draw(spell.Object.Position);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_WARD " + spell.Caster.BaseSkinName + " " + spell.Name, Color.Cyan);
                if (DrawWardFix == false)
                {
                    Chat.Print("Temporarily Fix Ward Timer", Color.Gold);
                    DrawWardFix = true;
                }
                else
                {
                    Chat.Print("Fixing failed, Disable Ward Timer, Please report bugs with CODE", Color.Gold);
                    Config.Menu["wardTimer"].Cast<CheckBox>().CurrentValue = false;
                }
            }
        }

        public static void Initialize()
        {

        }


        /*
        public static void DrawLine()
        {
            var s1 = Config.DebugMenu["s1"].Cast<Slider>().CurrentValue;
            var s2 = Config.DebugMenu["s2"].Cast<Slider>().CurrentValue;
            var s3 = Config.DebugMenu["s3"].Cast<Slider>().CurrentValue;

            foreach (var hero in EntityManager.Heroes.AllHeroes.Where(h => h.IsValid && h.VisibleOnScreen && h.IsHPBarRendered))
            {
                var mainpos = hero.HPBarPosition;
                var startpos = hero.IsMe ? mainpos + new Vector2(29, 29) : mainpos + new Vector2(3, 32);   // 25, 37 / 3, 32 /me 29, 29

                //float length = spell.GetRemainTimeFloat() / Utility.GetDatabase(spell).EndTime * s2;
                var length = 100;
                var endpos = startpos + new Vector2(length, 0);
                var endpos2 = endpos + new Vector2(0, 6);

                Drawing.DrawLine(startpos, endpos, 1f, System.Drawing.Color.White);
                Drawing.DrawLine(endpos, endpos2, 1f, System.Drawing.Color.White);

                var textpos = endpos2 + new Vector2(10, 0);
                Drawing.DrawText(textpos, System.Drawing.Color.White, "3.7", 10);
                TextureDraw.DrawTest(textpos);
            }
        }

        public static void DrawLine23(Spell spell)
        {
            try
            {
                var s1 = Config.DebugMenu["s1"].Cast<Slider>().CurrentValue;
                var s2 = Config.DebugMenu["s2"].Cast<Slider>().CurrentValue;
                var s3 = Config.DebugMenu["s3"].Cast<Slider>().CurrentValue;

                var hero = spell.Caster;

                if (!hero.VisibleOnScreen || !hero.IsHPBarRendered || !hero.IsHero())
                    return;

                var mainpos = hero.HPBarPosition;
                //var mainpos = hero.Position.WorldToScreen();
                var startpos = hero.IsMe ? mainpos + new Vector2(25, 25) : mainpos + new Vector2(3, 32);
                //var startpos = mainpos + new Vector2(-50, 45);

                float length = spell.Buff == true
                    ? spell.GetRemainTime() / spell.GetFullTime() * 100
                    : spell.GetRemainTime() / spell.GetFullTime() * 100;
                var endpos = startpos + new Vector2(length, 0);
                var endpos2 = endpos + new Vector2(0, 6);

                var lineColor = System.Drawing.Color.White;
                var textColor = Utility.ConvertColor(spell.GetColor());

                Drawing.DrawLine(startpos, endpos, 1f, lineColor);
                Drawing.DrawLine(endpos, endpos2, 1f, lineColor);

                var textpos = endpos2 + new Vector2(10, 0);
                Drawing.DrawText(textpos, textColor, spell.GetRemainTimeString(), 10);
                var spritepos = textpos + new Vector2(-18, 0);
                TextureDraw.DrawSprite(spritepos, spell);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW LINE " + spell.Name);
            }
        }

        public static void DrawLine2()
        {
            try
            {
                var s1 = Config.DebugMenu["s1"].Cast<Slider>().CurrentValue;
                var s2 = Config.DebugMenu["s2"].Cast<Slider>().CurrentValue;
                var s3 = Config.DebugMenu["s3"].Cast<Slider>().CurrentValue;
                var s4 = Config.DebugMenu["s4"].Cast<Slider>().CurrentValue;
                var s5 = Config.DebugMenu["s5"].Cast<Slider>().CurrentValue;

                var hero = Player.Instance;

                if (!hero.VisibleOnScreen || !hero.IsHPBarRendered)
                    return;

                //var mainpos = hero.HPBarPosition + new Vector2(-s1, -s2);
                var mainpos = hero.Position.WorldToScreen();
                //var startpos = hero.IsMe ? mainpos + new Vector2(29, 29) : mainpos + new Vector2(3, 32);
                var startpos = mainpos + new Vector2(-50, 45);
                float length = 100;
                var endpos = startpos + new Vector2(length, 0);
                var endpos2 = endpos + new Vector2(0, 6);

                var lineColor = System.Drawing.Color.LawnGreen;
                var textColor = System.Drawing.Color.White; //Utility.ConvertColor(spell.GetColor());

                Drawing.DrawLine(startpos, endpos, s3, lineColor);
                Drawing.DrawLine(endpos, endpos2, s3, lineColor);

                var textpos = endpos2 + new Vector2(10, 0) + new Vector2(s4, s5);
                Drawing.DrawText(textpos, textColor, "3.7", 10);
                var spritepos = textpos + new Vector2(0, 0);
                TextureDraw.DrawTest(spritepos);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW LINE ");
            }
        }

        public static void DrawKappa()
        {
            var s1 = Config.DebugMenu["s1"].Cast<Slider>().CurrentValue;
            var s2 = Config.DebugMenu["s2"].Cast<Slider>().CurrentValue;
            var s3 = Config.DebugMenu["s3"].Cast<Slider>().CurrentValue;
            var s4 = Config.DebugMenu["s4"].Cast<Slider>().CurrentValue;
            var s5 = Config.DebugMenu["s5"].Cast<Slider>().CurrentValue;

            var centerpos = Game.CursorPos2D + new Vector2(-300, -200);

            Drawing.DrawLine(centerpos + new Vector2(-500, 0), centerpos + new Vector2(500, 0), 1, System.Drawing.Color.Red);
            Drawing.DrawLine(centerpos + new Vector2(0, -500), centerpos + new Vector2(0, 500), 1, System.Drawing.Color.Red);
            
            DrawText(Game.Time.ToString("F3"), (centerpos + new Vector2(-15, -13)), Color.White, SpellType.Spell);
            // -15, -13

            var length = s5 / 50f * 50f;

            var linestart = centerpos - new Vector2(length, 0);
            var lineend = centerpos + new Vector2(length, 0);

            Drawing.DrawLine(linestart, lineend, 4, System.Drawing.Color.LawnGreen);
        }

        public static void DrawDynamicTimer(Spell spell)
        {
            int s5 = 25;
            Vector2 centerpos = Drawing.WorldToScreen(spell.GameObject ? spell.Object.Position : spell.SkillShot ? spell.CastPosition : spell.Target.Position) + new Vector2(0, s5);

            //Drawing.DrawLine(centerpos + new Vector2(-500, 0), centerpos + new Vector2(500, 0), 1, System.Drawing.Color.Red);
            //Drawing.DrawLine(centerpos + new Vector2(0, -500), centerpos + new Vector2(0, 500), 1, System.Drawing.Color.Red);

            float remain = spell.GetRemainTime();
            float full = spell.GetFullTime();
            bool Kappa = full >= 3000 ? true : false;

            float length = Kappa ? remain / full * 70f : remain / full * 55f;
            
            if (spell.GetFullTime() >= 3500 && full - remain <= 500)
            {
                float length2 = (full - remain) / 500f * length;
                length = length2;
            }
            
            DrawText(spell.GetRemainTimeString(), centerpos + new Vector2(-15, -13), Color.White, SpellType.Spell);
            // -15, -13
            var barColor = spell.Team == Team.Ally ? System.Drawing.Color.LawnGreen : spell.Team == Team.Enemy ? System.Drawing.Color.Red : System.Drawing.Color.Orange;

            var linepos = centerpos + new Vector2(0, 15);
            var linestart = linepos - new Vector2(length, 0);
            var lineend = linepos + new Vector2(length, 0);

            Drawing.DrawLine(linestart - new Vector2(1, 0), lineend + new Vector2(1, 0), 6, System.Drawing.Color.Black);
            Drawing.DrawLine(linestart, lineend, 4, barColor);
        }
        public static void DrawBlink(this Spell spell)
        {
            try
            {
                Vector3 startpos = spell.StartPosition;
                Vector3 endpos = spell.KappaRoss(); 

                Drawing.DrawLine(Drawing.WorldToScreen(startpos), Drawing.WorldToScreen(endpos), 2, spell.Color.ConvertColor());
                new Circle { Color = spell.Color.ConvertColor(), Radius = 30f, BorderWidth = 1 }.Draw(endpos);
                Drawing.DrawText(Drawing.WorldToScreen(endpos) + new Vector2(-20, 15), System.Drawing.Color.White, spell.Caster.BaseSkinName, 10);
                //new Geometry.Polygon.Line(startpos, endpos).Draw(spell.Color);
                //new Geometry.Polygon.Sector(endpos, startpos, 50 * (float)Math.PI / 180, 50).Draw(spell.Color);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_BLINK " + spell.Name, Color.LightBlue);
            }
        }


        public static void DrawLine(Spell spell)
        {
            try
            {
                var hero = spell.SpellType == SpellType.Buff ? spell.Target : spell.Caster;

                if (!hero.VisibleOnScreen || !hero.IsHPBarRendered)
                    return;
                
                var barpos = hero.HPBarPosition;
                var startpos = barpos + new Vector2(20, 134);
                float length = spell.SpellType == SpellType.Buff 
                    ? spell.GetRemainTimeFloat() / spell.FullTime * 100
                    : spell.GetRemainTimeFloat() / Utility.GetDatabase(spell).EndTime * 100;
                var endpos = startpos + new Vector2(length, 0);
                var endpos2 = endpos + new Vector2(0, 6);

                var lineColor = System.Drawing.Color.White;
                var textColor = System.Drawing.Color.White; //Utility.ConvertColor(spell.GetColor());

                Drawing.DrawLine(startpos, endpos, 1f, lineColor);
                Drawing.DrawLine(endpos, endpos2, 1f, lineColor);

                var textpos = endpos2 + new Vector2(-8, 0);
                Drawing.DrawText(textpos, textColor, (spell.GetRemainTimeFloat() / 1000f).ToString("F1"), 10);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW LINE " + spell.Name);
            }
        }

        public static void Test()
        {
            var s1 = Config.DebugMenu["s1"].Cast<Slider>().CurrentValue;
            var s2 = Config.DebugMenu["s2"].Cast<Slider>().CurrentValue;
            var s3 = Config.DebugMenu["s3"].Cast<Slider>().CurrentValue;
            var s4 = Config.DebugMenu["s4"].Cast<Slider>().CurrentValue;
            var s5 = Config.DebugMenu["s5"].Cast<Slider>().CurrentValue;

            //var centerpos = new Vector2(Drawing.Width, Drawing.Height / 2) + new Vector2(-s1, -s2);
            var centerpos = Game.CursorPos2D + new Vector2(-500, -200);

            Drawing.DrawLine(centerpos + new Vector2(-500, 0), centerpos + new Vector2(500, 0), 1, System.Drawing.Color.Red);
            Drawing.DrawLine(centerpos + new Vector2(0, -500), centerpos + new Vector2(0, 500), 1, System.Drawing.Color.Red);
            Drawing.DrawLine(centerpos + new Vector2(0, 41), centerpos + new Vector2(240, 41), 82, System.Drawing.Color.Black);
            var sprite1 = TextureDraw.SpriteList["SC2Blue"];
            sprite1.Draw(centerpos);

        }

        public static void Test2()
        {
            var s1 = Config.DebugMenu["s1"].Cast<Slider>().CurrentValue;
            var s2 = Config.DebugMenu["s2"].Cast<Slider>().CurrentValue;
            var s3 = Config.DebugMenu["s3"].Cast<Slider>().CurrentValue;
            var s4 = Config.DebugMenu["s4"].Cast<Slider>().CurrentValue;
            var s5 = Config.DebugMenu["s5"].Cast<Slider>().CurrentValue;

            //var centerpos = new Vector2(Drawing.Width, Drawing.Height / 2) + new Vector2(-s1, -s2);
            var centerpos = Game.CursorPos2D + new Vector2(-500, -200);


            var namepos = centerpos + new Vector2(24, 1);
            var name = "Summoner Flash";
            TestFont.DrawText(null, name, (int)(namepos).X, (int)(namepos).Y, SharpDX.Color.White);

            var timerpos = centerpos + new Vector2(85, 21);
            var timer = 8.1f.ToString();
            TestFont2.DrawText(null, timer, (int)(timerpos).X, (int)(timerpos).Y, SharpDX.Color.White);

            var iconpos = centerpos + new Vector2(25, 25);
            var sprite2 = TextureDraw.SpriteList["sc2TwitchR"];
            sprite2.Draw(iconpos);

            var length = 200f / 200f * s5;
            Drawing.DrawLine(centerpos + new Vector2(s1, s2), centerpos + new Vector2(s1 + length, s2), s3, System.Drawing.Color.DarkCyan);
        }
        */
    }
}
