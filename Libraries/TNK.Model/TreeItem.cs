using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Model
{
    public class TreeItem
    {
        public string id { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public bool children { get; set; }
        public string type { get; set; }
    }
}
