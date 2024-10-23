using Karakoç.Bussiness.abstracts;
using Karakoç.Models;


namespace Karakoç.Bussiness.concrete
{
    public class LoginManager : LoginService
    {
        private readonly ResulContext _context;

        public LoginManager(ResulContext context)
        {
            _context = context;
        }

        public bool Reset(string email)
        {
            var user = _context.Calisans
               .FirstOrDefault(c => c.Email == email);
            if(user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Login(string username, string password, HttpContext httpContext)
        {


            var user = _context.Calisans
                .FirstOrDefault(c => c.Email == username && c.Password == password);

            if (user != null)
            {
                httpContext.Session.SetInt32("CalisanId", user.CalısanId);
                httpContext.Session.SetString("UserName", user.Name);
                httpContext.Session.SetString("UserSurName", user.Surname);
                httpContext.Session.SetString("UserEmail", user.Email);
                if (user.KayıtTarihi.HasValue) // Nullable kontrolü
                {
                    httpContext.Session.SetString("KayitTarihi", user.KayıtTarihi.Value.ToString("yyyy-MM-dd"));
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Register(long TC, string Rusername, string Rlastname, string Remail, DateTime BirthDay, string Rpassword)
        {
            var user = _context.Calisans.FirstOrDefault(c => c.Email == Remail);
            if (user == null) //mevcut kayıt yok ise 
            {
                var newCalisan = new Calisan
                {
                    TcKimlik = TC,
                    Name = Rusername,
                    Surname = Rlastname,
                    Email = Remail,
                    KayıtTarihi = DateTime.Now,
                    BirthDate = BirthDay,
                    Password = Rpassword
                };
                // Yeni çalışanı veritabanına ekle ve değişiklikleri kaydet
                _context.Calisans.Add(newCalisan);
                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
