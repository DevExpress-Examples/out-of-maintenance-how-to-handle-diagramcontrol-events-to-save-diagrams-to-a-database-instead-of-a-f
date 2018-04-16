using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXDiagram.CustomDiagramStorage {
    public class DiagramStorage : DbContext {
        public DbSet<DiagramData> DiagramData { get; set; }
    }
    public class DiagramData {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
