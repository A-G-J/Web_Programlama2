
using System.ComponentModel.DataAnnotations;
namespace Berber_Shop.Models
{
    public class KayitModeli
    {
        [Required(ErrorMessage = "Adınızı giriniz.")]
        public string? Ad { get; set; }

        [Required(ErrorMessage = "E-posta adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifrenizi giriniz.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string? Sifre { get; set; }

        [Required(ErrorMessage = "Şifreyi tekrar giriniz.")]
        [Compare("Sifre", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string? SifreTekrar { get; set; }
    }
}
