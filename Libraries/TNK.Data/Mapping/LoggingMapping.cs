using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class LoggingMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.Logging>
    {
        public LoggingMapping()
        {
            this.ToTable("Logging");
            this.HasKey(x => x.ID);
            this.Ignore(x => x.Id);
        }
    }
}
