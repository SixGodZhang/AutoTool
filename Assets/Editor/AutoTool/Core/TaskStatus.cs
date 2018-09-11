using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTool
{
    public enum TaskStatus
    {
        None,//设置UI上任务状态(仅执行一次)
        Start,//开始任务(仅执行一次)
        Running,//任务正在执行(执行多次)
        Success,//任务执行成功(仅执行一次)
        Failure//任务执行失败(仅执行一次)
    }
}
