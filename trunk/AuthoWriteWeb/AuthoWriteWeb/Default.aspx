<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AuthoWriteWeb._Default" %>

<%@ Register Assembly="RichTextEditor" Namespace="AjaxControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AuthoWriteWeb</title>
</head>
<body>
    <form id="edit_form" runat="server" >
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="AuthoWriteWebService.asmx" />
        </Services>
    </asp:ScriptManager>

    <script type="text/javascript">
        function updateResults(){
        CopyText();
        editor = top.document.getElementById(editorId);
        AuthoWriteWeb.AuthoWriteWebService.StructuredQuery(editor.value, OnSuccess, OnFailed,null);
        return(true);
        }
       // Sys.Application.add_load(function(){updateResults();});
       
       function OnSuccess(args){
       divUpdate.innerHTML = args;
       }
       function OnFailed(args){
       alert('The Web Service has failed'+args);
       }
       
    </script>

    <table>
        <tr>
            <td>
                <cc1:RichTextEditor ID="Rte1" Theme="Blue" runat="server" TabIndex="0" index="1" />
            </td>
            <td>
                <input id="btnUpdate" type="button" value="Check " onclick="updateResults();" />
            </td>
            <td>
                <div id="divUpdate" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
