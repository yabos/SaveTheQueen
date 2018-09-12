﻿using UnityEngine;
using UnityEditor;

namespace Lib.UniBt.Editor.Inspector
{
    [CustomEditor(typeof(Wait))]
    public sealed class WaitInspector : TaskInspector
    {
        private Wait wait;

        public override void OnEnable()
        {
            base.OnEnable();
            wait = task as Wait;
        }

        public override void OnInspectorGUI()
        {
            string name = wait.Name;
            string description = wait.description;
            BehaviorTreesEditorUtility.BeginInspectorGUI(ref name, ref description);
            if (name != wait.Name)
            {
                wait.Name = name;
                wait.description = description;
                //AssetCreator.SaveAIAsset();
            }
            GUILayout.Space(7f);
            if (BehaviorTreesEditorUtility.DrawHeader("Wait Time", false))
            {
                DrawTick();
            }
            BehaviorTreesEditorUtility.EndInspectorGUI(node);
        }

        private void DrawTick()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(7f);
            float tick = EditorGUILayout.FloatField("Tick", wait.tick);
            if (tick != wait.tick)
            {
                if (tick <= 0)
                    tick = 0.1f;
                wait.tick = tick;
                UpdateComment();
                AssetCreator.SaveAIAsset();
            }
            GUILayout.EndHorizontal();
        }

        private void UpdateComment()
        {
            string comment = "";
            comment += "Wait: ";
            comment += wait.tick + "s";
            wait.comment = comment;
        }
    }
}
