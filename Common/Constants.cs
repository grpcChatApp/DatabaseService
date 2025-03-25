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
            None = 1,
            Read = 2,
            Write = 3,
        }

        public const string KafkaDatabaseGroupId = "database-service-group";

        public const string DateFormat = "yyyy-MM-dd HH:mm:ss";
    }
}
