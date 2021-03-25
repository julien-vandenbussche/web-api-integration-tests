namespace Core.Validator
{
    public sealed class Error
    {
        public Error(string propertyName, string errorMessage)
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public string PropertyName { get; }
    }
}