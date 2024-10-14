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
		if (_loginManager.Register(Rusername, Rlastname, Remail, Rpassword))
		{
			ViewBag.Info = "Kayıt Başarılı";
			return View("Index");
		}
		else
		{
			ViewBag.Info = "Mail Zaten Kullanılıyor";
			return View("Index");
		}
	}

    


   
    public IActionResult Giris(string Email, string password)
	{
		return View();
    }

	
	public IActionResult KayıtOl(string Rusername, string Rlastname, string Remail, string Rpassword)
	{
        if (_loginManager.Register(Rusername, Rlastname, Remail, Rpassword))
        {
            ViewBag.Info = "Kayıt Başarılı";
            return View("Index");
        }
        else
        {
            ViewBag.Info = "Mail Zaten Kullanılıyor";
            return View("Home");
        }
    }
}
