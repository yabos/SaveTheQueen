using System;
using Lib.Parse;
using UnityEngine;

namespace Lib.AnimationEvent
{
    public static class AnimationEventDeserializer
    {
        enum E_SerializeState
        {
            None,
            Version,
            State,
        }

        private static char[] charSeparators = new char[] { ' ' };

        private static AnimationEventState m_currentAnimationEventState;

        public static bool Deserialize(AnimationEventScriptable eventScriptable,
            ref AnimationEventStates animationEventStates)
        {
            if (eventScriptable == null)
                return false;

            //orcaAnimationStates = new OrcaAnimationStates();
            animationEventStates.GraphName = eventScriptable.name.Replace("_animationevent", string.Empty);
            animationEventStates.BoneList = eventScriptable.boneNames;

            TextAsset graphAsset = eventScriptable.graphAsset;
            if (graphAsset != null)
            {
                DeserializeText(animationEventStates, graphAsset.text);
            }
            animationEventStates.StateSort();

            TextAsset sfxAsset = eventScriptable.sfxAsset;
            if (sfxAsset != null)
            {
                DeserializeText(animationEventStates, sfxAsset.text);
            }

            TextAsset vfxAsset = eventScriptable.vfxAsset;
            if (vfxAsset != null)
            {
                DeserializeText(animationEventStates, vfxAsset.text);
            }

            if (animationEventStates != null)
            {
                for (int i = 0; i < animationEventStates.StateList.Count; i++)
                {
                    AnimationEventState animationEventState = animationEventStates.StateList[i];
                    animationEventState.ConvertAnimationEvents();
                    animationEventState.SortAnimationEvent();
                }
            }
            return true;
        }

        private static void DeserializeText(AnimationEventStates animationEventStates, string text)
        {

            string tempText = text.Replace("\r", "");
            string[] lines = tempText.Split('\n');

            E_SerializeState serializeState = E_SerializeState.None;

            foreach (string line in lines)
            {
                string token = line.Trim();
                if (token.StartsWith("//"))
                    continue;

                if (token == "#Version")
                {
                    serializeState = E_SerializeState.Version;
                }
                if (token == "#State")
                {
                    serializeState = E_SerializeState.State;
                }
                else
                {
                    switch (serializeState)
                    {
                        case E_SerializeState.Version:
                            DeserializeVersion(token);
                            break;
                        case E_SerializeState.State:
                            DeserializeState(token, animationEventStates);
                            break;
                    }

                }

            }

        }

        private static void DeserializeVersion(string line)
        {
            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                //int start = line.IndexOf('[');
                //int last = line.LastIndexOf(']');
                //string versionName = line.Substring(start + 1, last - start - 1);
                //if (versionName == "converted60Frames")
                //{
                //	converted60Frames = true;
                //}

                //if (versionName == "convertedTrails")
                //{
                //	convertedTrails = true;
                //}
            }
        }

        private static void DeserializeState(string line, AnimationEventStates animationEventStates)
        {
            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                int start = line.IndexOf('[');
                int last = line.LastIndexOf(']');
                string stateName = line.Substring(start + 1, last - start - 1);

                m_currentAnimationEventState = animationEventStates.GetState(stateName);
                if (m_currentAnimationEventState == null)
                {
                    m_currentAnimationEventState = new AnimationEventState(stateName);
                    animationEventStates.AddState(m_currentAnimationEventState);
                }
            }
            else if (line.StartsWith("!"))
            {
                string animationEvent = line.Substring(1);
                DeserializeAnimationEvent(animationEvent, m_currentAnimationEventState);
            }
            else
            {
                string[] tokens = line.Split(charSeparators, System.StringSplitOptions.None);
                string tokenoption = tokens[0].Trim();

                if (m_currentAnimationEventState == null || tokens.Length != 2)
                {
                    return;
                }

                if (tokenoption == "clipTime")
                {
                    float AnimationTime;
                    StringConverter.TryConvert(tokens[1], out AnimationTime);
                    m_currentAnimationEventState.AnimationTime = AnimationTime;
                }
                else if (tokenoption == "isLooping")
                {
                    bool isLooping;
                    StringConverter.TryConvert(tokens[1], out isLooping);
                    m_currentAnimationEventState.isLooping = isLooping;
                }
                else if (tokenoption == "visible")
                {
                    bool Visible;
                    StringConverter.TryConvert<bool>(tokens[1], out Visible);
                    m_currentAnimationEventState.Visible = Visible;
                }
                else if (tokenoption == "clipName")
                {
                    m_currentAnimationEventState.clipName = tokens[1];
                }
            }
        }

        private static void DeserializeAnimationEvent(string line, AnimationEventState eventState)
        {
            string[] tokens = line.Split(charSeparators, System.StringSplitOptions.None);

            if (tokens.Length >= 4)
            {
                string functionName = tokens[0];
                int frame = int.Parse(tokens[1]);

                float time = 0.0f;
                if (tokens.Length >= 3)
                {
                    float.TryParse(tokens[2], out time);
                }

                string param = "";
                if (tokens.Length >= 4)
                {
                    param = tokens[3];
                }

                bool eventOnExit = false;
                if (tokens.Length >= 5)
                {
                    bool.TryParse(tokens[4], out eventOnExit);
                }


                AnimationEventInfo animationEventInfo = new AnimationEventInfo();
                animationEventInfo.FunctionName = functionName;

                animationEventInfo.Frame = frame;
                //animationEvent.Param = param;
                animationEventInfo.attribute = AnimationEventUtil.CreateAttribute(functionName, param);
                AnimationEventUtil.Serialize(out param, animationEventInfo.attribute);
                animationEventInfo.param = param;
                animationEventInfo.Time = time;
                animationEventInfo.EventOnExit = eventOnExit;
                animationEventInfo.NomalizeTime = time / eventState.AnimationTime;

                eventState.AddAnimationEvent(animationEventInfo);
            }
        }

    }
}