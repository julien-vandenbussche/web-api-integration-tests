namespace Zoo.Api.Tests.EndToEnd.Core
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    internal static class HttpContentExtensions
    {
        public static async Task<T> ReadContentAsync<T>(this HttpContent httpContent, Func<string, T> parser)
        {
            var content = await httpContent.ReadAsStringAsync();
            return string.IsNullOrEmpty(content) ? default : parser(content);
        }

        public static async Task<T> ReadContentAsync<T>(this HttpContent httpContent)
        {
            var content = await httpContent.ReadAsStringAsync();
            return string.IsNullOrEmpty(content) ? default : JsonConvert.DeserializeObject<T>(content);
        }
    }
}