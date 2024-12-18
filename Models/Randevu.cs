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
        public Calisan? Calisan { get; set; }

        public int HizmetId { get; set; }
        public Hizmet? Hizmet { get; set; }
    }
}
