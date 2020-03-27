namespace VG_AspNetCore_Web.Services
{
    public interface IHomeService
    {
        string GetIndexMessage();
        string GetAboutMessage();
        string GetContactMessage();
    }
}