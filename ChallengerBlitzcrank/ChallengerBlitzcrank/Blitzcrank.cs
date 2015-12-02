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

        static Spell.Skillshot Q { get { return SpellManager.Q; } }
        static Spell.Active W { get { return SpellManager.W; } }
        static Spell.Active E { get { return SpellManager.E; } }
        static Spell.Active R { get { return SpellManager.R; } }
        static readonly int[] QDamages = new int[] { 80, 135, 190, 245, 300 };
        static readonly int[] RDamages = new int[] { 250, 375, 500 };

        static Geometry.Polygon.Circle DashCircle; //don't use new, just what I wrote

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
                || unit.HasBuffOfType(BuffType.Snare) || unit.HasBuffOfType(BuffType.Slow)
                || unit.HasBuffOfType(BuffType.Taunt) || unit.HasBuffOfType(BuffType.Stun)
                || unit.HasBuffOfType(BuffType.Knockup) || unit.HasBuffOfType(BuffType.Suppression);
        }

        static Menu Menu;

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
            Drawing.OnEndScene += Drawing_OnEndScene;
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

            Target = TargetSelector.GetTarget(2000, DamageType.Magical);
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
                    (enemy => SpellDamage(SpellSlot.Q) >= enemy.Health + 50 && enemy.IsValidTarget(Config.SpellSetting.Q.MaxrangeQ));

                if (Qtarget != default(AIHeroClient) && !SpellShield(Qtarget))
                {
                    Chat.Print("KillSteal Q");
                    Q.Cast(Qtarget);
                }
            }

            if (Config.SpellSetting.R.KillstealR && R.IsReady())
            {
                var Rtarget = EntityManager.Heroes.Enemies.FirstOrDefault
                    (enemy => SpellDamage(SpellSlot.R) >= enemy.Health + 75 && enemy.IsValidTarget(R.Range));

                if (Rtarget != default(AIHeroClient) && !SpellShield(Rtarget))
                {
                    Chat.Print("KillSteal R");
                    R.Cast();
                }
            }
        }

        private static float SpellDamage(SpellSlot slot)
        {
            float damage = new float();

            switch (slot)
            {
                case SpellSlot.Q:
                    damage = Damage.CalculateDamageOnUnit(Player, Target, DamageType.Magical, QDamages[Q.Level - 1] + 1 * Player.TotalMagicalDamage);
                    break;
                case SpellSlot.E:
                    damage = Damage.CalculateDamageOnUnit(Player, Target, DamageType.Physical, Player.GetAutoAttackDamage(Target) * 2, true, true);
                    break;
                case SpellSlot.R:
                    damage = Damage.CalculateDamageOnUnit(Player, Target, DamageType.Magical, RDamages[R.Level - 1] + 1 * Player.TotalMagicalDamage);
                    break;
            }

            return damage;
        }

        private static float ComboDamage()
        {
            if (Target != null)
            {
                float comboDamage = new float();

                comboDamage = Q.IsReady() ? SpellDamage(SpellSlot.Q) : 0;
                //comboDamage += E.IsReady() ? SpellDamage(SpellSlot.E) : 0;
                comboDamage += R.IsReady() ? SpellDamage(SpellSlot.R) : 0;

                return comboDamage;
            }
            return 0;
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
            
            if (DashCircle != null)
                DashCircle.Draw(Color.Yellow);
            new Circle
            {
                Color = Color.LightYellow,
                BorderWidth = 6,
                Radius = 50
            }.Draw(Target.Position);
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Player.IsDead || !Config.Drawing.DrawDamage)
                return;

            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                if (enemy.IsValidTarget(2000))
                {
                    float TotalDamage = ComboDamage() > enemy.Health ? enemy.Health / enemy.MaxHealth : ComboDamage() / enemy.MaxHealth;
                    Line.DrawLine
                        (
                            Color.DarkRed, 9f,
                            new Vector2(enemy.HPBarPosition.X + 1, enemy.HPBarPosition.Y + 9),
                            new Vector2(enemy.HPBarPosition.X + 1 + TotalDamage * 104, enemy.HPBarPosition.Y + 9)
                        );
                }
            }
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

            if (Config.SpellSetting.E.AutoE && E.IsReady() && t.IsValidTarget())
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

            if (Config.SpellSetting.E.AAResetE && E.IsReady() && t.IsValidTarget(350))
            {
                Chat.Print("AA Reset");
                E.Cast();
                Orbwalker.ResetAutoAttack();
            }
        }

        private static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            if (Player.IsDead || Player.IsRecalling())
                return;

            DashCircle = new Geometry.Polygon.Circle(/*center*/ e.EndPos, /*radius*/ 70);
            Core.DelayAction(() => DashCircle = null, e.Duration); //I don't know if Duration is in miliseconds, you need to print it when doing a test

            if (!sender.IsEnemy || sender.HasBuff("powerfistslow") || SpellShield(sender) || Config.SpellSetting.Q.MinHealthQ > Player.HealthPercent
                || !Config.SpellSetting.Q.DashQ)
                return;
            
            if (e.EndPos.Distance(Player.ServerPosition) > Config.SpellSetting.Q.MinrangeQ &&
                     e.EndPos.Distance(Player.ServerPosition) < Config.SpellSetting.Q.MaxrangeQ)
            {
                Chat.Print("Dash");
                Q.Cast(sender);
            }
        }
    }
}