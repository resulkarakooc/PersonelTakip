using Karakoç.Bussiness.abstracts;
using Karakoç.Models;

namespace Karakoç.Bussiness.concrete
{
    public class OrganizerManager : IOrganizerService
    {
        private readonly ResulContext _context;

        public OrganizerManager(ResulContext context)
        {
            _context = context;
        }
        public List<Calisan> GetCalisans()
        {
            var calisanlar = _context.Calisans.ToList();
            return calisanlar;
        }

        public bool KaydetYevmiye(DateTime Tarih, List<int> isWorked)
        {
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
            return true;

        }

        public bool KaydetGider(int CalisanId, string Aciklama, int tutar)
        {
            var newGider = new Giderler
            {
                CalisanId = CalisanId,
                Description = Aciklama,
                Amount = tutar,
                Tarih = DateTime.Now
            };

            _context.Giderlers.Add(newGider);
            _context.SaveChanges();

            return true;
        }

        public List<Giderler> GetGiderlers()
        {
            throw new NotImplementedException();
        }




    }
}
