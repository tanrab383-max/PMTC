using System.Data.Entity.ModelConfiguration;

namespace TNK.Data.Mapping
{
    public abstract class TNKEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected TNKEntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {

        }
    }
}
