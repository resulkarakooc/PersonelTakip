using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Karakoç.Models; // Projendeki DbContext ve model adlarını kullan
// DbContext burada tanımlıysa bunu dahil et

namespace Karakoç.Controllers
{
    public class LoginController : Controller
    {
        private readonly ResulContext _context;

        public LoginController(ResulContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {

            var user = _context.Calisans
                .FirstOrDefault(c => c.Email == username && c.Password == password);

            if (user != null)
            {

                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserSurName", user.Surname);
                HttpContext.Session.SetString("UserEmail", user.Email);
                if (user.KayıtTarihi.HasValue) // Nullable kontrolü
                {
                    HttpContext.Session.SetString("KayitTarihi", user.KayıtTarihi.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }



                // Başarılı giriş durumunda ana sayfaya yönlendir
                return RedirectToAction("AnaSayfa", "Calisan");
            }
            else
            {
                // Başarısız giriş durumunda hata mesajı göster
                ViewBag.ErrorMessage = "Kullanıcı adı veya şifre hatalı.";
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult Register(string Rusername, string Rlastname, string Remail, string Rpassword)
        {
            var user = _context.Calisans.FirstOrDefault(c => c.Email == Remail);

            if (user == null) //mevcut kayıt yok ise 
            {
                var newCalisan = new Calisan
                {
                    Name = Rusername,
                    Surname = Rlastname,
                    Email = Remail,
                    KayıtTarihi = DateTime.Now,
                    Password = Rpassword
                };

                // Yeni çalışanı veritabanına ekle ve değişiklikleri kaydet
                _context.Calisans.Add(newCalisan);
                _context.SaveChanges();
                return View("Index");

            }
            else
            {
                ViewBag.ErrorMessage = "Kullanıcı Zaten Kayıtlı";
                return View("Index");

            }

        }

    }
}
