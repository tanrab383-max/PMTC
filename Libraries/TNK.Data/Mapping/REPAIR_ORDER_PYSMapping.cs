using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class REPAIR_ORDER_PYSMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.REPAIR_ORDER_PYS>
    {
        public REPAIR_ORDER_PYSMapping()
        {
            this.ToTable("REPAIR_ORDER_PYS");
            this.HasKey(x => x.SequenceId);
            this.Ignore(x => x.Id);
        }
    }
}
