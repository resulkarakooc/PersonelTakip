using Karakoç.Bussiness.concrete;
using Karakoç.Models; // Modelin bulunduğu namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // EF Core için gerekli
using System.Threading.Tasks;

namespace Karakoç.Controllers
{
    public class CalisanController : Controller
    {
		private readonly CalisanManager manager;
		public CalisanController(CalisanManager calisanManager)
		{
			manager = calisanManager;
		}

        [HttpGet]
        public IActionResult AnaSayfa()
        {
            var userName = HttpContext.Session.GetString("UserName");
            var userSurName = HttpContext.Session.GetString("UserSurName");
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var KayitTarihi = HttpContext.Session.GetString("KayitTarihi");

            ViewBag.UserName = userName;
            ViewBag.UserSurName = userSurName;
            ViewBag.UserEmail = userEmail;
            ViewBag.KayitTarihi = KayitTarihi;

            return View(manager.GetYevmiye(HttpContext)); // burası bir liste döndürücek geriye
        }

    }
}
