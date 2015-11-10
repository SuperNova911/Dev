using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
/*
        버그
    라인클리어할때 E를 안씀
 *  FocusTarget 작동안함

        해야할것
    정글몹에 E쓰기
 *  체력바에 데미지 드로잉
 *  텍스트쓰기
 * */

namespace ChallengerTristana
{
    internal class Program
    {
        public static Menu TristanaMenu, DrawMenu;
        public static string Version = "0.1";

        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }

        static void Main(string[] args)
        {
            if (args != null)
            {
                try
                {
                    Loading.OnLoadingComplete += Loading_OnLoadingComplete;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Bootstrap.Init(null);

            if (Player.ChampionName != "Tristana")
                return;

            TristanaMenu = MainMenu.AddMenu("ChallengerTristana", "cgTristana");
            TristanaMenu.AddGroupLabel("ChallengerTristana V" + Version);
            TristanaMenu.AddSeparator();
            TristanaMenu.AddLabel("Made by Tychus");

            SpellManager.Init();
            StateManager.Init();
            EventManager.Init();

            DrawMenu = TristanaMenu.AddSubMenu("Drawings", "Drawings");
            DrawMenu.AddGroupLabel("Drawings");
            DrawMenu.AddLabel("General");
            DrawMenu.Add("AAdraw", new CheckBox("Draw AA Range"));
            DrawMenu.AddLabel("Skills");
            DrawMenu.Add("Wdraw", new CheckBox("Draw W"));
            DrawMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawMenu.Add("Rdraw", new CheckBox("Draw R"));
            DrawMenu.AddLabel("MIsc");
//            DrawMenu.Add("Qdraw", new CheckBox("Show Q Statue"));
            DrawMenu.Add("ReadyDraw", new CheckBox("Draw Only When Skills Are Ready", false));

            Drawing.OnDraw += Drawing_OnDraw;

            Chat.Print("- Challenger Tristana - <font color=\"#FFFFFF\">Loaded</font>", Color.Aqua);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead) return;

            var drawAA = DrawMenu["AAdraw"].Cast<CheckBox>().CurrentValue;
//            var drawQ = DrawMenu["Qdraw"].Cast<CheckBox>().CurrentValue;
            var drawW = DrawMenu["Wdraw"].Cast<CheckBox>().CurrentValue;
            var drawE = DrawMenu["Edraw"].Cast<CheckBox>().CurrentValue;
            var drawR = DrawMenu["Rdraw"].Cast<CheckBox>().CurrentValue;
            var drawWhenReady = DrawMenu["ReadyDraw"].Cast<CheckBox>().CurrentValue;

/*            if (drawQ && SpellManager.Q.IsReady())
            {
                
            }*/

            if (drawWhenReady)
            {
                if (drawAA && (!SpellManager.E.IsReady() && !SpellManager.R.IsReady()))
                    new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = SpellManager.Q.Range }.Draw(Player.Position);

                if (SpellManager.W.IsReady() && drawW)
                    new Circle { Color = Color.SkyBlue, BorderWidth = 4, Radius = SpellManager.W.Range }.Draw(Player.Position);
                if (SpellManager.E.IsReady() && drawE && !drawR)
                    new Circle { Color = Color.Orange, BorderWidth = 4, Radius = SpellManager.E.Range }.Draw(Player.Position);
                if (SpellManager.R.IsReady() && drawR && !drawE)
                    new Circle { Color = Color.Violet, BorderWidth = 4, Radius = SpellManager.R.Range }.Draw(Player.Position);

                if (drawE && drawR)
                {
                    if (SpellManager.E.IsReady())
                    {
                        new Circle { Color = Color.Orange, BorderWidth = 4, Radius = SpellManager.E.Range }.Draw(Player.Position);
 //                       new Text("E RDY", new Font("Calisto MT", 10F, FontStyle.Bold));
                    }
                    if (SpellManager.R.IsReady() && !SpellManager.E.IsReady())
                        new Circle { Color = Color.Violet, BorderWidth = 4, Radius = SpellManager.R.Range }.Draw(Player.Position);
                }
                
            }
            else
            {
                if (drawAA || drawE || drawR)
                    new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = SpellManager.Q.Range }.Draw(Player.Position);
                if (drawW)
                    new Circle { Color = Color.SkyBlue, BorderWidth = 4, Radius = SpellManager.W.Range }.Draw(Player.Position);
            }
        }
    }
}
