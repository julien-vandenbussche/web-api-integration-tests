namespace Zoo.Infrastructure.Entities.Parameters
{
    public partial class Configuration
    {
        public enum DbContextType
        {
            Blue,
            Green
        }

        public const string WritableKey = "WRITABLE";
    }
}