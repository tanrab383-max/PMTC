using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class LichSuThaoTacMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.LichSuThaoTac>
    {
        public LichSuThaoTacMapping()
        {
            this.ToTable("LichSuThaoTac");
            this.HasKey(x => x.ID);
            this.Ignore(x => x.Id);
        }
    }
}
