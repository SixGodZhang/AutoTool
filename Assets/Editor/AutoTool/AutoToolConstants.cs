using System.Collections.Generic;

namespace AutoTool
{
    public enum TaskChain
    {
        默认任务链                   = 20000,
        资源打包                     = 20001,
        APK出包                      = 20002,
        测试任务链                   = 20003,
    }

    public enum BuildChannel
    {
        应用宝                       = 30001,
        华为                         = 30002,
        vivo                         = 30003,
        oppo                         = 30004,
    }

    public enum BuildPlatform
    {
        Android                      = 40001,
        IOS                          = 40002,
        PC                           = 40003,
    }

    //此处采用结构体的原因
    //1.结构体存储在堆栈中，查询速度快
    //2.struct.属性 = 属性,而枚举 enum.枚举值.ToString()
    //3.字典虽然查询速度快，但是在代码中的使用不方便修改(即键值变化，每一处都需要修改)
    public struct AutoToolPrefKeys
    {
        public static string isSettingBatVariable
        {
            get
            {
                return "isSettingBatVariable";
            }
        }

        public static string UnityEXE
        {
            get
            {
                return "UnityEXE";
            }
        }
    }

    public class AutoToolConstants
    {
        public static string UnityVersion = "4.7.2f1";
        public static string logPath = System.Environment.CurrentDirectory + "/Logs";

        #region Bat脚本 路径
        public static string BatRoot = System.Environment.CurrentDirectory + "\\AutoTools";
        public static Dictionary<string, string> BatDic = new Dictionary<string, string>
        {

        };
        #endregion
    }
}