using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationManagerSetting : ManagerSettingBase
{
	[SerializeField, HideInInspector]
	public string debugHandlerLogInfo = string.Empty;

	[SerializeField, HideInInspector]
	public string debugHandlerCallStackTraceInfo = string.Empty;

	[SerializeField, HideInInspector]
	protected List<string> eventHandlerDebugInfos = new List<string>();

	public void AddDebugHandlerInfo(string info)
	{
		eventHandlerDebugInfos.Add(info);
	}

	public List<string> GetDebugHandlerInfos()
	{
		return eventHandlerDebugInfos;
	}

	public void ClearDebugHandlerInfo()
	{
		eventHandlerDebugInfos.Clear();
	}

	public void AllClearDebugInfo()
	{
		debugHandlerLogInfo = string.Empty;
		debugHandlerCallStackTraceInfo = string.Empty;

		ClearDebugHandlerInfo();
	}
}
