using System.Collections;
using System.Collections.Generic;
using Aniz.Event;
using Lib.Event;
using UnityEngine;

public class LobbyScene : SceneBase
{
    public override IEnumerator OnEnter(float progress)
    {
        yield return base.OnEnter(progress);

        SetEnterPageProgressInfo(1.0f);
    }

    public override void OnExit()
    {
        base.OnExit();

        Global.WidgetMgr.HideAllWidgets(0.3f);
    }

    public override void OnInitialize()
    {
    }

    public override void OnFinalize()
    {

    }

    public override void OnRequestEvent(string netClentTypeName, string requestPackets)
    {
        Global.WidgetMgr.ShowLoadingWidget(0.3f);
    }

    public override void OnReceivedEvent(string netClentTypeName, string receivePackets)
    {
        Global.WidgetMgr.HideLoadingWidget(0.1f);
    }

    public override void OnNotify(INotify notify)
    {
        base.OnNotify(notify);
    }
}
