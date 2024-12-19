using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berber_Shop.Models
{
    public class Calisan
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "Ad 50 karakterden fazla olamaz.")]
        public string? Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "Soyad 50 karakterden fazla olamaz.")]
        public string? Soyad { get; set; }

        [StringLength(100, ErrorMessage = "Uzmanlık 100 karakterden fazla olamaz.")]
        public string? Uzmanlik { get; set; }

        // رقم تعريف الخدمة المرتبط بالعامل
        [Required]
        public int HizmetId { get; set; }

        [ForeignKey("HizmetId")]
        public Hizmet? Hizmet { get; set; }

        // معلومات إضافية (مثل الهاتف والبريد الإلكتروني)
        [StringLength(15, ErrorMessage = "Telefon numarası 15 karakterden fazla olamaz.")]
        public string? Telefon { get; set; }

        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        public string? Email { get; set; }

        // حالة العامل (نشط / غير نشط)
        public bool IsActive { get; set; } = true;
    }
}
