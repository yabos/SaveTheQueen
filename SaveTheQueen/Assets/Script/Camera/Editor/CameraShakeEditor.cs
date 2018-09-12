using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Xml;
using Aniz.Cam.Info;
using Aniz.Utils;

[CustomEditor(typeof(CameraShake))]
public class CameraShakeEditor : Editor
{
    MonoScript _script;
    GUIContent _tooltip;
    GUIContent _tooltip2;

    ReorderableList _atPresetsList;
    ReorderableList _eyePresetsList;

    static List<AtQuakeInfo> _playAtPresets = new List<AtQuakeInfo>();
    static List<EyeQuakeInfo> _playEyePresets = new List<EyeQuakeInfo>();
    static string _currentScene;

    public GameObject OffsetGameObject = null;

    void OnEnable()
    {
        if (target == null)
            return;

        var camera2DShake = (CameraShake)target;
        //if (camera2DShake.Impl == null & Camera.main != null)
        //{
        //    camera2DShake.Impl = Camera.main.GetComponent<CameraImpl>();
        //}
        //if (camera2DShake.Impl == null)
        //{
        //    camera2DShake.Impl = FindObjectOfType<CameraImpl>();
        //}

        _script = MonoScript.FromMonoBehaviour(camera2DShake);

        // Get presets from play mode
        if (_playAtPresets == null)
            _playAtPresets = new List<AtQuakeInfo>();

        if (_playEyePresets == null)
            _playEyePresets = new List<EyeQuakeInfo>();

        serializedObject.Update();

#if UNITY_5_3_OR_NEWER
        if (_currentScene != UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name)
        {
            _playAtPresets.Clear();
            _playEyePresets.Clear();
        }

        _currentScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
#else
            if (_currentScene != EditorApplication.currentScene)
                _playModePresets.Clear();

            _currentScene = EditorApplication.currentScene;
#endif

        if (!Application.isPlaying && (_playAtPresets.Count > 0 || _playEyePresets.Count > 0))
        {
            var atlist = serializedObject.FindProperty("AtCameraDatas");
            atlist.ClearArray();
            for (int i = 0; i < _playAtPresets.Count; i++)
            {
                atlist.InsertArrayElementAtIndex(i);
                var preset = atlist.GetArrayElementAtIndex(i);
                preset.FindPropertyRelative("EffectName").stringValue = _playAtPresets[i].EffectName;
                preset.FindPropertyRelative("UserUsed").boolValue = _playAtPresets[i].UserUsed;
                preset.FindPropertyRelative("LifeTime").floatValue = _playAtPresets[i].LifeTime;
                preset.FindPropertyRelative("LoopCount").intValue = _playAtPresets[i].LoopCount;
                preset.FindPropertyRelative("FadeTime").floatValue = _playAtPresets[i].FadeTime;
                preset.FindPropertyRelative("Power").floatValue = _playAtPresets[i].Power;
                preset.FindPropertyRelative("MaxRange").floatValue = _playAtPresets[i].MaxRange;
                preset.FindPropertyRelative("Direction").vector3Value = _playAtPresets[i].Direction;

            }
            _playAtPresets.Clear();

            var eyelist = serializedObject.FindProperty("EyeCameraDatas");
            eyelist.ClearArray();
            for (int i = 0; i < _playEyePresets.Count; i++)
            {
                eyelist.InsertArrayElementAtIndex(i);
                var preset = eyelist.GetArrayElementAtIndex(i);
                preset.FindPropertyRelative("EffectName").stringValue = _playEyePresets[i].EffectName;
                preset.FindPropertyRelative("UserUsed").boolValue = _playEyePresets[i].UserUsed;
                preset.FindPropertyRelative("eType").enumValueIndex = (int)_playEyePresets[i].eType;
                preset.FindPropertyRelative("LoadCount").intValue = _playEyePresets[i].LoadCount;
                preset.FindPropertyRelative("BlendWidth").intValue = (int)_playEyePresets[i].BlendWidth;
                preset.FindPropertyRelative("StepCount").intValue = _playEyePresets[i].StepCount;
                preset.FindPropertyRelative("TimeLength").floatValue = _playEyePresets[i].TimeLength;
                preset.FindPropertyRelative("MaxRange").floatValue = _playEyePresets[i].MaxRange;
                preset.FindPropertyRelative("RandState").boolValue = _playEyePresets[i].RandState;
                preset.FindPropertyRelative("RandLength").floatValue = _playEyePresets[i].RandLength;
                preset.FindPropertyRelative("LifeTime").floatValue = _playEyePresets[i].LifeTime;
            }
            _playEyePresets.Clear();
        }
        serializedObject.ApplyModifiedProperties();

        // Shake presets list
        _atPresetsList = new ReorderableList(serializedObject, serializedObject.FindProperty("AtCameraDatas"), false, true, false, true);

        _atPresetsList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            rect.y += 2;
            var element = _atPresetsList.serializedProperty.GetArrayElementAtIndex(index);

            // Name field
            EditorGUI.PropertyField(new Rect(
                    rect.x,
                    rect.y,
                    rect.width / 4f,
                    EditorGUIUtility.singleLineHeight * 1.1f),
                element.FindPropertyRelative("EffectName"), GUIContent.none);

            // Load button
            if (GUI.Button(new Rect(
                        rect.x + rect.width / 4f + 5,
                        rect.y,
                        rect.width / 4f - 5,
                        EditorGUIUtility.singleLineHeight * 1.1f), "Load"))
            {

                camera2DShake.LifeTime = element.FindPropertyRelative("LifeTime").floatValue;
                camera2DShake.LoopCount = element.FindPropertyRelative("LoopCount").intValue;
                camera2DShake.FadeTime = element.FindPropertyRelative("FadeTime").floatValue;
                camera2DShake.Power = element.FindPropertyRelative("Power").floatValue;
                camera2DShake.AtMaxRange = (uint)element.FindPropertyRelative("MaxRange").floatValue;
                camera2DShake.Direction = element.FindPropertyRelative("Direction").vector3Value;

                EditorUtility.SetDirty(target);
            }

            // Save button
            if (GUI.Button(new Rect(
                        rect.x + 2 * rect.width / 4f + 5,
                        rect.y,
                        rect.width / 4f - 5,
                        EditorGUIUtility.singleLineHeight * 1.1f), "Save"))
            {
                element.FindPropertyRelative("LifeTime").floatValue = camera2DShake.LifeTime;
                element.FindPropertyRelative("LoopCount").intValue = camera2DShake.LoopCount;
                element.FindPropertyRelative("FadeTime").floatValue = camera2DShake.FadeTime;
                element.FindPropertyRelative("Power").floatValue = camera2DShake.Power;
                element.FindPropertyRelative("MaxRange").floatValue = camera2DShake.AtMaxRange;
                element.FindPropertyRelative("Direction").vector3Value = camera2DShake.Direction;


                //proCamera2DShake.UseRandomInitialAngle = proCamera2DShake.InitialAngle < 0;

                EditorUtility.SetDirty(target);

                Repaint();
            }

            // Shake button
            GUI.enabled = Application.isPlaying;
            if (GUI.Button(new Rect(
                        rect.x + 3 * rect.width / 4f + 5,
                        rect.y,
                        rect.width / 4f - 5,
                        EditorGUIUtility.singleLineHeight * 1.1f), "Shake!"))
            {
                AtQuakeInfo data = new AtQuakeInfo();
                data.EffectName = "AtData" + camera2DShake.AtCameraDatas.Count;
                data.LifeTime = element.FindPropertyRelative("LifeTime").floatValue;
                data.LoopCount = element.FindPropertyRelative("LoopCount").intValue;
                data.FadeTime = element.FindPropertyRelative("FadeTime").floatValue;
                data.Power = element.FindPropertyRelative("Power").floatValue;
                data.MaxRange = element.FindPropertyRelative("MaxRange").floatValue;
                data.Direction = element.FindPropertyRelative("Direction").vector3Value;

                camera2DShake.AtShake(data);
            }
            GUI.enabled = true;
        };

        _atPresetsList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "At Presets");
        };

        _atPresetsList.elementHeight = 30;
        _atPresetsList.draggable = true;


        // Shake presets list
        _eyePresetsList = new ReorderableList(serializedObject, serializedObject.FindProperty("EyeCameraDatas"), false, true, false, true);

        _eyePresetsList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            rect.y += 2;
            var element = _eyePresetsList.serializedProperty.GetArrayElementAtIndex(index);

            // Name field
            EditorGUI.PropertyField(new Rect(
                    rect.x,
                    rect.y,
                    rect.width / 4f,
                    EditorGUIUtility.singleLineHeight * 1.1f),
                element.FindPropertyRelative("EffectName"), GUIContent.none);

            // Load button
            if (GUI.Button(new Rect(
                        rect.x + rect.width / 4f + 5,
                        rect.y,
                        rect.width / 4f - 5,
                        EditorGUIUtility.singleLineHeight * 1.1f), "Load"))
            {
                camera2DShake.eType = (EyeQuakeInfo.Type)element.FindPropertyRelative("eType").enumValueIndex;
                camera2DShake.LoadCount = element.FindPropertyRelative("LoadCount").intValue;
                camera2DShake.BlendWidth = (uint)element.FindPropertyRelative("BlendWidth").intValue;
                camera2DShake.StepCount = element.FindPropertyRelative("StepCount").intValue;
                camera2DShake.TimeLength = element.FindPropertyRelative("TimeLength").floatValue;
                camera2DShake.EyeMaxRange = element.FindPropertyRelative("MaxRange").floatValue;
                camera2DShake.RandState = element.FindPropertyRelative("RandState").boolValue;
                camera2DShake.RandLength = element.FindPropertyRelative("RandLength").floatValue;
                camera2DShake.EyeLifeTime = element.FindPropertyRelative("LifeTime").floatValue;

                //proCamera2DShake.UseRandomInitialAngle = proCamera2DShake.InitialAngle < 0;

                EditorUtility.SetDirty(target);
            }

            // Save button
            if (GUI.Button(new Rect(
                        rect.x + 2 * rect.width / 4f + 5,
                        rect.y,
                        rect.width / 4f - 5,
                        EditorGUIUtility.singleLineHeight * 1.1f), "Save"))
            {
                element.FindPropertyRelative("eType").enumValueIndex = (int)camera2DShake.eType;
                element.FindPropertyRelative("LoadCount").intValue = camera2DShake.LoadCount;
                element.FindPropertyRelative("BlendWidth").intValue = (int)camera2DShake.BlendWidth;
                element.FindPropertyRelative("StepCount").intValue = camera2DShake.StepCount;
                element.FindPropertyRelative("TimeLength").floatValue = camera2DShake.TimeLength;
                element.FindPropertyRelative("MaxRange").floatValue = camera2DShake.EyeMaxRange;
                element.FindPropertyRelative("RandState").boolValue = camera2DShake.RandState;
                element.FindPropertyRelative("RandLength").floatValue = camera2DShake.RandLength;
                element.FindPropertyRelative("LifeTime").floatValue = camera2DShake.EyeLifeTime;

                //proCamera2DShake.UseRandomInitialAngle = proCamera2DShake.InitialAngle < 0;

                EditorUtility.SetDirty(target);

                Repaint();
            }

            // Shake button
            GUI.enabled = Application.isPlaying;
            if (GUI.Button(new Rect(
                        rect.x + 3 * rect.width / 4f + 5,
                        rect.y,
                        rect.width / 4f - 5,
                        EditorGUIUtility.singleLineHeight * 1.1f), "Shake!"))
            {
                EyeQuakeInfo info = new EyeQuakeInfo();
                info.EffectName = "EyeData" + camera2DShake.EyeCameraDatas.Count;
                info.eType = (EyeQuakeInfo.Type)element.FindPropertyRelative("eType").enumValueIndex;
                info.LoadCount = element.FindPropertyRelative("LoadCount").intValue;
                info.BlendWidth = (uint)element.FindPropertyRelative("BlendWidth").intValue;
                info.StepCount = element.FindPropertyRelative("StepCount").intValue;
                info.TimeLength = element.FindPropertyRelative("TimeLength").floatValue;
                info.MaxRange = element.FindPropertyRelative("MaxRange").floatValue;
                info.RandState = element.FindPropertyRelative("RandState").boolValue;
                info.RandLength = element.FindPropertyRelative("RandLength").floatValue;
                info.LifeTime = element.FindPropertyRelative("LifeTime").floatValue;

                camera2DShake.EyeShake(info);
            }
            GUI.enabled = true;
        };

        _eyePresetsList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Eye Presets");
        };

        _eyePresetsList.elementHeight = 30;
        _eyePresetsList.draggable = true;
    }

    void OnDisable()
    {
        if (target == null)
            return;

        var cameraShake = (CameraShake)target;

        _playAtPresets = cameraShake.AtCameraDatas;
        _playEyePresets = cameraShake.EyeCameraDatas;
    }

    public override void OnInspectorGUI()
    {
        var cameraShake = (CameraShake)target;

        if (cameraShake.Impl == null)
            EditorGUILayout.HelpBox("CameraImpl is not set.", MessageType.Error, true);

        serializedObject.Update();

        // Show script link
        GUI.enabled = false;
        _script = EditorGUILayout.ObjectField("Script", _script, typeof(MonoScript), false) as MonoScript;
        GUI.enabled = true;

        // ProCamera2D
        //         _tooltip = new GUIContent("CameraImpl", "");
        //         EditorGUILayout.PropertyField(serializedObject.FindProperty("CameraImpl"), _tooltip);

        OffsetGameObject = (GameObject)EditorGUILayout.ObjectField("OffsetGameObject", OffsetGameObject, typeof(GameObject), true);

        _tooltip = new GUIContent("Offset", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Offset"), _tooltip);

        if (cameraShake.Impl != null)
        {
            if (OffsetGameObject != null)
            {
                cameraShake.Offset = OffsetGameObject.transform.position;
            }

            _tooltip = new GUIContent("CameraAt", "");
            Vector3 at = (cameraShake.Impl.UpdateInfo.At);
            _tooltip2 = new GUIContent(at.ToString(), "");
            EditorGUILayout.LabelField(_tooltip, _tooltip2);

            _tooltip = new GUIContent("Dist", "");
            float dist = Vector3.Distance((cameraShake.Offset), at);
            _tooltip2 = new GUIContent(string.Format("{0}", dist), "");
            EditorGUILayout.LabelField(_tooltip, _tooltip2);
        }

        // Strength
        _tooltip = new GUIContent("LifeTime", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("LifeTime"), _tooltip);
        _tooltip = new GUIContent("LoopCount", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("LoopCount"), _tooltip);
        _tooltip = new GUIContent("FadeTime", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("FadeTime"), _tooltip);

        // Duration
        _tooltip = new GUIContent("Power", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Power"), _tooltip);

        // Vibrato
        _tooltip = new GUIContent("AtMaxRange", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AtMaxRange"), _tooltip);

        // Smoothness
        _tooltip = new GUIContent("Direction", "Indicates how smooth the shake will be");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Direction"), _tooltip);


        // Random initial direction
        EditorGUILayout.BeginHorizontal();
        _tooltip = new GUIContent("CameraEyeQuakeStock.Type", "If enabled, the initial shaking angle will be random");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("eType"), _tooltip);

        //         if (!proCamera2DShake.UseRandomInitialAngle)
        //         {
        //             _tooltip = new GUIContent("Initial Angle", "");
        //             EditorGUILayout.PropertyField(serializedObject.FindProperty("InitialAngle"), _tooltip);
        //         }
        EditorGUILayout.EndHorizontal();

        // Rotation
        _tooltip = new GUIContent("LoadCount", "The maximum rotation the camera will reach");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("LoadCount"), _tooltip);

        // Ignore time scale
        _tooltip = new GUIContent("BlendWidth", "If enabled, the shake will occur even if the timeScale is 0");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("BlendWidth"), _tooltip);

        _tooltip = new GUIContent("StepCount", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("StepCount"), _tooltip);

        _tooltip = new GUIContent("TimeLength", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("TimeLength"), _tooltip);

        _tooltip = new GUIContent("EyeMaxRange", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EyeMaxRange"), _tooltip);

        _tooltip = new GUIContent("RandState", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("RandState"), _tooltip);

        _tooltip = new GUIContent("RandLength", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("RandLength"), _tooltip);

        _tooltip = new GUIContent("EyeLifeTime", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EyeLifeTime"), _tooltip);

        // Shake test buttons
        GUI.enabled = Application.isPlaying;
        if (GUILayout.Button("EyeShake!"))
        {
            cameraShake.EyeShake();
        }

        if (GUILayout.Button("AtShake!"))
        {
            cameraShake.AtShake();
        }

        if (GUILayout.Button("Stop!"))
        {
            cameraShake.StopShaking();
        }
        GUI.enabled = true;

        // Add to presets button
        if (GUILayout.Button("Add To AtData"))
        {
            cameraShake.AtCameraDatas.Add(new AtQuakeInfo()
            {
                EffectName = "AtData" + cameraShake.AtCameraDatas.Count,
                UserUsed = cameraShake.UserUsed,
                LifeTime = cameraShake.LifeTime,
                LoopCount = cameraShake.LoopCount,
                FadeTime = cameraShake.FadeTime,
                Power = cameraShake.Power,
                MaxRange = cameraShake.AtMaxRange,
                Direction = cameraShake.Direction,
            });
        }

        if (GUILayout.Button("Add To EyeData"))
        {
            cameraShake.EyeCameraDatas.Add(new EyeQuakeInfo()
            {
                EffectName = "EyeData" + cameraShake.EyeCameraDatas.Count,
                UserUsed = cameraShake.UserUsed,
                eType = cameraShake.eType,
                LoadCount = cameraShake.LoadCount,
                BlendWidth = cameraShake.BlendWidth,
                StepCount = cameraShake.StepCount,
                TimeLength = cameraShake.TimeLength,
                RandState = cameraShake.RandState,
                RandLength = cameraShake.RandLength,
                MaxRange = cameraShake.EyeMaxRange,
                LifeTime = cameraShake.EyeLifeTime,
            });
        }

        if (GUILayout.Button("Load To Xml"))
        {
            LoadXml();
        }

        if (GUILayout.Button("Export To Xml"))
        {
            SaveXml();
        }

        // Presets list
        EditorGUILayout.Space();
        _atPresetsList.DoLayoutList();
        EditorGUILayout.Space();
        _eyePresetsList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private void LoadXml()
    {
        var cameraShake = (CameraShake)target;
        _playEyePresets.Clear();
        _playAtPresets.Clear();

        cameraShake.AtCameraDatas.Clear();
        cameraShake.EyeCameraDatas.Clear();

        string filename = Application.dataPath + "/AssetBundle/Fx/CameraShakeData.xml";
        XmlDocument doc = new XmlDocument();
        doc.Load(filename);

        //CameraTable.LoadXmlData(doc, ref cameraShake.AtCameraDatas, ref cameraShake.EyeCameraDatas);


        /*
        XmlNode nodeData = doc.SelectSingleNode("ShakeData");
        XmlNode atNode = nodeData.SelectSingleNode("AtShakeList");
        XmlNodeList atNodeList = atNode.SelectNodes("AtShake");
        foreach (XmlNode datanode in atNodeList)
        {
            XmlElement dataelement = (XmlElement)datanode;
            AtQuakeStock atCameraData = new AtQuakeStock();
            atCameraData.EffectName = dataelement.GetAttribute("EffectName");
            atCameraData.UserUsed = DataParserAssist.BoolParser(dataelement.GetAttribute("UserUsed"));
            atCameraData.LifeTime = DataParserAssist.FloatParser(dataelement.GetAttribute("LifeTime"));
            atCameraData.LoopCount = DataParserAssist.IntParser(dataelement.GetAttribute("LoopCount"));
            atCameraData.FadeTime = DataParserAssist.FloatParser(dataelement.GetAttribute("FadeTime"));
            atCameraData.MaxRange = DataParserAssist.FloatParser(dataelement.GetAttribute("MaxRange"));
            atCameraData.Power = DataParserAssist.FloatParser(dataelement.GetAttribute("Power"));
            atCameraData.Direction = DataParserAssist.Vector3Parser(dataelement.GetAttribute("Direction"));

            cameraShake.AtCameraDatas.Add(atCameraData);
        }

        XmlNode eyeNode = nodeData.SelectSingleNode("EyeList");
        XmlNodeList eyeNodeList = eyeNode.SelectNodes("EyeQuake");
        foreach (XmlNode datanode in eyeNodeList)
        {
            XmlElement dataelement = (XmlElement)datanode;
            EyeQuakeStock eyeCameraData = new EyeQuakeStock();
            eyeCameraData.EffectName = dataelement.GetAttribute("EffectName");
            eyeCameraData.UserUsed = DataParserAssist.BoolParser(dataelement.GetAttribute("UserUsed"));
            eyeCameraData.eType = DataParserAssist.EnumParser<EyeQuakeStock.Type>(dataelement.GetAttribute("Type"));
            eyeCameraData.LoadCount = DataParserAssist.IntParser(dataelement.GetAttribute("LoadCount"));
            eyeCameraData.BlendWidth = DataParserAssist.UintParser(dataelement.GetAttribute("BlendWidth"));
            eyeCameraData.StepCount = DataParserAssist.IntParser(dataelement.GetAttribute("StepCount"));
            eyeCameraData.TimeLength = DataParserAssist.FloatParser(dataelement.GetAttribute("TimeLength"));
            eyeCameraData.MaxRange = DataParserAssist.FloatParser(dataelement.GetAttribute("MaxRange"));
            eyeCameraData.RandState = DataParserAssist.BoolParser(dataelement.GetAttribute("RandState"));
            eyeCameraData.RandLength = DataParserAssist.FloatParser(dataelement.GetAttribute("RandLength"));
            eyeCameraData.LifeTime = DataParserAssist.FloatParser(dataelement.GetAttribute("LifeTime"));

            cameraShake.EyeCameraDatas.Add(eyeCameraData);
        }*/

    }
    private void SaveXml()
    {
        var cameraShake = (CameraShake)target;

        if (cameraShake.AtCameraDatas.Count <= 0 && cameraShake.EyeCameraDatas.Count <= 0)
        {
            return;
        }

        XmlDocument xmldoc = new XmlDocument();
        //XmlDeclaration xmldec = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
        XmlDeclaration xmldec = xmldoc.CreateXmlDeclaration("1.0", "EUC-KR", "yes");
        xmldoc.AppendChild(xmldec);

        XmlElement xmlShakedata = xmldoc.CreateElement("ShakeData");

        XmlElement xmlatList = xmldoc.CreateElement("AtShakeList");
        foreach (AtQuakeInfo stock in cameraShake.AtCameraDatas)
        {
            XmlElement xmlTexture = xmldoc.CreateElement("AtShake");
            xmlTexture.SetAttribute("EffectName", stock.EffectName);
            xmlTexture.SetAttribute("UserUsed", stock.UserUsed.ToString());
            xmlTexture.SetAttribute("LifeTime", stock.LifeTime.ToString());
            xmlTexture.SetAttribute("LoopCount", stock.LoopCount.ToString());
            xmlTexture.SetAttribute("FadeTime", stock.FadeTime.ToString());
            xmlTexture.SetAttribute("Power", stock.Power.ToString());
            xmlTexture.SetAttribute("MaxRange", stock.MaxRange.ToString());
            xmlTexture.SetAttribute("Direction", DataParserAssist.ToVector3String(stock.Direction));
            xmlatList.AppendChild(xmlTexture);
        }

        xmlShakedata.AppendChild(xmlatList);

        XmlElement xmlEyeList = xmldoc.CreateElement("EyeList");
        foreach (EyeQuakeInfo stock in cameraShake.EyeCameraDatas)
        {
            XmlElement xmlTexture = xmldoc.CreateElement("EyeQuake");
            xmlTexture.SetAttribute("EffectName", stock.EffectName);
            xmlTexture.SetAttribute("UserUsed", stock.UserUsed.ToString());
            xmlTexture.SetAttribute("Type", stock.eType.ToString());
            xmlTexture.SetAttribute("LoadCount", stock.LoadCount.ToString());
            xmlTexture.SetAttribute("BlendWidth", stock.BlendWidth.ToString());
            xmlTexture.SetAttribute("StepCount", stock.StepCount.ToString());
            xmlTexture.SetAttribute("TimeLength", stock.TimeLength.ToString());
            xmlTexture.SetAttribute("MaxRange", stock.MaxRange.ToString());
            xmlTexture.SetAttribute("RandState", stock.RandState.ToString());
            xmlTexture.SetAttribute("RandLength", stock.RandLength.ToString());
            xmlTexture.SetAttribute("LifeTime", stock.LifeTime.ToString());
            xmlEyeList.AppendChild(xmlTexture);
        }

        xmlShakedata.AppendChild(xmlEyeList);

        xmldoc.AppendChild(xmlShakedata);
        string filename = Application.dataPath + "/Resources/Fx/CameraShakeData.xml";
        xmldoc.Save(filename);
    }
}