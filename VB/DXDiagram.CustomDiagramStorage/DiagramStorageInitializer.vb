Imports DevExpress.Diagram.Core
Imports DevExpress.Xpf.Core
Imports DevExpress.Xpf.Diagram
Imports System
Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows

Namespace DXDiagram.CustomDiagramStorage
    Friend Class DiagramStorageInitializer
        Inherits DropCreateDatabaseIfModelChanges(Of DiagramStorage)

        Protected Overrides Sub Seed(ByVal storage As DiagramStorage)
            MyBase.Seed(storage)
            Dim diagram = New DiagramControl()
            For i As Integer = 0 To 4
                diagram.Items.Add(New DiagramShape() With {.Position = New Point(200, 100 + i * 100), .Width = 100, .Height = 50, .Content = "Item " & (i + 1).ToString()})
                If i = 0 Then
                    Continue For
                End If
                Using stream = New MemoryStream()
                    diagram.SaveDocument(stream)
                    Dim diagramData = New DiagramData() With {.Name = (i + 1).ToString() & " items", .Data = stream.ToArray()}
                    storage.DiagramData.Add(diagramData)
                End Using
            Next i
            storage.SaveChanges()
        End Sub
    End Class
End Namespace
