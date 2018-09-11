using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEditor;

namespace AutoTool
{
    public interface IBuildTask
    {
        string Name { get; }//名称
        int ID { set; get; }//ID
        TaskStatus Status { set; get; }//状态
        TimeSpan Elapsed { get; }//耗时
        void OnReady();
        void DoTask();//任务的主要逻辑
        void OnFinal();
        void OnStatusChanged(TaskStatus status);
    }
}
