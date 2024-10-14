using Karakoç.Models;

namespace Karakoç.Bussiness.abstracts
{
    public interface IAdminService
    {
        List<Calisan> GetCalisans();
        bool KaydetOdeme(int CalisanId, string Aciklama, int tutar);
        List<Odemeler> GetOdeme();
        public List<Yevmiyeler> GetYevmiyeler();
    }
}