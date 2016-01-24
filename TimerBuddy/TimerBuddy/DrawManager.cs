using EloBuddy;
using EloBuddy.SDK;
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
                }
                else
                {
                    DrawText(list.Caster.BaseSkinName, list.CastPosition + new Vector3(-60, 10, 0), list.GetColor(), list.SpellType);
                    DrawText("Canceled", list.CastPosition + new Vector3(-70, 65, 0), Color.Red, list.SpellType);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_TP " + list.Caster.BaseSkinName);
            }
        }

        public static void DrawItem(Spell list)
        {
            try
            {
                DrawText(list.GetRemainTime(), list.CastPosition + new Vector3(-15, -10, 0), list.GetColor(), list.SpellType);
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
                DrawText(list.GetRemainTime(), list.CastPosition + new Vector3(-20, 0, 0), list.GetColor(), list.SpellType);

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
                if (list.GameObject)
                    DrawText(list.GetRemainTime(), list.CastPosition, list.GetColor(), list.SpellType);
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
                    case SpellType.Teleport:
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

        public static void DrawBuff()
        {
            foreach (var hero in EntityManager.Heroes.AllHeroes.Where(h => h.IsValidTarget() && h.VisibleOnScreen))
            {
                if (hero.HasBuff("VladimirHemoplagueDebuff"))
                    DrawText(hero.BuffRemainTime("VladimirHemoplagueDebuff").ToString("F2"), hero.Position, Color.White, SpellType.Spell);

                if (hero.HasBuff("RengarRBuff"))
                    DrawText(hero.BuffRemainTime("RengarRBuff").ToString("F2"), hero.Position, Color.White, SpellType.Spell);
            }
        }
    }
}
