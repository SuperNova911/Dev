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

        public static readonly Dictionary<string, Sprite> SpriteList = new Dictionary<string, Sprite>();

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
                var sc2List = SC2TimerDatabase.Database.Where(d => (heroName.Contains(d.ChampionName) && d.SC2Type == SC2Type.Spell) || (d.SC2Type == SC2Type.SummonerSpell)).ToList();

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

                foreach (var list in sc2List)
                {
                    var menucode = list.SC2Type ==
                        SC2Type.SummonerSpell ? "sc2" + list.DisplayName : 
                        list.SC2Type == SC2Type.Spell ? "sc2" + list.ChampionName + list.Slot.ToString() : 
                        list.DisplayName;

                    if (SpriteList.ContainsKey(menucode))
                        continue;

                    TextureLoader.Load(menucode, list.SpriteName);
                    SpriteList.Add(menucode, new Sprite(() => TextureLoader[menucode]));
                }

                if (true)
                {
                    TextureLoader.Load("SC2Blue", Resources.SC2Blue);
                    SpriteList.Add("SC2Blue", new Sprite(() => TextureLoader["SC2Blue"]));
                    TextureLoader.Load("SC2Orange", Resources.SC2Orange);
                    SpriteList.Add("SC2Orange", new Sprite(() => TextureLoader["SC2Orange"]));
                    TextureLoader.Load("SC2Red", Resources.SC2Red);
                    SpriteList.Add("SC2Red", new Sprite(() => TextureLoader["SC2Red"]));
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

        public static void DrawSC2Hud(Team team, Vector2 position)
        {
            switch (team)
            {
                case Team.Ally:
                    SpriteList["SC2Blue"].Draw(position);
                    break;
                case Team.Enemy:
                    SpriteList["SC2Red"].Draw(position);
                    break;
                case Team.Neutral:
                    SpriteList["SC2Orange"].Draw(position);
                    break;
            }
        }
        
        public static void Initialize()
        {

        }
    }
}
