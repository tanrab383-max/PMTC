using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ChiTietGopVonMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.ChiTietGopVon>
    {
        public ChiTietGopVonMapping()
        {
            this.ToTable("ChiTietGopVon");
        
        }
    }
}
