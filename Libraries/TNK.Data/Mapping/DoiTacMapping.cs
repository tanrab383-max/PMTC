using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class DoiTacMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.DoiTac>
    {
        public DoiTacMapping()
        {
            this.ToTable("Doi_Tac");
            this.HasKey(x => x.MaDoiTac);
            this.Ignore(x => x.Id);
        }
    }
}
