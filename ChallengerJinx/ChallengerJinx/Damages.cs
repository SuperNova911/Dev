﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;

namespace ChallengerJinx
{
    class Damages
    {
        public static int WDamage(Obj_AI_Base target)
        {
            return
                (int)
                    (new int[] {10, 60, 110, 160, 210}[ - 1] +
                     1.4*(ObjectManager.Player.TotalAttackDamage));
        }

        public static int EDamage(Obj_AI_Base target)
        {
            return
                (int)
                    (new double[] { 80, 135, 190, 245, 300 }[SpellManager.E.Level -1]
                                    + 1 * ObjectManager.Player.FlatMagicDamageMod);
        }
    }
}