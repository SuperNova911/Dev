using System;
using System.Linq;
using Essential.AutoLevel;
using Essential.Hud;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

/*
 *          만들어야 할 것
 *      리콜 트래커
 *      와드 트래커
 *      자동 레벨업 - 커스텀 로직 만들기
 *      아이템 사용
 *      
 *          만든거
 *      워터마크
 *      자동 레벨업
 */

namespace Essential
{
    internal class Program
    {
        public static Menu Menu;
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;

            
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Menu = MainMenu.AddMenu("Essential", "ess");
            AutoLevelUp.Init();
            HudDraw.Init();

            if (HudDraw.watermark())
            {
                EloBuddy.Hacks.RenderWatermark = true;
            }
            else
            {
                EloBuddy.Hacks.RenderWatermark = false;
            }
        }
    }
}
