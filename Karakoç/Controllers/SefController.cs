using Karakoç.Bussiness.concrete;
using Microsoft.AspNetCore.Mvc;

namespace Karakoç.Controllers
{
    public class SefController : Controller
    {
        private SefManager _sefManager;

        public SefController(SefManager sefManager)
        {
            _sefManager = sefManager;
        }


        public IActionResult Index() // çalışan sayısı  //Hosgeldiniz
        {

            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }

            // Tüm çalışanları al
            var calisanlar = _sefManager.GetCalisansVerify();


             ViewBag.UserName =  HttpContext.Request.Cookies["UserName"];
             ViewBag.UserSurName =  HttpContext.Request.Cookies["UserSurName"];

            // Çalışan sayısını ViewBag ile gönder
            ViewBag.CalisanSayisi = calisanlar.Count;

            
            return View(); // Çalışanları da model olarak gönder
        }

        public async Task<IActionResult> GetYevmiye()
        {
            var yevmiyeler = await _sefManager.GetYevmiyeler();

            var veriler = yevmiyeler.Select(c => new
            {

                c.Tarih,
                c.IsWorked,
                CalisanID = c.CalisanId,
                CalisanAd = c.Calisan.Name,
                CalisanSoyad = c.Calisan.Surname

            }).ToList();

            // Normal olarak view döndür
            return Json(veriler);
        }

        public IActionResult YevmiyeGor()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }


            return View();
        }


        public IActionResult MesaiGor()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }


            return View();
        }

        public async Task<IActionResult> GetMesai()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }


            var yevmiyeler = await _sefManager.GetMesai();

            var veriler = yevmiyeler.Select(c => new
            {

                c.Tarih,
                c.IsWorked,
                CalisanID = c.CalisanId,
                CalisanAd = c.Calisan.Name,
                CalisanSoyad = c.Calisan.Surname

            }).ToList();

            // Normal olarak view döndür
            return Json(veriler);
        }

        

        public IActionResult Ödemeler()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }


            return View(_sefManager.GetGelir());
        }

        [Route("/Sef/Calisan/{id}")]
        public IActionResult Calisan(int id)
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }

            var calisan = _sefManager.GetCalisanById(id);

            if (calisan == null)
            {
                return NotFound();
            }

            return View(calisan);
        }

        public IActionResult CalisanList()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }

            return View(_sefManager.GetCalisansVerify());
        }

        public bool Control()
        {
            if (HttpContext.Request.Cookies["Authority"] == "4") //admin veya  mimar ise
            {
                return true;
            }
            else
            {
                return false;
            }


        }

    }
}
