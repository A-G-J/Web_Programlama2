using System.ComponentModel.DataAnnotations;

namespace Berber_Shop.Models
{
    public class GirisModeli
    {
        [Required(ErrorMessage = "E-posta gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir")]
        public string? Sifre { get; set; }
    }

}
