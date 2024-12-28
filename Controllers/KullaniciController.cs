using Microsoft.AspNetCore.Mvc;
using Berber_Shop.Data;
using Berber_Shop.Models;
using System.Linq;

public class KullaniciController : Controller
{
    private readonly BerberDbContext _context;

    public KullaniciController(BerberDbContext context)
    {
        _context = context;
    }

    // الصفحة الرئيسية للمستخدم
    public IActionResult KullaniciAnasayfa()
    {
        return View();
    }

    // عرض الخدمات المتاحة
    public IActionResult Hizmetler()
    {
        var hizmetler = _context.Hizmetler.ToList();
        return View(hizmetler);
    }

    // عرض الأوقات المتاحة
    private List<TimeSpan> GetAvailableTimes(DateTime date, int calisanId)
    {
        // التأكد من أن اليوم يقع بين الاثنين والخميس
        if (date.DayOfWeek < DayOfWeek.Monday || date.DayOfWeek > DayOfWeek.Thursday)
        {
            return new List<TimeSpan>(); // إذا كان اليوم خارج المدى المحدد، لا توجد أوقات متاحة
        }

        // الأوقات الافتراضية للعمل: من 9 صباحًا حتى 5 مساءً
        var workHours = Enumerable.Range(9, 9).Select(h => new TimeSpan(h, 0, 0)).ToList();

        // جلب الأوقات المحجوزة من قاعدة البيانات
        var bookedTimes = _context.Randevular
            .Where(r => r.Tarih.Date == date.Date && r.CalisanId == calisanId)
            .Select(r => r.Saat)
            .ToList();

        // استخراج الأوقات المتاحة
        var availableTimes = workHours.Except(bookedTimes).ToList();

        return availableTimes;
    }


    [HttpGet]
    public IActionResult YeniRandevuAl()
    {
        var tarih = DateTime.Now.Date; // افتراض تاريخ اليوم الحالي
        var calisanId = _context.Calisanlar.FirstOrDefault()?.Id ?? 0; // العامل الافتراضي

        var availableTimes = GetAvailableTimes(tarih, calisanId);

        ViewBag.Hizmetler = _context.Hizmetler.ToList(); // جلب الخدمات
        ViewBag.AvailableTimes = availableTimes; // جلب الأوقات المتاحة

        return View();
    }



    // عرض الأوقات المتاحة مع تاريخ وعامل (GET)
    [HttpGet("AvailableTimes")]
    public IActionResult AvailableTimes(DateTime? date, int? calisanId)
    {
        if (date == null)
        {
            date = DateTime.Now.Date; // التاريخ الافتراضي هو اليوم الحالي
        }

        if (calisanId == null)
        {
            calisanId = _context.Calisanlar.FirstOrDefault()?.Id ?? 0; // العامل الافتراضي
        }

        var availableTimes = GetAvailableTimes(date.Value, calisanId.Value);

        ViewBag.AvailableTimes = availableTimes;
        ViewBag.SelectedDate = date.Value;
        ViewBag.SelectedCalisanId = calisanId.Value;

        return View();
    }

    // حجز موعد - معالجة النموذج (POST)
    [HttpPost("YeniRandevuAl")]
    public IActionResult YeniRandevuAlPost(Randevu model)
    {
        // استرداد المستخدم الحالي بناءً على البريد الإلكتروني
        var kullaniciEmail = HttpContext.User.Identity.Name; // استرداد البريد الإلكتروني للمستخدم الحالي
        var mevcutKullanici = _context.Kullanicilar.FirstOrDefault(u => u.Email == kullaniciEmail);

        if (mevcutKullanici == null)
        {
            return RedirectToAction("Giris", "Hesap");
        }

        if (ModelState.IsValid)
        {
            // إضافة معرف المستخدم الحالي إلى الحجز
            model.KullaniciId = mevcutKullanici.Id;

            _context.Randevular.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Randevularim");
        }

        ViewBag.Hizmetler = _context.Hizmetler.ToList();
        return View("YeniRandevuAl", model);
    }

    // عرض مواعيد المستخدم
    public IActionResult Randevularim()
    {
        var kullaniciEmail = HttpContext.User.Identity.Name; // استرداد البريد الإلكتروني للمستخدم الحالي
        var mevcutKullanici = _context.Kullanicilar.FirstOrDefault(u => u.Email == kullaniciEmail);

        if (mevcutKullanici == null)
        {
            return RedirectToAction("Giris", "Hesap");
        }

        // عرض المواعيد المرتبطة بالمستخدم الحالي فقط
        var randevular = _context.Randevular
            .Where(r => r.KullaniciId == mevcutKullanici.Id)
            .ToList();

        return View(randevular);
    }
    [HttpGet]
    public IActionResult SacModelleri()
    {
        return View();
    }

}