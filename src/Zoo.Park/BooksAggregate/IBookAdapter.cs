namespace Zoo.Park.BooksAggregate
{
    using System.Collections.Immutable;
    using System.Threading.Tasks;

    using Models;

    public interface IBookAdapter
    {
        Task<IImmutableList<Book>> GetBooksAsync(string searchedWord);
    }
}