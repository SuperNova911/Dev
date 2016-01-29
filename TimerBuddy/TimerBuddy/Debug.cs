using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using Color = SharpDX.Color;

namespace TimerBuddy
{
    public class Debug
    {
        static Debug()
        {
            //Obj_AI_Base.OnBuffGain += Obj_AI_Base_OnBuffGain;
            //Obj_AI_Base.OnBuffUpdate += Obj_AI_Base_OnBuffUpdate;
            //Obj_AI_Base.OnBuffLose += Obj_AI_Base_OnBuffLose;
            GameObject.OnCreate += GameObject_OnCreate;
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.IsValid)
                return;

            if (sender.Name.Contains("Minion") || sender.GetType().Name == "MissileClient" || sender.Name.Contains("SRU") || sender.Name.Contains("FeelNoPain") || sender.Name.Contains("crystal_beam"))
                return;
            
            Console.WriteLine("Add\tType: {0} | Name: {1} | NetID: {2} | objectName: {3}", sender.GetType().Name, sender.Name, sender.NetworkId, sender.BaseObjectName());
        }

        private static void Obj_AI_Base_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (!sender.IsMe)
                return;

            Chat.Print(args.Buff.DisplayName + " " + args.Buff.Name, System.Drawing.Color.LawnGreen);
        }

        private static void Obj_AI_Base_OnBuffUpdate(Obj_AI_Base sender, Obj_AI_BaseBuffUpdateEventArgs args)
        {
            if (!sender.IsMe)
                return;

            Chat.Print(args.Buff.DisplayName, System.Drawing.Color.Orange);
        }

        private static void Obj_AI_Base_OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (!sender.IsMe)
                return;

            Chat.Print(args.Buff.DisplayName, System.Drawing.Color.Red);
        }

        public static void Initialize()
        {

        }
    }
}
