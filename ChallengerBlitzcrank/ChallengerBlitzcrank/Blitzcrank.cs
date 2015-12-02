using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Enumerations;

namespace ChallengerBlitzcrank
{
    public class Blitzcrank
    {
        private static AIHeroClient Player = EloBuddy.Player.Instance;

        private static Spell.Skillshot Q { get { return SpellManager.Q; } }
        private static Spell.Active W { get { return SpellManager.W; } }
        private static Spell.Active E { get { return SpellManager.E; } }
        private static Spell.Active R { get { return SpellManager.R; } }

        static Geometry.Polygon.Circle DashCircle; //don't use new, just what I wrote

        private static bool SpellShield(AIHeroClient unit)
        {
            return unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield);
        }
        private static bool SpellShield(Obj_AI_Base unit)
        {
            return unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield);
        }

        private static bool CC(AIHeroClient unit)
        {
            return unit.HasBuffOfType(BuffType.Charm) || unit.HasBuffOfType(BuffType.Fear)
                || unit.HasBuffOfType(BuffType.Snare) || unit.HasBuffOfType(BuffType.Slow)
                || unit.HasBuffOfType(BuffType.Taunt) || unit.HasBuffOfType(BuffType.Stun)
                || unit.HasBuffOfType(BuffType.Knockup) || unit.HasBuffOfType(BuffType.Suppression);
        }

        public static Menu Menu;

        static Blitzcrank()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        static void Main(string[] args)
        {
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "Blitzcrank")
                return;



            Config.Initialize();

            Menu = Config.Menu.AddSubMenu("Grab Mode", "grabMenu");
            Menu.AddGroupLabel("Grab Mode");
            Menu.AddLabel("1 = Don't Grab, 2 = Normal Grab, 3 = Auto Grab");
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                Menu.Add("grabMode" + enemy.ChampionName, new Slider(enemy.ChampionName, 2, 1, 3));
            }

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Interrupt.OnInterruptableTarget += Interrupt_OnInterruptableTarget;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Orbwalker.OnAttack += Orbwalker_OnAttack;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Dash.OnDash += Dash_OnDash;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.IsDead || Player.IsRecalling())
                return;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                Combo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                Harass();

            KillSteal();
            Immobile();
        }

        private static void Combo()
        {
            var Qtarget = TargetSelector.GetTarget(Config.SpellSetting.Q.MaxrangeQ, DamageType.Magical);
            var Starget = TargetSelector.SelectedTarget;

            if (Config.SpellSetting.Q.ComboQ && Q.IsReady() &&
                Qtarget.Distance(Player.ServerPosition) > Config.SpellSetting.Q.MinrangeQ && SpellShield(Qtarget))
            {
                if (Starget != null && Config.SpellSetting.Q.FocusQ && Starget.IsValidTarget(2000))
                {
                    var predic = Q.GetPrediction(Starget);

                    if (predic.HitChance > (HitChance)Config.SpellSetting.Q.HitchanceQ + 2)
                    {
                        Q.Cast(Starget);
                    }
                }
                else if (Qtarget.IsValidTarget(Config.SpellSetting.Q.MaxrangeQ) &&
                    Menu["grabMode" + Qtarget.ChampionName].Cast<Slider>().CurrentValue != 1)
                {
                    var predic = Q.GetPrediction(Qtarget);

                    if (predic.HitChance > (HitChance)Config.SpellSetting.Q.HitchanceQ + 2)
                    {
                        Q.Cast(Qtarget);
                    }
                }
            }

            var Etarget = TargetSelector.GetTarget(300, DamageType.Physical);

            if (Config.SpellSetting.E.ComboE && Config.SpellSetting.E.AutoE && E.IsReady() &&
                Etarget.IsValidTarget(300))
            {
                if (Etarget.HasBuff("rocketgrab2"))
                {
                    E.Cast();
                }
            }

            var Rtarget = TargetSelector.GetTarget(R.Range, DamageType.Magical);

            if (Config.SpellSetting.R.ComboR && R.IsReady() && Rtarget.IsValidTarget(R.Range))
            {
                if (Rtarget.HasBuff("powerfistslow"))
                    R.Cast();
                if (Rtarget.HasBuff("rocketgrab2") && E.IsOnCooldown && Config.SpellSetting.E.ComboE)
                    R.Cast();
                if (Player.CountEnemiesInRange(R.Range) >= Config.SpellSetting.R.MinEnemyR)
                    R.Cast();
            }
        }

        private static void Harass()
        {
            var Qtarget = TargetSelector.GetTarget(Config.SpellSetting.Q.MaxrangeQ, DamageType.Magical);
            var Starget = TargetSelector.SelectedTarget;

            if (Config.SpellSetting.Q.HarassQ && Q.IsReady() &&
                Qtarget.Distance(Player.ServerPosition) > Config.SpellSetting.Q.MinrangeQ && SpellShield(Qtarget))
            {
                if (Starget != null && Config.SpellSetting.Q.FocusQ && Starget.IsValidTarget(2000))
                {
                    var predic = Q.GetPrediction(Starget);

                    if (predic.HitChance > (HitChance)Config.SpellSetting.Q.HitchanceQ + 2)
                    {
                        Q.Cast(Starget);
                    }
                }
                else if (Qtarget.IsValidTarget(Config.SpellSetting.Q.MaxrangeQ) &&
                    Menu["grabMode" + Qtarget.ChampionName].Cast<Slider>().CurrentValue != 1)
                {
                    var predic = Q.GetPrediction(Qtarget);

                    if (predic.HitChance > (HitChance)Config.SpellSetting.Q.HitchanceQ + 2)
                    {
                        Q.Cast(Qtarget);
                    }
                }
            }

            var Etarget = TargetSelector.GetTarget(300, DamageType.Physical);

            if (Config.SpellSetting.E.HarassE && Config.SpellSetting.E.AutoE && E.IsReady() &&
                Etarget.IsValidTarget(300))
            {
                if (Etarget.HasBuff("rocketgrab2"))
                {
                    E.Cast();
                }
            }

            var Rtarget = TargetSelector.GetTarget(R.Range, DamageType.Magical);

            if (Config.SpellSetting.R.HarassR && R.IsReady() && Rtarget.IsValidTarget(R.Range))
            {
                if (Rtarget.HasBuff("powerfistslow"))
                    R.Cast();
                if (Rtarget.HasBuff("rocketgrab2") && E.IsOnCooldown && Config.SpellSetting.E.ComboE)
                    R.Cast();
                if (Player.CountEnemiesInRange(R.Range) >= Config.SpellSetting.R.MinEnemyR)
                    R.Cast();
            }
        }

        private static void KillSteal()
        {
            if (Config.SpellSetting.Q.KillstealQ && Q.IsReady())
            {
                var Qtarget = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

                if (Qtarget.IsValidTarget(Config.SpellSetting.Q.MaxrangeQ) &&
                    Player.GetSpellDamage(Qtarget, SpellSlot.Q) > Qtarget.Health + Qtarget.AllShield && !SpellShield(Qtarget) &&
                    Menu["grabMode" + Qtarget.ChampionName].Cast<Slider>().CurrentValue == 2)
                {
                    Chat.Print("KillSteal Q");
                    SpellManager.Q.Cast(Qtarget);
                }
            }

            if (Config.SpellSetting.R.KillstealR && R.IsReady())
            {
                var Rtarget = TargetSelector.GetTarget(R.Range, DamageType.Magical);

                if (Rtarget.IsValidTarget(SpellManager.R.Range) &&
                    Player.GetSpellDamage(Rtarget, SpellSlot.R) > Rtarget.Health + Rtarget.AllShield &&
                    !SpellShield(Rtarget))
                {
                    Chat.Print("KillSteal R");
                    SpellManager.R.Cast();
                }
            }
        }

        private static void Immobile()
        {
            var Qtarget = TargetSelector.GetTarget(Config.SpellSetting.Q.MaxrangeQ, DamageType.Magical);

            if (Qtarget == null || SpellShield(Qtarget) || Config.SpellSetting.Q.MinHealthQ > Player.HealthPercent)
                return;

            if (Config.SpellSetting.Q.ImmobileQ && Q.IsReady() && Qtarget.IsValidTarget(Config.SpellSetting.Q.MaxrangeQ) &&
                Qtarget.Distance(Player.ServerPosition) > Config.SpellSetting.Q.MinrangeQ && CC(Qtarget) &&
                Menu["grabMode" + Qtarget.ChampionName].Cast<Slider>().CurrentValue == 2)
            {
                Chat.Print("Immobile");
                Q.Cast(Qtarget);
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;

            if (Config.Drawing.SmartDrawing)
            {
                if (Config.Drawing.DrawQ && Q.IsLearned)
                {
                    if (Config.SpellSetting.Q.MinHealthQ > Player.HealthPercent)
                        new Circle { Color = Color.Red, BorderWidth = 4, Radius = Config.SpellSetting.Q.MaxrangeQ }.Draw(Player.ServerPosition);
                    else
                    {
                        if (SpellManager.Q.IsReady())
                            new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = Config.SpellSetting.Q.MaxrangeQ }.Draw(Player.ServerPosition);
                        else
                            new Circle { Color = Color.Orange, BorderWidth = 4, Radius = Config.SpellSetting.Q.MaxrangeQ }.Draw(Player.ServerPosition);
                    }
                }

                if (Config.Drawing.DrawR && SpellManager.R.IsLearned)
                {
                    if (SpellManager.R.IsReady())
                        new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = SpellManager.R.Range }.Draw(Player.ServerPosition);
                    else
                        new Circle { Color = Color.Orange, BorderWidth = 4, Radius = SpellManager.R.Range }.Draw(Player.ServerPosition);
                }
            }
            else
            {
                if (Config.Drawing.DrawQ)
                {
                    new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = Config.SpellSetting.Q.MaxrangeQ }.Draw(Player.ServerPosition);
                }
                if (Config.Drawing.DrawR)
                {
                    new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = SpellManager.R.Range }.Draw(Player.ServerPosition);
                }
            }
            
            if (DashCircle != null)
                DashCircle.Draw(Color.Yellow);
        }

        private static void Interrupt_OnInterruptableTarget(AIHeroClient sender, Interrupt.InterruptableTargetEventArgs args)
        {
            if (Player.IsDead || Player.IsRecalling())
                return;

            if (!sender.IsEnemy || SpellShield(sender))
                return;
            
            if (Config.SpellSetting.R.InterruptR && R.IsReady() &&
                sender.IsValidTarget(R.Range))
            {
                Chat.Print("Interrupt R");
                SpellManager.R.Cast();
            }
            else if (Config.SpellSetting.Q.InterruptQ && Q.IsReady() &&
                sender.IsValidTarget(Config.SpellSetting.Q.MaxrangeQ) &&
                Config.SpellSetting.Q.MinHealthQ < Player.HealthPercent)
            {
                    Chat.Print("Interrupt Q");
                    Q.Cast(sender);
            }
            else if (Config.SpellSetting.E.InterruptE && E.IsReady() &&
                sender.IsValidTarget(200))
            {
                Chat.Print("Interrupt E");
                E.Cast();
                Orbwalker.ForcedTarget = sender;
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Player.IsDead || Player.IsRecalling())
                return;

            if (Config.SpellSetting.R.GapcloseR && R.IsReady() &&
                sender.IsValidTarget(R.Range) && !SpellShield(sender))
            {
                Chat.Print("Gapcloser");
                R.Cast();
            }
        }

        private static void Orbwalker_OnAttack(AttackableUnit target, EventArgs args)
        {
            var t = target as AIHeroClient;
            
            if (SpellShield(t))
                return;

            if (Config.SpellSetting.E.AutoE && E.IsReady() && t.IsValidTarget(200))
            {
                Chat.Print("Auto AA");
                E.Cast();
            }
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            var t = target as AIHeroClient;

            if (SpellShield(t))
                return;

            if (Config.SpellSetting.E.AAResetE && E.IsReady() && t.IsValidTarget(500))
            {
                Chat.Print("AA Reset");
                E.Cast();
            }
        }

        private static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            if (Player.IsDead || Player.IsRecalling())
                return;

            DashCircle = new Geometry.Polygon.Circle(/*center*/ e.EndPos, /*radius*/ 70);
            Core.DelayAction(() => DashCircle = null, e.Duration); //I don't know if Duration is in miliseconds, you need to print it when doing a test

            if (!sender.IsEnemy || sender.HasBuff("powerfistslow") || SpellShield(sender) || Config.SpellSetting.Q.MinHealthQ > Player.HealthPercent)
                return;
            
            if (e.EndPos.Distance(Player.ServerPosition) > Config.SpellSetting.Q.MinrangeQ &&
                     e.EndPos.Distance(Player.ServerPosition) < Config.SpellSetting.Q.MaxrangeQ)
            {
                Chat.Print("Dash");
                Q.Cast(e.EndPos);
            }
        }
    }
}
