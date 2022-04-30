namespace Goal.Demo2.Infra.Crosscutting.Constants
{
    public partial class ApplicationConstants
    {
        public struct Messages
        {
            public const string SaveDataFailed = "Opss... An error occurred while saving the data";
            public const string CustomerNameRequired = "Customer name is required";
            public const string CustomerNameLengthInvalid = "Customer name length must be between 2 and 150 characters";
            public const string CustomerBirthDateRequired = "Customer birth date is required";
            public const string CustomerBirthDateLengthInvalid = "Customer's age must be greater than or equal to 18 years";
            public const string CustomerEmailRequired = "Customer e-mail is required";
            public const string CustomerEmailAddressInvalid = "Customer e-mail address is invalid";
            public const string CustomerIdRequired = "Customer id is required";
            public const string CustomerEmailDuplicated = "Customer e-mail has already been taken";
            public const string CustomerNotFound = "Customer was not found";
        }
    }
}
