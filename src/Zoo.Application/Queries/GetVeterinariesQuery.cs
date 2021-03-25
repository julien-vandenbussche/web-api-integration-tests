namespace Zoo.Application.Queries
{
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;

    using Infirmary.VeterinaryAggregate;
    using Infirmary.VeterinaryAggregate.Models;

    public interface IGetVeterinariesQuery
    {
        Task GetAsync(FoundCallback<IImmutableList<VeterinaryContact>> found, NotFoundCallback notFound);
    }
    
    internal sealed class GetVeterinariesQuery : IGetVeterinariesQuery
    {
        private readonly IVeterinaryAdapter veterinaryAdapter;

        public GetVeterinariesQuery(IVeterinaryAdapter veterinaryAdapter)
        {
            this.veterinaryAdapter = veterinaryAdapter;
        }

        public async Task GetAsync(FoundCallback<IImmutableList<VeterinaryContact>> found, NotFoundCallback notFound)
        {  
            var veterinaryContacts = await this.veterinaryAdapter.GetAsync();
            if (!veterinaryContacts.Any())
            {
                notFound();
                return;
            }

            found(veterinaryContacts);
        }
    }
}