using System;
using System.Diagnostics;
using UnityEditor;

namespace AutoTool
{
    class TestCallBatByThreadTask : BuildTask<TestCallBatByThreadTask>
    {
        public override string Name { get { return "测试线程调用批处理"; } }

        public EditorWindow _currentWindow = null;
        private Stopwatch stopwatch = new Stopwatch();

        public TestCallBatByThreadTask(EditorWindow window,int ID = 20020)
        {
            ResetTask();
            _id = ID;
            _currentWindow = window;
        }

        /// <summary>
        /// 重置任务
        /// </summary>
        public void ResetTask()
        {
            _status = TaskStatus.None;
            _elapsed = new TimeSpan();
            _currentWindow = null;
            stopwatch = new Stopwatch();
        }

        private int _id = 0;
        public override int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        private TaskStatus _status = TaskStatus.None;
        public override TaskStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        private TimeSpan _elapsed = new TimeSpan();
        public override TimeSpan Elapsed
        {
            get
            {
                return _elapsed;
            }
        }


        public override void DoTask()
        {
            //TODO
            //主要任务内容
            ATLog.Info("任务: " + Name + "Begin...");
            BatTool.CallBatByThread(AutoToolConstants.BatDic["svnOP_Update"],this);
        }

        public override void OnReady()
        {
            stopwatch.Reset();
            stopwatch.Start();
        }

        public override void OnFinal()
        {
            stopwatch.Stop();
            _elapsed = stopwatch.Elapsed;
        }

        public override void OnStatusChanged(TaskStatus status)
        {
            _status = status;
            ATLog.Info("Task: " + Name + "   Status: " + status.ToString());
            if (_currentWindow != null)
            {
                // UnityEngine.Debug.Log("重绘窗口!");
                _currentWindow.Focus();
                _currentWindow.Repaint();
            }
        }
    }
}
