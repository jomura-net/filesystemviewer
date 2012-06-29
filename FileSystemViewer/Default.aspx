<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FSViewer.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title> </title>
    <link rel="alternate" type="application/rss+xml" title="RSS" href="http://jomura.net/FSViewer/backuplog.aspx?format=rss" />
    <style type="text/css">
    a {text-decoration:none;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width:98%;"><tr><td>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </td><td align="right">
        <asp:Panel ID="Panel1" runat="server" DefaultButton="SearchButton" Wrap="False" style="text-align:right;" Enabled="False">
            <asp:TextBox ID="SearchKey" runat="server"></asp:TextBox>
            <asp:Button ID="SearchButton" runat="server" OnClick="SearchButton_Click" Text="Search" />
        </asp:Panel>
        </td></tr></table>
    </div>
    <div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" EnableViewState="False" CellPadding="4" ForeColor="#333333" AllowSorting="True" >
            <Columns>
                <asp:BoundField DataField="LastWriteTime" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}"
                    HeaderText="LastWriteTime" HtmlEncode="False" SortExpression="LastWriteTime">
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="Length" HeaderText="Length" SortExpression="Length" NullDisplayText="&amp;lt;dir&amp;gt;">
                    <ItemStyle Wrap="False" HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Name" HeaderText="Name" HtmlEncode="False" SortExpression="Name">
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="Path" HeaderText="Path" SortExpression="Path">
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#E3EAEB" />
            <EditRowStyle BackColor="#7C6F57" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <br />
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetFSinfoList"
            TypeName="FSViewer.FSAdapter">
            <SelectParameters>
                <asp:QueryStringParameter Name="currentDir" QueryStringField="path"
                    Type="String" DefaultValue="" />
                <asp:QueryStringParameter DefaultValue="*" Name="searchPattern" QueryStringField="s"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    
    </div>
    </form>
</body>
</html>
