namespace Zoo.Infrastructure.Entities.Zoo
{
    using Microsoft.EntityFrameworkCore;

    using Store;

    public sealed class ZooContextBlue : ZooContext
    {
        public ZooContextBlue(DbContextOptions options)
            : base(options)
        {
        }
    }
    
    public sealed class ZooContextGreen : ZooContext
    {
        public ZooContextGreen(DbContextOptions options)
            : base(options)
        {
        }
    }
    
    public partial class ZooContext : IDbContext
    {
        protected ZooContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
