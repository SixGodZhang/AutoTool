using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AutoTool
{
    enum ATBuildPiplineStatus
    {
        Unoccupied,//未占用
        Occupied,//占用
    }

    class ATBuildPipline
    {
        private static ATBuildPipline _instance = null;
        public static ATBuildPipline Instance {
            get {
                if (_instance == null)
                    _instance = new ATBuildPipline();
                return _instance;
            }
        }

        public ATBuildPiplineStatus PiplineStatus = ATBuildPiplineStatus.Unoccupied;//任务管线状态

        //所有的任务
        public Queue<IBuildTask> Tasks = new Queue<IBuildTask>();
        //当前执行的任务ID
        //private int currentTaskID = -1;

        public IBuildTask _currentTask = null;
        public IBuildTask CurrentTask {
            get {
                return _currentTask;
            }
        }

        //记录管线中上一次任务的执行状态
        private TaskStatus LastTask = TaskStatus.None;

        /// <summary>
        /// 向任务管线中增加任务
        /// </summary>
        /// <param name="task"></param>
        public ATBuildPipline AddBuildTask(IBuildTask task)
        {
            if (Tasks == null)
            {
                Tasks = new Queue<IBuildTask>();
            }

            Tasks.Enqueue(task);

            return _instance;
        }

        /// <summary>
        /// 清空管线中的任务
        /// </summary>
        /// <returns></returns>
        public void ClearBuildTasks()
        {
            Tasks.Clear();
            LastTask = TaskStatus.None;
        }

        private DateTime  _lastTime = new DateTime();
        private DateTime _currentTime = new DateTime();

        /// <summary>
        /// 任务管线的执行函数
        /// </summary>
        public void Run()
        {
            //任务管线中无任务队列
            if (Tasks.Count == 0)
            {
                //EditorUtility.DisplayDialog("提示", "任务管线中无任务队列!", "OK");
                EndATBuildPipline();//重置相关属性
                return;
            }

            //上次执行任务如果失败，直接返回，管线终止
            if (LastTask == TaskStatus.Failure)
            {
                EndATBuildPipline();//重置相关属性
                return;
            }

            PiplineStatus = ATBuildPiplineStatus.Occupied;
            if (Tasks.Count > 0)
            {
                IBuildTask currentTask = Tasks.Peek();

                switch (currentTask.Status)
                {
                    case TaskStatus.None:
                        {//只执行一次
                            currentTask.Status = TaskStatus.Start;
                            _currentTask = currentTask;
                            currentTask.OnStatusChanged(currentTask.Status);
                            _lastTime = DateTime.Now;
                        }
                        break;
                    case TaskStatus.Start:
                        {//开始任务
                            if (OnRePaintWindow())
                            {//此处为了Repaint
                                currentTask.Status = TaskStatus.Running;
                                currentTask.OnReady();
                                currentTask.DoTask();
                            }
                        }
                        break;
                    case TaskStatus.Running:
                        {//和刷新的帧数一样

                        }
                        break;
                    case TaskStatus.Success:
                        {
                            //TODO
                            currentTask.OnFinal();
                            currentTask = null;

                            Tasks.Dequeue();
                            _currentTask = null;
                            _currentTime = new DateTime();
                            _lastTime = new DateTime();
                        }
                        break;
                    case TaskStatus.Failure:
                        {
                            //TODO
                            //任务失败
                            currentTask.OnFinal();
                            currentTask = null;

                            LastTask = TaskStatus.Failure;
                            _currentTime = new DateTime();
                            _lastTime = new DateTime();
                        }
                        break;
                }
                
            }

            
        }

        private bool OnRePaintWindow()
        {
            _currentTime = DateTime.Now;
            //TODO
            //此任务正在进行
            //牺牲1.5秒换取界面的刷新
            if (_currentTime.Subtract(_lastTime).TotalMilliseconds > 1500)
            {
                _lastTime = new DateTime();
                _currentTime = new DateTime();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 任务管线结束
        /// </summary>
        public void EndATBuildPipline()
        {
            PiplineStatus = ATBuildPiplineStatus.Unoccupied;
            BuildPiplineWindow.Instance.isExcuteATBuildPipline = false;
        }
    }
}
