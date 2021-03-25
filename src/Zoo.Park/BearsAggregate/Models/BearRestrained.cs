namespace Zoo.Park.BearsAggregate.Models
{
    using Common;
    using Common.Models;

    public sealed class BearRestrained : AnimalRestrained
    {
        public override string Family => "Bear";
    }
}