    using Berber_Shop.Data;
    using Berber_Shop.Models;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Cryptography;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication;
    using System.Security.Claims;

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
        [HttpGet]
        public IActionResult Giris()
        {
            return View(new GirisModeli());
        }

    [HttpPost]
    public async Task<IActionResult> Giris(GirisModeli model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var kullanici = _context.Kullanicilar.FirstOrDefault(u => u.Email == model.Email);

        if (kullanici != null)
        {
            // تحقق من كلمة المرور المشفرة
            var salt = Convert.FromBase64String(kullanici.Salt);
            var hashedPassword = HashPassword(model.Sifre, salt);

            if (hashedPassword == kullanici.Sifre)
            {
                // إنشاء قائمة الـ Claims للمستخدم
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, kullanici.Email),
                new Claim("FullName", $"{kullanici.Ad} {kullanici.Soyad}"),
                new Claim(ClaimTypes.Role, kullanici.IsAdmin ? "Admin" : "User")
            };

                // إنشاء هوية المستخدم
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // تسجيل الدخول باستخدام هوية المستخدم
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // توجيه المستخدم بناءً على دوره
                return RedirectToAction(kullanici.IsAdmin ? "AdminAnasayfa" : "KullaniciAnasayfa");
            }
            else
            {
                ModelState.AddModelError("", "E-posta veya şifre hatalı.");
            }
        }
        else
        {
            ModelState.AddModelError("", "E-posta veya şifre hatalı.");
        }

        return View(model);
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
