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
        public List<Calisan> GetCalisans()
        {
            return _resulContext.Calisans.ToList();
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
    }
}
