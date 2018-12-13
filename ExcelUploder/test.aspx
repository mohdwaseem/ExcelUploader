<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="ExcelUploder.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>Select File</td>
                <td><asp:FileUpload ID="FileUpload1" runat="server" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" OnClick="Button1_Click" runat="server" Text="Upload" />
                </td>
                <td>
                     <asp:Button ID="Button2" OnClick="Button2_Click" runat="server" Text="Save" />
                     <asp:Button ID="Button3" OnClick="Button3_Click" runat="server" Text="Cancel" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
