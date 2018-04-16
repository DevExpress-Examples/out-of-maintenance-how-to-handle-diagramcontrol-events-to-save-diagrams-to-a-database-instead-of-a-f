Imports System
Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace DXDiagram.CustomDiagramStorage
    Public Class DiagramStorage
        Inherits DbContext

        Public Property DiagramData() As DbSet(Of DiagramData)
    End Class
    Public Class DiagramData
        Public Property Id() As Integer
        Public Property Name() As String
        Public Property Data() As Byte()
    End Class
End Namespace
