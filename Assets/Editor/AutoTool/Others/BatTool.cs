using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace AutoTool
{
    enum BatResult
    {
        None,
        Success,//成功
        Failure//失败
    }

    class BatTool
    {
        public static string normalLogFile = System.IO.Directory.GetCurrentDirectory() + "/BatNormalLog.log";
        public static string errorLogFile = System.IO.Directory.GetCurrentDirectory() + "/BatErrorLog.log";
        public static bool batExcuteResult = true;//批处理执行结果

        static BatTool()
        {
            if (!File.Exists(normalLogFile))
            {
                File.Create(normalLogFile);
            }

            if (!File.Exists(errorLogFile))
            {
                File.Create(errorLogFile);
            }
        }

        /// <summary>
        /// 多线程处理批处理任务(此方法专为任务管线定制而成)
        /// 目的:在主线程中避免进行耗时操作
        /// </summary>
        /// <param name="batPath"></param>
        public static void CallBatByThread<T>(string batPath, BuildTask<T> task)
        {
            Func<string, BatResult> callBatThread = CallBatFunc;
            callBatThread.BeginInvoke(batPath, (ar) => {
                BatResult result = callBatThread.EndInvoke(ar);
                //UnityEngine.Debug.LogError("CallBatByThread result : " + result);
                switch (result)
                {
                    case BatResult.Success:
                        task.Status = TaskStatus.Success;
                        break;
                    case BatResult.Failure:
                        task.Status = TaskStatus.Failure;
                        break;
                }
            }, callBatThread);
        }

        private static BatResult CallBatFunc(string batPath)
        {
            return CallBat(batPath) ? BatResult.Success : BatResult.Failure;
        }

        //[UnityEditor.MenuItem("Editor/测试_批处理错误情况")]
        //public static void CallBatTestErrorLevel()
        //{
        //    UnityEngine.Debug.LogError("bat command result: " + CallBat(AutoToolConstants.BatDic["svnOP_Update"]));
        //    UnityEngine.Debug.LogError("end...");
        //}

        /// <summary>
        /// 调用批处理
        /// </summary>
        /// <param name="batPath">Bat路径</param>
        public static bool CallBat(string batPath)
        {
            //Initialize
            batExcuteResult = true;
              
            
            Thread t1 = new Thread(new ParameterizedThreadStart(ReadOutput));
            t1.IsBackground = true;
            Thread t2 = new Thread(new ParameterizedThreadStart(ReadError));
            t1.IsBackground = true;

            using (Process pro = new Process())
            {
                FileInfo file = new FileInfo(batPath);
                pro.StartInfo.WorkingDirectory = file.Directory.FullName;
                pro.StartInfo.FileName = batPath;
                pro.StartInfo.CreateNoWindow = true;
                pro.StartInfo.RedirectStandardOutput = true;
                pro.StartInfo.RedirectStandardError = true;
                pro.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("gb2312");
                pro.StartInfo.StandardErrorEncoding = Encoding.GetEncoding("gb2312");
                pro.StartInfo.UseShellExecute = false;

                pro.Start();
                t1.Start(pro);
                t2.Start(pro);

                pro.WaitForExit();
                if (pro.HasExited)
                {
                    pro.Close();
                }

                return batExcuteResult;
            }
        }

        /// <summary>
        /// 读取错误输出流
        /// </summary>
        /// <param name="data"></param>
        public static void ReadError(object data)
        {
            Process temp = null;
            if (data is Process)
            {
                temp = data as Process;
            }
            else
            {
                return;
            }

            StringBuilder errorLog = new StringBuilder();
            string outputStr = string.Empty;
            while ((outputStr = temp.StandardError.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(outputStr.Trim()))
                {
                    batExcuteResult = false;
                }

                errorLog.Append("e: " + outputStr + "\n");
            }

            FileHelper.WriteToFile(errorLogFile, "errorLog: \n DateTime: " + DateTime.Now + "\n" + errorLog, FileMode.Append);
        }

        /// <summary>
        /// 读取标准输出流
        /// </summary>
        /// <param name="data"></param>
        public static void ReadOutput(object data)
        {
            Process temp = null;
            if (data is Process)
            {
                temp = data as Process;
            }
            else
            {
                return;
            }

            StringBuilder integralNormalLog = new StringBuilder();
            string standardOutputStr = string.Empty;
            while ((standardOutputStr = temp.StandardOutput.ReadLine()) != null)
            {
                if (standardOutputStr.Contains("errorlevel="))
                {
                    //string tp = standardOutputStr.Substring(standardOutputStr.IndexOf("=") + 1, 1);
                    //UnityEngine.Debug.LogError("tp: " + tp);

                    if ("0".Equals(standardOutputStr.Substring(standardOutputStr.IndexOf("=") + 1, 1)))
                    {
                        batExcuteResult = true;
                    }
                    else
                    {
                        batExcuteResult = false;
                    }
                }

                integralNormalLog.Append("d: " + standardOutputStr + "\n");
            }

            FileHelper.WriteToFile(normalLogFile, "normalLog: \n DateTime: " + DateTime.Now + "\n" + integralNormalLog, FileMode.Append);
        }

        /// <summary>
        /// 创建批处理
        /// </summary>
        /// <param name="bat">批处理内容</param>
        /// <returns></returns>
        public static bool GenerateBat(string batPath, string bat)
        {
            //if (!File.Exists(batPath) && File.Create(batPath) != null)
            //{
            //    UnityEngine.Debug.Log("Create CallArtBat.bat Success....");
            //}
            //else if(File.Exists(batPath))
            //{
            //    UnityEngine.Debug.Log("Bat exist...");
            //    return true;
            //}

            string resultMsg = FileHelper.WriteToFile(batPath, bat);

            if (!string.IsNullOrEmpty(resultMsg))
            {
                UnityEngine.Debug.Log(resultMsg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 转换字符串编码UTF8
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string get_uft8(string str)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(str);
            String decodedString = utf8.GetString(encodedBytes);
            return decodedString;
        }

        /// <summary>
        /// 转换字符串编码ASCII
        /// </summary>
        /// <param name="str">ascii</param>
        /// <returns></returns>
        public static string get_ascii(string str)
        {
            byte[] Buff = System.Text.Encoding.ASCII.GetBytes(str);
            string retStr = System.Text.Encoding.Default.GetString(Buff, 0, Buff.Length);

            return retStr;
        }
    }

}
