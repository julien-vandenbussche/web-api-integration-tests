namespace Zoo.Infrastructure.Translators
{
    using System.Linq;

    using Administration.AnimalsRegistrationAggregate.Models;

    using AutoMapper;

    using Entities.Zoo;

    internal class BearCreatingTranslator : Profile
    {
        public BearCreatingTranslator()
        {
            this.CreateMap<BearCreating, Animal>()
                .ForMember(entity => entity.Family, opt => opt.Ignore())
                .ForMember(entity => entity.FamilyId, opt => opt.MapFrom(bear => 3))
                .ForMember( entity => entity.AnimalEats, opt => opt.MapFrom(
                                                         model =>
                                                             model.Foods.Select(food => new AnimalEat { FoodId = food }).ToList()));
        }
    }
}