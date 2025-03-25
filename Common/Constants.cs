namespace Common
{
    public static class Constants
    {
        public class ServiceNames
        {
            public const string BackendHostService = "HostingService";
            public const string BrokerService = "BrokerService";
            public const string AuthenticationServer = "AuthenticationServer";
            public const string DatabaseService = "DatabaseService";
            public const string ClientApp = "ClientApp";
        }

        public enum PermissionsEnum
        {
            None = 0,
            Read = 1,
            Write = 2,
        }
        

        public const string DateFormat = "yyyy-MM-dd HH:mm:ss";
    }
}
