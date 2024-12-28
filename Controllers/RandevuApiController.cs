using Microsoft.AspNetCore.Mvc;
using Berber_Shop.Data;
using Berber_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class RandevuApiController : ControllerBase
{
    private readonly BerberDbContext _context;

    public RandevuApiController(BerberDbContext context)
    {
        _context = context;
    }

    // جلب الأوقات المتاحة للعامل
    [HttpGet("MevcutSaatler")]
    public IActionResult MevcutSaatler(DateTime tarih, int calisanId)
    {
        try
        {
            // التأكد من أن اليوم بين الاثنين والخميس
            if (tarih.DayOfWeek < DayOfWeek.Monday || tarih.DayOfWeek > DayOfWeek.Friday)
            {
                return Ok(new List<TimeSpan>()); // إذا لم يكن اليوم بين الاثنين والخميس، ارجع قائمة فارغة
            }

            // تحديد ساعات العمل (9 صباحًا - 5 مساءً)
            var calismaSaatleri = Enumerable.Range(9, 9).Select(saat => new TimeSpan(saat, 0, 0)).ToList();

            // جلب الأوقات المحجوزة للعامل في التاريخ المحدد
            var rezerveEdilenSaatler = _context.Randevular
                .Where(r => r.Tarih.Date == tarih.Date && r.CalisanId == calisanId)
                .Select(r => r.Saat)
                .ToList();

            // استخراج الساعات المتاحة
            var mevcutSaatler = calismaSaatleri.Except(rezerveEdilenSaatler).ToList();

            return Ok(mevcutSaatler);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
        }
    }

    // إنشاء موعد جديد
    [HttpPost("RandevuOlustur")]
    public IActionResult RandevuOlustur([FromBody] Randevu model)
    {
        if (model == null)
        {
            return BadRequest("Geçersiz randevu bilgileri.");
        }

        try
        {
            // التحقق من وجود العامل
            var mevcutCalisan = _context.Calisanlar.FirstOrDefault(c => c.Id == model.CalisanId);
            if (mevcutCalisan == null)
            {
                return NotFound("Çalışan bulunamadı.");
            }

            // التحقق من وجود الخدمة
            var mevcutHizmet = _context.Hizmetler.FirstOrDefault(h => h.Id == model.HizmetId);
            if (mevcutHizmet == null)
            {
                return NotFound("Hizmet bulunamadı.");
            }

            // التحقق من توفر الساعة المحددة
            var rezerveEdilenSaatler = _context.Randevular
                .Where(r => r.Tarih.Date == model.Tarih.Date && r.CalisanId == model.CalisanId)
                .Select(r => r.Saat)
                .ToList();

            if (rezerveEdilenSaatler.Contains(model.Saat))
            {
                return Conflict("Bu saat için randevu zaten alınmış.");
            }

            // إضافة الموعد إلى قاعدة البيانات
            _context.Randevular.Add(model);
            _context.SaveChanges();

            return Ok("Randevu başarıyla oluşturuldu.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
        }
    }
}
