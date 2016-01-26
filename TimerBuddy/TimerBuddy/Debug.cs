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
            Obj_AI_Base.OnBuffGain += Obj_AI_Base_OnBuffGain;
            Obj_AI_Base.OnBuffUpdate += Obj_AI_Base_OnBuffUpdate;
            Obj_AI_Base.OnBuffLose += Obj_AI_Base_OnBuffLose;
        }

        private static void Obj_AI_Base_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (!sender.IsMe)
                return;

            Chat.Print(args.Buff.DisplayName, System.Drawing.Color.LawnGreen);
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
