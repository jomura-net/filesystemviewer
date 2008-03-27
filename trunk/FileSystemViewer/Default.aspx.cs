using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace FSViewer
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["s"]))
                {
                    SearchKey.Text = Request.QueryString["s"];
                }

                string currentDir = Request.QueryString["path"];
                if (!string.IsNullOrEmpty(currentDir))
                {
                    Label1.Text = currentDir;
                    this.Panel1.Enabled = true;
                }
                else
                {
                    Label1.Text = "[Top Directory]";
                }

                this.Header.Title = this.Label1.Text + " - FSViewer";
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            StringBuilder url = new StringBuilder(Request.Path);
            foreach (string key in Request.QueryString.Keys)
            {
                if ("s" != key)
                {
                    url.Append(url.ToString().IndexOf('?') == -1 ? "?" : "&");
                    url.Append(key);
                    url.Append("=");
                    url.Append(Uri.EscapeUriString(Request.QueryString[key]));
                }
            }
            url.Append(url.ToString().IndexOf('?') == -1 ? "?" : "&");
            url.Append("s=");
            url.Append(Uri.EscapeUriString(SearchKey.Text));

            string urlStr = url.ToString();
            Response.Redirect(urlStr, true);
        }
    }
}
