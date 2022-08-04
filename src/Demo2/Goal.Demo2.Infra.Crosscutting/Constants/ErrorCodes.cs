namespace Goal.Demo2.Infra.Crosscutting.Constants
{
    public partial class ApplicationConstants
    {
        public struct ErrorCodes
        {
            public const string SaveDataFailed = "ER-0001";
            public const string CustomerNameRequired = "VD-0001";
            public const string CustomerNameLengthInvalid = "VD-0002";
            public const string CustomerBirthdateRequired = "VD-0003";
            public const string CustomerBirthdateLengthInvalid = "VD-0004";
            public const string CustomerEmailRequired = "VD-0005";
            public const string CustomerEmailAddressInvalid = "VD-0006";
            public const string CustomerIdRequired = "VD-0006";
            public const string CustomerEmailDuplicated = "VL-0001";
            public const string CustomerNotFound = "VL-0002";
        }
    }
}
