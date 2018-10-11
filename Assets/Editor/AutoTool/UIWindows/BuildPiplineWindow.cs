using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AutoTool
{
    public class BuildPiplineWindow : EditorWindow, IHasCustomMenu
    {
        public static BuildPiplineWindow _instance = null;

        public List<IBuildTask> buildTasks = new List<IBuildTask>();//显示任务列表

        #region UI控件--(映射)-->需要进行的操作
        public bool isExcuteATBuildPipline = false;

        public bool isSelectedCompileCode_compile = true;

        public bool isSelectedUpdate_update = true;

        public int selectBuildChannel = 0;

        public int selectBuildPlatform = 0;

        private int _selectTaskChainType = 0;
        private int SelectTaskChainType
        {
            set
            {
                if (value != _selectTaskChainType)
                {
                    //值发生变化
                    buildTasks.Clear();
                    ATBuildPipline.Instance.ClearBuildTasks();
                }

                _selectTaskChainType = value;
            }

            get
            {
                return _selectTaskChainType;
            }
        }

        

        private bool isSettingBatVariable = false;//配置

        public static string UnityEXE = "";

        #endregion

        #region 其他
        private static string Root = Environment.CurrentDirectory;
        string line = new string('_', 150);//分割线
        string[] chainTypes = Enum.GetNames(typeof(TaskChain));//与枚举一一对应
        #endregion

        #region 样式
        private GUIStyle _fontGreenStyle;
        private GUIStyle _fontGreyStyle;
        private GUIStyle _fontRedStyle;
        private GUIStyle _fontYellowStyle;
        private GUIStyle _titleStyle;

        private Vector2 _scrollPosition;
        #endregion

        public static BuildPiplineWindow Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = GetWindow<BuildPiplineWindow>();
                }
                return _instance;
            }
        }

        [MenuItem("Editor/BuildAndroidPipline")]
        public static void BuildAndroidPipline()
        {
            BuildPiplineWindow piplineWindow = Instance;
            piplineWindow.titleContent = new GUIContent("自动化打包工具");
            piplineWindow.minSize = new Vector2(1000, 654);
            piplineWindow.maxSize = new Vector2(1000, 654);
            piplineWindow.Show();
        }

        //增加功能到右侧扩展功能
        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("重置所有设置"), false, () => { ResetKeys(); });
            menu.AddItem(new GUIContent("设置Unity版本"), false, () => { ConfigUnityVersion(); });
            menu.AddItem(new GUIContent("查看Log日志"), false, () => { ShowLogDirectory(); });
            menu.AddItem(new GUIContent("清除Log"), false, () => { ClearLog(); });

        }

        /// <summary>
        /// 设置Unity版本
        /// </summary>
        private void ConfigUnityVersion()
        {
            ConfigUnityVersionPopWindow.Init();
        }

        /// <summary>
        /// 清除所有任务
        /// </summary>
        public void ClearAllTasks()
        {
            buildTasks.Clear();
            ATBuildPipline.Instance.ClearBuildTasks();
        }

        public void OnInspectorUpdate()
        {
            // This will only get called 10 times per second.
            Repaint();
        }

        /// <summary>
        /// 添加任务到任务管线中
        /// </summary>
        private void AddTasksToATBuildPipline(TaskChain taskChain)
        {
            buildTasks.Clear();
            ATBuildPipline.Instance.ClearBuildTasks();


            switch (taskChain)
            {
                case TaskChain.默认任务链:
                    {
                        #region 默认任务链
                        //TODO
                        //主要任务链
                        #endregion
                    }
                    break;
                case TaskChain.资源打包:
                    {

                    }
                    break;
                case TaskChain.APK出包:
                    {

                    }
                    break;
                case TaskChain.测试任务链:
                    {
                        #region 相关测试任务
                        //测试
                        //for (int i = 0; i < 10; i++)
                        //{
                        //    ATBuildPipline.Instance.AddBuildTask(new TestTask(Instance));
                        //}

                        //测试任务
                        //ATBuildPipline.Instance.AddBuildTask(new TestFailureTask(Instance));
                        //ATBuildPipline.Instance.AddBuildTask(new TestCallBatByThreadTask(Instance));
                        //ATBuildPipline.Instance.AddBuildTask(new TestTask(Instance));

                        //测试回滚
                        for (int i = 0; i < 5; i++)
                        {
                            ATBuildPipline.Instance.AddBuildTask(new TestTask(Instance));
                        }

                        ATBuildPipline.Instance.AddBuildTask(new TestReverseTask(Instance));

                        ATBuildPipline.Instance.AddBuildTask(new TestFailureTask(Instance));
                        #endregion
                    }
                    break;
            }

            //获取所有任务
            foreach (var item in ATBuildPipline.Instance.Tasks)
            {
                buildTasks.Add(item);
            }
        }

        private void ClearLog()
        {
            ATLog.ClearLog();
            EditorUtility.DisplayDialog("提示", "Log清理完成!", "OK");
        }


        private void OnClickSelectUnityInstallDirectoryBtn()
        {
            UnityEXE = EditorUtility.OpenFilePanel("Unity安装路径", System.Environment.CurrentDirectory, "exe");

            System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            //Debug.LogError(currentProcess.MainModule.FileName);
            if (currentProcess.MainModule.FileName.Replace("\\", "/") != UnityEXE || !AutoToolConstants.UnityVersion.Equals(Application.unityVersion))
            {
                EditorUtility.DisplayDialog("错误", "未选择"+AutoToolConstants.UnityVersion+"的Unity.exe!", "OK");
                ATLog.Warn("未找到相应版本的Unity!");
                UnityEXE = null;
                return;
            }

            EditorPrefs.SetString(AutoToolPrefKeys.UnityEXE, UnityEXE);
        }

        void OnEnable()
        {
            _fontGreyStyle = new GUIStyle()
            {
                richText = true,
                fontSize = 11,
                normal = new GUIStyleState() { textColor = Color.grey }
            };

            _fontYellowStyle = new GUIStyle()
            {
                richText = true,
                fontSize = 11,
                normal = new GUIStyleState() { textColor = Color.yellow }
            };

            _fontGreenStyle = new GUIStyle()
            {
                richText = true,
                fontSize = 11,
                normal = new GUIStyleState() { textColor = Color.green }
            };

            _fontRedStyle = new GUIStyle()
            {
                richText = true,
                fontSize = 11,
                normal = new GUIStyleState() { textColor = Color.red }
            };

            _titleStyle = new GUIStyle()
            {
                richText = true,
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                border = new RectOffset(0, 0, 5, 5),
                normal = new GUIStyleState() { textColor = Color.white * 0.7f }
            };

        }

        private void OnGUI()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("仅在第一次使用该功能时设定:");
            EditorGUILayout.BeginHorizontal();

            UnityEXE = EditorPrefs.HasKey(AutoToolPrefKeys.UnityEXE) ? EditorPrefs.GetString(AutoToolPrefKeys.UnityEXE) : "";
            EditorGUI.BeginDisabledGroup(true);
            UnityEXE = EditorGUILayout.TextField("Unity安装路径", UnityEXE);
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("选择Unity安装目录", GUILayout.Width(200)))
            {
                OnClickSelectUnityInstallDirectoryBtn();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField(line);
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            SelectTaskChainType = EditorGUILayout.Popup("任务链类型",SelectTaskChainType, Enum.GetNames(typeof(TaskChain)), GUILayout.Width(400));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField(line);
            GUILayout.Space(10);

            if (chainTypes[SelectTaskChainType].Equals(Enum.GetName(typeof(TaskChain), TaskChain.默认任务链)))
            {
                OnPaintDefaultChainUI();
            }
            else if (chainTypes[SelectTaskChainType].Equals(Enum.GetName(typeof(TaskChain), TaskChain.资源打包)))
            {
                OnPaintResourceChainUI();
            }
            else if (chainTypes[SelectTaskChainType].Equals(Enum.GetName(typeof(TaskChain), TaskChain.APK出包)))
            {
                OnPaintAPKChainUI();
            }
            else if (chainTypes[SelectTaskChainType].Equals(Enum.GetName(typeof(TaskChain), TaskChain.测试任务链)))
            {
                OnPaintTestChainUI();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            //ClearAllTasks
            if (GUILayout.Button("清除所有任务", GUILayout.Width(200)))
            {
                ClearAllTasks();
            }

            if (GUILayout.Button("添加任务", GUILayout.Width(200)))
            {
                switch (SelectTaskChainType)
                {
                    case 0:
                        {
                            if (chainTypes[0].Equals(Enum.GetName(typeof(TaskChain),TaskChain.默认任务链)))
                            {//验证(可以省略,此处是为了验证0是否是默认任务链)
                                AddTasksToATBuildPipline(TaskChain.默认任务链);
                            }
                        }
                        break;
                    case 1:
                        {
                            if (chainTypes[1].Equals(Enum.GetName(typeof(TaskChain), TaskChain.资源打包)))
                            {//验证(可以省略,此处是为了验证0是否是默认任务链)
                                AddTasksToATBuildPipline(TaskChain.资源打包);
                            }
                        }
                        break;
                    case 2:
                        {
                            if (chainTypes[2].Equals(Enum.GetName(typeof(TaskChain), TaskChain.APK出包)))
                            {//验证(可以省略,此处是为了验证0是否是默认任务链)
                                AddTasksToATBuildPipline(TaskChain.APK出包);
                            }
                        }
                        break;
                    case 3:
                        {
                            if (chainTypes[3].Equals(Enum.GetName(typeof(TaskChain), TaskChain.测试任务链)))
                            {//验证(可以省略,此处是为了验证0是否是默认任务链)
                                AddTasksToATBuildPipline(TaskChain.测试任务链);
                            }
                        }
                        break;
                }
            }

            if (GUILayout.Button("开始打包", GUILayout.Width(200)))
            {
                if (ValidateSomeSetting())
                {
                    isExcuteATBuildPipline = true;
                }
            }

            //////////////////////////////////////////////////////////////////////
            ///启动管线要求：
            ///isExcuteATBuildPipline = true 开启管线
            ///ATBuildPipline.Instance.PiplineStatus == ATBuildPiplineStatus.Unoccupied 管线未被占用
            /////////////////////////////////////////////////////////////////////
            if (isExcuteATBuildPipline)
            {//执行任务
                //Debug.LogError("ATBuildPipline  run ...");
                //ATBuildPipline.Instance.PiplineStatus = ATBuildPiplineStatus.Unoccupied;
                //isExcuteATBuildPipline = false;
                ATBuildPipline.Instance.Run();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            {
                EditorGUILayout.LabelField("当前任务", _titleStyle);
                //TODO
                //当前执行任务
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField(string.Format("=======> {0}", ATBuildPipline.Instance.CurrentTask == null ? "当前无任务" : ATBuildPipline.Instance.CurrentTask.Name),_fontGreenStyle);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("任务列表", _titleStyle);
                EditorGUILayout.Space();

                foreach (var task in buildTasks)
                {
                    EditorGUILayout.BeginHorizontal("box");
                    {
                        EditorGUILayout.LabelField((task.ID).ToString("d2") + ".", _fontGreyStyle, GUILayout.Width(50));
                        EditorGUILayout.LabelField(string.Format("["), _fontGreyStyle, GUILayout.Width(20));
                        EditorGUILayout.LabelField(string.Format("{0}", task.Name), _fontGreyStyle, GUILayout.Width(480));
                        EditorGUILayout.LabelField(string.Format("] : "), _fontGreyStyle, GUILayout.Width(70));
                        EditorGUILayout.LabelField(string.Format("[  {0}  ]", task.Status), SelectFontStyleByTaskStatus(task.Status), GUILayout.Width(100));

                        EditorGUILayout.LabelField(string.Format("{0:d4}.{1:d3}",
                                (int)task.Elapsed.TotalSeconds,
                                task.Elapsed.Milliseconds),
                            GUILayout.Width(200));
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 测试任务链
        /// </summary>
        private void OnPaintTestChainUI()
        {
            
        }

        /// <summary>
        /// APK任务链
        /// </summary>
        private void OnPaintAPKChainUI()
        {
            
        }

        /// <summary>
        /// 资源任务链
        /// </summary>
        private void OnPaintResourceChainUI()
        {
            
        }

        /// <summary>
        /// 绘制默认任务链GUI
        /// </summary>
        private void OnPaintDefaultChainUI()
        {
            EditorGUILayout.BeginHorizontal();
            selectBuildPlatform = EditorGUILayout.Popup("Platform", selectBuildPlatform, Enum.GetNames(typeof(BuildPlatform)), GUILayout.Width(400));
            selectBuildChannel = EditorGUILayout.Popup("Channel", selectBuildChannel, Enum.GetNames(typeof(BuildChannel)), GUILayout.Width(400));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            isSelectedUpdate_update = EditorGUILayout.Toggle("工程更新(保留本地修改)", isSelectedUpdate_update);
            EditorGUILayout.EndHorizontal();

            isSelectedCompileCode_compile = EditorGUILayout.Toggle("编译代码", isSelectedCompileCode_compile);
            GUILayout.Space(10);
            EditorGUILayout.LabelField(line);
        }

        private void ResetKeys()
        {
            //重置 Bat 变量设置
            if (EditorPrefs.HasKey(AutoToolPrefKeys.isSettingBatVariable))
            {
                EditorPrefs.DeleteKey(AutoToolPrefKeys.isSettingBatVariable);
                isSettingBatVariable = false;
            }

            //重置 Unity路径设置
            if (EditorPrefs.HasKey(AutoToolPrefKeys.UnityEXE))
            {
                EditorPrefs.DeleteKey(AutoToolPrefKeys.UnityEXE);
                UnityEXE = null;
            }


        }

        private GUIStyle SelectFontStyleByTaskStatus(TaskStatus status)
        {
            GUIStyle fontStyle = null;
            switch (status)
            {//不需要Running
                case TaskStatus.None:
                    fontStyle = _fontGreyStyle;
                    break;
                case TaskStatus.Start:
                case TaskStatus.Running:
                    fontStyle = _fontYellowStyle;
                    break;
                case TaskStatus.Success:
                    fontStyle = _fontGreenStyle;
                    break;
                case TaskStatus.Failure:
                    fontStyle = _fontRedStyle;
                    break;
            }

            return fontStyle;
        }

        private void ShowLogDirectory()
        {
            FileHelper.ShowExplorerWindow(AutoToolConstants.logPath + "/" + string.Format("Log-{0}", DateTime.Now.ToString("yyyyMMdd")));
        }

        /// <summary>
        /// 验证打包时一些相关设置
        /// </summary>
        /// <returns></returns>
        private bool ValidateSomeSetting()
        {
            bool isPass = true;

            //1.验证Unity路径是否配置
            string UnityEXEPath = EditorPrefs.HasKey(AutoToolPrefKeys.UnityEXE) ? EditorPrefs.GetString(AutoToolPrefKeys.UnityEXE) : null;
            if (string.IsNullOrEmpty(UnityEXEPath))
            {
                EditorUtility.DisplayDialog("提示", "请设置Unity路径!", "OK");
                isPass = false;
            }

            return isPass;
        }
    }
}