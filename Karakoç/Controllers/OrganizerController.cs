using Karakoç.Bussiness.concrete;
using Karakoç.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karakoç.Controllers
{
	public class OrganizerController : Controller
	{
		private readonly ResulContext _context;
		private readonly OrganizerManager _organizerManager;

		public OrganizerController(ResulContext context, OrganizerManager manager)
		{
			_context = context;
			_organizerManager = manager;
		}

		[HttpGet]
		public IActionResult Yevmiyeler()  //yevmiye girme ekranı
		{
			return View(_organizerManager.GetCalisans());
		}

        public class CalisanViewModel 
        {
            public List<Calisan> Calisanlar { get; set; }
        }

        [HttpPost]
		public IActionResult Kaydet(DateTime Tarih, List<int> isWorked) //yevmiyeleri kaydet
		{
			// Öncelikle tüm çalışanları alın
			if (_organizerManager.KaydetYevmiye(Tarih, isWorked)){
                ViewBag.Onay = "Yevmiyeler Kaydedildi";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
		}

		[HttpGet]
		public IActionResult Giderler() //giderler listesi
		{
            return View(_organizerManager.GetCalisans());
		}

		[HttpPost]
		public IActionResult KaydetGider(int CalisanId, string Aciklama,int tutar)
		{
			if(_organizerManager.KaydetGider(CalisanId, Aciklama, tutar))
            {
                return RedirectToAction("Giderler");
            }
            return RedirectToAction("Giderler");
        }

        public class GiderViewModel
        {
            public List<Giderler> Giderler { get; set; }
            public List<Calisan> Calisanlar { get; set; } // Çalışanlar listesini ekliyoruz
            public decimal ToplamTutar { get; set; } // Toplam tutar
        }

        [HttpGet]
        public IActionResult GetGider()
        {
            // Giderler ile birlikte Calisan bilgilerini de dahil ediyoruz
            var giderler = _context.Giderlers.Include(g => g.Calisan).ToList();
            var calisanlar = _context.Calisans.ToList(); // Çalışanları alıyoruz

            var toplamTutar = giderler.Sum(g => g.Amount);
            // View'e gönderilecek model
            var model = new GiderViewModel
            {
				ToplamTutar= toplamTutar,
                Giderler = giderler,
                Calisanlar = calisanlar // Çalışanlar listesini modelde tutuyoruz
            };
            return View(model);
        }

    }
}