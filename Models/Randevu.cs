using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berber_Shop.Models
{
    public class Randevu
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public TimeSpan Saat { get; set; }

        public string? KimlikNo { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }

        public int CalisanId { get; set; }
        [ForeignKey("CalisanId")]
        public Calisan? Calisan { get; set; }

        public int HizmetId { get; set; }
        [ForeignKey("HizmetId")]
        public Hizmet? Hizmet { get; set; }

        // إضافة خاصية KullaniciId لربط الموعد بالمستخدم
        public int KullaniciId { get; set; }
        [ForeignKey("KullaniciId")]
        public Kullanici? Kullanici { get; set; }
    }
}
