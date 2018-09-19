using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace AutoTool
{
    public abstract class BuildTask<T> : IBuildTask
    {
        public virtual string Name
        {
            get {
                return "DefaultTask";
            }
        }

        public abstract int ID { get; set; }
        public abstract TaskStatus Status { get; set; }

        public abstract TimeSpan Elapsed { get; }

        public virtual bool IsCanReverse { get { return false; } }

        public abstract void DoTask();
        public abstract void OnReady();
        public abstract void OnFinal();
        public virtual void OnReverse() { }

        public abstract void OnStatusChanged(TaskStatus status);

        //public Action OnTaskEnter;//任务开始
        //public Action<TaskStatus> OnTaskEnd;//任务结束
    }
}
