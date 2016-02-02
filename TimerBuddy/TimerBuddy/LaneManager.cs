using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TimerBuddy
{
    public enum Lane
    {
        Top, Mid, Jungle, Bottom
    }

    public static class LaneManager
    {
        public static List<AIHeroClient> Top = new List<AIHeroClient>();
        public static List<AIHeroClient> Mid = new List<AIHeroClient>();
        public static List<AIHeroClient> Jungle = new List<AIHeroClient>();
        public static List<AIHeroClient> Bottom = new List<AIHeroClient>();

        public static Lane MyLane;

        public static Vector3 toppos = new Vector3(2139, 12576, 53);
        public static Vector3 midpos = new Vector3(7469, 7469, 54);
        public static Vector3 botpos = new Vector3(12576, 2139, 53);

        static LaneManager()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            SetLaneMember();
        }

        private static void SetLaneMember()
        {
            Chat.Print("Setlane.....");
            foreach (var hero in EntityManager.Heroes.AllHeroes)
            {
                if (hero.HasSmite() && Jungle.Count < 2)
                {
                    Jungle.Add(hero);
                    if (hero.IsMe)
                        MyLane = Lane.Jungle;
                    continue;
                }

                if (!hero.IsValid || !hero.OnLane())
                    continue;
                

                hero.SetLane();
            }
            Chat.Print((Top.Count + Mid.Count + Bottom.Count + Jungle.Count).ToString(), System.Drawing.Color.HotPink);
            if (EntityManager.Heroes.AllHeroes.Count != Top.Count + Mid.Count + Bottom.Count + Jungle.Count)
            {
                Chat.Print("Reset");
                Top.Clear();
                Mid.Clear();
                Bottom.Clear();
                Jungle.Clear();
                Core.DelayAction(() => SetLaneMember(), 10000);
                return;
            }

            if (EntityManager.Heroes.AllHeroes.Count == Top.Count + Mid.Count + Bottom.Count + Jungle.Count)
            {
                Chat.Print("Top Lane", System.Drawing.Color.Red);
                foreach (var hero in Top)
                    Chat.Print(hero.BaseSkinName, System.Drawing.Color.Gold);

                Chat.Print("Mid Lane", System.Drawing.Color.Red);
                foreach (var hero in Mid)
                    Chat.Print(hero.BaseSkinName, System.Drawing.Color.Gold);

                Chat.Print("Bot Lane", System.Drawing.Color.Red);
                foreach (var hero in Bottom)
                    Chat.Print(hero.BaseSkinName, System.Drawing.Color.Gold);

                Chat.Print("Jungle Lane", System.Drawing.Color.Red);
                foreach (var hero in Jungle)
                    Chat.Print(hero.BaseSkinName, System.Drawing.Color.Gold);
            }
                
        }

        private static void SetLane(this AIHeroClient unit)
        {
            var top = unit.Distance(toppos);
            var mid = unit.Distance(midpos);
            var bot = unit.Distance(botpos);

            if (top <= mid && top <= bot)
            {
                Top.Add(unit);
                if (unit.IsMe)
                    MyLane = Lane.Top;
            }

            if (mid <= top && mid <= bot)
            {
                Mid.Add(unit);
                if (unit.IsMe)
                    MyLane = Lane.Mid;
            }

            if (bot <= top && bot <= mid)
            {
                Bottom.Add(unit);
                if (unit.IsMe)
                    MyLane = Lane.Mid;
            }
        }


        private static bool OnLane(this AIHeroClient unit)
        {
            try
            {
                var home = unit.Team == GameObjectTeam.Order ? new Vector3(394, 461, 171) : new Vector3(14340, 14391, 179);

                if (unit.Distance(home) > 7000)
                    return true;

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Chat.Print("<font color='#FF0000'>ERROR:</font> CODE ON_LANE " + unit.BaseSkinName, Color.LightBlue);
                return false;
            }
        }

        public static List<AIHeroClient> MyLaneMember()
        {
            switch (MyLane)
            {
                case Lane.Top:
                    return Top;

                case Lane.Mid:
                    return Mid;

                case Lane.Jungle:
                    return Jungle;

                case Lane.Bottom:
                    return Bottom;
            }

            return Top;
        }

        public static void Initialize()
        {

        }
    }
}
