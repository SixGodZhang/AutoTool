using System.Collections.Generic;

namespace AutoTool
{
    public enum ErrorCode
    {
        NONE                         = 10000,
        ConfigBatError               = 10001,
        NotFoundUnityEXE             = 10002,
        CloseLog                     = 10003,
        RemoveFolderFail             = 10004,
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

        public static string[] BuildChannel ={
            "应用宝",
            "华为",
            "vivo",
            "oppo"
        };

        public static string[] BuildPlatform ={
            "Android",
            "IOS",
            "PC"
        };

        public static Dictionary<ErrorCode, string> ErrorCodeDict = new Dictionary<ErrorCode, string>
        {
            { ErrorCode.NONE,"默认Error,测试使用!" },
            { ErrorCode.ConfigBatError,"配置脚本出错!"},
            { ErrorCode.NotFoundUnityEXE,"未找到相应版本的Unity!"},
            { ErrorCode.CloseLog,"关闭Log!"},
            { ErrorCode.RemoveFolderFail,"删除文件夹失败!"},
        };

        #region Bat脚本 路径
        public static string BatRoot = System.Environment.CurrentDirectory + "\\AutoTools";
        public static Dictionary<string, string> BatDic = new Dictionary<string, string>
        {

        };
        #endregion
    }
}