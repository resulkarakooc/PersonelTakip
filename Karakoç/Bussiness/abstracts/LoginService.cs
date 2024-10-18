namespace Karakoç.Bussiness.abstracts
{
    public interface LoginService
    {
        public bool Login(string username, string password, HttpContext httpContext);
        public bool Register(long TC, string Rusername, string Rlastname, string Remail, DateTime BirthDay, string Rpassword);
    }
}
