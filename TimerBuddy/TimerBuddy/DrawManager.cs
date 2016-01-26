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
    class DrawManager
    {
        static Font TeleportFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 17));
        static Font TrapFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Gill Sans MT Pro", 15));

        public static void DrawTeleport(Spell list)
        {
            try
            {
                if (list.ChampionName == "Shen" && list.Slot == SpellSlot.R)
                {
                    if (!list.Cancel)
                    {
                        DrawText(list.Caster.BaseSkinName, list.Target.Position + new Vector3(-60, 10, 0), list.GetColor(), list.SpellType);
                        DrawText(list.GetRemainTime(), list.Target.Position + new Vector3(-30, 65, 0), Color.LawnGreen, list.SpellType);
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
                    DrawText(list.GetRemainTime(), list.CastPosition + new Vector3(-30, 65, 0), Color.LawnGreen, list.SpellType);
                    
                    DrawText(list.GetRemainTime(), list.Caster.Position + new Vector3(-15, -10, 0), Color.LawnGreen, list.SpellType);
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
                DrawText(list.GetRemainTime(), list.CastPosition + new Vector3(-15, 0, 0), list.GetColor(), list.SpellType);

                if (list.Team == Team.Enemy)
                    new Circle
                    {
                        Color = System.Drawing.Color.Red,
                        BorderWidth = 4,
                        Radius = 50,
                    }.Draw(list.CastPosition);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_TRAP " + list.Name);
            }
        }
        
        public static void DrawSpell(Spell list)
        {
            try
            {
                if (list.Buff == true)
                {
                    DrawLine(list);
                    return;
                }

                if (list.GameObject == false && list.SkillShot == false)
                {
                    DrawLine(list);
                    return;
                }

                if (list.GameObject)
                    DrawKappa(list);
                //DrawText(list.GetRemainTime(), list.CastPosition, list.GetColor(), list.SpellType);
                else
                    DrawText(list.GetRemainTime(), list.SkillShot ? list.CastPosition : list.Caster.Position + new Vector3(-50, -30, 0), list.GetColor(), list.SpellType);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_SPELL " + list.Name);
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

                if (!hero.VisibleOnScreen || !hero.IsHPBarRendered)
                    return;

                var mainpos = hero.HPBarPosition;
                //var mainpos = hero.Position.WorldToScreen();
                var startpos = hero.IsMe ? mainpos + new Vector2(29, 29) : mainpos + new Vector2(3, 32);
                //var startpos = mainpos + new Vector2(-50, 45);
                float length = spell.Buff == true
                    ? spell.GetRemainTimeFloat() / spell.FullTime * 100
                    : spell.GetRemainTimeFloat() / Utility.GetDatabase(spell).EndTime * 100;
                var endpos = startpos + new Vector2(length, 0);
                var endpos2 = endpos + new Vector2(0, 6);

                var lineColor = System.Drawing.Color.White;
                var textColor = Utility.ConvertColor(spell.GetColor());

                Drawing.DrawLine(startpos, endpos, 1f, lineColor);
                Drawing.DrawLine(endpos, endpos2, 1f, lineColor);

                var textpos = endpos2 + new Vector2(10, 0);
                Drawing.DrawText(textpos, textColor, (spell.GetRemainTimeFloat() / 1000f).ToString("F1"), 10);
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

            DrawText((s5/10f).ToString("F1"), (centerpos + new Vector2(-15, -13)), Color.White, SpellType.Spell);
            // -15, -13

            var length = s5 / 50f * 50f;

            var linestart = centerpos - new Vector2(length, 0);
            var lineend = centerpos + new Vector2(length, 0);

            Drawing.DrawLine(linestart, lineend, 4, System.Drawing.Color.LawnGreen);
        }

        public static void DrawKappa(Spell spell)
        {
            var centerpos = Drawing.WorldToScreen(spell.CastPosition);

            Drawing.DrawLine(centerpos + new Vector2(-500, 0), centerpos + new Vector2(500, 0), 1, System.Drawing.Color.Red);
            Drawing.DrawLine(centerpos + new Vector2(0, -500), centerpos + new Vector2(0, 500), 1, System.Drawing.Color.Red);

            float length = spell.Buff == true
                    ? spell.GetRemainTimeFloat() / spell.FullTime * 100
                    : spell.GetRemainTimeFloat() / Utility.GetDatabase(spell).EndTime * 50;

            DrawText(spell.GetRemainTime(), centerpos + new Vector2(-15, -13), Color.White, SpellType.Spell);
            // -15, -13
            

            var linestart = centerpos - new Vector2(length, 0);
            var lineend = centerpos + new Vector2(length, 0);

            Drawing.DrawLine(linestart, lineend, 4, System.Drawing.Color.LawnGreen);
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
    }
}
