/*using System;
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

namespace Essential.AutoLevel
{
    internal class AutoLevelUp
    {
        public static Menu SequenceMenu;

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

            SequenceMenu.AddGroupLabel("Logic");
            SequenceMenu.Add("q", new Slider("Q", 2, 1, 4));
            SequenceMenu.Add("w", new Slider("W", 4, 1, 4));
            SequenceMenu.Add("e", new Slider("E", 3, 1, 4));
            SequenceMenu.Add("r", new Slider("R", 1, 1, 4));

            SequenceMenu.AddGroupLabel("Start Level");
            SequenceMenu.Add("startLevel", new Slider("Level", 2, 2, 7));

            SequenceMenu.AddGroupLabel("Misc");
            SequenceMenu.Add("chat", new CheckBox("Chat Notification (Local)", false));
            SequenceMenu.Add("delay", new Slider("LevelUp Delay (ms)", 150, 0, 3000));

            Obj_AI_Base.OnLevelUp += AIHeroClient_OnLevelUp;
        }

        private static void AIHeroClient_OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            var level = (uint)Player.Level;
            var start = SequenceMenu["startlevel"].Cast<Slider>().CurrentValue;
            var qLogic = SequenceMenu["q"].Cast<Slider>().CurrentValue;
            var wLogic = SequenceMenu["w"].Cast<Slider>().CurrentValue;
            var eLogic = SequenceMenu["e"].Cast<Slider>().CurrentValue;
            var rLogic = SequenceMenu["r"].Cast<Slider>().CurrentValue;
            var chat = SequenceMenu["chat"].Cast<CheckBox>().CurrentValue;
            var delay = SequenceMenu["delay"].Cast<Slider>().CurrentValue;
            var qlvl = Player.Spellbook.GetSpell(SpellSlot.Q).Level;
            var wlvl = Player.Spellbook.GetSpell(SpellSlot.W).Level;
            var elvl = Player.Spellbook.GetSpell(SpellSlot.E).Level;
            var rlvl = Player.Spellbook.GetSpell(SpellSlot.R).Level;

        //    if (level < start)
        //        return;

            ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q);
            Chat.Print("level up");
            Chat.Print(qlvl);

            if (level < 4)
            {
                if ((qlvl == 0) && ((qLogic > wLogic) || (qLogic > eLogic)))
                {
                    ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q);
                }
                else if ((wlvl == 0) && ((wLogic > qLogic) || (wLogic > eLogic)))
                {
                    ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.W);
                }
                else if ((elvl == 0) && ((eLogic > qLogic) || (eLogic > wLogic)))
                {
                    ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.E);
                }
            }

            var slot = new[] { 0, 0, 0, 0 };
            for (var i = 0; i < 4; i++)
            {
                if (qLogic == i)
                    slot[i] = 1;
                else if (wLogic == i)
                    slot[i] = 2;
                else if (eLogic == i)
                    slot[i] = 3;
                else if (rLogic == i)
                    slot[i] = 4;
            }

            for (var i = 1; i < 5; i++)
            {
                if (slot[i] == 4)
                {
                    if ((level == 6) || (level == 12) || (level == 18))
                        ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.R);
                    else
                    {
                        if (slot[i] == 1)
                        {
                            ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q);
                            if (qlvl == 4)
                            {
                                if 
                            }
                        }
                            

                        else if (slot[i] == 2)
                            ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.W);
                        else if (slot[i] == 3)
                            ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.E);
                    }
                }
            }
        }
    }
}
*/