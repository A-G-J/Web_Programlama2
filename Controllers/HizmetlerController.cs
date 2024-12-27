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
            var hizmetler = _context.Hizmetler.ToList();
            if (hizmetler == null || !hizmetler.Any())
            {
                ViewBag.Mesaj = "Hizmet bulunmamaktadır.";
            }
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
            if (string.IsNullOrWhiteSpace(kimlikNo) || string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(soyad))
            {
                ViewBag.Mesaj = "Lütfen tüm alanları doldurun.";
                return RedirectToAction("Detay", new { hizmetId });
            }

            var hizmet = _context.Hizmetler.FirstOrDefault(h => h.Id == hizmetId);
            var calisan = _context.Calisanlar.FirstOrDefault(c => c.Id == calisanId);

            if (hizmet == null || calisan == null)
                return NotFound("Hizmet veya çalışan bulunamadı.");

            bool mevcut = _context.Randevular.Any(r =>
                r.CalisanId == calisanId && r.Tarih.Date == tarih.Date && r.Saat == saat);

            if (mevcut)
            {
                ViewBag.Mesaj = "Bu tarih ve saat için zaten rezervasyon var!";
            }
            else
            {
                // الحصول على المستخدم الحالي
                var kullaniciId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "KullaniciId")?.Value ?? "0");

                if (kullaniciId == 0)
                {
                    ViewBag.Mesaj = "Lütfen giriş yapın!";
                    return RedirectToAction("Giris", "Hesap");
                }

                // إنشاء موعد جديد
                var yeniRandevu = new Randevu
                {
                    Tarih = tarih.Date,
                    Saat = saat,
                    KimlikNo = kimlikNo,
                    Ad = ad,
                    Soyad = soyad,
                    CalisanId = calisanId,
                    HizmetId = hizmetId,
                    KullaniciId = kullaniciId
                };

                _context.Randevular.Add(yeniRandevu);
                _context.SaveChanges();

                ViewBag.Mesaj = "Rezervasyon başarıyla alındı!";
            }

            var calisanlar = _context.Calisanlar.Where(c => c.HizmetId == hizmetId).ToList();
            ViewBag.Hizmet = hizmet.Ad;
            ViewBag.HizmetId = hizmetId;

            return View("Detay", calisanlar);
        }
    }
}
