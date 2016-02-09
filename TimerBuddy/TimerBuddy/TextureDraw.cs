using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using TimerBuddy.Properties;
using Sprite = EloBuddy.SDK.Rendering.Sprite;

namespace TimerBuddy
{
    public class TextureDraw
    {
        private static readonly TextureLoader TextureLoader = new TextureLoader();

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
                var sc2List = SC2TimerDatabase.Database.Where(d => (heroName.Contains(d.ChampionName) && d.SC2Type == SC2Type.Spell) || (d.SC2Type != SC2Type.Spell)).ToList();

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
                    var menucode = list.GetMenuCode();

                    if (SpriteList.ContainsKey(menucode))
                        continue;

                    TextureLoader.Load(menucode, list.SpriteName);
                    SpriteList.Add(menucode, new Sprite(() => TextureLoader[menucode]));
                }

                if (true)
                {
                    TextureLoader.Load("SC2Green", Resources.SC2Green);
                    SpriteList.Add("SC2Green", new Sprite(() => TextureLoader["SC2Green"]));
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
                e.ErrorMessage("SPRITE_LOAD");
            }
        }

        public static void DrawSprite(Vector2 pos, Spell spell)
        {
            try
            {
                SpriteList[spell.MenuCode].Draw(pos);
            }
            catch (Exception e)
            {
                e.ErrorMessage("DRAW_SPRITE", spell.MenuCode);
            }
        }
        
        public static void DrawSC2Hud(SC2Timer sc2, Vector2 position)
        {
            try
            {
                if (sc2.Caster != null && sc2.Caster.IsMe)
                {
                    SpriteList["SC2Green"].Draw(position);
                    return;
                }

                switch (sc2.Team)
                {
                    case Team.Ally:
                        SpriteList["SC2Blue"].Draw(position);
                        return;

                    case Team.Enemy:
                        SpriteList["SC2Red"].Draw(position);
                        return;

                    case Team.Neutral:
                        SpriteList["SC2Orange"].Draw(position);
                        return;
                }
            }
            catch (Exception e)
            {
                e.ErrorMessage("DRAW_SC2_HUD");
            }
        }
        
        public static void Initialize()
        {
            try
            {

            }
            catch (Exception e)
            {
                e.ErrorMessage("TEXTURE_DRAW_INIT");
            }
        }
    }
}
