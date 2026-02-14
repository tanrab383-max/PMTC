using TNK.Core;

namespace TNK.Data.Mapping
{
    public partial class MenuMapping : TNKEntityTypeConfiguration<TNK.Core.Domain.Menu>
    {
        public MenuMapping()
        {
            this.ToTable("Menu");

        }
    }
}
