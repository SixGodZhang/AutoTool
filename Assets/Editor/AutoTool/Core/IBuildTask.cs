using System;

namespace AutoTool
{
    public interface IBuildTask
    {
        string Name { get; }//名称
        int ID { set; get; }//ID
        TaskStatus Status { set; get; }//状态
        TimeSpan Elapsed { get; }//耗时
        bool IsCanReverse { get; }//任务是否可回滚
        void OnReverse();//回滚(当任务失败时执行回滚)
        void OnReady();
        void DoTask();//任务的主要逻辑
        void OnFinal();
        void OnStatusChanged(TaskStatus status);
    }
}
