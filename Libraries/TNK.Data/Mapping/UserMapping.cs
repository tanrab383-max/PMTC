using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Data.Mapping
{
    public partial class UserMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.User>
    {
        //public UserMapping()
        //{
        //    this.ToTable("User");
        //    this.Ignore(x => x.UserId);
        //    this.HasKey(x => x.Id);
        //}
    }
}
