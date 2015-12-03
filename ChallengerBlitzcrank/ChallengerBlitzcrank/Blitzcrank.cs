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
using SharpDX;

namespace ChallengerBlitzcrank
{
    public class Blitzcrank
    {
        static AIHeroClient Player = EloBuddy.Player.Instance;
        static AIHeroClient Target = null;
        static DamageIndicator Indicator;
        static Menu Menu;

        static Spell.Skillshot Q { get { return SpellManager.Q; } }
        static Spell.Active W { get { return SpellManager.W; } }
        static Spell.Active E { get { return SpellManager.E; } }
        static Spell.Active R { get { return SpellManager.R; } }

        static bool SpellShield(AIHeroClient unit)
        {
            return unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield);
        }
        static bool SpellShield(Obj_AI_Base unit)
        {
            return unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield);
        }
        static bool CC(AIHeroClient unit)
        {
            return unit.HasBuffOfType(BuffType.Charm) || unit.HasBuffOfType(BuffType.Fear)
                || unit.HasBuffOfType(BuffType.Snare) 
                || unit.HasBuffOfType(BuffType.Taunt) || unit.HasBuffOfType(BuffType.Stun)
                || unit.HasBuffOfType(BuffType.Knockup) || unit.HasBuffOfType(BuffType.Suppression);
        }

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
                Menu.Add("grabMode" + enemy.ChampionName, new Slider(enemy.ChampionName, 
                    TargetSelector.GetPriority(enemy) > 2 ? 3 : 2,
                    1, 3));
            }

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Interrupt.OnInterruptableTarget += Interrupt_OnInterruptableTarget;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Orbwalker.OnAttack += Orbwalker_OnAttack;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Dash.OnDash += Dash_OnDash;

            Indicator = new DamageIndicator();

            Chat.Print("Dat Blitzcrank Loaded, [By Tychus] , Version: 1.0.0.0", Color.Aquamarine);
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

            Target = TargetSelector.GetTarget(Config.SpellSetting.Q.MaxrangeQ, DamageType.Magical);
        }

        private static void Combo()
        {
            var Qtarget = TargetSelector.GetTarget(Config.SpellSetting.Q.MaxrangeQ, DamageType.Magical);
            var Starget = TargetSelector.SelectedTarget;

            if (Config.SpellSetting.Q.ComboQ && Q.IsReady() &&
                Qtarget.Distance(Player.ServerPosition) > Config.SpellSetting.Q.MinrangeQ && !SpellShield(Qtarget))
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
                Qtarget.Distance(Player.ServerPosition) > Config.SpellSetting.Q.MinrangeQ && !SpellShield(Qtarget))
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
                var Qtarget = EntityManager.Heroes.Enemies.FirstOrDefault
                    (enemy => DamageHandler.QDamage(enemy) >= enemy.Health + 50 && enemy.IsValidTarget(Config.SpellSetting.Q.MaxrangeQ));

                if (Qtarget != default(AIHeroClient) && !SpellShield(Qtarget))
                {
                    Q.Cast(Qtarget);
                }
            }

            if (Config.SpellSetting.R.KillstealR && R.IsReady())
            {
                var Rtarget = EntityManager.Heroes.Enemies.FirstOrDefault
                    (enemy => DamageHandler.RDamage(enemy) >= enemy.Health + 75 && enemy.IsValidTarget(R.Range));

                if (Rtarget != default(AIHeroClient) && !SpellShield(Rtarget))
                {
                    R.Cast();
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
                Menu["grabMode" + Qtarget.ChampionName].Cast<Slider>().CurrentValue == 3)
            {
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
                    new Circle
                    {
                        Color = Config.SpellSetting.Q.MinHealthQ > Player.HealthPercent ? Color.Red : Q.IsReady() ? Color.LawnGreen : Color.Orange,
                        BorderWidth = 4,
                        Radius = Config.SpellSetting.Q.MaxrangeQ
                    }.Draw(Player.Position);

                if (Config.Drawing.DrawR && SpellManager.R.IsLearned)
                    new Circle
                    {
                        Color = R.IsReady() ? Color.LawnGreen : Color.Orange,
                        BorderWidth = 4,
                        Radius = R.Range
                    }.Draw(Player.Position);
            }
            else
            {
                if (Config.Drawing.DrawQ)
                    new Circle
                    {
                        Color = Color.LawnGreen,
                        BorderWidth = 4,
                        Radius = Config.SpellSetting.Q.MaxrangeQ
                    }.Draw(Player.Position);
                if (Config.Drawing.DrawR)
                    new Circle
                    {
                        Color = Color.LawnGreen,
                        BorderWidth = 4,
                        Radius = R.Range
                    }.Draw(Player.Position);
            }

            if (TargetSelector.SelectedTarget == null)
                new Circle
                {
                    Color = Color.LightYellow,
                    BorderWidth = 6,
                    Radius = 50
                }.Draw(Target.Position);
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
                SpellManager.R.Cast();
            }
            else if (Config.SpellSetting.Q.InterruptQ && Q.IsReady() &&
                sender.IsValidTarget(Config.SpellSetting.Q.MaxrangeQ) &&
                Config.SpellSetting.Q.MinHealthQ < Player.HealthPercent)
            {
                    Q.Cast(sender);
            }
            else if (Config.SpellSetting.E.InterruptE && E.IsReady() &&
                sender.IsValidTarget(300))
            {
                E.Cast();
                EloBuddy.Player.IssueOrder(GameObjectOrder.AttackTo, sender);
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Player.IsDead || Player.IsRecalling())
                return;

            if (Config.SpellSetting.R.GapcloseR && R.IsReady() &&
                sender.IsValidTarget(R.Range) && !SpellShield(sender))
            {
                R.Cast();
            }
        }

        private static void Orbwalker_OnAttack(AttackableUnit target, EventArgs args)
        {
            var t = target as AIHeroClient;
            
            if (SpellShield(t))
                return;

            if (Config.SpellSetting.E.AutoE && E.IsReady() && t.IsValidTarget())
            {
                E.Cast();
            }
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            var t = target as AIHeroClient;

            if (SpellShield(t))
                return;

            if (Config.SpellSetting.E.AAResetE && E.IsReady() && t.IsValidTarget(350))
            {
                E.Cast();
                Orbwalker.ResetAutoAttack();
            }
        }

        private static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            if (Player.IsDead || Player.IsRecalling())
                return;
            
            if (!sender.IsEnemy || sender.HasBuff("powerfistslow") || SpellShield(sender) || Config.SpellSetting.Q.MinHealthQ > Player.HealthPercent
                || !Config.SpellSetting.Q.DashQ)
                return;
            
            if (e.EndPos.Distance(Player.ServerPosition) > Config.SpellSetting.Q.MinrangeQ &&
                     e.EndPos.Distance(Player.ServerPosition) < Config.SpellSetting.Q.MaxrangeQ)
            {
                Q.Cast(sender);
            }
        }
    }
}