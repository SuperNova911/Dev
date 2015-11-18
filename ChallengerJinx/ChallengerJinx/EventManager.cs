using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace ChallengerJinx
{
    internal class EventManager
    {
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }

        public static float FishBoneBonus { get { return 75f + 25f * SpellManager.Q.Level; } }

        public static float MinigunRange(Obj_AI_Base target = null)
        {
            return (590 + (target != null ? target.BoundingRadius : 0));
        }

        public static bool FishBoneActive { get { return _Player.AttackRange > 525; } }

        public const int AoeRadius = 200;

        public static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Config.Misc.GapCloser && sender.IsEnemy && e.End.Distance(_Player) < 200)
            {
                SpellManager.E.Cast(e.End);
            }
        }
    }
}
