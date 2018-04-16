Imports System.Text
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Xml
Imports System.Xml.Linq

'This class is used as a user-related data source. 
'It is required only for an online example
Public Class ExampleSessionStorage
    Private Shared _sessionStorage As String = "__SessionStorage__"
    Private _dataSource As XmlDataSource
    Private _xmlDocument As XmlDocument
    Public Shared ReadOnly Property Current() As ExampleSessionStorage
        Get
            Dim session As ExampleSessionStorage = TryCast(HttpContext.Current.Session(_sessionStorage), ExampleSessionStorage)
            If session Is Nothing Then
                session = New ExampleSessionStorage()
                HttpContext.Current.Session(_sessionStorage) = session
            End If
            Return session
        End Get
    End Property
    Private ReadOnly Property XmlDocument() As XmlDocument
        Get
            If _xmlDocument Is Nothing Then
                _xmlDocument = New XmlDocument()
                _xmlDocument.Load(HttpContext.Current.Server.MapPath("XmlData.xml"))
            End If
            Return _xmlDocument
        End Get
    End Property
    Public Sub UpdateXmlDocument(ByVal xDocument As XDocument)
        XmlDocument.LoadXml(GetXmlString(xDocument))
        _dataSource.Data = GetXmlString()
    End Sub
    Public Sub InitializeDataSource(ByVal dataSource As XmlDataSource)
        _dataSource = dataSource
        _dataSource.EnableCaching = False
        _dataSource.Data = GetXmlString()
    End Sub
    Private Function GetXmlString(ByVal xDocument As XDocument) As String
        Dim sb As New StringBuilder()
        Dim xw As XmlWriter = XmlWriter.Create(sb)
        xDocument.Save(xw)
        xw.Flush()
        Return sb.ToString()
    End Function
    Private Function GetXmlString() As String
        Dim sb As New StringBuilder()
        Dim xw As XmlWriter = XmlWriter.Create(sb)
        XmlDocument.WriteTo(xw)
        xw.Flush()
        Return sb.ToString()
    End Function
End Class