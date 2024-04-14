namespace ShredStorePresentation.Extensions
{
    public static class LogMessages
    {
        public static string LogErrorMessage() => "Exception caught at {Name}Controller: {Msg} at : {Now}";
        public static string LogActionAccessedMessage() => "{Action} accessed at {Now}";

        public static string LogLoginMessage() => "{Name} logged at {Now} with Id {Id}";
        public static string LogFailedLoginMessage() => "Failed Login Attempt At : {Now}";
        public static string LogLogoutMessage() => "User {Name} with Id {Id} LoggedOut at : {Now}";
    }
}
