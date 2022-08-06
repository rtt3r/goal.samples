namespace Goal.Demo2.Infra.Crosscutting.Constants
{
    public partial class ApplicationConstants
    {
        public struct Messages
        {
            public const string SAVING_DATA_FAILURE = "Opss... An error occurred while saving the data";
            public const string CUSTOMER_NAME_REQUIRED = "Customer name is required";
            public const string CUSTOMER_NAME_LENGTH_INVALID = "Customer name length must be between 2 and 150 characters";
            public const string CUSTOMER_BIRTHDATE_REQUIRED = "Customer birth date is required";
            public const string CUSTOMER_BIRTHDATE_LENGTH_INVALID = "Customer's age must be greater than or equal to 18 years";
            public const string CUSTOMER_EMAIL_REQUIRED = "Customer e-mail is required";
            public const string CUSTOMER_EMAIL_INVALID = "Customer e-mail address is invalid";
            public const string CUSTOMER_ID_REQUIRED = "Customer id is required";
            public const string CUSTOMER_EMAIL_DUPLICATED = "Customer e-mail has already been taken";
            public const string CUSTOMER_NOT_FOUND = "Customer was not found";
        }
    }
}
