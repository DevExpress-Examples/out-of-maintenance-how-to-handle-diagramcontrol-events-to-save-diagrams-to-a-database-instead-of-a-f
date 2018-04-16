Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports DevExpress.Xpf.Core
Imports DevExpress.Diagram.Core
Imports DevExpress.Mvvm
Imports DevExpress.Mvvm.POCO
Imports System.IO

Namespace DXDiagram.CustomDiagramStorage
    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow
        Inherits DXWindow

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub OnLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dispatcher.BeginInvoke(New Action(AddressOf diagram.OpenFile))
        End Sub

        Private Sub OnShowingOpenDialog(ByVal sender As Object, ByVal e As DevExpress.Xpf.Diagram.DiagramShowingOpenDialogEventArgs)
            Dim viewModel = SelectDiagramViewModel.Create()
            Dim result = openDialogService.ShowDialog(MessageButton.OKCancel, "Choose a diagram to open", viewModel)
            If result = MessageResult.OK Then
                e.DocumentSourceToOpen = viewModel.SelectedName
            Else
                e.Cancel = True
            End If
        End Sub

        Private Sub OnCustomLoadDocument(ByVal sender As Object, ByVal e As DevExpress.Xpf.Diagram.DiagramCustomLoadDocumentEventArgs)
            If e.DocumentSource Is Nothing Then
                diagram.NewDocument()
                Return
            End If
            Dim storage = New DiagramStorage()
            Dim diagramInfo = storage.DiagramData.FirstOrDefault(Function(x) x.Name = CStr(e.DocumentSource))
            If diagramInfo IsNot Nothing Then
                diagram.LoadDocument(New MemoryStream(diagramInfo.Data))
            End If
            e.Handled = True
        End Sub

        Private Sub OnShowingSaveDialog(ByVal sender As Object, ByVal e As DevExpress.Xpf.Diagram.DiagramShowingSaveDialogEventArgs)
            Dim viewModel = SelectDiagramViewModel.Create()
            viewModel.SelectedName = CStr(diagram.DocumentSource)
            Dim result = saveDialogService.ShowDialog(MessageButton.OKCancel, "Choose a save location", viewModel)
            If result = MessageResult.OK Then
                e.DocumentSourceToSave = viewModel.SelectedName
            Else
                e.Cancel = True
            End If
        End Sub

        Private Sub OnCustomSaveDocument(ByVal sender As Object, ByVal e As DevExpress.Xpf.Diagram.DiagramCustomSaveDocumentEventArgs)
            Dim storage = New DiagramStorage()
            Dim diagramInfo = storage.DiagramData.FirstOrDefault(Function(x) x.Name = CStr(e.DocumentSource))
            If diagramInfo Is Nothing Then
                diagramInfo = New DiagramData() With {.Name = CStr(e.DocumentSource)}
                storage.DiagramData.Add(diagramInfo)
            End If
            Dim stream = New MemoryStream()
            diagram.SaveDocument(stream)
            diagramInfo.Data = stream.ToArray()
            storage.SaveChanges()
            e.Handled = True
        End Sub
    End Class
    Public Class SelectDiagramViewModel
        Public Shared Function Create() As SelectDiagramViewModel
            Return ViewModelSource.Create(Function() New SelectDiagramViewModel())
        End Function
        Protected Sub New()
            Dim storage = New DiagramStorage()
            Names = storage.DiagramData.Select(Function(x) x.Name).ToArray()
            SelectedName = Names.FirstOrDefault()
        End Sub
        Private privateNames As String()
        Public Property Names() As String()
            Get
                Return privateNames
            End Get
            Private Set(ByVal value As String())
                privateNames = value
            End Set
        End Property
        Public Overridable Property SelectedName() As String
    End Class
End Namespace
