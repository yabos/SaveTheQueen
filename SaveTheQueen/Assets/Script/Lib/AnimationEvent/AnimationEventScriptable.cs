using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lib.AnimationEvent
{
    public class AnimationEventScriptable : ScriptableObject
    {
        public string[] boneNames;
        public TextAsset graphAsset;
        public TextAsset sfxAsset;
        public TextAsset vfxAsset;
    }
}