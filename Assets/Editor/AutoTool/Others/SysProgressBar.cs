using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace AutoTool
{
    class SysProgressBar
    {
        /// <summary>
        /// 显示进度条
        /// </summary>
        /// <param name="current">当前进度</param>
        /// <param name="total">总进度</param>
        /// <param name="taskName">任务名字</param>
        public static void ShowProgressBar(float current, float total = 100.0f, string taskName = "请稍后...")
        {
            float rate = current / total;
            if (rate >= 1)
            {
                EditorUtility.ClearProgressBar();
                return;
            }
            EditorUtility.DisplayProgressBar("任务进度", taskName, current / total);
        }

        /// <summary>
        /// 关闭进度条
        /// </summary>
        [MenuItem("Editor/ClearProgressBar")]
        public static void ClearProgressBar()
        {
            EditorUtility.ClearProgressBar();
        }
    }
}


