using Berber_Shop.Data;
using Berber_Shop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Berber_Shop.Controllers
{
    public class HizmetlerController : Controller
    {
        private readonly BerberDbContext _context;

        public HizmetlerController(BerberDbContext context)
        {
            _context = context;
        }

        // عرض قائمة الخدمات
        public IActionResult Index()
        {
            // استرجاع الخدمات من قاعدة البيانات
            var hizmetler = _context.Hizmetler.ToList();

            // فحص إذا كانت القائمة فارغة أو null
            if (hizmetler == null || !hizmetler.Any())
            {
                ViewBag.Mesaj = "No services available.";  // رسالة عند عدم وجود خدمات
            }

            // إرجاع الخدمات إلى العرض
            return View(hizmetler);
        }

        // عرض تفاصيل الخدمة
        public IActionResult Detay(int hizmetId)
        {
            var hizmet = _context.Hizmetler.FirstOrDefault(h => h.Id == hizmetId);
            if (hizmet == null) return NotFound("Hizmet bulunamadı.");

            var calisanlar = _context.Calisanlar.Where(c => c.HizmetId == hizmetId).ToList();

            ViewBag.Hizmet = hizmet.Ad;
            ViewBag.HizmetId = hizmet.Id;

            // إذا لم يوجد أي عاملين، تعيين رسالة في ViewBag
            if (calisanlar == null || !calisanlar.Any())
            {
                ViewBag.CalisanMesaj = "Bu hizmet için herhangi bir çalışan bulunmamaktadır.";
            }

            return View(calisanlar);
        }



        // حجز موعد
        [HttpPost]
        public IActionResult RandevuAl(int calisanId, int hizmetId, DateTime tarih, TimeSpan saat, string kimlikNo, string ad, string soyad)
        {
            // التحقق من صحة الخدمة والعامل
            var hizmet = _context.Hizmetler.FirstOrDefault(h => h.Id == hizmetId);
            var calisan = _context.Calisanlar.FirstOrDefault(c => c.Id == calisanId);

            // في حالة عدم وجود الخدمة أو العامل
            if (hizmet == null || calisan == null)
                return NotFound("Hizmet veya çalışan bulunamadı.");

            // التحقق من وجود موعد بنفس التاريخ والساعة
            bool mevcut = _context.Randevular.Any(r =>
                r.CalisanId == calisanId && r.Tarih.Date == tarih.Date && r.Saat == saat);

            if (mevcut)
            {
                ViewBag.Mesaj = "Bu tarih ve saat için zaten rezervasyon var!";
            }
            else
            {
                // إنشاء موعد جديد
                var yeniRandevu = new Randevu
                {
                    Tarih = tarih.Date,
                    Saat = saat,
                    KimlikNo = kimlikNo,
                    Ad = ad,
                    Soyad = soyad,
                    CalisanId = calisanId,
                    HizmetId = hizmetId
                };

                // إضافة الموعد إلى قاعدة البيانات
                _context.Randevular.Add(yeniRandevu);
                _context.SaveChanges();

                ViewBag.Mesaj = "Rezervasyon başarıyla alındı!";
            }

            // جلب العاملين الذين يقدمون نفس الخدمة بعد الحجز
            var calisanlar = _context.Calisanlar.Where(c => c.HizmetId == hizmetId).ToList();
            ViewBag.Hizmet = hizmet.Ad;
            ViewBag.HizmetId = hizmetId;

            return View("Detay", calisanlar);
        }
    }
}
