using Microsoft.AspNetCore.Mvc;
using Berber_Shop.Data;
using Berber_Shop.Models;

public class AdminController : Controller
{
    private readonly BerberDbContext _context;

    public AdminController(BerberDbContext context)
    {
        _context = context;
    }

    // الصفحة الرئيسية للمسؤول
    public IActionResult AdminAnasayfa()
    {
        return View();
    }

    // عرض قائمة العاملين
    public IActionResult Calisanlar()
    {
        var calisanlar = _context.Calisanlar.ToList();
        return View(calisanlar);
    }

    // إضافة عامل جديد
    [HttpGet]
    public IActionResult CalisanEkle()
    {
        ViewBag.Hizmetler = _context.Hizmetler.ToList(); // جلب قائمة الخدمات
        return View();
    }

    [HttpPost]
    public IActionResult CalisanEkle(Calisan model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Calisanlar.Add(model);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Çalışan başarıyla eklendi.";
                return RedirectToAction("Calisanlar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Bir hata oluştu: " + ex.Message);
            }
        }
        else
        {
            ModelState.AddModelError("", "Lütfen tüm alanları doğru şekilde doldurun.");
        }
        ViewBag.Hizmetler = _context.Hizmetler.ToList(); // إعادة تعبئة القائمة عند وجود خطأ
        return View(model);
    }



    // تعديل بيانات عامل
    [HttpGet]
    public IActionResult CalisanDuzenle(int id)
    {
        var calisan = _context.Calisanlar.Find(id);
        if (calisan == null)
        {
            TempData["ErrorMessage"] = "Çalışan bulunamadı.";
            return RedirectToAction("Calisanlar");
        }
        return View(calisan);
    }

    [HttpPost]
    public IActionResult CalisanDuzenle(Calisan model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Calisanlar.Update(model);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Çalışan başarıyla güncellendi.";
                return RedirectToAction("Calisanlar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Bir hata oluştu: " + ex.Message);
            }
        }
        return View(model);
    }

    // حذف عامل
    [HttpPost]
    public IActionResult CalisanSil(int id)
    {
        var calisan = _context.Calisanlar.Find(id);
        if (calisan != null)
        {
            try
            {
                _context.Calisanlar.Remove(calisan);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Çalışan başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Çalışan bulunamadı.";
        }
        return RedirectToAction("Calisanlar");
    }
}
