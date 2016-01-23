using EloBuddy;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerBuddy
{
    public enum Team
    {
        Ally, Enemy, None
    }

    public enum ObjType
    {
        Teleport, Item, Spell, Trap
    }

    public class Timer
    {
        public ObjType type;

        public Team team = Team.None;

        public Obj_AI_Base caster;

        public Vector3 castPosition;

        public string name;

        public float endTime;

        public bool cancel;

        public SpellSlot slot;
    }

    public class TimerInfo
    {
        public ObjType type;

        public string name;

        public float endTime;

        public SpellSlot slot;

        public string championName;
    }

    public class GameObj
    {
        public ObjType type;

        public Team team = Team.None;

        public Vector3 position;

        public float endTime;

        public string name;

        public string deleteName;

        public int networkID;
    }

    public class GameObjInfo
    {
        public ObjType type;

        public string name;

        public string deleteName;

        public float endTime;
    }
    
    public class BuffInfo
    {
        public string buffname;
    }
}
