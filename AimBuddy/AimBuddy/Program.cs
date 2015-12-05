using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using AimBuddy.SDK;
using Color = System.Drawing.Color;
using SPrediction;

namespace AimBuddy
{
    class Program
    {
		public static List<SDK.Spell> SpellList = new List<SDK.Spell>();
		private static AIHeroClient Player = ObjectManager.Player;

		public static SDK.Spell Q;
		public static SDK.Spell W;
		public static SDK.Spell E;
		public static SDK.Spell R;

		public static EloBuddy.SDK.Spell.Skillshot QQ { get; private set; }

		public static void Main(string[] args)
        {
			CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

		public static void Game_OnGameLoad(EventArgs args)
		{
			QQ = new EloBuddy.SDK.Spell.Skillshot(SpellSlot.Q, 950, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 250, 1800, 85);
			foreach (var spell in SpellDatabase.Spells)
			{
				if (spell.ChampionName == Player.BaseSkinName)
				{
					if (spell.Slot == SpellSlot.Q)
					{
						Q = new SDK.Spell(spell.Slot, spell.Range);
						Q.SetSkillshot(spell.Delay / 1000, spell.Radius, spell.MissileSpeed, spell.CanBeRemoved, spell.Type);
						SpellList.Add(Q);
					}
					if (spell.Slot == SpellSlot.W)
					{
						W = new SDK.Spell(spell.Slot, spell.Range);
						W.SetSkillshot(spell.Delay / 1000, spell.Radius, spell.MissileSpeed, spell.CanBeRemoved, spell.Type);
						SpellList.Add(W);
					}
					if (spell.Slot == SpellSlot.E)
					{
						E = new SDK.Spell(spell.Slot, spell.Range);
						E.SetSkillshot(spell.Delay / 1000, spell.Radius, spell.MissileSpeed, spell.CanBeRemoved, spell.Type);
						SpellList.Add(E);
					}
					if (spell.Slot == SpellSlot.R)
					{
						R = new SDK.Spell(spell.Slot, spell.Range);
						R.SetSkillshot(spell.Delay / 1000, spell.Radius, spell.MissileSpeed, spell.CanBeRemoved, spell.Type);
						SpellList.Add(R);
					}
				}
			}

			Config.Initialize();

			Game.OnTick += Game_OnTick;
			Drawing.OnDraw += Drawing_OnDraw;
			SPrediction.Prediction.Initialize(Config.Menu);
			Chat.Print("Loaded");
		}

		private static void Drawing_OnDraw(EventArgs args)
		{
			if (Q != null && Config.DrawMenu.qDraw && Q.IsReady())
				new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = Q.Range }.Draw(Player.Position);
			if (W != null && Config.DrawMenu.wDraw && W.IsReady())
				new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = W.Range }.Draw(Player.Position);
			if (E != null && Config.DrawMenu.eDraw && E.IsReady())
				new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = E.Range }.Draw(Player.Position);
			if (R != null && Config.DrawMenu.rDraw && R.IsReady())
				new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = R.Range }.Draw(Player.Position);
		}

		private static void Game_OnTick(EventArgs args)
		{
			if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
			{
				comboQ();
				/*
				comboW();
				comboE();
				comboR();
				*/
			}
		}

		private static void comboQ()
		{
			EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
			foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(x => x.IsEnemy && x.IsValidTarget(Q.Range)))
			{

				HitChance hCance = Utility.HitchanceArray[Config.HitValue];
				Q.SPredictionCast(enemy, hCance);
				Chat.Print(enemy.ChampionName);
				Vector3 castPos = Q.GetSPosition(enemy);
				QQ.Cast(castPos);
			}
		}
		/*
		private static void comboW()
		{
			EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
			foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(x => x.IsEnemy && x.IsValidTarget(Q.Range)))
			{

				HitChance hCance = Utility.HitchanceArray[Config.HitValue];
				W.SPredictionCast(enemy, hCance);
			}
		}

		private static void comboE()
		{
			EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
			foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(x => x.IsEnemy && x.IsValidTarget(Q.Range)))
			{

				HitChance hCance = Utility.HitchanceArray[Config.HitValue];
				E.SPredictionCast(enemy, hCance);
			}
		}

		private static void comboR()
		{
			EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
			foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(x => x.IsEnemy && x.IsValidTarget(Q.Range)))
			{

				HitChance hCance = Utility.HitchanceArray[Config.HitValue];
				R.SPredictionCast(enemy, hCance);
			}
		}
		*/
	}
}
