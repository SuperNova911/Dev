using System;
using EloBuddy;
using EloBuddy.SDK;

namespace ChallengerBlitzcrank
{
    internal class DamageHandler
    {
        static Spell.Skillshot Q { get { return SpellManager.Q; } }
        static Spell.Active W { get { return SpellManager.W; } }
        static Spell.Active E { get { return SpellManager.E; } }
        static Spell.Active R { get { return SpellManager.R; } }

        static int[] QDamages = new int[] { 80, 135, 190, 245, 300 };
        static int[] RDamages = new int[] { 250, 375, 500 };

        public static double ComboDamage(Obj_AI_Base target)
        {
            double dmg = 0;

            dmg += Q.IsReady()
                ? Qdmg()
                : 0;

            dmg += R.IsReady()
                ? Rdmg()
                : 0;

            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, (float)dmg);
        }

        public static double QDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, (float)Qdmg());
        }

        public static double RDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, (float)Rdmg());
        }

        public static double Qdmg()
        {
            return QDamages[Q.Level - 1] + 1 * Player.Instance.TotalMagicalDamage;
        }

        public static double Rdmg()
        {
            return RDamages[R.Level - 1] + 1 * Player.Instance.TotalMagicalDamage;
        }
    }
}
