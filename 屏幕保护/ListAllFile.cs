using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace 屏幕保护
{
    class ListAllFile
    {
        private List<string> suffixList;//文件后缀 查找条件
        private List<string> filePathList;
        public ListAllFile(List<string> filePathList, List<string> suffixList)
        {
            this.filePathList = filePathList;
            this.suffixList = suffixList;
        }
        /// <summary>
        /// 开始遍历某一文件夹下的所有满足条件的文件
        /// </summary>
        public void StartFind(string sourceDir)
        {
            ListAllFiles(sourceDir);
        }
        //递归方法   
        private void ListAllFiles(string sourceDir)
        {
            try
            {
                foreach (string fileName in Directory.GetFiles(sourceDir))
                {
                    if (suffixList.Contains(Path.GetExtension(fileName).ToLower()))
                        filePathList.Add(fileName);
                }
                foreach (string subDir in Directory.GetDirectories(sourceDir))
                {

                    ListAllFiles(subDir);
                }
            }
            catch { return; }
            
        }
        public List<string> GetAllDrive()
        {
            List<string> driveList = new List<string>();
            try
            {  
                foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
                {
                    if (driveInfo.IsReady)
                        driveList.Add(driveInfo.RootDirectory.ToString());
                }
            }
            catch { }
            return driveList;
        }
    }
}
