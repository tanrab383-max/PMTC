using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class ViewREPAIR_ORDER_PYSMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.View.ViewREPAIR_ORDER_PYS>
    {
        public ViewREPAIR_ORDER_PYSMapping()
        {
            this.ToTable("ViewREPAIR_ORDER_PYS");
            this.HasKey(x => x.DETAIL_ID);
            this.Ignore(x => x.Id);
        }
    }
}
