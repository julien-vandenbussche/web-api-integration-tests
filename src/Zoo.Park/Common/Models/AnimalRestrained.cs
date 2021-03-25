namespace Zoo.Park.Common.Models
{
    public abstract class AnimalRestrained
    {
        public abstract string Family { get; }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
