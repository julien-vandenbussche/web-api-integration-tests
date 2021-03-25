namespace Zoo.Infrastructure.Adapters
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using AutoMapper;

    using AxaFrance.Extensions.ServiceModel;

    using BookService;

    using Humanizer;

    using Park.BooksAggregate;

    using Book = Park.BooksAggregate.Models.Book;

    internal class BookAdapter : IBookAdapter
    {
        private readonly IServiceClientWrapper<BookServiceChannel> serviceClientWrapper;

        private readonly IMapper mapper;

        public BookAdapter(IServiceClientWrapper<BookServiceChannel> serviceClientWrapper, IMapper mapper)
        {
            this.serviceClientWrapper = serviceClientWrapper;
            this.mapper = mapper;
        }

        public async Task<IImmutableList<Book>> GetBooksAsync(string searchedWord)
        {
            var lowerWord = searchedWord.ToLowerInvariant();
            var pattern = $"{lowerWord.Pluralize()}|{lowerWord.Singularize()}";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var response = await this.serviceClientWrapper.Channel.GetAllBooksAsync(new GetAllBooksRequest());
            var foundedBooks = response.Book.AsEnumerable()
                                       .Where(book => regex.IsMatch(book.Title.ToLowerInvariant()));
            return this.mapper.Map<List<Book>>(foundedBooks).ToImmutableList();
        }
    }
}