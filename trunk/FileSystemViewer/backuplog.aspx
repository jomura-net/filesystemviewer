<%@ Page Language="C#" AutoEventWireup="true" CodeFile="backuplog.aspx.cs" Inherits="backuplog" %>
<%
    if ("rss" == Request.Params["format"])
    {
	    // RSS
	    Response.ContentType = "text/xml; charset=UTF-8";
        string linkUrl = Request.Url.ToString().Substring(0, Request.Url.ToString().Length - Request.Url.Query.Length);
%><?xml version="1.0" encoding="UTF-8" ?>
<rss version="2.0">
   <channel>
      <title>backup archive</title>
      <link><%= linkUrl%></link>
      <description>
<%
	    // 表示を省略したログのカウント
	    foreach (System.Collections.Generic.KeyValuePair<string, int> omitted in omittedLogs)
        {
            Response.Write(string.Format("{0, 5}", omitted.Value) + " " + HttpUtility.HtmlEncode(omitted.Key) + "<br />\n");
	    }
%>      </description>
<%
        System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
	    foreach (string log in logs)
        {
		    string title = log.Substring(20);
            string pubDate = DateTime.Parse(log.Substring(0, 18)).ToString("ddd, dd MMM yyyy HH:mm:ss zzz", ci);
            string guid = DateTime.Parse(log.Substring(0, 18)).ToString("yyyy-MM-ddTHH:mm:sszzz", ci);
%>      <item>
         <title><%= HttpUtility.HtmlEncode(title) %></title>
         <link><%= HttpUtility.HtmlEncode(linkUrl) %></link>
         <pubDate><%= pubDate %></pubDate>
         <guid isPermaLink="false"><%= guid %></guid>
      </item>
<%
        }
%>   </channel>
</rss>
<%
    } else {
        // TEXT
        Response.ContentType = "text/plain; charset=UTF-8";
        // 表示を省略したログのカウント
        foreach (System.Collections.Generic.KeyValuePair<string, int> omitted in omittedLogs)
        {
            Response.Write(string.Format("{0, 5}", omitted.Value) + " " + omitted.Key + "\n");
        }
        Response.Write("\n");
        foreach (string log in logs)
        {
            Response.Write(log + "\n");
        }
    }
%>
