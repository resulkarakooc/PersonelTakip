using Karakoç.Bussiness.abstracts;
using Karakoç.Bussiness.concrete;
using Karakoç.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Karakoç.Controllers
{
    public class AdminController : Controller
    {
        private AdminManager _adminManager;

        public AdminController(AdminManager adminManager)
        {
            _adminManager = adminManager;
        }

        public IActionResult YevmiyeGor()
        {
            

            return View(_adminManager.GetYevmiyeler());
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

        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// </summary>


        public class OdemelerViewModel
        {
            public List<Odemeler> Odemeler { get; set; }
            public decimal ToplamTutar { get; set; }
        }

        public IActionResult Odemeler()
        {
            var odemeListesi = _adminManager.GetOdeme(); // AdminManager'dan Odemeler'i al
            var toplamTutar = odemeListesi.Sum(o => o.Amount); // Toplam tutarı hesapla

            // ViewModel'i doğrudan burada tanımla
            var odemeViewModel = new OdemelerViewModel
            {
                Odemeler = odemeListesi,
                ToplamTutar = toplamTutar
            };

            return View(odemeViewModel); // ViewModel'i View'a gönder
            
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
