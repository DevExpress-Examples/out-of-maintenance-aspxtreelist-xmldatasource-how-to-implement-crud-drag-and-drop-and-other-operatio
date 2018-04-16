Imports DevExpress.Web.ASPxTreeList
Imports DevExpress.Web.Data
Imports System
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Linq
Imports System.Web.UI.WebControls
Imports System.Xml
Imports System.Xml.Linq

Public NotInheritable Class XmlHelper

    Private Sub New()
    End Sub

    Public Shared Sub InsertNode(ByVal treeList As ASPxTreeList, ByVal e As ASPxDataInsertingEventArgs)

        Dim xmlDataXource As XmlDataSource = TryCast(treeList.Page.FindControl(treeList.DataSourceID), XmlDataSource)
        Dim docum As XmlDocument = xmlDataXource.GetXmlDocument()

        AddNewNodeId(e.NewValues, docum)

        Dim parentNodeInTreeList = treeList.FindNodeByKeyValue(treeList.NewNodeParentKey)
        Dim parentID = parentNodeInTreeList.GetValue("Id").ToString()
        Dim parentNode As XmlNode = FindNode(docum, parentID)

        Dim level = Convert.ToInt32(parentNode.Attributes("Level").Value) + 1
        Dim newNode As XmlNode = docum.CreateElement("Level" & level & "Node")

        For Each entry As DictionaryEntry In e.NewValues
            newNode.Attributes.Append(GetNewAttribute(docum, entry.Key, entry.Value))
        Next entry

        newNode.Attributes.Append(GetNewAttribute(docum, "Level", level))
        newNode.Attributes.Append(GetNewAttribute(docum, "ParentKey", parentID))

        parentNode.AppendChild(newNode)
        RearrangeAndSaveXml(docum)
    End Sub
    Public Shared Sub UpdateNode(ByVal treeList As ASPxTreeList, ByVal e As ASPxDataUpdatingEventArgs)
        AddHiddenNodeValues(e.NewValues, e.Keys(0), treeList)

        Dim xmlDataXource As XmlDataSource = TryCast(treeList.Page.FindControl(treeList.DataSourceID), XmlDataSource)
        Dim docum As XmlDocument = xmlDataXource.GetXmlDocument()

        Dim node As XmlNode = FindNode(docum, e.NewValues("Id"))

        For Each entry As DictionaryEntry In e.NewValues
            If entry.Key.ToString() <> "Id" Then
                node.Attributes(entry.Key.ToString()).Value = entry.Value.ToString()
            End If
        Next entry
        RearrangeAndSaveXml(docum)
    End Sub
    Public Shared Sub DeleteNode(ByVal treeList As ASPxTreeList, ByVal e As ASPxDataDeletingEventArgs)
        AddHiddenNodeValues(e.Values, e.Keys(0), treeList)

        Dim xmlDataXource As XmlDataSource = TryCast(treeList.Page.FindControl(treeList.DataSourceID), XmlDataSource)
        Dim docum As XmlDocument = xmlDataXource.GetXmlDocument()

        Dim node As XmlNode = FindNode(docum, e.Values("Id"))
        node.ParentNode.RemoveChild(node)

        RearrangeAndSaveXml(docum)
    End Sub
    Public Shared Sub DragAndDropNode(ByVal treeList As ASPxTreeList, ByVal e As TreeListNodeDragEventArgs)
        Dim xmlDataXource As XmlDataSource = TryCast(treeList.Page.FindControl(treeList.DataSourceID), XmlDataSource)
        Dim docum As XmlDocument = xmlDataXource.GetXmlDocument()

        Dim oldChildNode As XmlNode = FindNode(docum, e.Node.GetValue("Id"))
        Dim parentNode As XmlNode = FindNode(docum, e.NewParentNode.GetValue("Id"))

        ReplaceNode(docum, parentNode, oldChildNode)
        RearrangeAndSaveXml(docum)
    End Sub
    Private Shared Sub ReplaceNode(ByVal docum As XmlDocument, ByVal newParentNode As XmlNode, ByVal oldParentChildNode As XmlNode)
        Dim level = Convert.ToInt32(newParentNode.Attributes("Level").Value) + 1
        Dim newChildNode As XmlNode = docum.CreateElement("Level" & level & "Node")

        For Each att As XmlAttribute In oldParentChildNode.Attributes
            Dim value As Object = Nothing
            Select Case att.Name
                Case "ParentKey"
                    value = newParentNode.Attributes("Id").Value
                Case "Level"
                    value = level
                Case Else
                    value = att.Value
            End Select
            newChildNode.Attributes.Append(GetNewAttribute(docum, att.Name, value))
        Next att
        newParentNode.AppendChild(newChildNode)

        Do While oldParentChildNode.ChildNodes.Count > 0
            Dim node As XmlNode = oldParentChildNode.FirstChild
            ReplaceNode(docum, newChildNode, node)
        Loop
        oldParentChildNode.ParentNode.RemoveChild(oldParentChildNode)
    End Sub
    Private Shared Sub RearrangeAndSaveXml(ByVal docum As XmlDocument)

        Dim xDoc As XDocument = XDocument.Parse(docum.OuterXml)
        If xDoc.Root IsNot Nothing Then
            SortXml(xDoc.Root)
        End If

        'Saving into temporary in-memory storage. For this online example only
        ExampleSessionStorage.Current.UpdateXmlDocument(xDoc)

        'Uncomment the following line for saving into a local file
        'xDoc.Save(Server.MapPath("XmlData.xml"));
    End Sub
    Private Shared Sub SortXml(ByVal parent As XContainer)
        Dim elements = parent.Elements().OrderBy(Function(e) e.Name.LocalName).ThenBy(Function(e) CStr(e.Attribute("ItemNumber"))).ToArray()

		Array.ForEach(elements, Sub(e) e.Remove())

		For Each element In elements
            parent.Add(element)
            SortXml(element)
        Next element
    End Sub
    Private Shared Function GetNextID(ByVal n As XmlNode) As Integer
        Dim lastId As Integer = GetLastID(n, -1)
        lastId += 1
        Return lastId
    End Function
    Private Shared Function GetLastID(ByVal n As XmlNode, ByVal lastID As Integer) As Integer
        For Each node As XmlElement In n.ChildNodes
            If node.HasChildNodes = True Then
                lastID = GetLastID(node, lastID)
            End If
            Dim currentID = Convert.ToInt32(node.Attributes("Id").Value)
            lastID = If(lastID < currentID, currentID, lastID)
        Next node
        Return lastID
    End Function
    Private Shared Function GetNewAttribute(ByVal docum As XmlDocument, ByVal name As Object, ByVal value As Object) As XmlAttribute
        Return GetNewAttribute(docum, name.ToString(), value.ToString())
    End Function
    Private Shared Function GetNewAttribute(ByVal docum As XmlDocument, ByVal name As String, ByVal value As String) As XmlAttribute
        Dim att As XmlAttribute = docum.CreateAttribute(name)
        att.Value = value
        Return att
    End Function
    Private Shared Function FindNode(ByVal docum As XmlDocument, ByVal nodeID As Object) As XmlNode
        Return FindNode(docum, nodeID.ToString())
    End Function
    Private Shared Function FindNode(ByVal docum As XmlDocument, ByVal nodeID As String) As XmlNode
        Return docum.SelectSingleNode(String.Format("//*[@Id='{0}']", nodeID))
    End Function
    Private Shared Sub AddHiddenNodeValues(ByVal values As OrderedDictionary, ByVal key As Object, ByVal treeList As ASPxTreeList)
        Dim node As TreeListNode = treeList.FindNodeByKeyValue(key.ToString())
        values("Id") = node.GetValue("Id")
    End Sub
    Private Shared Sub AddNewNodeId(ByVal values As OrderedDictionary, ByVal docum As XmlDocument)
        Dim n As XmlNode = docum.SelectSingleNode("Root")
        values("Id") = GetNextID(n)
    End Sub
End Class