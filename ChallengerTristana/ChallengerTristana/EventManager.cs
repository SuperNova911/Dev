using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ChallengerTristana
{
    class EventManager
    {
        public static Menu MiscMenu;

        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            MiscMenu = Program.TristanaMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Misc");
            MiscMenu.AddLabel("GapCloser/Interrupter");
            MiscMenu.Add("Rgapclose", new CheckBox("Use R on GapCloser"));
            MiscMenu.Add("Rinterrupt", new CheckBox("Use R to Interrupter"));

            Obj_AI_Base.OnLevelUp += AIHeroClient_OnLevelUp;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
        }

        static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsValidTarget(SpellManager.Q.Range) || e.DangerLevel != DangerLevel.High)
                return;
            if (SpellManager.R.IsReady() && SpellManager.R.IsInRange(sender) && MiscMenu["Rinterrrupt"].Cast<CheckBox>().CurrentValue)
            {
                SpellManager.R.Cast(sender);
            }
        }

        private static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || !MiscMenu["Rgapclose"].Cast<CheckBox>().CurrentValue)
                return;

            SpellManager.R.Cast(e.Sender);
        }

        private static void AIHeroClient_OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            var level = (uint)Player.Level;
            SpellManager.Q = new Spell.Active(SpellSlot.Q, (uint)Player.AttackRange);
            SpellManager.E = new Spell.Targeted(SpellSlot.E, (uint)Player.AttackRange);
            SpellManager.R = new Spell.Targeted(SpellSlot.R, (uint)Player.AttackRange);
        }
    }
}