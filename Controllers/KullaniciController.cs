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

    // حجز موعد
    [HttpGet]
    public IActionResult RandevuAl()
    {
        return View();
    }

    [HttpPost]
    public IActionResult RandevuAl(Randevu model)
    {
        var kullanici = HttpContext.User.Identity.Name; // اسم المستخدم المسجل حاليًا
        var mevcutKullanici = _context.Kullanicilar.FirstOrDefault(u => u.Email == kullanici);

        if (mevcutKullanici == null)
        {
            ModelState.AddModelError("", "Lütfen giriş yapın.");
            return RedirectToAction("Giris", "Hesap");
        }

        if (ModelState.IsValid)
        {
            // إرفاق KullaniciId بناءً على المستخدم الحالي
            model.KullaniciId = mevcutKullanici.Id;

            _context.Randevular.Add(model);
            _context.SaveChanges();
            return RedirectToAction("KullaniciAnasayfa");
        }
        return View(model);
    }

    public IActionResult Randevularim()
    {
        if (!User.Identity.IsAuthenticated)
        {
            TempData["ErrorMessage"] = "Lütfen giriş yapın.";
            return RedirectToAction("Giris", "Hesap");
        }

        var kullaniciEmail = User.Identity.Name; // يحصل على البريد الإلكتروني من المستخدم الحالي
        var mevcutKullanici = _context.Kullanicilar.FirstOrDefault(u => u.Email == kullaniciEmail);

        if (mevcutKullanici == null)
        {
            TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
            return RedirectToAction("Giris", "Hesap");
        }

        var randevular = _context.Randevular
            .Where(r => r.KullaniciId == mevcutKullanici.Id)
            .ToList();

        return View(randevular);
    }


}
