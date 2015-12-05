using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace AimBuddy
{
	public static class Config
	{
		public static Menu Menu { get; set; }
		public static Slider Hit;

		public static int HitValue
		{
			get { return Hit.CurrentValue; }
		}

		static Config()
		{
			Menu = MainMenu.AddMenu("AimBuddy", "AimBuddy");
			Menu.AddGroupLabel("AimBuddy");
			Menu.AddLabel("Version : 1.0.0.0");
			Menu.AddSeparator();

			Hit = new Slider("Hit Chance: " + Utility.HitchanceNameArray[0], 2, 0, Utility.HitchanceNameArray.Length - 1);
			Hit.OnValueChange += delegate { Hit.DisplayName = "Hit Chance: " + Utility.HitchanceNameArray[Hit.CurrentValue]; };
			Menu.Add("hit", Hit);
			Menu.AddLabel("Hit Chance List:");
			for (var i = 0; i < Utility.HitchanceNameArray.Length; i++)
			{
				Menu.AddLabel(string.Format("  - {0}: {1}", i, Utility.HitchanceNameArray[i]));
			}

			Menu.AddSeparator();
			Menu.AddGroupLabel("Credits");
			Menu.AddLabel("Tychus: Addon creator");

			ComboMenu.Initialize();
			DrawMenu.Initialize();
		}
		
		public static void Initialize()
		{
		}

		public static class ComboMenu
		{
			public static Menu Menu;

			static ComboMenu()
			{
				Menu = Config.Menu.AddSubMenu("Combo", "combo");

				Menu.AddGroupLabel("Combo Settings");
				Menu.Add("qCombo", new CheckBox("Use Q"));
				Menu.Add("wCombo", new CheckBox("Use W"));
				Menu.Add("eCombo", new CheckBox("Use E"));
				Menu.Add("rCombo", new CheckBox("Use R"));
			}

			public static void Initialize()
			{
			}
		}

		public static class DrawMenu
		{
			public static Menu Menu;

			public static CheckBox _qDraw;
			public static CheckBox _wDraw;
			public static CheckBox _eDraw;
			public static CheckBox _rDraw;

			public static bool qDraw { get { return _qDraw.CurrentValue; } }
			public static bool wDraw { get { return _wDraw.CurrentValue; } }
			public static bool eDraw { get { return _eDraw.CurrentValue; } }
			public static bool rDraw { get { return _rDraw.CurrentValue; } }

			static DrawMenu()
			{
				Menu = Config.Menu.AddSubMenu("Draw", "Draw");

				Menu.AddGroupLabel("Draw Settings");
				_qDraw = Menu.Add("qDraw", new CheckBox("Q Range"));
				_wDraw = Menu.Add("wDraw", new CheckBox("W Range"));
				_eDraw = Menu.Add("eDraw", new CheckBox("E Range"));
				_rDraw = Menu.Add("rDraw", new CheckBox("R Range"));
			}

			public static void Initialize()
			{
			}
		}
	}
}
