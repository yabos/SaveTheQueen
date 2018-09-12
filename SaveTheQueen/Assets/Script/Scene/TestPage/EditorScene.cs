using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Aniz.Widget.Panel;

public class EditorScene : SceneBase
{
    protected InGameWidget m_inGameWidget = null;

    public override IEnumerator OnEnter(float progress)
    {
        yield return base.OnEnter(progress);

        yield return Global.WidgetMgr.OnCreateWidgetAsync<InGameWidget>("InGameWidget", widget =>
        {
            m_inGameWidget = widget;
            if (m_inGameWidget != null)
            {
                m_inGameWidget.Show();
                SetEnterPageProgressInfo(0.5f);
            }
        });

        Global.NodeGraphMgr.CreateWorld();

        yield return Global.NodeGraphMgr.OnLoadLevel(loadLevelProgress =>
        {
            SetEnterPageProgressInfo(loadLevelProgress);
        });

        SetEnterPageProgressInfo(1.0f);
    }

    public override void OnExit()
    {
        base.OnExit();

        Global.NodeGraphMgr.ReleaseWorld();

        Global.WidgetMgr.HideAllWidgets(0.3f);
    }

    public override void OnInitialize()
    {
        Global.NodeGraphMgr.OnStartLevel();

        if (m_inGameWidget != null)
        {
            if (m_inGameWidget.TouchStickWidget != null)
            {
                m_inGameWidget.TouchStickWidget.OnUpdateEvent = Global.InputMgr.OnUpdateTouchStickEvent;
            }
        }
    }

    public override void OnFinalize()
    {
        if (m_inGameWidget != null)
        {
            if (m_inGameWidget.TouchStickWidget != null)
            {
                m_inGameWidget.TouchStickWidget.OnUpdateEvent = null;
            }
        }
    }

    public override void OnRequestEvent(string netClentTypeName, string requestPackets)
    {
        if (IsInGamePage() == true)
        {
            return;
        }

        Global.WidgetMgr.ShowLoadingWidget(0.3f);
    }

    public override void OnReceivedEvent(string netClentTypeName, string receivePackets)
    {
        if (IsInGamePage() == true)
        {
            return;
        }

        Global.WidgetMgr.HideLoadingWidget(0.1f);
    }

    public override bool IsInGamePage()
    {
        return true;
    }
}
