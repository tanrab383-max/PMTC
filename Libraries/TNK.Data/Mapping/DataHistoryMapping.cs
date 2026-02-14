using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class DataHistoryMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.DataHistory>
    {
        public DataHistoryMapping()
        {
            this.ToTable("DataHistory");
        }
    }
}
