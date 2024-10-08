using Karakoç.Models; // Modelin bulunduğu namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // EF Core için gerekli
using System.Threading.Tasks;

namespace Karakoç.Controllers
{
    public class CalisanController : Controller
    {
        private readonly ResulContext _resulContext;

        public CalisanController(ResulContext resulContext)
        {
            _resulContext = resulContext;
        }

        public class YevmiyeViewClass
        {
            public List<Yevmiyeler> YevmiyelerList { get; set; } // İsim değişikliği
        }

        [HttpGet]
        public IActionResult AnaSayfa()
        {
            var userName = HttpContext.Session.GetString("UserName");
            var userSurName = HttpContext.Session.GetString("UserSurName");
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var KayitTarihi = HttpContext.Session.GetString("KayitTarihi");
            var calisanId = HttpContext.Session.GetInt32("CalisanId");

            

            var yevmiyeler = _resulContext.Yevmiyelers
                .Where(y => y.CalisanId == calisanId)
                .ToList();

            var yevmiye = new YevmiyeViewClass
            {
                YevmiyelerList = yevmiyeler // İsim değişikliği
            };

            // ViewBag ile view'a aktar
            ViewBag.UserName = userName;
            ViewBag.UserSurName = userSurName;
            ViewBag.UserEmail = userEmail;
            ViewBag.KayitTarihi = KayitTarihi;

            return View(yevmiye); // Değişiklik yapıldı
        }

        

    }
}
