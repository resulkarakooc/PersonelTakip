using Karakoç.Bussiness.concrete;
using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly LoginManager _loginManager;

    public LoginController(LoginManager loginManager)
    {
        _loginManager = loginManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (_loginManager.Login(username, password, HttpContext)) // HttpContexti doğrudan kullanıcam
        {
            return RedirectToAction("Home", "Calisan");
        }
        else
        {
            ViewBag.ErrorMessage = "Email veya Parola Yanlış";
            return View("Giris");
        }
    }

    [HttpPost]
    public IActionResult Register(string Rusername, string Rlastname, string Remail, string Rpassword)
    {
        // Kayıt işlemi geçerli mi kontrol ediliyor
        if (!ModelState.IsValid)
        {
            ViewBag.Info = "Geçersiz bilgi girdiniz.";
            return View("KayıtOl");
        }

        // Kayıt işlemi yapılmaya çalışılıyor
        if (_loginManager.Register(Rusername, Rlastname, Remail, Rpassword))
        {
            ViewBag.SuccessMessage = "Kayıt işlemi başarılı!";
            return View("Giris"); // Kayıt başarılıysa giriş sayfasına yönlendiriliyor
        }
        else
        {
            ViewBag.Info = "Mail zaten kullanılıyor";
            return View("KayıtOl"); // Kayıt başarısızsa tekrar kayıt sayfasına dönülüyor
        }
    }



    public IActionResult Giris(string Email, string password)
    {
        return View();
    }

    public IActionResult KayıtOl()
    {
        return View();
    }
}
