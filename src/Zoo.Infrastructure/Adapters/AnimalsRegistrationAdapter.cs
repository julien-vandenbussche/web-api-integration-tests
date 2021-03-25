namespace Zoo.Infrastructure.Adapters
{
    using System.Linq;
    using System.Threading.Tasks;

    using Administration.AnimalsRegistrationAggregate;
    using Administration.Common;

    using AutoMapper;

    using Entities.Zoo;
    
    using Park.BearsAggregate.Models;

    using Specification;

    using Store;

    internal class AnimalsRegistrationAdapter : IAnimalsRegistrationAdapter
    {
        private readonly IMapper mapper;
        
        private readonly IWriter writer;

        private readonly IReader reader;

        public AnimalsRegistrationAdapter(IMapper mapper, IWriter writer, IReader reader)
        {
            this.mapper = mapper;
            this.writer = writer;
            this.reader = reader;
        }
        
        public async Task<TDetails> RegisterAsync<TCreating, TDetails>(TCreating creatingAnimal) 
            where TCreating : AnimalCreating
        {  
            var mappedAnimal = this.mapper.Map<Animal>(creatingAnimal);
            this.writer.Create(mappedAnimal);
            await this.writer.SaveAsync();
            var bearsQuery = this.reader.Get(new AnimalsSpecification<TDetails>()).Where(animal => animal == mappedAnimal);
            return this.mapper.ProjectTo<TDetails>(bearsQuery).Single();
        }
    }
}