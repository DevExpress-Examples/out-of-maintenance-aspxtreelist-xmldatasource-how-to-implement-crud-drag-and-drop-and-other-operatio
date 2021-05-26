<!-- default file list -->
*Files to look at*:

* [ExampleSessionStorage.cs](./CS/App_Code/ExampleSessionStorage.cs) (VB: [ExampleSessionStorage.vb](./VB/App_Code/ExampleSessionStorage.vb))
* [XmlHelper.cs](./CS/App_Code/XmlHelper.cs) (VB: [XmlHelper.vb](./VB/App_Code/XmlHelper.vb))
* [Default.aspx](./CS/Default.aspx) (VB: [Default.aspx](./VB/Default.aspx))
* [Default.aspx.cs](./CS/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/Default.aspx.vb))
<!-- default file list end -->
# ASPxTreeList - XmlDataSource - How to implement CRUD, drag-and-drop and other operations
<!-- run online -->
**[[Run Online]](https://codecentral.devexpress.com/t367481/)**
<!-- run online end -->


<p>This example describes how to implement <em>CRUD,</em> <em>drag-and-drop </em>and <em>sorting</em> operations with updating an <strong>Xml</strong> file in <strong>ASPxTreeList</strong> bound to <strong>XmlDataSource. XmlDataSource</strong> doesn't support the <strong>CRUD</strong> and <strong>drag-and-drop</strong> operations out of the box. So, corresponding custom operations are implemented. Sorting of an <strong>Xml</strong> file also requires custom implementation.<br>To complete the task, follow the next steps:<br>1) To implement <strong>CRUD</strong> and <strong>drag-and-drop,</strong> handle the <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxTreeListASPxTreeList_NodeInsertingtopic">NodeInserting</a>, <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxTreeListASPxTreeList_NodeUpdatingtopic">NodeUpdating</a>, <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxTreeListASPxTreeList_NodeDeletingtopic">NodeDeleting</a> and <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxTreeListASPxTreeList_ProcessDragNodetopic">ProcessDragNode</a> events. Node names are based at the nodes' levels. It is necessary to manually change a node name after the <strong>drag-and-grop</strong> operation has been performed.<br>2) To change an <strong>Xml</strong> file, use the <a href="https://msdn.microsoft.com/en-us/library/system.xml.xmldocument(v=vs.110).aspx">XmlDocument</a> class. To find a single node, use the <a href="https://www.w3schools.com/xml/xpath_syntax.asp">XPath</a> expression.<br>3) To implement sorting with updating an <strong>Xml</strong> file, it is necessary to update the source after every change of existing data. So, perform node sorting and data updating after every <strong>CRUD</strong> and <strong>drag-and-drop</strong> operation. The ascending sorting by the <em>ItemNumber</em> attribute of the <strong>Xml</strong> node is implemented in the example.<br><br></p>
<p><strong><em>Note:</em></strong><br>In the example, a custom <strong>ExampleSessionStorage</strong> class is used as temporary in-memory data storage to prevent changing of the original data file. This class is not required in a real application.</p>

<br/>


