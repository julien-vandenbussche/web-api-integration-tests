namespace Zoo.Park.Common.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class AnimalDetails
    {
        public abstract string Family { get; }

        public IList<string> Foods { get; set; } = Enumerable.Empty<string>().ToList();

        public int Id { get; set; }

        public int Legs { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
