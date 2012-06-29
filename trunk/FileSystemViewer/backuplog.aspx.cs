//
// Backup archive Log Viewer
//
// @author Jomora ( kazuhiko@jomura.net http://jomura.net/ )
// @version 2012.06.29 Jomora created

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

public partial class backuplog : System.Web.UI.Page
{
    // ログファイルの指定
    private string logFile = "E:/@backup/backupArchive.log";

    // 表示を省略するログ
    protected Dictionary<string, int> omittedLogs = new Dictionary<string, int>();

    // 最大行数
    private int maxLineSize = 500;
    // 1行の最大文字数
    private int maxLength = 4096;

    // 出力するログ
    protected List<string> logs = new List<string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        omittedLogs.Add(" [notice] ", 0);

        // ファイルの読み込み
        string[] allLines = File.ReadAllLines(logFile, Encoding.GetEncoding("Shift_JIS"));

        // 表示を省略するかどうか判定して、logsに追加
        foreach (string buffer in allLines)
        {
            if (string.IsNullOrEmpty(buffer)) continue;
            bool canView = true;
            foreach (string omittedKey in omittedLogs.Keys)
            {
                if (canView && -1 != buffer.IndexOf(omittedKey))
                {
                    omittedLogs[omittedKey]++;
                    canView = false;
                }
            }

            if (canView)
            {
                if (buffer.Length > maxLength)
                {
                    logs.Add(buffer.Remove(maxLength));
                }
                else
                {
                    logs.Add(buffer);
                }
            }
        }

        // 最新のログから最大表示行数分だけ表示する
        if (maxLineSize < logs.Count) 
        {
            logs.RemoveRange(0, logs.Count - maxLineSize);
        }

        // 新しいものから表示
        logs.Reverse();

        // for debug
        for (int i = 0; i < logs.Count; i++)
        {
            Debug.WriteLine(i + " : " + logs[i]);
        }
    }
}
