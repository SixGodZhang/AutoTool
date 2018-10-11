#define TRACE

using System.Diagnostics;
using System.IO;

namespace AutoTool
{
    class ATLog
    {
        static ATLog()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ATLogListener());
        }

        private static bool m_openLog = true;
        /// <summary>
        /// Log开关
        /// </summary>
        public static bool OpenLog
        {
            get { return m_openLog; }
            set { m_openLog = value; }
        }

        public static void Debug(object msg)
        {
            Trace.WriteLineIf(m_openLog, msg, "Debug");
        }

        public static void Error(object msg)
        {
            Trace.WriteLineIf(m_openLog, msg, "Error");
        }

        public static void Info(object msg)
        {
            Trace.WriteLineIf(m_openLog, msg, "Info");
        }

        public static void Warn(object msg)
        {
            Trace.WriteLineIf(m_openLog, msg, "Warn");
        }

        public static void ClearLog()
        {
            if (Directory.Exists(AutoToolConstants.logPath))
            {
                string[] logsPath = Directory.GetFiles(AutoToolConstants.logPath);
                for (int i = 0; i < logsPath.Length; i++)
                {
                    File.Delete(logsPath[i]);
                }
            }
        }

        [UnityEditor.MenuItem("Editor/Log测试")]
        public static void LogTest()
        {
            UnityEngine.Debug.LogError(AutoToolConstants.logPath);

            ATLog.Debug("this is debug log !");
            ATLog.Warn("this is warn log !");
            ATLog.Error("this is error log !");
            ATLog.Info("this is info log !");
        }
    }
}
