using System.Collections;
using System.Collections.Generic;
using Aniz;
using Aniz.Event;
using Aniz.Widget.Panel;
using Lib.Event;
using UnityEngine;

public class InGameScene : SceneBase
{
    private InGameWidget InGameWidget = null;

    public override IEnumerator OnEnter(float progress)
    {
        yield return base.OnEnter(progress);

        yield return Global.WidgetMgr.OnCreateWidgetAsync<InGameWidget>("InGameWidget", widget =>
        {
            InGameWidget = widget;
            if (InGameWidget != null)
            {
                InGameWidget.Show();
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

        if (InGameWidget != null)
        {
            if (InGameWidget.TouchStickWidget != null)
            {
                InGameWidget.TouchStickWidget.OnUpdateEvent = Global.InputMgr.OnUpdateTouchStickEvent;
            }
        }
    }

    public override void OnFinalize()
    {
        if (InGameWidget != null)
        {
            if (InGameWidget.TouchStickWidget != null)
            {
                InGameWidget.TouchStickWidget.OnUpdateEvent = null;
            }
        }
    }

    public override bool IsInGamePage()
    {
        return true;
    }


    public override void OnNotify(INotify notify)
    {
        eMessage msgType = (eMessage)notify.MsgCode;
        switch (msgType)
        {
            case eMessage.PageTransition:
                {
                    string title = StringUtil.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(Color.blue), "System Message");
                    Global.WidgetMgr.ShowMessageBox(title, "Exit Page", eMessageBoxType.OKAndCancel, result =>
                    {
                        if (result == true)
                        {
                            Global.SceneMgr.Transition(new SceneTransition(typeof(LoginScene).ToString(), "Login", 0.5f, 0.3f, (code) =>
                            {
                                Global.SceneMgr.LogWarning(StringUtil.Format("Page Transition -> {0}", "LoginPage"));
                            }));
                        }
                        else
                        {
                            title = StringUtil.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(Color.red), "System Message");
                            string messageBoxMessage = StringUtil.Format("<color=#{0}>{1}</color> {2}", ColorUtility.ToHtmlStringRGBA(Color.red), "Error", "Exit Page");
                            Global.WidgetMgr.ShowMessageBox(title, messageBoxMessage, eMessageBoxType.OK, null, 0.4f);
                        }
                    });
                }
                break;
        }

        base.OnNotify(notify);
    }
}
