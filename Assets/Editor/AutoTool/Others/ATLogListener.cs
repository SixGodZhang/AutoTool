using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AutoTool
{
    class ATLogListener : TraceListener
    {
        private string m_logFileName;

        public ATLogListener()
        {
            string logPath = AutoToolConstants.logPath;
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            this.m_logFileName = logPath + "/" + string.Format("Log-{0}", DateTime.Now.ToString("yyyyMMdd"));
        }



        public override void Write(string message)
        {
            message = Format(message, "");
            File.AppendAllText(m_logFileName, message);
        }

        public override void WriteLine(string message)
        {
            message = Format(message, "");
            File.AppendAllText(m_logFileName, message);
        }

        public override void WriteLine(object o)
        {
            string message = Format(o, "");
            File.AppendAllText(m_logFileName, message);
        }

        public override void WriteLine(string message, string category)
        {
            message = Format(message, category);
            File.AppendAllText(m_logFileName, message);
        }

        public override void WriteLine(object o, string category)
        {
            string message = Format(o, category);
            File.AppendAllText(m_logFileName, message);
        }

        private string Format(object obj, string category)
        {
            StringBuilder message = new StringBuilder();
            message.AppendFormat("{0} ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            if (!string.IsNullOrEmpty(category))
            {
                message.AppendFormat("[{0}]", category);
            }

            if (obj is Exception)
            {
                var ex = obj as Exception;
                message.Append(ex.Message + "\r\n");
                message.Append(ex.StackTrace + "\r\n");
            }
            else
            {
                message.Append(obj.ToString() + "\r\n");
            }

            return message.ToString();
        }
    }
}
