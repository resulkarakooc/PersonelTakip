using Karakoç.Bussiness.concrete;
using MernisServiceReference;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;


public class LoginController : Controller
{
    private readonly LoginManager _loginManager;
    private readonly KPSPublicSoapClient _mernisClient;

    public LoginController(LoginManager loginManager)
    {
        _loginManager = loginManager;
        _mernisClient = new KPSPublicSoapClient(KPSPublicSoapClient.EndpointConfiguration.KPSPublicSoap);
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<bool> TCKimlikDogrula(long tckimlikNo, string ad, string soyad, int dogumYili)
    {
        var requestBody = new TCKimlikNoDogrulaRequestBody(tckimlikNo, ad, soyad, dogumYili);
        var request = new TCKimlikNoDogrulaRequest(requestBody);

        try
        {
            var response = await _mernisClient.TCKimlikNoDogrulaAsync(request);
            return response.Body.TCKimlikNoDogrulaResult;
        }
        catch (Exception ex)
        {
            // Hata durumunu yönet
            Console.WriteLine("Mernis doğrulama hatası: " + ex.Message);
            return false;
        }
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
    public async Task<IActionResult> Register(long RTC, string Rusername, string Rlastname, string Remail, DateTime RBirth, string Rpassword)
    {
        // Mernis doğrulaması
        bool dogrulamaSonucu = await TCKimlikDogrula(RTC, Rusername, Rlastname, RBirth.Year);

        if (dogrulamaSonucu)
        {
            // Kayıt işlemi geçerli mi kontrol ediliyor
            if (!ModelState.IsValid)
            {
                ViewBag.Info = "Geçersiz bilgi girdiniz.";
                return View("KayıtOl");
            }

            // Kayıt işlemi yapılmaya çalışılıyor
            if (_loginManager.Register(RTC, Rusername, Rlastname, Remail, RBirth, Rpassword))
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
        else
        {
            ViewBag.Info = "MERNİS Veritabanında Kişi Bulunamadı.";
            return View("KayıtOl");
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
