namespace Zoo.Infirmary.VeterinaryAggregate
{
    using System.Collections.Immutable;
    using System.Threading.Tasks;

    using Models;

    public interface IVeterinaryAdapter
    {
        Task<IImmutableList<VeterinaryContact>> GetAsync();
    }
}