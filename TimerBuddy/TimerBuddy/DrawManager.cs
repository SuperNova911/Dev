using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
//using EloBuddy.SDK.Rendering;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public static Font SC2Font = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Arial", 15));
        public static Font SC2Font2 = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Arial", 30));
        public static Font TrapFont2 = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 17));

        public static List<Spell> Line = new List<Spell>();
        public static List<Spell> Timer = new List<Spell>();
        public static List<Spell> TimerLine = new List<Spell>();

        public static List<TimerSlot> TimerSlot = new List<TimerSlot>();

        private static bool DrawWardFix = false;
        private static bool DrawTrapFix = false;
        private static bool DrawBlinkFix = false;

        static DrawManager()
        {
            try
            {
                ClearTimerSlot();

                Game.OnTick += Game_OnTick;
                Drawing.OnEndScene += Drawing_OnEndScene;
            }
            catch (Exception e)
            {
                e.ErrorMessage("DRAWMANAGER_INIT2");
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            try
            {
                if (Line.Count > 0)
                    Line.RemoveAll(d => d.EndTime < (d.Buff ? Game.Time : Utility.TickCount));
                if (Timer.Count > 0)
                    Timer.RemoveAll(d => d.EndTime < (d.Buff ? Game.Time : Utility.TickCount));
                if (TimerLine.Count > 0)
                    TimerLine.RemoveAll(d => d.EndTime < (d.Buff ? Game.Time : Utility.TickCount));
            }
            catch (Exception e)
            {
                e.ErrorMessage("DRAWMANAGER_ONTICK");
            }
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            try
            {
                foreach (var spell in Program.SpellList.Where(d => d.EndTime >= (d.Buff ? Game.Time : Utility.TickCount)))
                    DrawAssign(spell);

                LineManager();
                TimerManager();
                TimerLineManager();
            }
            catch (Exception e)
            {
                e.ErrorMessage("DRAWMANAGER_DRAW");
            }
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
                e.ErrorMessage("CLEAR_TIMER_SLOT");
            }
        }

        private static void DrawAssign(Spell spell)
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

                    case SpellType.SummonerSpell:
                        if (spell.Teleport == true)
                        {
                            DrawTeleport(spell);
                            return;
                        }
                        break;
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
                e.ErrorMessage("DRAW_ASSIGN", spell.Name + " " + spell.DrawType.ToString());
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
                e.ErrorMessage("LINE_MANAGER");
            }
        }

        private static void TimerManager()
        {
            try
            {
                int minImportance = Config.Menu.ComboBoxValue("minImportance");

                foreach (var list in Timer.Where(d => d.EndTime >= (d.Buff ? Game.Time : Utility.TickCount) &&
                ((d.SpellType == SpellType.Spell || d.SpellType == SpellType.SummonerSpell) ? d.GetImportance().ToInt() >= minImportance : true)))
                {
                    DrawTimer(list);
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("TIMER_MANAGER");
            }
        }

        private static void TimerLineManager()
        {
            try
            {
                int minImportance = Config.Menu.ComboBoxValue("minImportance");

                foreach (var list in TimerLine.Where(d => d.EndTime >= (d.Buff ? Game.Time : Utility.TickCount) &&
                ((d.SpellType == SpellType.Spell || d.SpellType == SpellType.SummonerSpell) ? d.GetImportance().ToInt() >= minImportance : true)))
                {
                    DrawTimerLine(list);
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("TIMER_LINE_MANAGER");
            }
        }

        private static void DrawLine(Spell spell)
        {
            try
            {
                Obj_AI_Base hero = spell.Caster;

                if (!hero.VisibleOnScreen || !hero.IsHPBarRendered || !hero.IsHero())
                    return;

                float length = spell.GetRemainTime() / spell.GetFullTime() * 100f;

                Vector2 mainpos = hero.HPBarPosition;
                Vector2 startpos = hero.IsMe ? mainpos + new Vector2(26, 25) : mainpos + new Vector2(1, 30);
                Vector2 endpos = startpos + new Vector2(length, 0);
                Vector2 endpos2 = endpos + new Vector2(0, 6);
                Vector2 textpos = endpos2 + new Vector2(10, 3);
                Vector2 spritepos = textpos + new Vector2(-18, 0);
                
                Color lineColor = spell.GetColor().ConvertColor();
                SharpDX.Color textColor = spell.GetColor();

                Drawing.DrawLine(startpos, endpos, 1f, lineColor);
                Drawing.DrawLine(endpos, endpos2, 1f, lineColor);
                
                LineFont.DrawText(null, spell.GetRemainTimeString(), (int)textpos.X, (int)textpos.Y, textColor);
                TextureDraw.DrawSprite(spritepos, spell);
            }
            catch (Exception e)
            {
                e.ErrorMessage("DRAW_LINE" + spell.Caster.BaseSkinName + " " + spell.Name);
            }
        }

        private static void DrawTimer(Spell spell)
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
                e.ErrorMessage("DRAW_TIMER", spell.Caster.BaseSkinName + " " + spell.Name);
            }
        }

        private static void DrawTimerLine(Spell spell)
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
                e.ErrorMessage("DRAW_TIMER_LINE", spell.Caster.BaseSkinName + " " + spell.Name);
            }
        }

        private static void DrawTrap(Spell spell)
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

                    // Kappa Circle.Draw(spell.GetColor(), spell.Object.BoundingRadius, 4, spell.Object.Position);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_TRAP " + spell.Caster.BaseSkinName + " " + spell.Name, Color.SpringGreen);
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

        private static void DrawBlink(Spell spell)
        {
            try
            {
                if (!Config.MiscMenu.CheckboxValue("blinkAlly") && spell.Team == Team.Ally)
                    return;

                if (!Config.MiscMenu.CheckboxValue("blinkEnemy") && spell.Team == Team.Enemy)
                    return;

                Vector3 startpos = spell.StartPosition;
                Vector3 endpos = spell.KappaRoss();

                Drawing.DrawLine(Drawing.WorldToScreen(startpos), Drawing.WorldToScreen(endpos), 2, spell.Color.ConvertColor());
                Drawing.DrawText(Drawing.WorldToScreen(endpos) + new Vector2(-20, 15), Color.White, spell.Caster.BaseSkinName, 10);
                if (DrawBlinkFix == false)
                    // Kappa new Circle { Color = spell.Color.ConvertColor(), Radius = 30f, BorderWidth = 1 }.Draw(endpos);
                    ;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_BLINK " + spell.Caster.BaseSkinName, Color.SpringGreen);
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

        private static void DrawWard(Spell spell)
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
                    // Kappa Circle.Draw(spell.GetColor(), 50, 4, spell.Object.Position);
                    //new Circle { Color = spell.Color.ConvertColor(), Radius = 50f, BorderWidth = 2 }.Draw(spell.Object.Position);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_WARD " + spell.Caster.BaseSkinName + " " + spell.Name, Color.SpringGreen);
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

        private static void DrawTeleport(Spell spell)
        {
            try
            {
                Vector2 centerpos = Drawing.WorldToScreen(spell.Object.Position) + new Vector2(0, 25);
                string text = spell.ChampionName;
                Vector2 textpos = centerpos + new Vector2(-35, 18);
                SharpDX.Color color = SharpDX.Color.Orange;
                SpellFont.DrawText(null, text, (int)textpos.X, (int)textpos.Y, color);
                DrawTimerLine(spell);
            }
            catch (Exception e)
            {
                e.ErrorMessage("DRAW_TELEPORT" + spell.Caster.BaseSkinName + " " + spell.Name);
            }
        }

        public static void Initialize()
        {
            try
            {

            }
            catch (Exception e)
            {
                e.ErrorMessage("DRAWMANAGER_INIT");
            }
        }
    }
}
