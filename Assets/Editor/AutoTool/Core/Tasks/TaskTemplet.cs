using System;
using System.Diagnostics;
using UnityEditor;

namespace AutoTool
{
    class TaskTemplet : BuildTask<TaskTemplet>
    {
        public override string Name { get { return "任务模板"; } }

        public EditorWindow _currentWindow = null;
        private Stopwatch stopwatch = new Stopwatch();

        public TaskTemplet(EditorWindow window,int ID = 99999)
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
            //批处理不需要手动添加任务状态的改变
            //非批处理任务需手动添加任务状态的改变
            ATLog.Info("任务: " + Name + "Begin...");
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
