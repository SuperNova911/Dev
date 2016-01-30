using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using SharpDX;
using TimerBuddy.Properties;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using Sprite = EloBuddy.SDK.Rendering.Sprite;

namespace TimerBuddy
{
    public class TextureDraw
    {
        public static readonly TextureLoader TextureLoader = new TextureLoader();

        private static Sprite MainBar { get; set; }

        private static readonly Dictionary<string, Sprite> SpriteList = new Dictionary<string, Sprite>();

        static TextureDraw()
        {
            try
            {
                var hero = EntityManager.Heroes.AllHeroes;
                var heroName = hero.Select(h => h.BaseSkinName).ToArray();
                var summonerList = SpellDatabase.Database.Where(i => i.SpellType == SpellType.SummonerSpell).ToList();
                var itemList = SpellDatabase.Database.Where(i => i.SpellType == SpellType.Item).ToList();
                var trapList = SpellDatabase.Database.Where(t => heroName.Contains(t.ChampionName) && t.SpellType == SpellType.Trap).ToList();
                var spellList = SpellDatabase.Database.Where(s => heroName.Contains(s.ChampionName) && s.SpellType == SpellType.Spell).ToList();

                foreach (var list in summonerList)
                {
                    if (SpriteList.ContainsKey(list.MenuCode))
                        continue;

                    TextureLoader.Load(list.MenuCode, list.SpriteName);
                    SpriteList.Add(list.MenuCode, new Sprite(() => TextureLoader[list.MenuCode]));
                }

                foreach (var list in itemList)
                {
                    if (SpriteList.ContainsKey(list.MenuCode))
                        continue;

                    TextureLoader.Load(list.MenuCode, list.SpriteName);
                    SpriteList.Add(list.MenuCode, new Sprite(() => TextureLoader[list.MenuCode]));
                }

                foreach (var list in trapList)
                {
                    if (SpriteList.ContainsKey(list.MenuCode))
                        continue;

                    TextureLoader.Load(list.MenuCode, list.SpriteName);
                    SpriteList.Add(list.MenuCode, new Sprite(() => TextureLoader[list.MenuCode]));
                }

                foreach (var list in spellList)
                {
                    if (SpriteList.ContainsKey(list.MenuCode))
                        continue;

                    TextureLoader.Load(list.MenuCode, list.SpriteName);
                    SpriteList.Add(list.MenuCode, new Sprite(() => TextureLoader[list.MenuCode]));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE SPRITE_LOAD");
            }
        }

        public static void DrawSprite(Vector2 pos, Spell spell)
        {
            try
            {
                var sprite = SpriteList[spell.MenuCode];

                sprite.Draw(pos);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("error: CODE DRAW_SPRITE " + spell.MenuCode);
            }
        }

        public static void DrawTest(Vector2 vector2)
        {
            var sprite = SpriteList["Summoner Teleport"];

            sprite.Draw(vector2 + new Vector2(-18, 0));
        }
        
        public static void Initialize()
        {

        }
    }
}
