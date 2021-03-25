namespace Zoo.Application.Commands
{
    using System.Threading.Tasks;

    using Administration.AnimalsRegistrationAggregate;
    using Administration.AnimalsRegistrationAggregate.Models;
    using Administration.Common;

    public interface IAnimalRegistrationCommand
    {
        Task ExecuteAsync<TCreatingAnimal, TAnimalDetails>(
            TCreatingAnimal createAnimal,
            CreatedCallback<TAnimalDetails> created,
            NotCreatedCallback<AnimalCreating> notCreated) where TCreatingAnimal : AnimalCreating;
    }

    internal class AnimalRegistrationCommand : IAnimalRegistrationCommand
    {
        private readonly IAnimalsRegistrationAdapter animalsRegistrationAdapter;

        public AnimalRegistrationCommand(IAnimalsRegistrationAdapter animalsRegistrationAdapter)
        {
            this.animalsRegistrationAdapter = animalsRegistrationAdapter;
        }

        public async Task ExecuteAsync<TCreatingAnimal, TAnimalDetails>(
            TCreatingAnimal createAnimal,
            CreatedCallback<TAnimalDetails> created,
            NotCreatedCallback<AnimalCreating> notCreated) where TCreatingAnimal : AnimalCreating
        {
            var createError = createAnimal.Validate();
            if (createError is IModelError)
            {
                notCreated(createError);
                return;
            }

            var createdAnimal =
                await this.animalsRegistrationAdapter.RegisterAsync<TCreatingAnimal, TAnimalDetails>(createAnimal);
            created(createdAnimal);
        }
    }
}