namespace Zoo.Infrastructure.Translators
{
    using System;
    using System.Collections.Immutable;

    using AutoMapper;

    using Contracts.Veterinary.Models;

    using Infirmary.VeterinaryAggregate.Models;

    using Park.BooksAggregate.Models;

    public class BookTranslator : Profile
    {
        private readonly string newLine = Environment.NewLine;

        public BookTranslator()
        {
            this.CreateMap<Book, BookService.Book>().ReverseMap();
        }
    }
}