namespace Zoo.Api.Options
{
    public class OidcOptions
    {
        public string Authority { get; set; } = "";

        public bool ValidateLifetime { get; set; }

        public string Audience { get; set; } = "";
    }
}