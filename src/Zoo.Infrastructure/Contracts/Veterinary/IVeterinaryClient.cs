namespace Zoo.Infrastructure.Contracts.Veterinary
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    using Refit;

    internal interface IVeterinaryClient
    {
        [Get("/")]
        Task<List<Veterinary>> GetAsync();
    }
}