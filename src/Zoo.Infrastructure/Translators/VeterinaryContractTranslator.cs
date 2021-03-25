namespace Zoo.Infrastructure.Translators
{
    using System;

    using AutoMapper;

    using Contracts.Veterinary.Models;

    using Infirmary.VeterinaryAggregate.Models;

    public class VeterinaryContractTranslator : Profile
    {
        private readonly string newLine = Environment.NewLine;

        public VeterinaryContractTranslator()
        {
            this.CreateMap<Veterinary, VeterinaryContact>()
                .ForMember(
                    v => v.Address,
                    opt => opt.MapFrom(
                        veterinary =>
                            $"{veterinary.Address}{this.newLine}{veterinary.PostalCode} {veterinary.City}{this.newLine}{veterinary.Country}"));
        }
    }
}