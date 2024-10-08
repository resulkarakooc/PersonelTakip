using Karakoç.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Karakoç.Controllers.OrganizerController;

namespace Karakoç.Controllers
{
    public class AdminController : Controller
    {
        private readonly ResulContext _resulContext;

        public AdminController(ResulContext resulContext)
        {
            _resulContext = resulContext;
        }

        public IActionResult Index()
        {
            var calisans =  _resulContext.Calisans.ToList();
            return View(calisans);
        }
        public class CalisanViewModel //calisanlar listesi oluşturdum ben
        {
            public List<Calisan> Calisanlar { get; set; }
        }

        public IActionResult OdemeGir()
        {
            var calisanlar = _resulContext.Calisans.ToList();

            // View'e gönderilecek model
            var model = new CalisanViewModel
            {
                Calisanlar = calisanlar
            };

            return View(model);

        }

        [HttpPost]
        public IActionResult KaydetOdeme()
        {
            return View();
        }
    }
}
