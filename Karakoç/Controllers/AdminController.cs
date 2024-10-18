using Karakoç.Bussiness.abstracts;
using Karakoç.Bussiness.concrete;
using Karakoç.Models;
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



        public IActionResult Index()
        {
            // Tüm çalışanları al
            var calisanlar = _adminManager.GetCalisans();
            var yevmiyeler = _adminManager.GetYevmiyeler();

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

            var calisan = _adminManager.GetCalisanById(id);

            if (calisan == null)
            {
                return NotFound();
            }

            return View(calisan);
        }

        public IActionResult CalisanList()
        {
            return View(_adminManager.GetCalisans());
        }


        public IActionResult YevmiyeGor()
        {
            return View(_adminManager.GetYevmiyeler());
        }

        public IActionResult YevmiyeGiris()
        {
            var calisanList = _adminManager.GetCalisans();
            return View(calisanList);
        }

        public IActionResult MesaiGiris()
        {
            var calisanList = _adminManager.GetCalisans();
            return View(calisanList);
        }

        public IActionResult MesaiGor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MesaiKaydet(DateTime Tarih, List<int> isWorked)
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
            return View(_adminManager.GetCalisans()); 
        }

        public IActionResult OdemeGor()
        {
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
    }
}
