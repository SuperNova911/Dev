using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Color = System.Drawing.Color;

namespace Essential.AutoLevel
{
    internal class AutoLevelUp
    {
        public static Menu SequenceMenu;

        public static int rStart;

        public static int[] Sequence;

        private static SpellSlot Smite;

        private static SpellSlot Heal;

        public static string Concept = "";

        public static AIHeroClient Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        public static void Init()
        {
            var menu = Program.Menu;

            SequenceMenu = menu.AddSubMenu("AutoLevelUp");

//            SequenceMenu.AddGroupLabel("Logic");
//            SequenceMenu.Add("custom", new CheckBox("Custom Logic", false));
//            SequenceMenu.Add("q", new Slider("Q", 2, 1, 4));
//            SequenceMenu.Add("w", new Slider("W", 3, 1, 4));
//            SequenceMenu.Add("e", new Slider("E", 4, 1, 4));

            SequenceMenu.AddGroupLabel("Start Level");
            SequenceMenu.Add("startLevel", new Slider("Level", 2, 2, 7));

            SequenceMenu.AddGroupLabel("Misc");
            SequenceMenu.Add("delay", new Slider("LevelUp Delay (ms)", 150, 0, 3000));
            SequenceMenu.Add("chat", new CheckBox("Chat Notification (Local)", false));
            SequenceMenu.Add("switch", new CheckBox("Enable AutoLevelUp", true));

            var Smite = EloBuddy.Player.Spells.FirstOrDefault(s => s.SData.Name == "summonerSmite");
            var Heal = EloBuddy.Player.Spells.FirstOrDefault(h => h.SData.Name == "summonerHeal");

            switch (Player.ChampionName)
            {
                case "QWE":
                    Sequence = new int[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "QEW":
                    Sequence = new int[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "WQE":
                    Sequence = new int[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "WEQ":
                    Sequence = new int[] { 2, 3, 1, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "EQW":
                    Sequence = new int[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "EWQ":
                    Sequence = new int[] { 3, 2, 1, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                    break;
                case "Aatrox":
                    Sequence = new int[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Ahri":
                    Sequence = new int[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Akali":
                    Sequence = new int[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Amumu":
                    Sequence = new int[] { 3, 2, 1, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Anivia":
                    Sequence = new int[] { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Annie":
                    Sequence = new int[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Ashe":
                    Sequence = new int[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "Azir":
                    Sequence = new int[] { 2, 1, 3, 2, 2, 4, 1, 2, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3 };
                    break;
                case "Blitzcrank":
                    Sequence = new int[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Brand":
                    Sequence = new int[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Braum":
                    Sequence = new int[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Caitlyn":
                    Sequence = new int[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Cassiopeia":
                    Sequence = new int[] { 1, 3, 3, 2, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Chogath":
                    Sequence = new int[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Corki":
                    if (Player.PercentMagicDamageMod < Player.PercentPhysicalDamageMod)
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " AD";
                    }
                    else
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " AP";
                    }
                    break;
                case "Darius":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Diana":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "DrMundo":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Draven":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Elise":
                    rStart = 1;
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 2, 1, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " Lane";
                    }
                    break;
                case "Evelynn":
                    Sequence = new[] { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Ezreal":
                    if (Heal != null && Heal.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " AD";
                    }
                    else
                    {
                        Sequence = new[] { 1, 2, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        Concept = " AP";
                    }
                    break;
                case "Ekko":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "FiddleSticks":
                    Sequence = new[] { 2, 3, 1, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "Fiora":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 3, 1, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " Jungle";
                    }
                    else
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " Lane";
                    }
                    break;
                case "Fizz":
                    Sequence = new[] { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Galio":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Gangplank":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Garen":
                    Sequence = new[] { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Gnar":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Gragas":
                    Sequence = new[] { 1, 3, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Graves":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Hecarim":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Heimerdinger":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Irelia":
                    Sequence = new[] { 3, 1, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Janna":
                    Sequence = new[] { 3, 1, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                    break;
                case "JarvanIV":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 3, 1, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 2, 3, 2, 4, 2, 2 };
                        Concept = " Lane";
                    }
                    break;
                case "Jax":
                    Sequence = new[] { 3, 1, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Jayce":
                    Sequence = new[] { 1, 3, 2, 1, 1, 2, 1, 3, 1, 3, 1, 3, 3, 2, 2, 3, 2, 2 };
                    rStart = 1;
                    break;
                case "Jinx":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Karma":
                    Sequence = new[] { 1, 3, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    rStart = 1;
                    break;
                case "Karthus":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Kassadin":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Katarina":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Kalista":
                    Sequence = new[] { 3, 1, 3, 2, 3, 4, 1, 3, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Kayle":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 3, 1, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 2, 3, 2, 4, 2, 2 };
                        Concept = " Lane";
                    }
                    break;
                case "Kennen":
                    Sequence = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "Khazix":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Kindred":
                    Sequence = new[] { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "KogMaw":
                    if (Heal != null && Heal.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        Concept = " AD";
                    }
                    else
                    {
                        Sequence = new[] { 3, 2, 1, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        Concept = " AP";
                    }
                    break;
                case "Leblanc":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 3, 4, 3, 3, 3, 2, 4, 2, 2 };
                    break;
                case "LeeSin":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " Lane";
                    }
                    break;
                case "Leona":
                    Sequence = new[] { 3, 1, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Lissandra":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Lucian":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Lulu":
                    Sequence = new[] { 1, 3, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                    break;
                case "Lux":
                    Sequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Malphite":
                    Sequence = new[] { 3, 1, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                    break;
                case "Malzahar":
                    Sequence = new[] { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Maokai":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 3, 2, 1, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 3, 1, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " Lane";
                    }
                    break;
                case "MasterYi":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "MissFortune":
                    Sequence = new[] { 1, 2, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "Mordekaiser":
                    Sequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Morgana":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Nami":
                    Sequence = new[] { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Nasus":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 3, 1, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 1, 2, 1, 4, 1, 1 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " Lane";
                    }
                    break;
                case "Nautilus":
                    Sequence = new[] { 2, 3, 1, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                    break;
                case "Nidalee":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    rStart = 1;
                    break;
                case "Nocturne":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 2, 3, 2, 4, 2, 2 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " Lane";
                    }
                    break;
                case "Nunu":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 1, 3, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        Concept = " Lane";
                    }
                    break;
                case "Olaf":
                    Sequence = new[] { 2, 1, 3, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Orianna":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Pantheon":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Poppy":
                    Sequence = new[] { 2, 1, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Quinn":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Rammus":
                    Sequence = new[] { 2, 1, 3, 2, 3, 4, 2, 3, 3, 3, 4, 2, 2, 1, 1, 4, 1, 1 };
                    break;
                case "Renekton":
                    Sequence = new[] { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Rengar":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " Lane";
                    }
                    break;
                case "Riven":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 1, 2, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " Lane";
                    }
                    break;
                case "Rumble":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "RekSai":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " Lane";
                    }
                    break;
                case "Ryze":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 2, 4, 2, 2, 2, 3, 4, 3, 3 };
                    break;
                case "Sejuani":
                    Sequence = new[] { 2, 3, 1, 2, 2, 4, 2, 1, 2, 3, 4, 3, 3, 3, 1, 4, 1, 1 };
                    break;
                case "Shaco":
                    if (Player.PercentMagicDamageMod > Player.PercentPhysicalDamageMod)
                    {
                        Sequence = new[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                        Concept = " AP";
                    }
                    else
                    {
                        Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " AD";
                    }
                    break;
                case "Shen":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Shyvana":
                    Sequence = new[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Singed":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Sion":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Sivir":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Skarner":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 3, 4, 3, 2, 2, 3, 4, 3, 2 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 2, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 3, 3, 3, 4, 3, 3 };
                        Concept = " Lane";
                    }
                    break;
                case "Sona":
                    Sequence = new[] { 1, 2, 3, 1, 2, 4, 1, 2, 1, 2, 4, 1, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Soraka":
                    Sequence = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "Swain":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Syndra":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Talon":
                    Sequence = new[] { 2, 3, 1, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "Taric":
                    Sequence = new[] { 3, 2, 1, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "TahmKench":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Teemo":
                    Sequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Thresh":
                    Sequence = new[] { 1, 3, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Tristana":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Trundle":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 3, 4, 2, 2, 2, 3, 4, 3, 3 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 2, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " Lane";
                    }
                    break;
                case "Tryndamere":
                    Sequence = new[] { 3, 2, 1, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "TwistedFate":
                    if (Player.FlatMagicDamageMod > 0)
                    {
                        Sequence = new[] { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Concept = " AP";
                    }
                    else
                    {
                        Sequence = new[] { 2, 3, 3, 2, 3, 4, 3, 2, 3, 2, 4, 2, 4, 1, 1, 1, 1, 1 };
                        Concept = " AD";
                    }
                    break;
                case "Twitch":
                    Sequence = new[] { 3, 2, 1, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Udyr":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 4, 1, 3, 4, 4, 3, 4, 3, 4, 3, 3, 1, 1, 1, 1, 2, 2, 2 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 3, 1, 2, 1, 2, 3, 2, 3, 3, 2, 4, 4, 4 };
                        Concept = " Lane";
                    }
                    break;
                case "Urgot":
                    Sequence = new[] { 3, 1, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Varus":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Vayne":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 3, 2, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        Concept = " Lane";
                    }
                    break;
                case "Veigar":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Velkoz":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Vi":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 3, 1, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 3, 1, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " Lane";
                    }
                    break;
                case "Viktor":
                    Sequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Vladimir":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Volibear":
                    Sequence = new[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Warwick":
                    Sequence = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "MonkeyKing":
                    Sequence = new[] { 3, 1, 2, 1, 1, 4, 3, 1, 3, 1, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Xerath":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "XinZhao":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 2, 4, 2, 3, 2, 3, 4, 2, 3 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Concept = " Lane";
                    }
                    break;
                case "Yasuo":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Yorick":
                    Sequence = new[] { 2, 3, 1, 3, 3, 4, 3, 2, 3, 1, 4, 2, 1, 2, 1, 4, 2, 1 };
                    break;
                case "Zac":
                    if (Smite != null && Smite.Slot != SpellSlot.Unknown)
                    {
                        Sequence = new[] { 2, 1, 3, 3, 1, 4, 3, 1, 3, 1, 4, 3, 1, 2, 2, 4, 2, 2 };
                        Concept = " Jungler";
                    }
                    else
                    {
                        Sequence = new[] { 2, 3, 1, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                        Concept = " Lane";
                    }
                    break;
                case "Zed":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 3, 1, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Ziggs":
                    Sequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Zilean":
                    Sequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Zyra":
                    Sequence = new[] { 3, 2, 1, 3, 1, 4, 3, 1, 3, 1, 4, 3, 1, 2, 2, 4, 2, 2 };
                    break;
            }

            Obj_AI_Base.OnLevelUp += AIHeroClient_OnLevelUp;
        }
        void qLevelUp()
        {

        }
        private static void AIHeroClient_OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            var lvlsw = SequenceMenu["switch"].Cast<CheckBox>().CurrentValue;
            if (!sender.IsMe || !sender.IsValid || !lvlsw)
                return;
            var level = (uint)Player.Level;
//            var customSW = SequenceMenu["custom"].Cast<CheckBox>().CurrentValue;
            var start = SequenceMenu["startlevel"].Cast<Slider>().CurrentValue;
//            var qLogic = SequenceMenu["q"].Cast<Slider>().CurrentValue;
//            var wLogic = SequenceMenu["w"].Cast<Slider>().CurrentValue;
//            var eLogic = SequenceMenu["e"].Cast<Slider>().CurrentValue;
            var chat = SequenceMenu["chat"].Cast<CheckBox>().CurrentValue;
            var delay = SequenceMenu["delay"].Cast<Slider>().CurrentValue;
//            var qlvl = Player.Spellbook.GetSpell(SpellSlot.Q).Level;
//            var wlvl = Player.Spellbook.GetSpell(SpellSlot.W).Level;
//            var elvl = Player.Spellbook.GetSpell(SpellSlot.E).Level;
//            var rlvl = Player.Spellbook.GetSpell(SpellSlot.R).Level;

            if (level < start)
                return;

            if (Sequence[level] == 1)
            {
                Core.DelayAction(() => ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q), delay);
            }
            else if (Sequence[level] == 2)
            {
                Core.DelayAction(() => ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.W), delay);
            }
            else if (Sequence[level] == 3)
            {
                Core.DelayAction(() => ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.E), delay);
            }
            else if (Sequence[level] == 4)
            {
                Core.DelayAction(() => ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.R), delay);
            }

            if (chat == true)
            {
                Chat.Print("Level UP!!", Color.LawnGreen);
            }
        }
        
    }
}