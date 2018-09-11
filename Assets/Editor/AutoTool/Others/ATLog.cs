

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AutoTool
{
    class ATLog
    {
        public static string autoToolLogPath = System.Environment.CurrentDirectory + "/autotoolLog.log";
        public static bool isPrintLog = true;//打印Log开关

        /// <summary>
        /// 清除Log日志
        /// </summary>
        public static void ClearLog()
        {
            string target = autoToolLogPath.Replace("/", "\\");
            try
            {
                if (File.Exists(target))
                {
                    File.Delete(target);
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            finally
            {

            }

            EditorUtility.DisplayDialog("提示", "Log清理完成!", "OK");
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="content"></param>
        public static void Log(string content)
        {
            FileStream fs = null;
            try
            {
                if (!File.Exists(autoToolLogPath))
                {
                    fs = File.Create(autoToolLogPath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("创建Log文件失败!");
                Debug.LogError(ex.Message); 
                EditorUtility.DisplayDialog("错误", "创建Log文件失败!", "OK");
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

            FileHelper.WriteToFile(autoToolLogPath, System.DateTime.UtcNow + " : " + content, FileMode.Append);
        }



        /// <summary>
        /// 根据Log编码,输出到Log日志
        /// </summary>
        /// <param name="errorCode">Log编码</param>
        public static void Log(ErrorCode errorCode)
        {
            if (!isPrintLog)
            {
                Debug.LogError("已经关闭Log日志!");
                return;
            }

            FileStream fs = null;
            try
            {
                if (!File.Exists(autoToolLogPath))
                {
                    fs = File.Create(autoToolLogPath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("创建Log文件失败!");
                Debug.LogError(ex.Message);
                EditorUtility.DisplayDialog("错误", "创建Log文件失败!", "OK");
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

            FileHelper.WriteToFile(autoToolLogPath, System.DateTime.Now + " : " + AutoToolConstants.ErrorCodeDict[errorCode], FileMode.Append);
        }
    }
}
