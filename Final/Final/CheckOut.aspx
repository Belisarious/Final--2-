<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckOut.aspx.cs" Inherits="Final.CheckOut" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" DataKeyNames="BookID" HorizontalAlign="Center">
            <Columns>
                <asp:BoundField DataField="BookID" HeaderText="BookID" InsertVisible="False" ReadOnly="True" SortExpression="BookID" />
                <asp:BoundField DataField="BookName" HeaderText="BookName" SortExpression="BookName" />
                <asp:BoundField DataField="Author" HeaderText="Author" SortExpression="Author" />
                <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
            </Columns>
        </asp:GridView>
    
    </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BookStore %>" ProviderName="<%$ ConnectionStrings:BookStore.ProviderName %>" SelectCommand="SELECT * FROM [Books]"></asp:SqlDataSource>
        <br />
        <br />
        <asp:DropDownList ID="BookList" runat="server" DataSourceID="SqlDataSource2" DataTextField="BookName" DataValueField="BookID">
        </asp:DropDownList>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="quantityBox" runat="server" ></asp:TextBox>
        <br />
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:BookStore %>" ProviderName="<%$ ConnectionStrings:BookStore.ProviderName %>" SelectCommand="SELECT [BookName], [BookID] FROM [Books] WHERE ([Quantity] &gt; ?)">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="Quantity" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="AddBt" runat="server" OnClick="AddBt_Click" Text="Add" Width="74px" />
        <br />
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="CheckOutBt" runat="server" OnClick="CheckOutBt_Click" Text="Check Out" Width="94px" />
        <br />
        <br />
        <asp:GridView ID="CheckOutView" runat="server" HorizontalAlign="Center" style="margin-left: 0px">
        </asp:GridView>
        <br />
        <p style="margin-left: 280px">
            <asp:Label ID="CheckOutLB" runat="server"></asp:Label>
        </p>
    </form>
</body>
</html>
