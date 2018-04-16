using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using DevExpress.Diagram.Core;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System.IO;

namespace DXDiagram.CustomDiagramStorage {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DXWindow {
        public MainWindow() {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke(new Action(diagram.OpenFile));
        }

        private void OnShowingOpenDialog(object sender, DevExpress.Xpf.Diagram.DiagramShowingOpenDialogEventArgs e) {
            var viewModel = SelectDiagramViewModel.Create();
            var result = openDialogService.ShowDialog(MessageButton.OKCancel, "Choose a diagram to open", viewModel);
            if(result == MessageResult.OK)
                e.DocumentSourceToOpen = viewModel.SelectedName;
            else
                e.Cancel = true;
        }

        private void OnCustomLoadDocument(object sender, DevExpress.Xpf.Diagram.DiagramCustomLoadDocumentEventArgs e) {
            if(e.DocumentSource == null) {
                diagram.NewDocument();
                return;
            }
            var storage = new DiagramStorage();
            var diagramInfo = storage.DiagramData.FirstOrDefault(x => x.Name == (string)e.DocumentSource);
            if(diagramInfo != null)
                diagram.LoadDocument(new MemoryStream(diagramInfo.Data));
            e.Handled = true;
        }

        private void OnShowingSaveDialog(object sender, DevExpress.Xpf.Diagram.DiagramShowingSaveDialogEventArgs e) {
            var viewModel = SelectDiagramViewModel.Create();
            viewModel.SelectedName = (string)diagram.DocumentSource;
            var result = saveDialogService.ShowDialog(MessageButton.OKCancel, "Choose a save location", viewModel);
            if(result == MessageResult.OK)
                e.DocumentSourceToSave = viewModel.SelectedName;
            else
                e.Cancel = true;
        }

        private void OnCustomSaveDocument(object sender, DevExpress.Xpf.Diagram.DiagramCustomSaveDocumentEventArgs e) {
            var storage = new DiagramStorage();
            var diagramInfo = storage.DiagramData.FirstOrDefault(x => x.Name == (string)e.DocumentSource);
            if(diagramInfo == null) {
                diagramInfo = new DiagramData() { Name = (string)e.DocumentSource };
                storage.DiagramData.Add(diagramInfo);
            }
            var stream = new MemoryStream();
            diagram.SaveDocument(stream);
            diagramInfo.Data = stream.ToArray();
            storage.SaveChanges();
            e.Handled = true;
        }
    }
    public class SelectDiagramViewModel {
        public static SelectDiagramViewModel Create() {
            return ViewModelSource.Create(() => new SelectDiagramViewModel());
        }
        protected SelectDiagramViewModel() {
            var storage = new DiagramStorage();
            Names = storage.DiagramData.Select(x => x.Name).ToArray();
            SelectedName = Names.FirstOrDefault();
        }
        public string[] Names { get; private set; }
        public virtual string SelectedName { get; set; }
    }
}
