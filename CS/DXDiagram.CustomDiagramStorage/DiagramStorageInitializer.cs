using DevExpress.Diagram.Core;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Diagram;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DXDiagram.CustomDiagramStorage {
    class DiagramStorageInitializer : DropCreateDatabaseIfModelChanges<DiagramStorage> {
        protected override void Seed(DiagramStorage storage) {
            base.Seed(storage);
            var diagram = new DiagramControl();
            for(int i = 0; i < 5; i++) {
                diagram.Items.Add(new DiagramShape() {
                    Position = new Point(200, 100 + i * 100),
                    Width = 100,
                    Height = 50,
                    Content = "Item " + (i + 1).ToString(),
                });
                if(i == 0)
                    continue;
                using(var stream = new MemoryStream()) {
                    diagram.SaveDocument(stream);
                    var diagramData = new DiagramData() {
                        Name = (i + 1).ToString() + " items",
                        Data = stream.ToArray(),
                    };
                    storage.DiagramData.Add(diagramData);
                }
            }
            storage.SaveChanges();
        }
    }
}
