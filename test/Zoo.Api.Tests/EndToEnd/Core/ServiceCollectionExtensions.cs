namespace Zoo.Api.Tests.EndToEnd.Core
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    internal static class ServiceCollectionExtensions
    {
        public static void RegisterMock<T>(this IServiceCollection services, Action<IServiceCollection, Mock<T>> configure) where T : class
        {
            var mock = new Mock<T>();
            configure(services, mock);
        }
    }
}