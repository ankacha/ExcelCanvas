using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace CanvasTest.Models
{
    public class ConnectionModel
    {

        public Guid SourceNodeId { get; set; }
        public string SourcePort { get; set; }  // "RightConnection"
        public Guid TargetNodeId { get; set; }
        public string TargetPort { get; set; }  // "LeftConnection"

        // Optional: Store the actual line UI element


    }
}
