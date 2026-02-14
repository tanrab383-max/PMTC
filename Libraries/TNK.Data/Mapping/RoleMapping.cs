using TNK.Core;
namespace TNK.Data.Mapping
{
    public partial class RoleMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.Role>
    {
        public RoleMapping()
        {
            this.ToTable("Roles");

        }
    }
}
