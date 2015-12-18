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
using System.Collections.Generic;

namespace DatDarius
{
    public class Darius
    {
        public static Spell.Active Q = new Spell.Active(SpellSlot.Q, 425);
        public static Spell.Active W = new Spell.Active(SpellSlot.W, 175);
        public static Spell.Skillshot E = new Spell.Skillshot(SpellSlot.E, 540, SkillShotType.Cone, 250, int.MaxValue, 225);
        public static Spell.Targeted R = new Spell.Targeted(SpellSlot.R, 460);
        public static Spell.Skillshot Flash;
        public static Spell.Targeted Ignite;
        public static float Angle = 50 * (float)Math.PI / 180;

        public static AIHeroClient Player { get { return ObjectManager.Player; } }

        public static Item Randuin;
        public static Item RavenousHydra;
        public static Item TitanicHydra;

        public static Menu Menu;

        /// <summary>
        /// Check enemy target has spell shield.
        /// </summary>
        /// <param name="unit">The target.</param>
        /// <returns>true = true, false = false.</returns>
        public static bool SpellShield(AIHeroClient unit)
        {
            return unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield);
        }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.BaseSkinName != "Darius")
                return;

            E.ConeAngleDegrees = 50;
            E.MinimumHitChance = HitChance.Medium;

            Randuin = new Item(3143, 500);
            RavenousHydra = new Item(3074, 400);
            TitanicHydra = new Item(3748, 385);

            SpellDataInst flash = Player.Spellbook.Spells.Where(s => s.Name.Contains("summonerflash")).Any()
                ? Player.Spellbook.Spells.Where(spell => spell.Name.Contains("summonerflash")).First() : null;
            SpellDataInst ignite = Player.Spellbook.Spells.Where(s => s.Name.Contains("summonerdot")).Any()
                ? Player.Spellbook.Spells.Where(spell => spell.Name.Contains("summonerdot")).First() : null;
            if (flash != null)
            {
                Flash = new Spell.Skillshot(flash.Slot, 425, SkillShotType.Linear);
            }
            if (ignite != null)
            {
                Ignite = new Spell.Targeted(ignite.Slot, 600);
            }

            Menu = MainMenu.AddMenu("Dat Darius", "Dat Darius");

            Menu.AddGroupLabel("Combo");
            {
                Menu.Add("useQcombo", new CheckBox("Use Q"));
                Menu.Add("useWcombo", new CheckBox("Use W"));
                Menu.Add("useEcombo", new CheckBox("Use E"));
                Menu.Add("useRcombo", new CheckBox("Use R"));
                Menu.Add("IgniteTime", new Slider("Ignite Tick", 3, 1, 5));
                Menu.AddSeparator();
            }

            Menu.AddGroupLabel("Harass");
            {
                Menu.AddSeparator();
            }

            Menu.AddGroupLabel("LastHit");
            {
                Menu.AddSeparator();
            }

            Menu.AddGroupLabel("LaneClear");
            {
                Menu.AddSeparator();
            }

            Menu.AddGroupLabel("Flee");
            {
                Menu.AddSeparator();
            }

            Menu.AddSubMenu("Misc");

            Menu.AddSubMenu("Draw");

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Dash.OnDash += Dash_OnDash;
            SDK.Interrupt.OnInterruptableTarget += Interrupt_OnInterruptableTarget;

            Chat.Print("Dat Darius Loaded", Color.OrangeRed);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;

            new Circle
            {
                Color = Q.IsReady() ? Color.LawnGreen : Color.Red,
                BorderWidth = 2,
                Radius = Q.Range
            }.Draw(Player.Position);

            new Geometry.Polygon.Sector(Player.Position, Game.CursorPos, Angle, E.Range).Draw(Color.Yellow);

            // Debug zone
            if (true)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.VisibleOnScreen))
                {
                    var i = 0;
                    const int step = 20;

                    var Damage = new Dictionary<string, object>
                            {
                                { "Passive Stack", enemy.HasBuff("dariushemo") ? enemy.GetBuffCount("dariushemo") : 0 },
                                { "Passive Remain Time", enemy.HasBuff("dariushemo") ? enemy.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                                                        .Where(buff => buff.Name == "dariushemo")
                                                        .Select(buff => buff.EndTime)
                                                        .FirstOrDefault() - Game.Time
                                                        : 0 },
                                { "Passive Damage", DamageHandler.PassiveDamage(enemy) },
                                { "R Min Damage", new double[] { 100, 200, 300 }[SpellManager.R.Level - 1] + (0.75 * ObjectManager.Player.FlatPhysicalDamageMod) },
                                { "R Max Damage", DamageHandler.RDamage(enemy) },
                                { "Passive + Ignite", DamageHandler.PassiveDamage(enemy) + DamageHandler.IgniteDamage(true) },
                                { "Health", enemy.Health },
                                { "Hp Regen rate", enemy.HPRegenRate }
                            };
                    Drawing.DrawText(enemy.Position.WorldToScreen() + new Vector2(0, i), Color.Orange, "Damage properties", 10);
                    i += step;
                    foreach (var dataEntry in Damage)
                    {
                        Drawing.DrawText(enemy.Position.WorldToScreen() + new Vector2(0, i), Color.NavajoWhite, string.Format("{0}: {1}", dataEntry.Key, dataEntry.Value), 10);
                        i += step;
                    }

                    var Qlogic = new Dictionary<string, object>
                            {
                                { "Chasing    ", Player.IsFacing(enemy) && !enemy.IsFacing(Player) ? "true" : "false" },
                                { "Run away   ", !Player.IsFacing(enemy) && enemy.IsFacing(Player) ? "true" : "false" },
                                { "Both Facing", Player.IsBothFacing(enemy) ? "true" : "false" },
                                { "Distance ", Player.ServerPosition.Distance(enemy.ServerPosition) },
                                { "Speed", Player.MoveSpeed > enemy.MoveSpeed ? "Slow" : "Fast" }
                            };
                    Drawing.DrawText(enemy.Position.WorldToScreen() + new Vector2(0, i), Color.Orange, "Q Logic", 10);
                    i += step;
                    foreach (var dataEntry in Qlogic)
                    {
                        Drawing.DrawText(enemy.Position.WorldToScreen() + new Vector2(0, i), Color.NavajoWhite, string.Format("{0}: {1}", dataEntry.Key, dataEntry.Value), 10);
                        i += step;
                    }
                }

                if (!Player.IsDead)
                {
                    var i = 0;
                    const int step = 20;

                    var Damage = new Dictionary<string, object>
                            {
                                { "Path Length", Player.Path.Length }
								//{ "Path Long Length", Player.Path.LongLength }
							};
                    Drawing.DrawText(Player.Position.WorldToScreen() + new Vector2(0, i), Color.Orange, "General properties", 10);
                    i += step;
                    foreach (var dataEntry in Damage)
                    {
                        Drawing.DrawText(Player.Position.WorldToScreen() + new Vector2(0, i), Color.NavajoWhite, string.Format("{0}: {1}", dataEntry.Key, dataEntry.Value), 10);
                        i += step;
                    }
                }
                for (var i = 1; Player.Path.Length > i; i++)
                {
                    Drawing.DrawLine(Drawing.WorldToScreen(Player.Path[i - 1]), Drawing.WorldToScreen(Player.Path[i]), 5, Color.Green);
                }
            }

        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.IsDead || MenuGUI.IsChatOpen)
                return;

            AutoUlt();
            KillSecure();

            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Mode.Combo();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    Mode.Harass();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                    Mode.LastHit();
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    Mode.LaneClear();
                    break;
                case Orbwalker.ActiveModes.Flee:
                    Mode.Flee();
                    break;
            }
        }

        public class Mode
        {
            public static void Combo()
            {
                var Qtarget = TargetSelector.GetTarget(1000, DamageType.Physical);

                if (Qtarget.IsValidTarget() && Q.IsReady())
                {
                    CastQ(Qtarget);
                }


            }

            public static void Harass()
            {

            }

            public static void LastHit()
            {

            }

            public static void LaneClear()
            {

            }

            public static void Flee()
            {

            }
        }

        public static void AutoUlt()
        {
            foreach (var target in EntityManager.Heroes.Enemies.Where(target => target.Distance(Player.ServerPosition) < R.Range + Flash.Range))
            {
                if (DamageHandler.RDamage(target) > target.Health + target.AllShield + target.HPRegenRate &&
                    R.IsReady() && target.IsValidTarget(R.Range) && !SpellShield(target) && !target.HasBuff("kindredrnodeathbuff"))
                {
                    R.Cast(target);
                    Chat.Print(DamageHandler.RDamage(target));
                }
            }
        }

        public static void KillSecure()
        {
            if (Player.IsDead)
                return;

            if (Ignite.IsReady())
            {
                var IgniteTarget = EntityManager.Heroes.Enemies.FirstOrDefault(t => DamageHandler.IgniteDamage(true) >= t.Health);

                if (!IgniteTarget.IsValidTarget(Ignite.Range + Flash.Range))
                    return;

                // Damage Prediction
                var time = Menu["IgniteTime"].Cast<Slider>().CurrentValue;
                var heal = HealHandler.Potion.GetHeal(IgniteTarget, time);

                if (heal == 0)
                {
                    if (DamageHandler.IgniteDamage(false) * time >= IgniteTarget.Health
                        - DamageHandler.PassiveDamage(IgniteTarget)
                        + (IgniteTarget.HPRegenRate * time) / 2
                        + HealHandler.Potion.HealTick(IgniteTarget) * (time - 1))
                        Ignite.Cast(IgniteTarget);
                }
                else if (heal > 0)
                {
                    if (DamageHandler.IgniteDamage(false) * time >= IgniteTarget.Health
                        - DamageHandler.PassiveDamage(IgniteTarget)
                        + (IgniteTarget.HPRegenRate * time) / 2
                        + heal)
                        Ignite.Cast(IgniteTarget);
                }
            }
        }

        /// <summary>
        /// Q logic.
        /// </summary>
        public static void CastQ(AIHeroClient Qtarget)
        {
            var MySpeed = Player.MoveSpeed;
            var TargetSpeed = Qtarget.MoveSpeed;
            var TargetBoundingRadius = Qtarget.BoundingRadius;
            var Distance = Qtarget.ServerPosition.Distance(Player.ServerPosition);

            // Minimun distance
            if (Distance > Q.Range - 220)
            {
                // Check target is moving
                if (Qtarget.IsMoving)
                {
                    // Chasing enemy
                    if (Player.IsFacing(Qtarget) && !Qtarget.IsFacing(Player))
                    {
                        if (Distance - (MySpeed * 0.75f) + (TargetSpeed * 0.75f) < Q.Range + TargetBoundingRadius)
                        {
                            Q.Cast();
                        }
                    }
                    // Running away from enemy
                    else if (!Player.IsFacing(Qtarget) && Qtarget.IsFacing(Player))
                    {
                        if (Distance + (MySpeed * 0.75f) - (TargetSpeed * 0.75f) < Q.Range + TargetBoundingRadius)
                        {
                            Q.Cast();
                        }
                    }
                    // Facing each other
                    else if (Player.IsBothFacing(Qtarget))
                    {
                        if (Distance > Q.Range + TargetBoundingRadius)
                        {
                            if (Distance + 100 - (MySpeed * 0.75f) - (TargetSpeed * 0.75f) < Q.Range + TargetBoundingRadius)
                            {
                                Q.Cast();
                            }
                        }
                        else if (Distance < Q.Range + TargetBoundingRadius)
                        {
                            Q.Cast();
                        }
                    }
                }
                else
                {
                    // Move to enemy
                    if (Player.IsFacing(Qtarget))
                    {
                        if (Distance - (MySpeed * 0.75f) / 2 < Q.Range + TargetBoundingRadius)
                        {
                            Q.Cast();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// E Logic.
        /// </summary>
        /// <param name="target"></param>
        public static void CastE(AIHeroClient target)
        {

        }

        public static void TowerE()
        {

        }

        /*
		public static Vector3 GetBestEPos()
		{
			var CS = new List<Geometry.Polygon.Sector>();
			var Vectors = new List<Vector3>()
			{
				new Vector3(Target.ServerPosition.X + 550, Target.ServerPosition.Y, Target.ServerPosition.Z),
				new Vector3(Target.ServerPosition.X - 550, Target.ServerPosition.Y, Target.ServerPosition.Z),
				new Vector3(Target.ServerPosition.X, Target.ServerPosition.Y + 550, Target.ServerPosition.Z),
				new Vector3(Target.ServerPosition.X, Target.ServerPosition.Y - 550, Target.ServerPosition.Z),
				new Vector3(Target.ServerPosition.X + 230, Target.ServerPosition.Y, Target.ServerPosition.Z),
				new Vector3(Target.ServerPosition.X - 230, Target.ServerPosition.Y, Target.ServerPosition.Z),
				new Vector3(Target.ServerPosition.X, Target.ServerPosition.Y + 230, Target.ServerPosition.Z),
				new Vector3(Target.ServerPosition.X, Target.ServerPosition.Y - 230, Target.ServerPosition.Z), Target.ServerPosition };

			var CS1 = new Geometry.Polygon.Sector(Player.Position, Vectors[0], Angle, 600);
			var CS2 = new Geometry.Polygon.Sector(Player.Position, Vectors[1], Angle, 600);
			var CS3 = new Geometry.Polygon.Sector(Player.Position, Vectors[2], Angle, 600);
			var CS4 = new Geometry.Polygon.Sector(Player.Position, Vectors[3], Angle, 600);
			var CS5 = new Geometry.Polygon.Sector(Player.Position, Vectors[4], Angle, 600);
			var CS6 = new Geometry.Polygon.Sector(Player.Position, Vectors[5], Angle, 600);
			var CS7 = new Geometry.Polygon.Sector(Player.Position, Vectors[6], Angle, 600);
			var CS8 = new Geometry.Polygon.Sector(Player.Position, Vectors[7], Angle, 600);
			var CS9 = new Geometry.Polygon.Sector(Player.Position, Vectors[8], Angle, 600);

			CS.Add(CS1);
			CS.Add(CS2);
			CS.Add(CS3);
			CS.Add(CS4);
			CS.Add(CS5);
			CS.Add(CS6);
			CS.Add(CS7);
			CS.Add(CS8);
			CS.Add(CS9);

			var CSHits = new List<byte>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

			for (byte j = 0; j < 9; j++)
			{
				foreach (AIHeroClient hero in EntityManager.Heroes.Enemies.Where(enemy => !enemy.IsDead && enemy.IsValidTarget(W.Range)))
				{
					if (CS.ElementAt(j).IsInside(hero)) CSHits[j]++;
					if (hero == Target) CSHits[j] += 10;
				}
			}

			byte i = (byte)CSHits.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;

			if (CSHits[i] == 0) return default(Vector3);

			return Vectors[i];
		}
		*/
        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            var t = target as AIHeroClient;

            if (t.IsValidTarget() && W.IsReady())
            {
                W.Cast();
                Orbwalker.ResetAutoAttack();
                if (target != null)
                    EloBuddy.Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
        }

        private static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            if (Player.IsDead || Player.IsRecalling() || MenuGUI.IsChatOpen)
                return;

            if (e.EndPos.Distance(Player.ServerPosition) < Q.Range + sender.BoundingRadius &&
                e.EndPos.Distance(Player.ServerPosition) > Q.Range + sender.BoundingRadius - 220)
            {
                if (e.Duration < 750)
                    Q.Cast();
                else
                    Core.DelayAction(() => Q.Cast(), e.Duration - 750);
            }

            if (e.StartPos.Distance(Player.ServerPosition) < E.Range && e.EndPos.Distance(Player.ServerPosition) > E.Range)
            {
                if (sender.IsMe)
                    return;
                E.Cast(sender);
                Chat.Print("Dash E");
            }

        }

        private static void Interrupt_OnInterruptableTarget(AIHeroClient sender, SDK.Interrupt.InterruptableTargetEventArgs args)
        {
            if (Player.IsDead || Player.IsRecalling() || MenuGUI.IsChatOpen)
                return;

            if (sender.IsValidTarget(E.Range + sender.BoundingRadius) && E.IsReady())
            {
                E.Cast(sender);
            }
        }
    }
}
