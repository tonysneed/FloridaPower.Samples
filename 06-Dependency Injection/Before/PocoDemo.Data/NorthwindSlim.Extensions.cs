using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PocoDemo.Data
{
    public partial class NorthwindSlim
    {
        partial void Initialize()
        {
            // Explicitly disable dynamic proxy generation
            Configuration.ProxyCreationEnabled = false;

            // Instruct Code First to use an existing database
            Database.SetInitializer(new NullDatabaseInitializer<NorthwindSlim>());
        }

        partial void ModelCreating(DbModelBuilder modelBuilder)
        {
            // Remove the pluralizing table name convention
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
