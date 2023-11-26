<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="AzureStorageWeb._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script language="javascript" type="text/javascript">
        $(document).ready(function ()
        {
            $("[ID$='Date']").datepicker();
            $("[ID$='FileUpload']").css("width", "425px");
        });

    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="Upload">
        <table>
            <tr valign="top">
                <td>Description:</td>
                <td>
                    <asp:TextBox runat="server" ID="Description" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                </td>
            </tr>
            <tr valign="top">
                <td>Date:</td>
                <td>
                    <asp:TextBox runat="server" ID="Date"></asp:TextBox>
                </td>
            </tr>
            <tr valign="top">
                <td>Select file:</td>
                <td><asp:FileUpload ID="FileUpload" runat="server" /></td>
            </tr>
        </table>

        <asp:Button runat="server" ID="SendMessage" OnClick="OnUpload" Text="Upload"/>
    </div>
</asp:Content>
