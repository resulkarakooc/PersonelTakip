using Karakoç.Models;
using Microsoft.AspNetCore.Mvc;


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
			// Tüm çalışanları veritabanından al
			var calisanlar = _context.Calisans.ToList();

			// View'e gönderilecek model
			var model = new CalisanViewModel
			{
				Calisanlar = calisanlar
			};

			return View(model);

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

		[HttpPost]
		public IActionResult Kaydet(DateTime Tarih, List<int> isWorked)
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


		public class CalisanViewModel
		{
			public List<Calisan> Calisanlar { get; set; }
		}
	}
}
