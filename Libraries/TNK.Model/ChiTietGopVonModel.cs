using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Model
{
    public class ChiTietGopVonModel
    {
        public DotGopVon DGV { get; set; }
        public List<TNK.Core.Domain.View.ViewDoiTac> DT { get; set; }
        public List<ChiTietGopVon> CTGV { get; set; }
        public ChiTietGopVon Item { get; set; }

        public List<DotGopVon> LstDGV { get; set; }
    }
}
