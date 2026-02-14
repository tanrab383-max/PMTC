using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class LoginMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.Login>
    {
        public LoginMapping()
        {
            this.ToTable("LichSuLogin");
            this.HasKey(x => x.ID);
            this.Ignore(x => x.Id);
        }
    }
}
