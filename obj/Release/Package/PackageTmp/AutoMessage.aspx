<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoMessage.aspx.cs" Inherits="BirthdayCard.AutoMessage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Button ID="OnClick" runat="server" BackColor="Red" ForeColor="White" Height="100px" OnClick="OnClick_Click" Text="启动线程" Width="100px" />
    </form>
</body>
</html>
