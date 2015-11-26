using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using SharpDX;
using Color = System.Drawing.Color;

namespace KurisuBlitz
{
    //  _____ _ _ _                       _   
    // | __  | |_| |_ ___ ___ ___ ___ ___| |_ 
    // | __ -| | |  _|- _|  _|  _| .'|   | '_|
    // |_____|_|_|_| |___|___|_| |__,|_|_|_,_|
    //  Copyright © Kurisu Solutions 2015

    internal class Program
    {
        private static Menu Menu;
        private static Menu DrawingMenu;
        private static Menu MiscMenu;
        private static Menu SpellMenu;
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Active R { get; private set; }
        static Geometry.Polygon.Circle DashCircle;

        private static readonly AIHeroClient Me = ObjectManager.Player;

        static void Main(string[] args)
        {
            Console.WriteLine("Blitzcrank injected...");
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Me.ChampionName != "Blitzcrank")
                return;

            // Set spells      
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 250, 1800, 70);
            W = new Spell.Active(SpellSlot.W, 0);
            E = new Spell.Active(SpellSlot.E, 150);
            R = new Spell.Active(SpellSlot.R, 550);

            Menu = MainMenu.AddMenu("Kurisu's Blitz", "blitz");
            Menu.AddGroupLabel("1");
            Menu.AddLabel("2");

            SpellMenu = Menu.AddSubMenu("Spells", "spells");

            SpellMenu.AddGroupLabel("Q Settings");
            SpellMenu.Add("usecomboq", new CheckBox("Combo"));
            SpellMenu.Add("interruptq", new CheckBox("Interrupt"));
            SpellMenu.Add("secureq", new CheckBox("Killsteal", true));
            SpellMenu.AddSeparator();
            SpellMenu.Add("qdashing", new CheckBox("Use Q on Dashing Enemy"));
            SpellMenu.Add("qimmobil", new CheckBox("Use Q on Immobile Enemy"));
            SpellMenu.Add("hitchanceq", new Slider("Hitchance", 3, 1, 3));

            SpellMenu.AddGroupLabel("E Settings");
            SpellMenu.Add("usecomboe", new CheckBox("Combo"));
            SpellMenu.Add("interrupte", new CheckBox("Interrupt"));
            SpellMenu.Add("securee", new CheckBox("Killsteal", false));

            SpellMenu.AddGroupLabel("R Settings");
            SpellMenu.Add("usecombor", new CheckBox("Combo"));
            SpellMenu.Add("interruptr", new CheckBox("Interrupt"));
            SpellMenu.Add("securer", new CheckBox("Killsteal", true));

            DrawingMenu = Menu.AddSubMenu("Drawings", "dmenu");
            DrawingMenu.AddGroupLabel("Spells");
            DrawingMenu.Add("drawQ", new CheckBox("Draw Q"));
            DrawingMenu.Add("drawR", new CheckBox("Draw R"));

            MiscMenu = Menu.AddSubMenu("Misc", "bmisc");
            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.Add("mindist", new Slider("Mininum Distance to Q", 450, 0, 950));
            MiscMenu.Add("maxdist", new Slider("Maximum Distance to Q", 950, 0, 950));
            MiscMenu.Add("hnd", new Slider("Dont grab if below health %", 0, 0, 100));
            MiscMenu.AddGroupLabel("GrabMode");
            MiscMenu.AddLabel("0 = Don't Grab, 1 = Normal Grab, 2 = Auto Grab");
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                MiscMenu.Add("dograb" + enemy.ChampionName, new Slider(enemy.ChampionName, 1, 0, 2));
            }
            //MiscMenu.AddLabel("KeyBind");
            //MiscMenu.Add("grabkey", new KeyBind("Grab (Active)", false, KeyBind.BindTypes.HoldActive, 'T'));
            //MiscMenu.Add("combokey", new KeyBind("Combo (Active)", false, KeyBind.BindTypes.HoldActive, 32));

            // events
            Drawing.OnDraw += BlitzOnDraw;
            Game.OnTick += Game_OnTick;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Dash.OnDash += Dash_OnDash;

            Chat.Print("<font color=\"#FF9900\"><b>KurisuBlitz:</b></font> Loaded");

        }

        private static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            DashCircle = new Geometry.Polygon.Circle(/*center*/ e.EndPos, /*radius*/ 70);
            Core.DelayAction(() => DashCircle = null, e.Duration); //I don't know if Duration is in miliseconds, you need to print it when doing a test
        }
        
        private static bool Immobile(AIHeroClient unit)
        {
            return unit.HasBuffOfType(BuffType.Charm) || unit.HasBuffOfType(BuffType.Knockup) ||
                   unit.HasBuffOfType(BuffType.Snare) || unit.HasBuffOfType(BuffType.Taunt) || 
                   unit.HasBuffOfType(BuffType.Stun) ||
                   unit.HasBuffOfType(BuffType.Suppression);
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy || !sender.IsValidTarget(Q.Range))
                return;

            if (SpellMenu["interruptq"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                if (sender.Distance(Me.ServerPosition, true) <= Q.RangeSquared)
                {
                    Chat.Print("Interrupt Q to sender", Color.Red);
                    Q.Cast(sender);
                }
            }

            if (SpellMenu["interruptr"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                if (sender.Distance(Me.ServerPosition, true) <= R.RangeSquared)
                {
                    Chat.Print("Interrupt R", Color.Blue);
                    R.Cast();
                }
            }

            if (SpellMenu["interrupte"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                if (sender.Distance(Me.ServerPosition, true) <= E.RangeSquared)
                {
                    Chat.Print("Interrupt E", Color.Green);
                    E.Cast();
                }
            }
        }

        private static void BlitzOnDraw(EventArgs args)
        {
            if (DashCircle != null) DashCircle.Draw(Color.Yellow);
            if (!Me.IsDead)
            {
                var rcircle = DrawingMenu["drawR"].Cast<CheckBox>().CurrentValue;
                var qcircle = DrawingMenu["drawQ"].Cast<CheckBox>().CurrentValue;

                if (qcircle)
                    new Circle { Color = Color.LawnGreen, BorderWidth = 4, Radius = Q.Range }.Draw(Me.Position);

                if (rcircle)
                    new Circle { Color = Color.Orange, BorderWidth = 4, Radius = R.Range }.Draw(Me.Position);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Me.IsDead || MenuGUI.IsChatOpen || Me.IsRecalling())
                return;
            foreach (var target in EntityManager.Heroes.Enemies.Where(
                x => x.IsValidTarget(25000)))
            {
                if (target.IsDashing())
                {
                    
                    Chat.Print(target.ChampionName, Color.LawnGreen);
                    Chat.Print("Dashing!!!", Color.LightCyan);
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo(SpellMenu["usecomboq"].Cast<CheckBox>().CurrentValue, SpellMenu["usecomboe"].Cast<CheckBox>().CurrentValue,
                          SpellMenu["usecombor"].Cast<CheckBox>().CurrentValue);
            }
            Secure(SpellMenu["secureq"].Cast<CheckBox>().CurrentValue, SpellMenu["securee"].Cast<CheckBox>().CurrentValue,
                   SpellMenu["securer"].Cast<CheckBox>().CurrentValue);

            if (Me.HealthPercent >= MiscMenu["hnd"].Cast<Slider>().CurrentValue)
            {
                AutoCast(SpellMenu["qdashing"].Cast<CheckBox>().CurrentValue,
                         SpellMenu["qimmobil"].Cast<CheckBox>().CurrentValue);

                /*if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) 왜 이 위의 if문이랑 겹치면 이 콤보가 실행이 안되는지 모르겠다.
                {
                    Chat.Print("Combo NOW!", Color.Yellow);
                    Combo(SpellMenu["usecomboq"].Cast<CheckBox>().CurrentValue, SpellMenu["usecomboe"].Cast<CheckBox>().CurrentValue,
                          SpellMenu["usecombor"].Cast<CheckBox>().CurrentValue);
                }
                if (MiscMenu["grabkey"].Cast<KeyBind>().CurrentValue)
                {
                    Chat.Print("Spacebar press", Color.HotPink);
                    Combo(true, false, false);
                }*/
            }
        }


        private static void AutoCast(bool dashing, bool immobile)
        {
            if (!Q.IsReady())
                return;

            foreach (var ii in ObjectManager.Get<AIHeroClient>().Where(x => x.IsValidTarget(MiscMenu["maxdist"].Cast<Slider>().CurrentValue)))
            {
                if (dashing && MiscMenu["dograb" + ii.ChampionName].Cast<Slider>().CurrentValue == 2)
                {
                    if (ii.Distance(Me.ServerPosition) > MiscMenu["mindist"].Cast<Slider>().CurrentValue &&
                        ii.Distance(Me.ServerPosition) <= 450f)
                    {
                        if (HitChance.Dashing == Q.GetPrediction(ii).HitChance)
                        {
                            Chat.Print("Dashing Q on ii", Color.Red);
                            Q.Cast(ii);
                        }
                    }
                }

                if (immobile && MiscMenu["dograb" + ii.ChampionName].Cast<Slider>().CurrentValue == 2)
                {

                    if (ii.Distance(Me.ServerPosition) > MiscMenu["mindist"].Cast<Slider>().CurrentValue)
                    {
                        if (HitChance.Immobile == Q.GetPrediction(ii).HitChance)
                        {
                            Chat.Print("Immobile Q on ii by HitChance", Color.Red);
                            Q.Cast(ii);
                        }
                    }
                }

                if (Immobile(ii) && MiscMenu["dograb" + ii.ChampionName].Cast<Slider>().CurrentValue == 2)
                {
                    if (ii.Distance(Me.ServerPosition) > MiscMenu["mindist"].Cast<Slider>().CurrentValue)
                    {
                        Chat.Print("Immobile Q on ii", Color.Red);
                        Q.Cast(ii);
                    }
                }
            }
        }

        private static void Combo(bool useq, bool usee, bool user) //Complete
        {
            if (useq && Q.IsReady())
            {
                var qtarget = TargetSelector.GetTarget(1250, DamageType.Magical);
                if (qtarget.IsValidTarget(MiscMenu["maxdist"].Cast<Slider>().CurrentValue))
                {
                    if (qtarget.Distance(Me.ServerPosition) > MiscMenu["mindist"].Cast<Slider>().CurrentValue)
                    {
                        if (MiscMenu["dograb" + qtarget.ChampionName].Cast<Slider>().CurrentValue != 0)
                        {
                            var pouput = Q.GetPrediction(qtarget);
                            Chat.Print(pouput.HitChance);
                            if (pouput.HitChance >= (HitChance) SpellMenu["hitchanceq"].Cast<Slider>().CurrentValue+3)
                            {
                                Q.Cast(pouput.CastPosition);
                            }
                        }
                    }
                }
            }

            if (usee && E.IsReady())
            {
                var etarget = TargetSelector.GetTarget(350, DamageType.Physical);
                if (etarget.IsValidTarget())
                {
                    E.Cast();
                }

                var qtarget = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                if (qtarget.IsValidTarget(MiscMenu["maxdist"].Cast<Slider>().CurrentValue))
                {
                    if (qtarget.HasBuff("rocketgrab2"))
                    {
                        E.Cast();
                    }
                }
            }

            if (user && R.IsReady())
            {
                var rtarget = TargetSelector.GetTarget(R.Range, DamageType.Magical);
                if (rtarget.IsValidTarget())
                {
                    if (!E.IsReady() && rtarget.HasBuffOfType(BuffType.Knockup))
                    {
                        if (rtarget.Health > rtarget.GetSpellDamage(rtarget, SpellSlot.R))
                        {
                            R.Cast();
                        }
                    }
                }
            }
        }

        private static void Secure(bool useq, bool usee, bool user)
        {
            if (user && R.IsReady())
            {
                var rtarget = ObjectManager.Get<AIHeroClient>().FirstOrDefault(h => h.IsEnemy);
                if (rtarget.IsValidTarget(R.Range))
                {
                    if (Me.GetSpellDamage(rtarget, SpellSlot.R) >= rtarget.Health)
                    {
                        R.Cast();
                    }
                }
            }

            if (usee && E.IsReady())
            {
                var etarget = ObjectManager.Get<AIHeroClient>().FirstOrDefault(h => h.IsEnemy);
                if (etarget.IsValidTarget(E.Range))
                {
                    if (Me.GetSpellDamage(etarget, SpellSlot.E) >= etarget.Health)
                    {
                        E.Cast();
                    }
                }
            }

            if (useq && Q.IsReady())
            {
                var qtarget = ObjectManager.Get<AIHeroClient>().FirstOrDefault(h => h.IsEnemy);
                if (qtarget.IsValidTarget(MiscMenu["maxdist"].Cast<Slider>().CurrentValue))
                {
                    if (Me.GetSpellDamage(qtarget, SpellSlot.Q) >= qtarget.Health)
                    {
                        var pouput = Q.GetPrediction(qtarget);
                        if (pouput.HitChance >= (HitChance) SpellMenu["hitchanceq"].Cast<Slider>().CurrentValue + 2)
                        {
                            Q.Cast(pouput.CastPosition);
                        }
                    }
                }
            }
        }
    }
}