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

namespace TimerBuddy
{
    public static class DrawManager
    {
        public static Font TeleportFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 17));
        public static Font TrapFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 15));
        public static Font TestFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Arial", 15));
        public static Font TestFont2 = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Arial", 30));

        public static void DrawTeleport(Spell list)
        {
            try
            {
                if (list.ChampionName == "Shen" && list.Slot == SpellSlot.R)
                {
                    if (!list.Cancel)
                    {
                        DrawText(list.Caster.BaseSkinName, list.Target.Position + new Vector3(-60, 10, 0), list.GetColor(), list.SpellType);
                        DrawText(list.GetRemainTimeString(), list.Target.Position + new Vector3(-30, 65, 0), Color.LawnGreen, list.SpellType);
                    }
                    else
                    {
                        DrawText(list.Caster.BaseSkinName, list.Target.Position + new Vector3(-60, 10, 0), list.GetColor(), list.SpellType);
                        DrawText("Canceled", list.Target.Position + new Vector3(-70, 65, 0), Color.Red, list.SpellType);
                    }

                    return;
                }
                if (!list.Cancel)
                {
                    DrawText(list.Caster.BaseSkinName, list.CastPosition + new Vector3(-60, 10, 0), list.GetColor(), list.SpellType);
                    DrawText(list.GetRemainTimeString(), list.CastPosition + new Vector3(-30, 65, 0), Color.LawnGreen, list.SpellType);
                    
                    DrawText(list.GetRemainTimeString(), list.Caster.Position + new Vector3(-15, -10, 0), Color.LawnGreen, list.SpellType);
                }
                else
                {
                    DrawText(list.Caster.BaseSkinName, list.CastPosition + new Vector3(-60, 10, 0), list.GetColor(), list.SpellType);
                    DrawText("Canceled", list.CastPosition + new Vector3(-70, 65, 0), Color.Red, list.SpellType);

                    DrawText("Canceled", list.Caster.Position + new Vector3(-15, -10, 0), Color.Red, list.SpellType);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_TP " + list.Caster.BaseSkinName);
            }
        }

        public static void DrawSummoner(Spell spell)
        {
            try
            {
                DrawLine(spell);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_SS " + spell.Name);
            }
        }

        public static void DrawItem(Spell list)
        {
            try
            {
                //DrawText(list.GetRemainTime(), list.CastPosition + new Vector3(-15, -10, 0), list.GetColor(), list.SpellType);
                if (list.DrawType == DrawType.NumberLine)
                {
                    DrawKappa(list);
                    return;
                }
                if (list.GameObject == true)
                {
                    DrawKappa(list);
                    return;
                }
                DrawLine(list);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_ITEM " + list.Caster.BaseSkinName);
            }
        }

        public static void DrawTrap(Spell list)
        {
            try
            {
                DrawText(list.GetRemainTimeString(), list.Object.Position + new Vector3(-15, 0, 0), list.GetColor(), list.SpellType);

                if (list.Team == Team.Enemy)
                    new Circle
                    {
                        Color = System.Drawing.Color.Red,
                        BorderWidth = 4,
                        Radius = 50,
                    }.Draw(list.Object.Position);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_TRAP " + list.Name);
            }
        }
        
        public static void DrawSpell(Spell spell)
        {
            try
            {
                if (spell.DrawType == DrawType.NumberLine)
                {
                    DrawKappa(spell);
                    return;
                }

                if (spell.GameObject || spell.SkillShot)
                {
                    DrawKappa(spell);
                    return;
                }
                DrawLine(spell);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_SPELL " + spell.Name);
            }
        }

        public static void DrawText(string text, Vector3 position, Color color, SpellType type)
        {
            try
            {
                switch (type)
                {
                    case SpellType.Trap:
                        TrapFont.DrawText(null,
                        text,
                        (int)Drawing.WorldToScreen(position).X,
                        (int)Drawing.WorldToScreen(position).Y,
                        color);
                        break;
                    case SpellType.SummonerSpell:
                        TeleportFont.DrawText(null,
                        text,
                        (int)Drawing.WorldToScreen(position).X,
                        (int)Drawing.WorldToScreen(position).Y,
                        color);
                        break;
                    case SpellType.Item:
                        TeleportFont.DrawText(null,
                        text,
                        (int)Drawing.WorldToScreen(position).X,
                        (int)Drawing.WorldToScreen(position).Y,
                        color);
                        break;
                    case SpellType.Spell:
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

        public static void DrawText(string text, Vector2 position, Color color, SpellType type)
        {
            try
            {
                switch (type)
                {
                    case SpellType.Trap:
                        TrapFont.DrawText(null,
                        text,
                        (int)position.X,
                        (int)position.Y,
                        color);
                        break;
                    case SpellType.SummonerSpell:
                        TeleportFont.DrawText(null,
                        text,
                        (int)position.X,
                        (int)position.Y,
                        color);
                        break;
                    case SpellType.Item:
                        TeleportFont.DrawText(null,
                        text,
                        (int)position.X,
                        (int)position.Y,
                        color);
                        break;
                    case SpellType.Spell:
                        TeleportFont.DrawText(null,
                        text,
                        (int)position.X,
                        (int)position.Y,
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

        public static void DrawBuff(Spell spell)
        {
            try
            {
                if (spell.DrawType == DrawType.NumberLine)
                {
                    DrawKappa(spell);
                    return;
                }
                    

                DrawLine(spell);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_BUFF " + spell.Name);
            }
        }

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

        public static void DrawLine(Spell spell)
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

        public static void DrawKappa(Spell spell)
        {
            int s5 = Config.DebugMenu["s5"].Cast<Slider>().CurrentValue;
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

        public static void DrawWard(this Spell spell)
        {
            try
            {
                if (spell.FullTime != 77777777)
                    DrawText(spell.GetRemainTimeString(), spell.Object.Position + new Vector3(-15, 0, 0), spell.GetColor(), SpellType.Trap);
                
                if (spell.Team == Team.Enemy)
                {
                    new Circle { Color = spell.Color.ConvertColor(), Radius = 50f, BorderWidth = 2 }.Draw(spell.Object.Position);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE DRAW_BLINK " + spell.Name, Color.LightBlue);
            }
        }

        /*
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
        */

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
            TestFont.DrawText(null, name, (int)(namepos).X, (int)(namepos).Y, Color.White);

            var timerpos = centerpos + new Vector2(85, 21);
            var timer = 8.1f.ToString();
            TestFont2.DrawText(null, timer, (int)(timerpos).X, (int)(timerpos).Y, Color.White);

            var iconpos = centerpos + new Vector2(25, 25);
            var sprite2 = TextureDraw.SpriteList["sc2TwitchR"];
            sprite2.Draw(iconpos);

            var length = 200f / 200f * s5;
            Drawing.DrawLine(centerpos + new Vector2(s1, s2), centerpos + new Vector2(s1 + length, s2), s3, System.Drawing.Color.DarkCyan);
        }
    }
}
