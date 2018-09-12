using System;
using System.Collections.Generic;


[System.Serializable]
public class FSMStateElement
{
    //public FsmState State;
    public string StateName;
    public float AnimationLength;
    public string AnimationName;
    public bool Loop;
    public List<string> LstTransition;
    public List<string> LstGlobalTransition;
    public bool skillChain;
}

[System.Serializable]
public class FSMStateSet
{
    public string name;
    public float weight = 1.0f;
    public List<FSMStateElement> stateElementList = new List<FSMStateElement>();

    private FSMStateElement GetElement(string stateName)
    {
        for (int i = 0; i < stateElementList.Count; ++i)
        {
            FSMStateElement element = stateElementList[i];
            if (element.StateName == stateName)
            {
                return element;
            }
        }
        return null;
    }

    //public void AddElement(FsmState state)
    //{
    //    FSMStateElement element = GetElement(state.Name);
    //    if (element != null)
    //        return;

    //    element = new FSMStateElement();
    //    //element.State = state;
    //    element.StateName = state.Name;
    //    element.LstGlobalTransition = new List<string>();
    //    element.LstTransition = new List<string>();
    //    element.AnimationLength = state.AnimLength;
    //    element.AnimationName = state.m_destAnimation.Value;
    //    element.Loop = state.looping;
    //    element.skillChain = false;
    //    for (int i = 0; i < state.Actions.Length; i++)
    //    {
    //        //if (state.Actions[i] is PawnSkillChainAction)
    //        //{
    //        //	element.skillChain = true;
    //        //	break;
    //        //}
    //    }

    //    for (int i = 0; i < state.Transitions.Length; i++)
    //    {
    //        FsmTransition transition = state.Transitions[i];
    //        element.LstTransition.Add(transition.EventName);
    //    }

    //    stateElementList.Add(element);
    //}

    public void Sort()
    {
        stateElementList.Sort(delegate (FSMStateElement fsmStateElement1, FSMStateElement fsmStateElement2)
        {
            return fsmStateElement1.StateName.CompareTo(fsmStateElement2.StateName);
        });

        for (int i = 0; i < stateElementList.Count; i++)
        {
            FSMStateElement stateElement = stateElementList[i];
            stateElement.LstTransition.Sort();
            stateElement.LstGlobalTransition.Sort();
        }
    }

    internal void Clear()
    {
        this.name = string.Empty;
        stateElementList.Clear();
    }
}