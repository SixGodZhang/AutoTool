using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AutoTool
{
    class FileHelper
    {
        /// <summary>
        /// 打开Window系统文件浏览器
        /// </summary>
        /// <param name="filePath"> 文件路径必须为右斜杠 </param>
        public static void ShowExplorerWindow(string filePath)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
            //路径采用双斜杠，否则找不到目录
            string target = filePath.Replace("/", "\\");
            if (!File.Exists(target))
            {
                EditorUtility.DisplayDialog("提示", "找不到文件!", "OK");
                return;
            }
            info.Arguments = "/select," + target;
            System.Diagnostics.Process.Start(info);
        }



        /// <summary>
        /// 写内容到文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string WriteToFile(string filePath, string content, FileMode mode = FileMode.OpenOrCreate)
        {
            string result = null;
            try
            {
                FileStream fs = new FileStream(filePath, mode, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(content);
                sw.Close();
            }
            catch (Exception ex)
            {
                result = ex.Message;
                ATLog.Error(ex);
                return result;
            }

            return result;
        }

        /// <summary>
        /// 剪切文件夹(区别于复制文件夹)
        /// 注意:不会将路径的目录移动过去
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="tPath"></param>
        public static void MoveFolder(string sPath, string tPath)
        {
            if (Directory.Exists(sPath))
            {
                try
                {
                    if (Directory.Exists(tPath))
                    {
                        RemoveFullFolder(tPath);

                        if (Directory.Exists(tPath))
                        {
                            DirectoryInfo directoryInfo = new DirectoryInfo(tPath);
                            if (directoryInfo.Attributes != FileAttributes.Normal)
                            {
                                directoryInfo.Attributes = FileAttributes.Normal;
                            }
                            directoryInfo.Delete();
                            //Directory.Delete(path);
                        }
                    }

                    Directory.Move(sPath, tPath);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }
            else
            {
                ATLog.Error("路径不存在! \r\n" + sPath);
                Debug.LogError(sPath + " : Not Found");
            }
        }

        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="path"></param>
        public static void RemoveFile(string path)
        {
            if (File.Exists(path)) {
                File.Delete(path);
            }
        }

        /// <summary>
        /// 移除文件夹及其子文件
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public static void RemoveFullFolder(string path)
        {
            path = path.Replace("\\", "/");

            if (!Directory.Exists(path))
            {
                ATLog.Warn("文件夹不存在! \r\n " + path);
                return;
            }

            List<string> files = new List<string>(Directory.GetFiles(path));
            files.ForEach(c =>
            {
                string tempFileName = Path.Combine(path, Path.GetFileName(c));
                FileInfo fileInfo = new FileInfo(tempFileName);
                if (fileInfo.Attributes != FileAttributes.Normal) {
                    fileInfo.Attributes = FileAttributes.Normal;
                }
                fileInfo.Delete();
                //File.Delete(tempFileName);
            });

            List<string> folders = new List<string>(Directory.GetDirectories(path));
            folders.ForEach(c =>
            {
                string tempFolderName = Path.Combine(path, Path.GetFileName(c));
                RemoveFullFolder(tempFolderName);
            });

            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                if (directoryInfo.Attributes != FileAttributes.Normal) {
                    directoryInfo.Attributes = FileAttributes.Normal;
                }
                directoryInfo.Delete();
                //Directory.Delete(path);
                return;
            }
        }


        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        public static void CopyFile(string sourcePath, string destPath, bool isrewrite = true)
        {
            sourcePath = sourcePath.Replace("\\", "/");
            destPath = destPath.Replace("\\","/");

            try {
                if (File.Exists(destPath))
                {
                    File.Delete(destPath);
                }

                System.IO.File.Copy(sourcePath, destPath, isrewrite);
            }
            catch (Exception ex) {
                Debug.LogError(ex.Message);
                Debug.LogError(ex.StackTrace);
            }
            
        }

        /// <summary>
        /// 复制文件夹及其子目录、文件到目标文件夹
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        public static void CopyFolder(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))
                {
                    //目标目录不存在则创建
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("create target folder fail...：" + ex.Message);
                    }
                }

                List<string> files = new List<string>(Directory.GetFiles(sourcePath));
                files.ForEach(c =>
                {
                    string destFile = Path.Combine(destPath, Path.GetFileName(c));
                    File.Copy(c, destFile, true);
                });

                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
                folders.ForEach(c =>
                {
                    string destDir = Path.Combine(destPath, Path.GetFileName(c));
                    CopyFolder(c, destDir);
                });
            }
            else
            {
                throw new DirectoryNotFoundException("sourcePath: " + "source folder not found！");
            }
        }
    }
}

