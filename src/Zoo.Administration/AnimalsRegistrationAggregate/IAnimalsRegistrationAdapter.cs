namespace Zoo.Administration.AnimalsRegistrationAggregate
{
    using System.Threading.Tasks;

    using Common;

    public interface IAnimalsRegistrationAdapter
    {
        Task<TDetails> RegisterAsync<TCreate, TDetails>(TCreate creatingAnimal) where TCreate : AnimalCreating;
    }
}