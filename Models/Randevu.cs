using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berber_Shop.Models
{
    public class Randevu
    {
        public int Id { get; set; } // المعرف الأساسي للرانديفو
        public DateTime Tarih { get; set; } // تاريخ الموعد
        public TimeSpan Saat { get; set; } // وقت الموعد
        public int CalisanId { get; set; } // معرف العامل المرتبط بالموعد
        public int KullaniciId { get; set; } // معرف المستخدم الذي حجز الموعد
        public int HizmetId { get; set; } // معرف الخدمة المرتبطة بالموعد

        // خصائص إضافية
        public string? KimlikNo { get; set; } // رقم الهوية
        public string? Ad { get; set; } // اسم المستخدم أو الحاجز
        public string? Soyad { get; set; } // لقب المستخدم أو الحاجز

        // العلاقات
        public Calisan? Calisan { get; set; } // العلاقة مع الكيان Calisan
        public Kullanici? Kullanici { get; set; } // العلاقة مع الكيان Kullanici
        public Hizmet? Hizmet { get; set; } // العلاقة مع الكيان Hizmet

        [NotMapped]
        public List<TimeSpan>? AvailableTimes { get; set; }
    }
}
