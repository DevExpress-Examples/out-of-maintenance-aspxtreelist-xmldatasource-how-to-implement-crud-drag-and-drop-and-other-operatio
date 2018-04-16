using System;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web.Data;

public partial class _Default : System.Web.UI.Page {
	protected void Page_Init(object sender, EventArgs e) {
		ExampleSessionStorage.Current.InitializeDataSource(XmlDataSource1);
		ASPxTreeList1.DataBind();
	}
	protected void ASPxTreeList1_NodeInserting(object sender, ASPxDataInsertingEventArgs e) {
		XmlHelper.InsertNode(sender as ASPxTreeList, e);
		ASPxTreeList1.CancelEdit();
		e.Cancel = true;
	}
	protected void ASPxTreeList1_NodeUpdating(object sender, ASPxDataUpdatingEventArgs e) {
		XmlHelper.UpdateNode(sender as ASPxTreeList, e);
		ASPxTreeList1.CancelEdit();
		e.Cancel = true;
	}
	protected void ASPxTreeList1_NodeDeleting(object sender, ASPxDataDeletingEventArgs e) {
		XmlHelper.DeleteNode(sender as ASPxTreeList, e);
		e.Cancel = true;
	}
	protected void ASPxTreeList1_ProcessDragNode(object sender, TreeListNodeDragEventArgs e) {
		XmlHelper.DragAndDropNode(sender as ASPxTreeList, e);
		e.Handled = true;
	}
	protected void ASPxTreeList1_BeforeGetCallbackResult(object sender, EventArgs e) {
		ASPxTreeList1.ExpandAll();
	}
}
