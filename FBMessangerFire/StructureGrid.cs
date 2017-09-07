using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBMessangerFire
{
    public abstract class StructureGrid
    {
        public string Entity { get; set; }
        public DateTime CreateAt { get; set; }
    }
    public class FiledGrid : StructureGrid
    {
        public string Errors { get; set; }
    }
    public class SucsessGrid : StructureGrid
    {
        public string Message { get; set; }
    }
}
