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
 *      갱 알림
 *      
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

        public static string Version = "1.0";
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;

            
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Menu = MainMenu.AddMenu("Essential", "ess");
            Menu.AddGroupLabel("Essential V" + Version);
            Menu.AddSeparator();

            Menu.AddGroupLabel("Master Control");
            Menu.Add("autolvlup", new CheckBox("Auto Level Up", true));
            Menu.Add("hud", new CheckBox("HUD", true));

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
        internal static bool AutoLevelUpSwitch()
        {
            var sw = Menu["autolvlup"].Cast<CheckBox>().CurrentValue;
            return sw;
        }
        internal static bool HudSwitch()
        {
            var sw = Menu["hud"].Cast<CheckBox>().CurrentValue;
            return sw;
        }
    }
}
