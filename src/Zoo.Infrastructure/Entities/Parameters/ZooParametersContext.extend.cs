namespace Zoo.Infrastructure.Entities.Parameters
{
    using Microsoft.EntityFrameworkCore;

    using Store;

    internal interface IZooParametersContext : IDbContext
    {
    }
    
    public partial class ZooParametersContext : IZooParametersContext
    {
        protected ZooParametersContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
