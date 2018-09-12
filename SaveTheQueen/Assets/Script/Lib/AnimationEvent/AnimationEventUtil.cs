using System.Collections.Generic;
using Lib.AnimationEvent;
using Lib.AnimationEvent.Attribute;
using Lib.Parse;
using UnityEngine;

namespace Lib.AnimationEvent
{
    public static class AnimationEventUtil
    {
        public const string SendAniEvent = "SendAniEvent";

        public const string Enter = "Enter";
        public const string Exit = "Exit";



        public delegate IAnimationEventAttribute OnCreate(string text);

        public class AnimationEventTypeInfo
        {
            public eAnimationEventTypeMask AnimationEventTypeMask = eAnimationEventTypeMask.None;
            public eAnimationEventType AnimationEventType = eAnimationEventType.Nothing;
            public string functionName = string.Empty;
            public System.Type type;
            public OnCreate onCreate;
        }

        public static List<AnimationEventTypeInfo> animationEventInfoList = null;

        public static void Init()
        {
            animationEventInfoList = new List<AnimationEventTypeInfo>();

            AddAnimationEventType(eAnimationEventTypeMask.Combat, eAnimationEventType.SendAniEvent, SendAniEvent, typeof(SendAniEventAttribute), SendAniEventAttribute.OnCreate);
            //AddAnimationEventType(eAnimationEventTypeMask.SFX, eAnimationEventType.PlaySFX, PlaySFX, typeof(PlaySFXAttribute), PlaySFXAttribute.OnCreate);
            //AddAnimationEventType(eAnimationEventTypeMask.VFX, eAnimationEventType.PlayVFX, PlayVFX, typeof(PlayVFXAttribute), PlayVFXAttribute.OnCreate);
            //AddAnimationEventType(eAnimationEventTypeMask.VFX, eAnimationEventType.StopVFX, StopVFX, typeof(StopVFXAttribute), StopVFXAttribute.OnCreate);
            //AddAnimationEventType(eAnimationEventTypeMask.VFX, eAnimationEventType.PlayPostEffect, PlayPostEffect, typeof(PlayPostEffectAttribute), PlayPostEffectAttribute.OnCreate);
            //AddAnimationEventType(eAnimationEventTypeMask.VFX, eAnimationEventType.PlayShaderFX, PlayShaderFX, typeof(PlayShaderFXAttribute), PlayShaderFXAttribute.OnCreate);
        }

        public static void AddAnimationEventType(eAnimationEventTypeMask animationEventTypeMask,
            eAnimationEventType animationEventType, string functionName, System.Type type, OnCreate onCreate)
        {
            AnimationEventTypeInfo animationEventTypeInfo = new AnimationEventTypeInfo();
            animationEventTypeInfo.AnimationEventTypeMask = animationEventTypeMask;
            animationEventTypeInfo.AnimationEventType = animationEventType;
            animationEventTypeInfo.functionName = functionName;
            animationEventTypeInfo.type = type;
            animationEventTypeInfo.onCreate = onCreate;
            animationEventInfoList.Add(animationEventTypeInfo);
        }

        public static eAnimationEventTypeMask GetAnimationEventTypeMask(string functionName)
        {
            if (animationEventInfoList == null)
            {
                Debug.LogError("AnimationEventUtil Init Error");
                return eAnimationEventTypeMask.None;
            }

            for (int i = 0; i < animationEventInfoList.Count; i++)
            {
                if (animationEventInfoList[i].functionName.Equals(functionName))
                {
                    return animationEventInfoList[i].AnimationEventTypeMask;
                }
            }
            return eAnimationEventTypeMask.None;
        }

        public static eAnimationEventType ParseAnimationEventType(string functionName)
        {
            if (animationEventInfoList == null)
            {
                //Init();
                Debug.LogError("AnimationEventUtil Init Error");
                return eAnimationEventType.Nothing;
            }

            foreach (AnimationEventTypeInfo animationEventTypeInfo in animationEventInfoList)
            {
                if (animationEventTypeInfo.functionName == functionName)
                {
                    return animationEventTypeInfo.AnimationEventType;
                }
            }
            return eAnimationEventType.Nothing;
        }

        public static void SetAnimationEventType(AnimationEventInfo animationEventInfo, eAnimationEventType animationEventType)
        {
            if (animationEventInfoList == null)
            {
                //Init();
                Debug.LogError("AnimationEventUtil Init Error");
                return;
            }

            foreach (AnimationEventTypeInfo animationEventTypeInfo in animationEventInfoList)
            {
                if (animationEventTypeInfo.AnimationEventType == animationEventType)
                {
                    animationEventInfo.FunctionName = animationEventTypeInfo.functionName;
                    return;
                }
            }
            animationEventInfo.FunctionName = string.Empty;
        }

        public static void Deserialize<T>(string param, ref T attribute) where T : IAnimationEventAttribute
        {
            KeyValueSerializer serializer = new KeyValueSerializer();
            serializer.Deserialize(param);

            attribute.OnSerialize(serializer);
        }

        public static void Serialize(out string param, IAnimationEventAttribute attribute)
        {
            if (attribute != null)
            {
                KeyValueSerializer serializer = new KeyValueSerializer();

                attribute.OnSerialize(serializer);

                param = serializer.SerializedText;
            }
            else
            {
                param = null;
            }
        }

        public static IAnimationEventAttribute CreateAttribute(string functionName, string text)
        {
            if (animationEventInfoList == null)
            {
                //Init();
                Debug.LogError("AnimationEventUtil Init Error");
                return null;
            }

            IAnimationEventAttribute attribute = null;
            for (int i = 0; i < animationEventInfoList.Count; ++i)
            {
                AnimationEventTypeInfo animationEventTypeInfo = animationEventInfoList[i];
                if (animationEventTypeInfo.functionName == functionName)
                {
                    if (animationEventTypeInfo.onCreate != null)
                    {
                        //Debug.Log(functionName);
                        attribute = animationEventTypeInfo.onCreate(text);
                    }
                    break;
                }
            }
            return attribute;
        }

    }
}