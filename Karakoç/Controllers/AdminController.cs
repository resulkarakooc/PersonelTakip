using Karakoç.Bussiness.abstracts;
using Karakoç.Bussiness.concrete;
using Karakoç.Models;
using MernisServiceReference;
using Microsoft.AspNetCore.Http;
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

        public async Task<IActionResult> GetYevmiye()
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



       


        public IActionResult Index()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }
            // Tüm çalışanları al
            var calisanlar = _adminManager.GetCalisansVerify();
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

        [Route("/Admin/calisanDelete/{id}")]
        public IActionResult CalisanDelete(int id)
        {
            bool isDeleted = _adminManager.CalisanDelete(id);

            if (isDeleted)
            {
                // Başarılı silme işlemi sonrası ana sayfaya veya çalışan listesine yönlendirin
                return RedirectToAction("CalisanList", "Admin"); 
            }
            else
            {
                // Silme işlemi başarısız olduysa bir hata mesajı gösterin
                ModelState.AddModelError("", "Çalışan silinemedi. Çalışan bulunamadı veya ilişkili veriler silinemedi.");
                return View();
            }
        }

        [Route("/Admin/UpdateAuthority/{id}/{deger}")]
        public IActionResult UpdateAuthority(int id, byte deger)
        {
            
            _adminManager.UpdateAuthority(id, deger);
            return RedirectToAction("CalisanList", "Admin");
        }

        [Route("/Admin/UpdateVerify/{id}")]
        public IActionResult UpdateVerify(int id)
        {
            _adminManager.UpdateVerify(id);
            return RedirectToAction("CalisanList", "Admin");
        }

        public IActionResult YevmiyeGiris()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }

            var calisanList = _adminManager.GetCalisansVerify();
            return View(calisanList);
        }

        public IActionResult MesaiGiris()
        {

            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }

            var calisanList = _adminManager.GetCalisansVerify();
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

            return View(_adminManager.GetCalisansVerify());
        }

        public class OdemeDto
        {
            public int OdemeId { get; set; }
            public int CalisanId { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public string CalisanAd { get; set; } // Calisan ismi
            public string CalisanSoyad { get; set; } // Calisan ismi
            public DateTime Tarih { get; set; } // Tarih alanı
        }

        // Eğer ihtiyaç varsa
        public class CalisanDto
        {
            public int Id { get; set; }
            public string Ad { get; set; }
            public string Soyad { get; set; }
        }

        [HttpGet("/Admin/Odeme")]
        public async Task<IActionResult> GetOdeme()
        {
            var odemeler = await _adminManager.GetOdeme();

            var odemeDtos = odemeler.Select(o => new OdemeDto
            {
                OdemeId = o.OdemeId,
                CalisanId = o.CalisanId,
                Description = o.Description,
                Amount = o.Amount,
                CalisanAd = o.Calisan.Name,
                CalisanSoyad = o.Calisan.Surname,
                Tarih = o.Tarih // Tarih alanını ekleyin
            }).ToList();

            return Json(odemeDtos); // DTO listesini JSON olarak döndür
        }

        public IActionResult OdemeGor()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }


            return View(); // View'a model olarak geçin
        }



        [HttpPost]
        public IActionResult KaydetOdeme(int CalisanId, string Aciklama, int tutar, DateTime Tarih)
        {
            _adminManager.KaydetOdeme(CalisanId, Aciklama, tutar, Tarih);
            ViewBag.Bilgi = "Kayıt Edildi";
            return RedirectToAction("OdemeGiris", "Admin");
        }

        public IActionResult Alınan()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }
            return View(_adminManager.GetGelir());
        }

        [HttpPost]
        public IActionResult KaydetGelir(string aciklama,DateTime Tarih,decimal miktar)
        {
            _adminManager.KaydetGelir(aciklama, Tarih, miktar);
            ViewBag.Info = "İşlem Kaydedildi";
            return View("GelirGiris");
        }

        public IActionResult GelirGiris()
        {
            if (!Control())
            {
                return RedirectToAction("Giris", "Login");
            }
            return View();
        }

        public bool Control()
        {
            if (HttpContext.Request.Cookies["Authority"] == "3") //admin ise al
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