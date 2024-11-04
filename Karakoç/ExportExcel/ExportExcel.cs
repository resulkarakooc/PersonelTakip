using ClosedXML.Excel;
using Karakoç.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Karakoç.ExportExcel
{
    public static class ExportExcel
    {
        public static IActionResult GenerateExcelWithWorkdays(ResulContext context, string templatePath)
        {
            // Çalışan verilerini al
            var calisanlar = context.Calisans
                .Include(x => x.Yevmiyelers) // İlişkili Yevmiye verilerini de dahil et
                .ToList();

            // Template dosyasını templatePath üzerinden yükle
            using (var workbook = new XLWorkbook(templatePath))
            {
                var worksheet = workbook.Worksheet(1); // İlk sayfayı seç

                // Başlangıç satırı
                int startRow = 2;

                foreach (var calisan in calisanlar)
                {
                    worksheet.Cell(startRow, 1).Value = calisan.CalısanId;
                    worksheet.Cell(startRow, 2).Value = calisan.Name;
                    worksheet.Cell(startRow, 3).Value = calisan.Email;

                    // Çalışma günlerini kontrol et
                    for (int day = 1; day <= 31; day++) // Ayın günlerini dolaş
                    {
                        // İlgili günü bul
                        var workday = calisan.Yevmiyelers.FirstOrDefault(w => w.Tarih.Value.Day == day);

                        if (workday != null && workday.IsWorked == true)
                        {
                            // İlgili gün sütununa git ve değer ekle, arka plan rengini mavi yap
                            var cell = worksheet.Cell(startRow, 3 + day);
                            cell.Value = "✓";
                            cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                        }
                    }
                    startRow++;
                }

                // Dosyayı döndür
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return new FileContentResult(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "Workdays.xlsx"
                    };
                }
            }
        }
    }
}
