using Microsoft.AspNetCore.Mvc;
using Berber_Shop.Data;
using Berber_Shop.Models;

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
        if (ModelState.IsValid)
        {
            _context.Randevular.Add(model);
            _context.SaveChanges();
            return RedirectToAction("KullaniciAnasayfa");
        }
        return View(model);
    }

    // عرض مواعيد المستخدم
    public IActionResult Randevularim()
    {
        var randevular = _context.Randevular.ToList(); // يمكن تحسين الكود لتحديد مواعيد المستخدم الحالي فقط
        return View(randevular);
    }
}
