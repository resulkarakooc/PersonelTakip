using Humanizer;
using Karakoç.Bussiness.abstracts;
using Karakoç.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace Karakoç.Bussiness.concrete
{
    public class AdminManager : IAdminService
    {
        private readonly ResulContext _resulContext;

        public AdminManager(ResulContext resulContext)
        {
            _resulContext = resulContext;
        }

        public List<GelirTablosu> GetGelir()
        {
            return _resulContext.GelirTablosus.ToList();
        }

        public bool KaydetGelir(string aciklama, DateTime Tarih, decimal miktar)
        {
            try
            {
                GelirTablosu gelir = new GelirTablosu
                {
                    Aciklama = aciklama,
                    AlınanMiktar = miktar,
                    AlınanTarih = Tarih
                };

                _resulContext.GelirTablosus.Add(gelir);
                _resulContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        public Calisan? GetCalisanById(int id)
        {
            return _resulContext.Calisans
                      .Include(c => c.Yevmiyelers)
                      .Include(c => c.Odemelers)
                      .Include(c => c.Mesais)
                      .FirstOrDefault(c => c.CalısanId == id);
        }




        public List<Calisan> GetCalisans()
        {
            return _resulContext.Calisans.ToList();
        }

        public async Task<List<Mesai>> GetMesai()
        {
            var yevmiyeList = await _resulContext.Mesais
                           .Include(o => o.Calisan)
                           .ToListAsync();
            return yevmiyeList;
        }
        public async Task<List<Odemeler>> GetOdeme()
        {
            // Veritabanından verileri al
            var odemeListesi = await _resulContext.Odemelers.Include(o => o.Calisan).ToListAsync();



            return odemeListesi;
        }





        public async Task<List<Yevmiyeler>> GetYevmiyeler()
        {
            var yevmiyeList = await _resulContext.Yevmiyelers
                            .Include(o => o.Calisan)
                            .ToListAsync();

            return yevmiyeList;
        }

        public List<Yevmiyeler> GetYevmiyelers()
        {
            var yevmiyeList = _resulContext.Yevmiyelers
                            .Include(o => o.Calisan)
                            .ToList();

            return yevmiyeList;
        }


        public bool KaydetOdeme(int CalisanId, string Aciklama, int tutar, DateTime tarih)
        {
            var odeme = new Odemeler
            {
                CalisanId = CalisanId,
                Amount = tutar,
                Description = Aciklama,
                Tarih = tarih
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



        public bool CalisanDelete(int id)
        {
            // Çalışanı veritabanından al
            var target = _resulContext.Calisans
                            .Include(c => c.Odemelers) // Çalışanın ödemelerini dahil et
                            .Include(c => c.Yevmiyelers)
                            .Include(c => c.Mesais)// Örneğin kayıtlar tablosuyla ilişkiliyse
                            .Include(c => c.Giderlers)
                            // Örneğin kayıtlar tablosuyla ilişkiliyse
                            .FirstOrDefault(x => x.CalısanId == id);

            if (target == null) return false;

            // İlişkili ödemeleri sil
            if (target.Odemelers != null)
            {
                _resulContext.Odemelers.RemoveRange(target.Odemelers);
            }

            // İlişkili kayıtları sil
            if (target.Giderlers != null)
            {
                _resulContext.Giderlers.RemoveRange(target.Giderlers);
            }
            
            if (target.Mesais != null)
            {
                _resulContext.Mesais.RemoveRange(target.Mesais);
            }
            
            if (target.Yevmiyelers != null)
            {
                _resulContext.Yevmiyelers.RemoveRange(target.Yevmiyelers);
            }

            // Çalışanı sil
            _resulContext.Calisans.Remove(target);

            // Değişiklikleri kaydet
            _resulContext.SaveChanges();
            return true;
        }



    }
}
