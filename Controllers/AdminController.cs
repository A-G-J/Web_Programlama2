using Microsoft.AspNetCore.Mvc;
using Berber_Shop.Data;
using Berber_Shop.Models;

namespace Berber_Shop.Controllers
{
    public class AdminController : Controller
    {
        private readonly BerberDbContext _context;

        public AdminController(BerberDbContext context)
        {
            _context = context;
        }

        // عرض صفحة إضافة العامل
        public IActionResult AddCalisan()
        {
            ViewBag.Hizmetler = _context.Hizmetler.ToList(); // لعرض قائمة الخدمات في النموذج
            return View();
        }

        // استقبال بيانات العامل الجديد
        [HttpPost]
        public IActionResult AddCalisan(Calisan calisan)
        {
            if (ModelState.IsValid)
            {
                _context.Calisanlar.Add(calisan);
                _context.SaveChanges();
                return RedirectToAction("AddCalisan");
            }

            ViewBag.Hizmetler = _context.Hizmetler.ToList(); // إعادة إرسال قائمة الخدمات في حالة وجود خطأ
            return View(calisan);
        }
    }
}
