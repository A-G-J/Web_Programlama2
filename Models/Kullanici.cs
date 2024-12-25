namespace Berber_Shop.Models
{
    //comment
    public class Kullanici
    {
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? Email { get; set; }
        public string? Sifre { get; set; }
        public bool IsAdmin { get; set; } = false;
        public string? Salt { get; set; }
    }
}
// comment
// comment
