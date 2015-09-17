<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Final.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="userTbx" runat="server">Username</asp:TextBox>
        <br />
    
    </div>
        <asp:TextBox ID="passTbx" runat="server" TextMode="Password">Password</asp:TextBox>
        <p>
            <asp:Button ID="loginBT" runat="server" Text="Login" OnClick="loginBT_Click" />
        </p>
        <p>
            <asp:Label ID="loginTx" runat="server"></asp:Label>
        </p>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
        <asp:TextBox ID="createUserTbx" runat="server">Username</asp:TextBox>
        <p>
            <asp:TextBox ID="createpassTbx" runat="server" TextMode="Password">Password</asp:TextBox>
        </p>
        <p>
            <asp:Button ID="Button1" runat="server" Text="Create User" OnClick="Button1_Click" />
        </p>
    </form>
</body>
</html>
