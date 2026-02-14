using TNK.Core.Domain.View;
namespace TNK.Data.Mapping
{
    public partial class ViewDoiTacMapping : TNKEntityTypeConfiguration<ViewDoiTac>
    {
        public ViewDoiTacMapping()
        {
            this.ToTable("ViewDoiTac");
            this.Ignore(x => x.Id);
            this.HasKey(x => x.madoitac);
        }
    }
}
