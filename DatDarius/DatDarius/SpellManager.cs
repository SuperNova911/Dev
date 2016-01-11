using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System;
using System.Linq;

namespace DatDarius
{
    public static class SpellManager
    {
        private static AIHeroClient Player = ObjectManager.Player;

        public static Spell.Active Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Targeted R { get; private set; }

        public static Spell.Skillshot Flash { get; private set; }
        public static Spell.Targeted Ignite { get; private set; }

        public static float eAngle { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, 425);
            W = new Spell.Active(SpellSlot.W, 200);
            E = new Spell.Skillshot(SpellSlot.E, 550, SkillShotType.Cone, 250, int.MaxValue, 225);
            R = new Spell.Targeted(SpellSlot.R, 460);

            SpellDataInst flash = Player.Spellbook.Spells.Where(s => s.Name.Contains("summonerflash")).Any()
                ? Player.Spellbook.Spells.Where(spell => spell.Name.Contains("summonerflash")).First() : null;
            SpellDataInst ignite = Player.Spellbook.Spells.Where(s => s.Name.Contains("summonerdot")).Any()
                ? Player.Spellbook.Spells.Where(spell => spell.Name.Contains("summonerdot")).First() : null;

            if (flash != null)
            {
                Flash = new Spell.Skillshot(flash.Slot, 425, SkillShotType.Linear);
            }
            if (ignite != null)
            {
                Ignite = new Spell.Targeted(ignite.Slot, 600);
            }

            eAngle = 50 * (float)Math.PI / 180;
        }
    }
}