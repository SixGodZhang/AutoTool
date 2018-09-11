using System;
using System.Diagnostics;
using UnityEditor;

namespace AutoTool
{
    class TestFailureTask : BuildTask<TestFailureTask>
    {
        public override string Name { get { return "测试任务(失败情况)"; } }

        public EditorWindow _currentWindow = null;
        private Stopwatch stopwatch = new Stopwatch();

        public TestFailureTask(EditorWindow window, int ID = 20000)
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
            ATLog.Log("任务: " + Name + "Begin...");
            System.Threading.Thread.Sleep(5000);

            _status = TaskStatus.Failure;
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
            ATLog.Log("Task: " + Name + "   Status: " + status.ToString());
            if (_currentWindow != null)
            {
                _currentWindow.Repaint();
            }
        }
    }
}
