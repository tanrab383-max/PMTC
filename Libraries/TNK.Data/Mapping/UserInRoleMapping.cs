using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class UserInRoleMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.UserInRole>
    {
        public UserInRoleMapping()
        {
            this.ToTable("UserInRole");
        }
    }
}
