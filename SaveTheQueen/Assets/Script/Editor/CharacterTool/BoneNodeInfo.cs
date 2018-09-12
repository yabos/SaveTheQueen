using System;
using System.Collections.Generic;
using UnityEngine;

public class BoneNodeInfo
{
#if UNITY_EDITOR
    private Transform m_boneTransform;
    public Transform BoneTransform
    {
        get { return m_boneTransform; }
        set { m_boneTransform = value; }
    }
#endif

    private string m_name;
	private string m_subname;
	private uint m_boneIndex;
	private List<BoneNodeInfo> m_childNode = new List<BoneNodeInfo>();

    public uint boneIndex
	{
		get { return m_boneIndex; }
		set { m_boneIndex = value; }
	}

	public string name
	{
		get { return m_name; }
		set { m_name = value; }
	}

	public List<BoneNodeInfo> childNode
	{
		get { return m_childNode; }
	}

	public string Subname
	{
		get { return m_subname; }
		set { m_subname = value; }
	}

	public bool isBone(string boneName)
	{
		if (boneName == m_name)
		{
			return true;
		}
		return false;
	}

	public bool findBone(string boneName)
	{
		if (isBone(boneName))
			return true;

		foreach (BoneNodeInfo info in m_childNode)
		{
			return info.findBone(boneName);
		}
		return false;
	}

	public void SetListBone(List<BoneNodeInfo> listNode)
	{
		listNode.Add(this);
		foreach (BoneNodeInfo info in m_childNode)
		{
			info.SetListBone(listNode);
		}
	}

	public void GetListNodeName(List<string> lstName)
	{
		foreach (BoneNodeInfo info in m_childNode)
		{
			lstName.Add(info.name);
			info.GetListNodeName(lstName);
		}
	}

	public void Release()
	{
#if UNITY_EDITOR
		m_boneTransform = null;
#endif

		for (int i = 0; i < m_childNode.Count; i++)
		{
			m_childNode[i].Release();
		}
		m_childNode.Clear();
	}

	public override string ToString()
	{
		return name;
	}
}
