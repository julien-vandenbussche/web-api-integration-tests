namespace Core.Specification.Tests.Testing
{
    using System.Collections.Generic;

    public sealed class Entity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<SubEntity> SubEntities { get; set; } = new HashSet<SubEntity>();
    }

    public sealed class SubEntity
    {
        public Entity Entity { get; set; }

        public int EntityId { get; set; }

        public int Id { get; set; }

        public ICollection<SubValue> Values { get; set; } = new HashSet<SubValue>();
    }

    public sealed class SubValue
    {
        public int Id { get; set; }

        public SubEntity SubEntity { get; set; }

        public int SubValueId { get; set; }
    }
}