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

    // عرض صفحة التسجيل
    public IActionResult Kayit()
    {
        return View(new KayitModeli());
    }

    // عملية التسجيل
    [HttpPost]
    public async Task<IActionResult> Kayit(KayitModeli model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // التحقق من وجود المستخدم
        var mevcutKullanici = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (mevcutKullanici != null)
        {
            ModelState.AddModelError("", "Bu e-posta adresi zaten kayıtlı.");
            return View(model);
        }

        // إنشاء Salt لتشفير كلمة المرور
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // تشفير كلمة المرور
        var hashedPassword = HashPassword(model.Sifre, salt);

        // إنشاء مستخدم جديد
        var yeniKullanici = new Kullanici
        {
            Ad = model.Ad,
            Email = model.Email,
            Sifre = hashedPassword,
            Salt = Convert.ToBase64String(salt),
            IsAdmin = false
        };

        _context.Kullanicilar.Add(yeniKullanici);
        await _context.SaveChangesAsync();

        // إعادة التوجيه إلى صفحة تسجيل الدخول بعد التسجيل الناجح
        return RedirectToAction("Giris");
    }

    // عرض صفحة تسجيل الدخول
    [HttpGet]
    public IActionResult Giris()
    {
        return View(new GirisModeli());
    }

    // عملية تسجيل الدخول
    [HttpPost]
    public async Task<IActionResult> Giris(GirisModeli model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (kullanici != null)
        {
            // التحقق من كلمة المرور المشفرة
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
                return RedirectToAction(kullanici.IsAdmin ? "AdminAnasayfa" : "KullaniciAnasayfa", kullanici.IsAdmin ? "Admin" : "Kullanici");
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

    // دالة لتشفير كلمة المرور
    private string HashPassword(string password, byte[] salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
    }
}
