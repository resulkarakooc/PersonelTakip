using Karakoç.Bussiness.concrete;
using Karakoç.Models; // Modelin bulunduğu namespace
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // EF Core için gerekli
using System.Threading.Tasks;

namespace Karakoç.Controllers
{
    public class CalisanController : Controller
    {
        private readonly ResulContext _resulContext;
        private readonly CalisanManager manager;
        public CalisanController(CalisanManager calisanManager, ResulContext resulContext)
        {
            manager = calisanManager;
            _resulContext = resulContext;
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

        public IActionResult Home()
        {
            var yevmiyeListesi = _resulContext.Yevmiyelers.Where(y => y.CalisanId == 1).ToList();

            // Çalışılan günleri sayma
            int calismaGunSayisi = yevmiyeListesi.Count(y => y.IsWorked == true);
            ViewBag.WorkedDays = calismaGunSayisi;
            ViewBag.UserName = "Resul Karakoç";
            return View();
        }

        public IActionResult Puantajım()
        {
            var kullaniciId = HttpContext.Session.GetInt32("CalisanId");
            kullaniciId = 1;

            // Veritabanından bu kullanıcıya ait yevmiye kayıtlarını çek
            var yevmiyeKayitlari = _resulContext.Yevmiyelers
                .Where(y => y.CalisanId == kullaniciId)
                .ToList();

            return View(yevmiyeKayitlari);
        }

        public IActionResult Odemeler() 
        {
            var liste = _resulContext.Odemelers.Where(y => y.CalisanId == 1).ToList(); 
            

            return View(liste); 
        }


    }
}
