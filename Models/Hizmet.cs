using System.ComponentModel.DataAnnotations.Schema;

namespace Berber_Shop.Models
{
    public class Hizmet
    {
        public int Id { get; set; }
        public string? Ad { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Fiyat { get; set; } // حدد الدقة والنطاق

    }
}
