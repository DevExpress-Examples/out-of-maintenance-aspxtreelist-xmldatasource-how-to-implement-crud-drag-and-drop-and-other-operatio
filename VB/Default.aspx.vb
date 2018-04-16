Imports System
Imports DevExpress.Web.ASPxTreeList
Imports DevExpress.Web.Data

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
        ExampleSessionStorage.Current.InitializeDataSource(XmlDataSource1)
        ASPxTreeList1.DataBind()
    End Sub
    Protected Sub ASPxTreeList1_NodeInserting(ByVal sender As Object, ByVal e As ASPxDataInsertingEventArgs)
        XmlHelper.InsertNode(TryCast(sender, ASPxTreeList), e)
        ASPxTreeList1.CancelEdit()
        e.Cancel = True
    End Sub
    Protected Sub ASPxTreeList1_NodeUpdating(ByVal sender As Object, ByVal e As ASPxDataUpdatingEventArgs)
        XmlHelper.UpdateNode(TryCast(sender, ASPxTreeList), e)
        ASPxTreeList1.CancelEdit()
        e.Cancel = True
    End Sub
    Protected Sub ASPxTreeList1_NodeDeleting(ByVal sender As Object, ByVal e As ASPxDataDeletingEventArgs)
        XmlHelper.DeleteNode(TryCast(sender, ASPxTreeList), e)
        e.Cancel = True
    End Sub
    Protected Sub ASPxTreeList1_ProcessDragNode(ByVal sender As Object, ByVal e As TreeListNodeDragEventArgs)
        XmlHelper.DragAndDropNode(TryCast(sender, ASPxTreeList), e)
        e.Handled = True
    End Sub
    Protected Sub ASPxTreeList1_BeforeGetCallbackResult(ByVal sender As Object, ByVal e As EventArgs)
        ASPxTreeList1.ExpandAll()
    End Sub
End Class
