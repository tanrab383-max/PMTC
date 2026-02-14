using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;

namespace TNK.Data.Mapping
{
    public partial class ActionLogMapping : TNKEntityTypeConfiguration<ActionLog>
    {
        public ActionLogMapping()
        {
            this.ToTable("ActionLog");
            this.HasKey(x => x.ID);
            this.Ignore(x => x.Id);
        }
    }
}
