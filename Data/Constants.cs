namespace Common
{
    public static class Constants
    {
        public class ProtectedServices
        {
            public const string HostApp = "host-app";
            public const string BrokerService = "broker-api";
            public const string AuthService = "auth-api";
            public const string DatabaseService = "database-api";
            public const string ClientApp = "chat-app";
        }

        public enum PermissionLevel
        {
            None = 1,
            Read = 2,
            Write = 3,
            Admin = 4
        }

        public enum SystemRoles
        {
            Admin = 1,
            User = 2
        }


        public const string KafkaDatabaseGroupId = "database-service-group";

        public const string DateFormat = "yyyy-MM-dd HH:mm:ss";
    }
}
