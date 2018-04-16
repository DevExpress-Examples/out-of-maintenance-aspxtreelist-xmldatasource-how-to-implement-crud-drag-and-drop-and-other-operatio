using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

//This class is used as a user-related data source. 
//It is required only for an online example
public class ExampleSessionStorage {
	private static string _sessionStorage = "__SessionStorage__";
	XmlDataSource _dataSource;
	private XmlDocument _xmlDocument;	
	public static ExampleSessionStorage Current {
		get {
			ExampleSessionStorage session = HttpContext.Current.Session[_sessionStorage] as ExampleSessionStorage;
			if(session == null) {
				session = new ExampleSessionStorage();
				HttpContext.Current.Session[_sessionStorage] = session;
			}
			return session;
		}
	}
	private XmlDocument XmlDocument {
		get {
			if(_xmlDocument == null) {
				_xmlDocument = new XmlDocument();
				_xmlDocument.Load(HttpContext.Current.Server.MapPath("XmlData.xml"));
			}
			return _xmlDocument;
		}
	}
	public void UpdateXmlDocument(XDocument xDocument) {
		XmlDocument.LoadXml(GetXmlString(xDocument));
		_dataSource.Data = GetXmlString();
	}
	public void InitializeDataSource(XmlDataSource dataSource) {
		_dataSource = dataSource;
		_dataSource.EnableCaching = false;
		_dataSource.Data = GetXmlString();
	}
	string GetXmlString(XDocument xDocument) {
		StringBuilder sb = new StringBuilder();
		XmlWriter xw = XmlWriter.Create(sb);
		xDocument.Save(xw);
		xw.Flush();
		return sb.ToString();
	}
	string GetXmlString() {
		StringBuilder sb = new StringBuilder();
		XmlWriter xw = XmlWriter.Create(sb);
		XmlDocument.WriteTo(xw);
		xw.Flush();
		return sb.ToString();
	}
}