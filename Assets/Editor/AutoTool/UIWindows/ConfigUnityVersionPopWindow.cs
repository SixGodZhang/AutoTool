using UnityEditor;
using UnityEngine;

namespace AutoTool
{
    class ConfigUnityVersionPopWindow:EditorWindow
    {
        public static void Init()
        {
            ConfigUnityVersionPopWindow window = GetWindow<ConfigUnityVersionPopWindow>("设置Unity路径");
            window.minSize = new Vector2(400, 100);
            window.maxSize = new Vector2(450, 150);
            window.ShowPopup();
        }

        private string currentConfigUnityVersion = null;

        private void OnInspectorUpdate()
        {
            Focus();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Unity版本: ",GUILayout.Width(100));
            currentConfigUnityVersion = EditorGUILayout.TextField(Application.unityVersion, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("确认",GUILayout.Width(200)))
            {
                if (EditorUtility.DisplayDialog("提示","确认将工具的适应版本改为 " + currentConfigUnityVersion, "OK"))
                {
                    AutoToolConstants.UnityVersion = currentConfigUnityVersion;
                    ClearUnitySelect();
                    Close();
                }
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void ClearUnitySelect()
        {
            if (EditorPrefs.HasKey(AutoToolPrefKeys.UnityEXE))
            {
                EditorPrefs.DeleteKey(AutoToolPrefKeys.UnityEXE);
                BuildPiplineWindow.UnityEXE = null;
            }
        }
    }
}
