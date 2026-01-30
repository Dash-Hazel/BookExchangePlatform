namespace BookExchangePlatform.Common
{
    public class ValidationConstants
    {
        //Book
        public const int TitleMinLength = 3;
        public const int TitleMaxLength = 30;
        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 300;
        public const int AuthorMinLength = 3;
        public const int AuthorMaxLength = 30;

        //User
        public const int FirstNameMinLength = 3;
        public const int FirstNameMaxLength = 30;
        public const int LastNameMinLength = 3;
        public const int LastNameMaxLength = 30;
        public const int EmailMinLength = 10;
        public const int EmailMaxLength = 50;

        //ExchangeBook
        public const int MinBooksForExchange = 1;

        // Error messages section
        public const string RequiredFieldErrorMessage = "{0} is required.";
        public const string StringLengthErrorMessage = "{0} must be between {2} and {1} characters.";
        public const string InvalidEmailErrorMessage = "Please enter a valid email address.";
        public const string InvalidPhoneErrorMessage = "Please enter a valid phone number.";
    }
}
