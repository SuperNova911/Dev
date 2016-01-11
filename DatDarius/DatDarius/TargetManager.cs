using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace DatDarius
{
    public class TargetManager
    {
        public static AIHeroClient Player = ObjectManager.Player;

        public static AIHeroClient RTarget = null;

        static TargetManager()
        {
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {

        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.IsDead && Player.CountEnemiesInRange(1000) == 0)
                return;

            GetRTarget();
        }

        private static AIHeroClient ETarget()
        {
            AIHeroClient target = null;

            return target;
        }

        private static void GetRTarget()
        {
            AIHeroClient target = null;

            foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => Ultimate.UltimateCal(e).Killable))
            {
                if (target == null)
                {
                    target = enemy;
                    continue;
                }

                if (TargetSelector.GetPriority(enemy) >= TargetSelector.GetPriority(target))
                {
                    if (TargetSelector.GetPriority(enemy) > TargetSelector.GetPriority(target))
                        target = enemy;
                    else if (enemy.ServerPosition.Distance(Player.ServerPosition) > target.ServerPosition.Distance(Player.ServerPosition))
                        target = enemy;
                }
            }

            RTarget = target;
        }

        public static void Initialize()
        {

        }
    }
}
