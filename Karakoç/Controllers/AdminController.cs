using Karakoç.Bussiness.abstracts;
using Karakoç.Bussiness.concrete;
using Karakoç.Models;
using MernisServiceReference;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Karakoç.Controllers
{
    public class AdminController : Controller
    {
        private AdminManager _adminManager;

        public AdminController(AdminManager adminManager)
        {
            _adminManager = adminManager;
        }

        public async  Task<IActionResult> GetYevmiye()
        {
            var yevmiyeler = await _adminManager.GetYevmiyeler();

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
            var yevmiyeler = await _adminManager.GetMesai();

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



        // İlk yüklendiğinde mevcut ay ve yıl bilgileri ile verileri yükleyen aksiyon


        public IActionResult Index()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }
            // Tüm çalışanları al
            var calisanlar = _adminManager.GetCalisans();
            var yevmiyeler = _adminManager.GetYevmiyelers();

            // Çalışan sayısını ViewBag ile gönder
            ViewBag.CalisanSayisi = calisanlar.Count;

            // Mevcut ayın yılı ve ayı
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            // Bu ayki true olan yevmiye sayısını hesapla
            var trueYevmiyeCount = yevmiyeler.Count(y => y.Tarih.HasValue && // Nullable kontrolü
                                                        y.Tarih.Value.Year == currentYear &&
                                                        y.Tarih.Value.Month == currentMonth &&
                                                        y.IsWorked == true);

            // Yevmiye sayısını ViewBag ile gönder
            ViewBag.TrueYevmiyeSayisi = trueYevmiyeCount;

            return View(); // Çalışanları da model olarak gönder
        }

        [Route("/Admin/Calisan/{id}")]
        public async Task<IActionResult> GetCalisan(int id)
        {
            var calisan = await _adminManager.GetCalisanById(id);
            return Json(calisan);
        }

        [Route("/Admin/Calisan/{id}")]
        public IActionResult Calisan(int id)
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }

            var calisan = _adminManager.GetCalisanById(id);

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

            return View(_adminManager.GetCalisans());
        }




        public IActionResult YevmiyeGiris()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }

            var calisanList = _adminManager.GetCalisans();
            return View(calisanList);
        }

        public IActionResult MesaiGiris()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }

            var calisanList = _adminManager.GetCalisans();
            return View(calisanList);
        }

        

        [HttpPost]
        public IActionResult MesaiKaydet(DateTime Tarih, List<int> isWorked)
        {
            // Öncelikle tüm çalışanları alın
            if (_adminManager.KaydetMesai(Tarih, isWorked))
            {
                ViewBag.Onay = "Mesailer Kaydedildi";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Onay = "bir hata oluştu";
                return RedirectToAction("MesaiGiris", "Admin");
            }
        }

        public IActionResult YevmiyeKaydet(DateTime Tarih, List<int> isWorked)
        {
            // Öncelikle tüm çalışanları alın
            if (_adminManager.KaydetYevmiye(Tarih, isWorked))
            {
                ViewBag.Onay = "Yevmiyeler Kaydedildi";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Onay = "bir hata oluştu";
                return RedirectToAction("YevmiyeGiris", "Admin");
            }
        }



        public IActionResult OdemeGiris()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }

            return View(_adminManager.GetCalisans());
        }

        public IActionResult OdemeGor()
        {

            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }

            var odemelist = _adminManager.GetOdeme().ToList();
            return View(odemelist);
        }


        public class OdemelerViewModel
        {
            public List<Odemeler> Odemeler { get; set; }
            public decimal ToplamTutar { get; set; }
        }


        [HttpPost]
        public IActionResult KaydetOdeme(int CalisanId, string Aciklama, int tutar)
        {
            _adminManager.KaydetOdeme(CalisanId, Aciklama, tutar);
            ViewBag.Bilgi = "Kayıt Edildi";
            return RedirectToAction("OdemeGiris", "Admin");
        }

        public bool Control()
        {
            if (HttpContext.Session.GetInt32("Authority") == 3) //admin ise
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
