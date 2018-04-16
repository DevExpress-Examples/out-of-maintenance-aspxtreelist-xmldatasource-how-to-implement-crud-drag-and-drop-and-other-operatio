using DevExpress.Web.ASPxTreeList;
using DevExpress.Web.Data;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

public static class XmlHelper {
	public static void InsertNode(ASPxTreeList treeList, ASPxDataInsertingEventArgs e) {

		XmlDataSource xmlDataXource = treeList.Page.FindControl(treeList.DataSourceID) as XmlDataSource;
		XmlDocument docum = xmlDataXource.GetXmlDocument();

		AddNewNodeId(e.NewValues, docum);

		var parentNodeInTreeList = treeList.FindNodeByKeyValue(treeList.NewNodeParentKey);
		var parentID = parentNodeInTreeList.GetValue("Id").ToString();
		XmlNode parentNode = FindNode(docum, parentID);

		var level = Convert.ToInt32(parentNode.Attributes["Level"].Value) + 1;
		XmlNode newNode = docum.CreateElement("Level" + level + "Node");

		foreach(DictionaryEntry entry in e.NewValues) {
			newNode.Attributes.Append(GetNewAttribute(docum, entry.Key, entry.Value));
		}

		newNode.Attributes.Append(GetNewAttribute(docum, "Level", level));
		newNode.Attributes.Append(GetNewAttribute(docum, "ParentKey", parentID));

		parentNode.AppendChild(newNode);
		RearrangeAndSaveXml(docum);
	}
	public static void UpdateNode(ASPxTreeList treeList, ASPxDataUpdatingEventArgs e) {
		AddHiddenNodeValues(e.NewValues, e.Keys[0], treeList);

		XmlDataSource xmlDataXource = treeList.Page.FindControl(treeList.DataSourceID) as XmlDataSource;
		XmlDocument docum = xmlDataXource.GetXmlDocument();

		XmlNode node = FindNode(docum, e.NewValues["Id"]);

		foreach(DictionaryEntry entry in e.NewValues) {
			if(entry.Key.ToString() != "Id") {
				node.Attributes[entry.Key.ToString()].Value = entry.Value.ToString();
			}
		}
		RearrangeAndSaveXml(docum);
	}
	public static void DeleteNode(ASPxTreeList treeList, ASPxDataDeletingEventArgs e) {
		AddHiddenNodeValues(e.Values, e.Keys[0], treeList);

		XmlDataSource xmlDataXource = treeList.Page.FindControl(treeList.DataSourceID) as XmlDataSource;
		XmlDocument docum = xmlDataXource.GetXmlDocument();

		XmlNode node = FindNode(docum, e.Values["Id"]);
		node.ParentNode.RemoveChild(node);

		RearrangeAndSaveXml(docum);
	}
	public static void DragAndDropNode(ASPxTreeList treeList, TreeListNodeDragEventArgs e) {
		XmlDataSource xmlDataXource = treeList.Page.FindControl(treeList.DataSourceID) as XmlDataSource;
		XmlDocument docum = xmlDataXource.GetXmlDocument();

		XmlNode oldChildNode = FindNode(docum, e.Node.GetValue("Id"));
		XmlNode parentNode = FindNode(docum, e.NewParentNode.GetValue("Id"));

		ReplaceNode(docum, parentNode, oldChildNode);
		RearrangeAndSaveXml(docum);
	}
	static void ReplaceNode(XmlDocument docum, XmlNode newParentNode, XmlNode oldParentChildNode) {
		var level = Convert.ToInt32(newParentNode.Attributes["Level"].Value) + 1;
		XmlNode newChildNode = docum.CreateElement("Level" + level + "Node");

		foreach(XmlAttribute att in oldParentChildNode.Attributes) {
			object value = null;
			switch(att.Name) {
				case "ParentKey":
					value = newParentNode.Attributes["Id"].Value;
					break;
				case "Level":
					value = level;
					break;
				default:
					value = att.Value;
					break;
			}
			newChildNode.Attributes.Append(GetNewAttribute(docum, att.Name, value));
		}
		newParentNode.AppendChild(newChildNode);

		while(oldParentChildNode.ChildNodes.Count > 0) {
			XmlNode node = oldParentChildNode.FirstChild;
			ReplaceNode(docum, newChildNode, node);
		}
		oldParentChildNode.ParentNode.RemoveChild(oldParentChildNode);
	}
	static void RearrangeAndSaveXml(XmlDocument docum) {

		XDocument xDoc = XDocument.Parse(docum.OuterXml);
		if(xDoc.Root != null)
			SortXml(xDoc.Root);

		//Saving into temporary in-memory storage. For this online example only
		ExampleSessionStorage.Current.UpdateXmlDocument(xDoc);

		//Uncomment the following line for saving into a local file
		//xDoc.Save(Server.MapPath("XmlData.xml"));
	}
	static void SortXml(XContainer parent) {
		var elements = parent.Elements()
			.OrderBy(e => e.Name.LocalName)
			.ThenBy(e => (string)e.Attribute("ItemNumber"))
			.ToArray();

		Array.ForEach(elements, e => e.Remove());

		foreach(var element in elements) {
			parent.Add(element);
			SortXml(element);
		}
	}
	static int GetNextID(XmlNode n) {
		int lastId = GetLastID(n, -1);
		return ++lastId;
	}
	static int GetLastID(XmlNode n, int lastID) {
		foreach(XmlElement node in n.ChildNodes) {
			if(node.HasChildNodes == true)
				lastID = GetLastID(node, lastID);
			var currentID = Convert.ToInt32(node.Attributes["Id"].Value);
			lastID = (lastID < currentID) ? currentID : lastID;
		}
		return lastID;
	}
	static XmlAttribute GetNewAttribute(XmlDocument docum, object name, object value) {
		return GetNewAttribute(docum, name.ToString(), value.ToString());
	}
	static XmlAttribute GetNewAttribute(XmlDocument docum, string name, string value) {
		XmlAttribute att = docum.CreateAttribute(name);
		att.Value = value;
		return att;
	}
	static XmlNode FindNode(XmlDocument docum, object nodeID) {
		return FindNode(docum, nodeID.ToString());
	}
	static XmlNode FindNode(XmlDocument docum, string nodeID) {
		return docum.SelectSingleNode(string.Format("//*[@Id='{0}']", nodeID));
	}
	static void AddHiddenNodeValues(OrderedDictionary values, object key, ASPxTreeList treeList) {
		TreeListNode node = treeList.FindNodeByKeyValue(key.ToString());
		values["Id"] = node.GetValue("Id");
	}
	static void AddNewNodeId(OrderedDictionary values, XmlDocument docum) {
		XmlNode n = docum.SelectSingleNode("Root");
		values["Id"] = GetNextID(n);
	}
}