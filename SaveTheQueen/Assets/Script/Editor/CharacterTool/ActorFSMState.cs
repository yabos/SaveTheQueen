using System.Collections.Generic;

public class ActorFSMState
{
    public List<FSMStateSet> FsmStateSetList = new List<FSMStateSet>();

    public FSMStateSet GetFirst()
    {
        if (FsmStateSetList.Count > 0)
        {
            return FsmStateSetList[0];
        }
        return null;
    }

    public void AddStateSet(FSMStateSet set)
    {
        FsmStateSetList.Add(set);
    }

    public void RemoveStateSet(int index)
    {
        FsmStateSetList.RemoveAt(index);
    }
}