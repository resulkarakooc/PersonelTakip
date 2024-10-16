﻿using Karakoç.Models;
using Microsoft.EntityFrameworkCore;

namespace Karakoç.Bussiness.abstracts
{
    public interface IAdminService
    {
        List<Calisan> GetCalisans();
        bool KaydetOdeme(int CalisanId, string Aciklama, int tutar);
        List<Odemeler> GetOdeme();
        public List<Yevmiyeler> GetYevmiyeler();
        public bool KaydetYevmiye(DateTime Tarih, List<int> isWorked);

        public Calisan? GetCalisanById(int id);

    }
}
