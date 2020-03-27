namespace VG_AspNetCore_Web.Services
{
    public class DefaultHomeService : IHomeService
    {
        public string GetIndexMessage()
        {
            return "Welcome to Test ASP.NET Core application";
        }

        public string GetAboutMessage()
        {
            return "Test ASP.NET Core application";
        }

        public string GetContactMessage()
        {
            return "In case of any questions, feel free to contact directly";
        }
    }
}