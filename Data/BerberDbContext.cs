using Microsoft.EntityFrameworkCore;
using Berber_Shop.Models;

namespace Berber_Shop.Data
{
    public class BerberDbContext : DbContext
    {
        public BerberDbContext(DbContextOptions<BerberDbContext> options) : base(options) { }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Randevu> Randevular { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تعديل العلاقة بين Randevular و Hizmetler لتحديد سلوك الحذف
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Hizmet) // العلاقة مع Hizmet
                .WithMany() // تحديد العلاقة العكسية إذا لزم الأمر
                .HasForeignKey(r => r.HizmetId)
                .OnDelete(DeleteBehavior.Restrict); // أو استخدم DeleteBehavior.SetNull حسب الحاجة

            // يمكنك إضافة المزيد من التعديلات على العلاقات الأخرى إذا لزم الأمر
        }
    }
}
