<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v15.2, Version=15.2.20.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v15.2, Version=15.2.20.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>ASPxTreeList with CRUD, drag-and-drop and ascending sorting</title>
</head>
<body>
	<form id="form1" runat="server">
		<div>			
			<asp:XmlDataSource ID="XmlDataSource1" runat="server">
			</asp:XmlDataSource>
			<dx:ASPxTreeList ID="ASPxTreeList1" KeyFieldName="Id" ClientInstanceName="ASPxTreeList1" AutoGenerateColumns="False" runat="server"
				DataSourceID="XmlDataSource1" OnNodeInserting="ASPxTreeList1_NodeInserting"
				OnNodeUpdating="ASPxTreeList1_NodeUpdating" OnNodeDeleting="ASPxTreeList1_NodeDeleting"
				OnProcessDragNode="ASPxTreeList1_ProcessDragNode" OnBeforeGetCallbackResult="ASPxTreeList1_BeforeGetCallbackResult">
				<SettingsBehavior AllowFocusedNode="True" AutoExpandAllNodes="True" />
				<SettingsEditing AllowNodeDragDrop="true" />
				<Columns>
					<dx:TreeListTextColumn FieldName="ItemNumber" VisibleIndex="0" Visible="true">
					</dx:TreeListTextColumn>
					<dx:TreeListTextColumn FieldName="Id" VisibleIndex="1" Visible="false" ReadOnly="true">
					</dx:TreeListTextColumn>
					<dx:TreeListTextColumn FieldName="Description" VisibleIndex="3" Visible="true">
					</dx:TreeListTextColumn>
					<dx:TreeListCommandColumn VisibleIndex="4">
						<EditButton Visible="True">
						</EditButton>
						<NewButton Visible="True">
						</NewButton>
						<DeleteButton Visible="True">
						</DeleteButton>
					</dx:TreeListCommandColumn>
				</Columns>
			</dx:ASPxTreeList>
		</div>
	</form>
</body>
</html>
