//#define ENABLE_WARNING_AS_ERROR
// http://forum.unity3d.com/threads/71445-How-To-Set-Project-Wide-pragma-Directives-with-JavaScript/page2

// Above says that there are two different files used depending on Build type (either smcs.rsp or gmcs.rsp).
// It looks that it is always mcs.rsp when building a standalone build - regardless of .net version or Flash target.

// To specify multiple defines separate them with a semicolon:
// "-define:USE_DYNAMIC_SIM_UPDATES;AI_DEBUG;COMBAT_DEBUG"


using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class CompilerSymbolManager
{
    private class CompilerSymbol
    {
        public string name;
        public bool active;
    }

    private static string CompilerSymbolPath { get { return Application.dataPath + "/mcs.rsp"; } }
    //private static string CompilerSymbolPath { get { return Application.dataPath + "/smcs.rsp"; } }

    private static List<CompilerSymbol> customSymbolList = null;

    public static int CustomSymbolCount { get { return customSymbolList.Count; } }

    // can't change in editor
    private static readonly string[] internalSymbolList = new string[]
    {
        "NOT_USED",
    };

    // it's able to activate and deactivate during build player.
    private static readonly string[] buildSymbolList = new string[]
    {
        "DEBUG_LOG",
        "ENABLE_LOCAL_LOG",
    };

    // can change in editor
    private static readonly string[] symbols = new string[]
    {
        "USE_DEBUG_KEYS",
        "DEBUG_EVENT_LOG",
        "ENABLE_LOG_LOCATION",
        "USE_ASSETBUNDLESYSTEM",
        "AI_DEBUG",

        "USE_SDKPLUGIN",
        "USE_JOYSTICK_AS_MAIN_CONTROLLER",

        "ENABLE_MEMORY_PROFILING",	// enable memory dump in game manager
		"ENABLE_DIAGNOSTIC",		// show fps, using memory, device info
		//"ENABLE_DIRECTION",		// enable 8 directions controls

		"USE_RENDERTEXTURE_SCALE",	// 메인 랜더 텍스쳐 스케일링
		"USE_ELASPSED_PROFILE",		// 지연 시간 프로파일링
		"USE_APAWN_MOTION_BLUR",	// 폰 잔상 효과

		"DEBUG_FILE_LOG",			// 파일에 heavy logging
		"DEBUG_PAWN_COLLISION",		// 몬스터 간 충돌 처리 디버깅
		"DEV_LOCKSTEP",				// 작업 완료 후 삭제 예정
		//"DEV_INGAME_SKILL_HUD_V2",	// 작업 완료 후 삭제 예정 ==> 2017-03-22 에 코드에서 제거됨
        "NETWORK_TEST",              // Login Scene에서 서버 접속 테스트 - 17.06.23. jonghyuk
    };

    private static void Initialize()
    {
        if (customSymbolList == null)
        {
            customSymbolList = new List<CompilerSymbol>();

            foreach (string symbol in buildSymbolList)
            {
                AddCustomSymbol(symbol);
            }

            foreach (string symbol in symbols)
            {
                AddCustomSymbol(symbol);
            }
        }
    }

    private static bool GetInternalSymbol(string name)
    {
        for (int i = 0; i < internalSymbolList.Length; ++i)
        {
            if (internalSymbolList[i] == name)
                return true;
        }
        return false;
    }

    public static void ForceUpdateCompilerSymbol()
    {
#if UNITY_EDITOR
        CompilerSymbol symbol = customSymbolList.Find(a => a.name.Equals("USE_ASSETBUNDLE") == true);
        if (symbol == null)
        {
            customSymbolList.Add(new CompilerSymbol
            {
                name = "USE_ASSETBUNDLE",
                active = true
            });
        }

        RefreshScripts();
#endif
    }

    public static void AddCustomSymbol(string name)
    {
        for (int i = 0; i < customSymbolList.Count; ++i)
        {
            if (customSymbolList[i].name == name)
            {
                Debug.LogWarning("[CompilerSymbol] duplicated symbol " + name);
                return;
            }
        }

        CompilerSymbol symbol = new CompilerSymbol();
        symbol.name = name;
        symbol.active = false;
        customSymbolList.Add(symbol);
    }

    public static bool ActivateSymbol(string name, bool active)
    {
        if (GetInternalSymbol(name))
            return true;

        for (int i = 0; i < customSymbolList.Count; ++i)
        {
            if (customSymbolList[i].name == name)
            {
                bool oldValue = customSymbolList[i].active;
                customSymbolList[i].active = active;
                return oldValue;
            }
        }

        for (int i = 0; i < customSymbolList.Count; ++i)
        {
            if (customSymbolList[i].name == name)
            {
                bool oldValue = customSymbolList[i].active;
                customSymbolList[i].active = active;
                return oldValue;
            }
        }

        Debug.LogWarning("[CompilerSymbol] deprecated symbol " + name);
        return false;
    }

    public static string GetCustomSymbolName(int index)
    {
        if (index >= 0 && index < customSymbolList.Count)
            return customSymbolList[index].name;
        return string.Empty;
    }

    public static bool IsCustomSymbolActive(string name)
    {
        for (int i = 0; i < customSymbolList.Count; ++i)
        {
            if (customSymbolList[i].name == name)
            {
                return customSymbolList[i].active;
            }
        }
        return false;
    }

    public static bool IsBuildSymbol(string name)
    {
        for (int i = 0; i < buildSymbolList.Length; ++i)
        {
            if (buildSymbolList[i] == name)
                return true;
        }
        return false;
    }

    public static void Load()
    {
        Initialize();

        string line = string.Empty;
        using (TextReader reader = new StreamReader(CompilerSymbolPath))
        {
            line = reader.ReadLine();
        }

        ParseConfiguration(line);
    }

    private static void ParseConfiguration(string text)
    {
        string[] tokens = text.Split(' ');
        for (int i = 0; i < tokens.Length; ++i)
        {
            string token = tokens[i];
            token = token.Trim(new char[] { '"' });
            if (token.StartsWith("-define"))
            {
                ParseSymbol(token);
            }
            /*
            else if (token.StartsWith("-warnaserror+"))
            {
                // do nothing
            }
            */
        }
    }

    private static void ParseSymbol(string token)
    {
        token = token.Replace("-define:", string.Empty);

        string[] symbols = token.Split(';');
        for (int i = 0; i < symbols.Length; ++i)
        {
            ActivateSymbol(symbols[i], true);
        }
    }

    public static void Save()
    {
        List<string> symbolList = new List<string>();

        foreach (string symbolName in internalSymbolList)
        {
            symbolList.Add(symbolName);
        }

        foreach (CompilerSymbol symbol in customSymbolList)
        {
            if (!symbol.active)
                continue;
            symbolList.Add(symbol.name);
        }
        string symbols = string.Join(";", symbolList.ToArray());
        string line = "";
        /*
#if ENABLE_WARNING_AS_ERROR
        line += "-warnaserror+";
        line += " ";
#endif // ENABLE_WARNING_AS_ERROR
*/
        line += string.Format("\"-define:{0}\"", symbols);

        using (TextWriter writer = new StreamWriter(new FileStream(CompilerSymbolPath, FileMode.Create)))
        {
            writer.WriteLine(line);
        }

        RefreshScripts();
    }

    [MenuItem("Joker/Tools/Refresh Scripts! %r", false, MenuItemUtil.ToolGroup + 10)]
    private static void RefreshScripts()
    {
        AssetDatabase.ImportAsset("Assets/Temp/RefreshScripts.cs", ImportAssetOptions.ForceUpdate);
    }

    public static bool CheckSymbolsForBuild()
    {
        Load();

        bool success = true;
        for (int i = 0; i < CustomSymbolCount; ++i)
        {
            string name = GetCustomSymbolName(i);
            bool active = IsCustomSymbolActive(name);
            if (active)
            {
                if (!IsBuildSymbol(name))
                {
                    Debug.LogError(string.Format("error: '{0}' is activated, you should turn it off", name));
                    success = false;
                }
            }
        }

        return success;
    }
}

public class CompilerSymbolEditor : EditorWindow
{
    [MenuItem("Joker/Tools/CompilerSymbol...", false, MenuItemUtil.ToolGroup + 1)]
    public static void OpenEditor()
    {
        CompilerSymbolEditor compilerSymbolEditor = EditorWindow.GetWindow<CompilerSymbolEditor>(true, "Symbol");
        compilerSymbolEditor.maxSize = new Vector2(400, 400);
        compilerSymbolEditor.ShowPopup();
    }

    private void OnEnable()
    {
        CompilerSymbolManager.Load();
    }

    private void OnDisable()
    {
    }

    private Vector2 scroll;

    private void OnGUI()
    {
        scroll = GUILayout.BeginScrollView(scroll);
        {
            GUILayout.Label("Build symbols", EditorStyles.boldLabel);
            GUILayout.BeginVertical("box");
            {
                for (int i = 0; i < CompilerSymbolManager.CustomSymbolCount; ++i)
                {
                    string name = CompilerSymbolManager.GetCustomSymbolName(i);
                    if (CompilerSymbolManager.IsBuildSymbol(name))
                    {
                        bool active = CompilerSymbolManager.IsCustomSymbolActive(name);
                        active = GUILayout.Toggle(active, name, "button");
                        CompilerSymbolManager.ActivateSymbol(name, active);
                    }
                }
            }
            GUILayout.EndVertical();

            GUILayout.Label("Custom symbols", EditorStyles.boldLabel);
            GUILayout.BeginVertical("box");
            {
                for (int i = 0; i < CompilerSymbolManager.CustomSymbolCount; ++i)
                {
                    string name = CompilerSymbolManager.GetCustomSymbolName(i);
                    if (!CompilerSymbolManager.IsBuildSymbol(name))
                    {
                        bool active = CompilerSymbolManager.IsCustomSymbolActive(name);
                        active = GUILayout.Toggle(active, name, "button");
                        CompilerSymbolManager.ActivateSymbol(name, active);
                    }
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndScrollView();

        if (GUILayout.Button("Confirm", GUILayout.Height(40)))
        {
            CompilerSymbolManager.Save();
        }
    }
}