namespace Zoo.Infrastructure.Adapters
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Threading.Tasks;

    using AutoMapper;

    using Contracts.Veterinary;

    using Infirmary.VeterinaryAggregate;
    using Infirmary.VeterinaryAggregate.Models;

    internal sealed class VeterinaryAdapter : IVeterinaryAdapter
    {
        private readonly IVeterinaryClient veterinaryClient;

        private readonly IMapper mapper;

        public VeterinaryAdapter(IVeterinaryClient veterinaryClient, IMapper mapper)
        {
            this.veterinaryClient = veterinaryClient;
            this.mapper = mapper;
        }

        public async Task<IImmutableList<VeterinaryContact>> GetAsync()
        {
            var veterinaries = await this.veterinaryClient.GetAsync();
            return this.mapper.Map<List<VeterinaryContact>>(veterinaries).ToImmutableList();
        }
    }
}