<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tuto.aspx.cs" Inherits="WebTest.Tuto" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Tuto</title>
    <style>
        body {
            background-color: #000;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 20px;
            padding: 25px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblDebug" runat="server" ForeColor="#00ff00"></asp:Label>
        </div>
    </form>
</body>
</html>
