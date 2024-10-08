using Karakoç.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karakoç.Controllers
{
	public class OrganizerController : Controller
	{
		private readonly ResulContext _context;

		public OrganizerController(ResulContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Yevmiyeler()
		{
			// Tüm çalışanları veritabanından al
			var calisanlar = _context.Calisans.ToList();

			// View'e gönderilecek model
			var model = new CalisanViewModel
			{
				Calisanlar = calisanlar
			};
			return View(model);
		}

        public class CalisanViewModel 
        {
            public List<Calisan> Calisanlar { get; set; }
        }

        [HttpPost]
		public IActionResult Kaydet(DateTime Tarih, List<int> isWorked) //yevmiyeler kaydet
		{
			// Öncelikle tüm çalışanları alın
			var calisanlar = _context.Calisans.ToList();

			foreach (var calisan in calisanlar)
			{
				// Bu çalışanın seçilen tarihte bir kaydı olup olmadığını kontrol et
				var yevmiye = _context.Yevmiyelers.FirstOrDefault(y => y.CalisanId == calisan.CalısanId && y.Tarih == Tarih);

				// Eğer kayıt varsa güncelle
				if (yevmiye != null)
				{
					yevmiye.IsWorked = isWorked.Contains(calisan.CalısanId);
				}
				// Eğer kayıt yoksa yeni bir yevmiye kaydı oluştur
				else
				{
					var yeniYevmiye = new Yevmiyeler
					{
						CalisanId = calisan.CalısanId,
						Tarih = Tarih,
						IsWorked = isWorked.Contains(calisan.CalısanId)
					};
					_context.Yevmiyelers.Add(yeniYevmiye);
				}
                _context.SaveChanges();
            }

			// Veritabanına değişiklikleri kaydet
			// İşlem sonrası sayfayı yeniden yükleyebilirsiniz
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Giderler()
		{
            // Tüm çalışanları veritabanından al
            var calisanlar = _context.Calisans.ToList();

            // View'e gönderilecek model
            var model = new CalisanViewModel
            {
                Calisanlar = calisanlar
            };
            return View(model);
		}

		[HttpPost]
		public IActionResult KaydetGider(int CalisanId, string Aciklama,int tutar)
		{
			var newGider = new Giderler
			{
				CalisanId= CalisanId,
				Description = Aciklama,
				Amount = tutar,
				Tarih = DateTime.Now
			};

			_context.Giderlers.Add(newGider);
			_context.SaveChanges();

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