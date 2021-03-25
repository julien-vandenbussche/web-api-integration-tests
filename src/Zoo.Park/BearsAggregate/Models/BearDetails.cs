namespace Zoo.Park.BearsAggregate.Models
{
    using Common;
    using Common.Models;

    public sealed class BearDetails : AnimalDetails
    {
        public override string Family => "Bear";
    }
}