namespace Zoo.Infrastructure.Translators
{
    using System.Linq;

    using AutoMapper;

    using Entities.Zoo;

    using Park.BearsAggregate.Models;

    internal class BearDetailsTranslator : Profile
    {
        public BearDetailsTranslator()
        {
            this.CreateMap<Animal, BearDetails>()
                .ForMember(bear => bear.Family, opt => opt.Ignore())
                .ForMember(
                    bear => bear.Foods,
                    opt => opt.MapFrom(entity => entity.AnimalEats.Select(ae => ae.Food.Name)));
        }
    }
}