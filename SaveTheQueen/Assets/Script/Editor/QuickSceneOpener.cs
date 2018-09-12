using EditorGUICtrl;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class QuickSceneOpener : EditorWindow
{
    [MenuItem("Joker/Quick SceneOpener %q")]
    private static void OpenEditor()
    {
        EditorWindow.GetWindow<QuickSceneOpener>("SceneOpener");
    }

    private enum SceneType
    {
        GameStage,
        Art,
        Design,
        Sound,
        GameFlow,
        Viewer,
        Level,
        Max
    }

    private string[] SceneTypeStrings = new string[] { "GameStage", "Art", "Design", "Sound", "GameFlow", "Viewer", "Levels" };

    private string[] SceneRootPaths = new string[] {
        "Assets/AssetBundle/Scenes/",
        "Assets/Scene/Stage/",
        "Assets/Scene/Stage/",
        "Assets/Scene/Stage/",
        "Assets/Scene/GameFlow/",
        "Assets/Scene/Viewer/",
        "Assets/Scene/Levels/"
    };

    private string selectScenePath = string.Empty;

    // options
    private SceneType sceneType = SceneType.Design;

    private bool closeAfterOpenScene = true;

    private ListCtrl[] sceneList = null;

    public QuickSceneOpener()
    {
        sceneList = new ListCtrl[(int)SceneType.Max];
        for (int i = 0; i < sceneList.Length; ++i)
        {
            sceneList[i] = new ListCtrl();
            sceneList[i].SetStyle(ListCtrl.Style.SelectOneRow);
            sceneList[i].AddColumn("Scene List");
            sceneList[i].SetOnDblClickRow(OnDblClickScene);
            sceneList[i].SetOnClickRow(OnClickScene);
        }
    }

    private void OnEnable()
    {
        EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;

        RefreshSceneList();

        LoadSetting();
    }

    private void OnDisable()
    {
        EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
    }

    private void OnDestroy()
    {
        SaveSettings();
    }

    private void OnPlaymodeStateChanged()
    {
        Repaint();
    }

    private void OnGUI()
    {
        if (EditorApplication.isPlaying)
            GUI.enabled = false;

        OnGUIHeader();

        sceneList[(int)sceneType].OnGUI();

        {
            if (GUILayout.Button("Start With Default Stage Scene", GUILayout.Height(40)))
            {
                OpenDefaultScene();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Start Game!", GUILayout.ExpandWidth(true), GUILayout.Height(60)))
            {
                StartGame();
            }

            //if (GUILayout.Button("Start Game with patcher!", GUILayout.ExpandWidth(true), GUILayout.Height(60)))
            //{
            //	StartGameWithPatcher();
            //}

            if (GUILayout.Button("Add Select Scene!", GUILayout.ExpandWidth(true), GUILayout.Height(60)))
            {
                AddSceneToCurrentScene();
            }

            EditorGUILayout.EndHorizontal();
        }

        OnGUIOptions();
    }

    private void OpenDefaultScene()
    {
        string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

        if (scenes.Length > 0)
        {
            var sc = EditorSceneManager.OpenScene(scenes[0]);
            if (sc != null)
                EditorApplication.isPlaying = true;
        }
    }

    private void StartGame()
    {
        //EditorApplication.OpenScene("Assets/Scenes/Levels/antaras_room2/antaras_room2.unity");
        if (string.IsNullOrEmpty(selectScenePath) == false)
        {
            EditorSceneManager.OpenScene(selectScenePath);
        }

        EditorApplication.isPlaying = true;
    }

    //private void StartGameWithPatcher()
    //{
    //	//EditorApplication.OpenScene("Assets/Scenes/Levels/antaras_room2/antaras_room2.unity");
    //	EditorSceneManager.OpenScene("Assets/AssetBundle/Scenes/GameScene/adencastle.unity");
    //	EditorApplication.isPlaying = true;
    //}

    private void AddSceneToCurrentScene()
    {
        EditorSceneManager.OpenScene(selectScenePath, OpenSceneMode.Additive);
    }

    private void RefreshSceneList()
    {
        for (int i = 0; i < (int)SceneType.Max; ++i)
        {
            sceneList[i].ClearRows();

            string[] paths = SceneRootPaths[i].Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string rootPath in paths)
            {
                if (!System.IO.Directory.Exists(rootPath))
                    continue;

                string[] scenes = System.IO.Directory.GetFiles(rootPath, "*.unity", System.IO.SearchOption.AllDirectories);
                foreach (string scene in scenes)
                {
                    if ((SceneType)i == SceneType.Design)
                    {
                        if (scene.Contains("Art_Templates"))
                            continue;
                    }

                    if ((SceneType)i == SceneType.Art)
                    {
                        if (scene.Contains("_art") == false)
                            continue;
                    }

                    if ((SceneType)i == SceneType.Design)
                    {
                        if (scene.Contains("_design") == false)
                            continue;
                    }

                    if ((SceneType)i == SceneType.Sound)
                    {
                        if (scene.Contains("_sound") == false)
                            continue;
                    }

                    ListCtrl.Row row = sceneList[i].AddRow();
                    //row.AddItem(System.IO.Path.GetDirectoryName(scene));
                    ListCtrl.Item item = row.AddItem(System.IO.Path.GetFileNameWithoutExtension(scene));
                    item.SetData(scene);
                }
            }
        }
    }

    private void OnGUIHeader()
    {
        sceneType = (SceneType)GUILayout.Toolbar((int)sceneType, SceneTypeStrings);
    }

    private void OnGUIOptions()
    {
        EditorGUILayout.BeginHorizontal();
        closeAfterOpenScene = GUILayout.Toggle(closeAfterOpenScene, "Close after open a scene");
        if (GUILayout.Button("Refresh"))
        {
            RefreshSceneList();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnClickScene(ListCtrl.Row row)
    {
        selectScenePath = (string)row.GetMainItem().Data;
    }

    private void OnDblClickScene(ListCtrl.Row row)
    {
        //if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            string scenePath = (string)row.GetMainItem().Data;
            //EditorApplication.OpenScene(scenePath);
            EditorSceneManager.OpenScene(scenePath);

            if (closeAfterOpenScene)
            {
                Close();
            }
        }
    }

    private void SaveSettings()
    {
        EditorPrefs.SetInt("SO_SceneType", (int)sceneType);
        EditorPrefs.SetBool("SO_CloseAfterOpenScene", closeAfterOpenScene);
    }

    private void LoadSetting()
    {
        sceneType = (SceneType)EditorPrefs.GetInt("SO_SceneType");
        closeAfterOpenScene = EditorPrefs.GetBool("SO_CloseAfterOpenScene");
    }
}