using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace FSViewer
{
    public static class FSAdapter
    {
        private static readonly string m_topDir = ConfigurationManager.AppSettings["rootDir"];

        public static FSInfoDS.FSInfoDTDataTable GetFSinfoList(string currentDir, string searchPattern)
        {
            //SearchOption   検索文字列がある->AllDirectories, ない->TopDirectoryOnly
            SearchOption sOption = SearchOption.TopDirectoryOnly;
            if ("*" != searchPattern) //ObjectDataSourceで指定したDefault値かどうか
            {
                sOption = SearchOption.AllDirectories;
                //searchPatternが単語のみの場合は、部分一致とする
                if (searchPattern.IndexOf('*') == -1) {
                    searchPattern = "*" + searchPattern + "*";
                }
            }

            string topDirPath = new DirectoryInfo(m_topDir).FullName;
            topDirPath += !topDirPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.CurrentCulture) ? Path.DirectorySeparatorChar.ToString() : string.Empty;
            string currentDirPath;
            //表示するフォルダの完全パス生成
            if (!string.IsNullOrEmpty(currentDir))
            {
                currentDirPath = Path.Combine(topDirPath, currentDir);
            }
            else
            {
                currentDirPath = topDirPath;
            }

            //画面表示するリスト
            FSInfoDS.FSInfoDTDataTable fstable = new FSInfoDS.FSInfoDTDataTable();

            // Forbidden
            if (!currentDirPath.StartsWith(topDirPath, StringComparison.CurrentCulture))
            {
                return fstable;
            }
            // Not Found
            if (!Directory.Exists(currentDirPath))
            {
                return fstable;
            }

            //フォルダ・ファイルの検索
            string[] dirpaths = Directory.GetDirectories(currentDirPath, searchPattern, sOption);
            string[] filepaths = Directory.GetFiles(currentDirPath, searchPattern, sOption);

            if (topDirPath != currentDirPath)
            {
                DirectoryInfo parentDirInfo = new DirectoryInfo(currentDirPath).Parent;

                FSInfoDS.FSInfoDTRow row = fstable.NewFSInfoDTRow();
                if (parentDirInfo.FullName.Length > topDirPath.Length)
                {
                    string path_query = parentDirInfo.FullName.Substring(topDirPath.Length);
                    row.Name = "<a href=\"?path=" + Uri.EscapeUriString(path_query) + "\">.. &nbsp; </a>";
                }
                else
                {
                    row.Name = "<a href=\"?\">.. &nbsp; </a>";
                }
                //row.Path = "";
                //row.Length = -1;
                row.LastWriteTime = parentDirInfo.LastWriteTime;

                fstable.AddFSInfoDTRow(row);
            }

            foreach (string dirpath in dirpaths)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirpath);
                if (FileAttributes.Hidden == (FileAttributes.Hidden & dirInfo.Attributes) 
                    || FileAttributes.System == (FileAttributes.System & dirInfo.Attributes)) continue;

                FSInfoDS.FSInfoDTRow row = fstable.NewFSInfoDTRow();
                string path_query = dirInfo.FullName.Substring(topDirPath.Length);
                row.Name = "<a href=\"?path=" + Uri.EscapeUriString(path_query) + "\">"
                    + HttpUtility.HtmlEncode(dirInfo.Name) + "</a>";
                row.Path = dirInfo.Parent.FullName.Substring(topDirPath.Length);
                //row.Length = -1;
                row.LastWriteTime = dirInfo.LastWriteTime;
                
                fstable.AddFSInfoDTRow(row);
            }
            foreach (string filepath in filepaths)
            {
                FileInfo fileInfo = new FileInfo(filepath);
                if (FileAttributes.Hidden == (FileAttributes.Hidden & fileInfo.Attributes)
                    || FileAttributes.System == (FileAttributes.System & fileInfo.Attributes)) continue;

                FSInfoDS.FSInfoDTRow row = fstable.NewFSInfoDTRow();
                row.Name = HttpUtility.HtmlEncode(fileInfo.Name);
                row.Path = fileInfo.DirectoryName.Substring(topDirPath.Length);
                row.Length = fileInfo.Length;
                row.LastWriteTime = fileInfo.LastWriteTime;
                fstable.AddFSInfoDTRow(row);
            }

            return fstable;
        }
    }
}
