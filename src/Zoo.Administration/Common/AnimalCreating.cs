namespace Zoo.Administration.Common
{
    using System.Collections.Generic;

    public abstract class AnimalCreating
    {
        public abstract string Family { get; }

        public IList<int> Foods { get; set; } = new List<int>();

        public int Legs { get; set; }

        public string Name { get; set; }

        public abstract AnimalCreating Validate();
    }
}
