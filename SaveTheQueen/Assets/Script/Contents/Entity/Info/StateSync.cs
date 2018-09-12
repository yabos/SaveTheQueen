using Lib.Battle;
using UnityEngine;

namespace Aniz.NodeGraph.Level.Group.Info
{

    public struct StateSync
    {
        public int StartTick { get; set; }
        public Vector2 Direction { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 ViewDirection { get; set; }

        public string BaseState { get; set; }
        public string SubState { get; set; }

        public string FsmEvent { get; set; }

        public bool ActionReset { get; set; }

        public int Argument { get; set; }

        public DamageInfo Damage { get; set; }

    }
}