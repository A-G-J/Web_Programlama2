using Berber_Shop.Data;
using Berber_Shop.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

public class HesapController : Controller
{
    private readonly BerberDbContext _context;

    public HesapController(BerberDbContext context)
    {
        _context = context;
    }

    // Gösterim için kayıt sayfası
    public IActionResult Kayit()
    {
        return View(new KayitModeli());
    }

    // Kayıt işlemi
    [HttpPost]
    public async Task<IActionResult> Kayit(KayitModeli model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); // Eğer model geçersizse tekrar sayfayı göster
        }

        // Kullanıcı kontrolü
        var mevcutKullanici = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (mevcutKullanici != null)
        {
            ModelState.AddModelError("", "Bu e-posta adresi zaten kayıtlı.");
            return View(model);
        }

        // Şifreleme işlemi için Salt oluşturulması
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt); // Salt değeri oluşturuluyor
        }

        // Şifreyi hash'leme
        var hashedPassword = HashPassword(model.Sifre, salt);

        // Yeni kullanıcı oluşturuluyor ve veritabanına ekleniyor
        var yeniKullanici = new Kullanici
        {
            Ad = model.Ad,
            Email = model.Email,
            Sifre = hashedPassword, // Şifre hash'lenmiş
            Salt = Convert.ToBase64String(salt), // Salt veritabanına kaydediliyor
            IsAdmin = false // Varsayılan olarak admin değil
        };

        _context.Kullanicilar.Add(yeniKullanici);
        await _context.SaveChangesAsync();

        // Başarılı bir kayıt sonrası giriş sayfasına yönlendirme
        return RedirectToAction("Giris");
    }

    // Giriş işlemi
    [HttpPost]
    public async Task<IActionResult> Giris(GirisModeli model)
    {
        // البحث عن المستخدم باستخدام البريد الإلكتروني
        var kullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(u => u.Email == model.Email);

        // إذا كان المستخدم موجودًا
        if (kullanici != null)
        {
            // أخذ الـ Salt المخزن من قاعدة البيانات
            var salt = Convert.FromBase64String(kullanici.Salt);

            // حساب الـ hashedPassword باستخدام الـ Salt الذي تم استخراجه
            var hashedPassword = HashPassword(model.Sifre, salt);

            // مقارنة كلمة المرور المدخلة مع الـ hashedPassword المخزن
            if (hashedPassword == kullanici.Sifre)
            {
                // إذا كان المستخدم هو مشرف (Admin)
                if (kullanici.IsAdmin)
                {
                    return RedirectToAction("AdminAnasayfa", "Admin");
                }
                else
                {
                    // إذا كان المستخدم غير مشرف (عادة مستخدم عادي)
                    return RedirectToAction("KullaniciAnasayfa", "Kullanici");
                }
            }
        }

        // إذا فشل التحقق، أضف رسالة خطأ إلى ModelState
        ModelState.AddModelError("", "E-posta veya şifre hatalı.");
        return View(model); // عرض النموذج مع رسائل الأخطاء
    }


    // Şifreyi hash'lemek için yardımcı metot
    private string HashPassword(string password, byte[] salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8)); // Şifre hash'leniyor
    }
}
