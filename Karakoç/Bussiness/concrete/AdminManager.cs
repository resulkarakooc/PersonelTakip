using Karakoç.Bussiness.abstracts;
using Karakoç.Models;
using Microsoft.EntityFrameworkCore;


namespace Karakoç.Bussiness.concrete
{
    public class AdminManager : IAdminService
    {
        private readonly ResulContext _resulContext;

        public AdminManager(ResulContext resulContext)
        {
            _resulContext = resulContext;
        }

        

        public Calisan? GetCalisanById(int id)
        {
            
                return _resulContext.Calisans
                    .Include(c => c.Yevmiyelers)
                    .Include(c => c.Odemelers)
                    .FirstOrDefault(c => c.CalısanId == id);
            
        }

        public List<Calisan> GetCalisans()
        {
            return _resulContext.Calisans.ToList();
        }

        public List<Mesai> GetMesai()
        {
            return _resulContext.Mesais.ToList();
        }

        public List<Odemeler> GetOdeme()
        {
            var odemeListesi = _resulContext.Odemelers
                            .Include(o => o.Calisan)
                            .ToList();


            return odemeListesi;
        }

        public List<Yevmiyeler> GetYevmiyeler()
        {
            var yevmiyeList = _resulContext.Yevmiyelers
                            .Include(o => o.Calisan)
                            .ToList();

            return yevmiyeList;
        }

        public bool KaydetOdeme(int CalisanId, string Aciklama, int tutar)
        {
            var odeme = new Odemeler
            {
                CalisanId = CalisanId,
                Amount = tutar,
                Description = Aciklama,
                Tarih = DateTime.Now
            };
            _resulContext.Odemelers.Add(odeme);
            _resulContext.SaveChanges();
            return true;
        }

        public bool KaydetMesai(DateTime Tarih, List<int> isWorked)
        {

            var calisanlar = _resulContext.Calisans.ToList();

            foreach (var calisan in calisanlar)
            {
                // Bu çalışanın seçilen tarihte bir kaydı olup olmadığını kontrol et
                var mesai = _resulContext.Mesais.FirstOrDefault(y => y.CalisanId == calisan.CalısanId && y.Tarih == Tarih);

                // Eğer kayıt varsa güncelle
                if (mesai != null)
                {
                    mesai.IsWorked = isWorked.Contains(calisan.CalısanId);
                }
                // Eğer kayıt yoksa yeni bir yevmiye kaydı oluştur
                else
                {
                    var yeniMesai = new Mesai
                    {
                        
                        CalisanId = calisan.CalısanId,
                        Tarih = Tarih,
                        IsWorked = isWorked.Contains(calisan.CalısanId)
                    };
                    _resulContext.Mesais.Add(yeniMesai);
                }
                _resulContext.SaveChanges();
            }
            return true;
        }

        public bool KaydetYevmiye(DateTime Tarih, List<int> isWorked)
        {
            var calisanlar = _resulContext.Calisans.ToList();

            foreach (var calisan in calisanlar)
            {
                // Bu çalışanın seçilen tarihte bir kaydı olup olmadığını kontrol et
                var yevmiye = _resulContext.Yevmiyelers.FirstOrDefault(y => y.CalisanId == calisan.CalısanId && y.Tarih == Tarih);

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
                    _resulContext.Yevmiyelers.Add(yeniYevmiye);
                }
                _resulContext.SaveChanges();
            }
            return true;
        }
    }
}
