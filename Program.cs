using Berber_Shop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// ????? ????? ??? ???????
builder.Services.AddControllersWithViews();

// ????? ????? ????????
builder.Services.AddDbContext<BerberDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ????? ???????? ???????? ????? ????? ????????
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Hesap/Giris"; // ???? ???? ????? ??????
        options.AccessDeniedPath = "/Hesap/ErisimEngellendi"; // ???? ???? ??? ??????
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // ?????? ?????? ??? 60 ?????
        options.SlidingExpiration = true; // ????? ?????? ??? ??????
    });

// ????? ????? ????????
builder.Services.AddAuthorization();

// ???? ???????
var app = builder.Build();

// ????? ?? ?????? ???????
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // ???? ???? ????? ?? ???? ???????
    app.UseStatusCodePagesWithRedirects("/Home/Error?code={0}"); // ?????? ??????? ????? ??????
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // ????? ????????
app.UseAuthorization(); // ????? ????????

// ????? ???????
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ????? ???????
app.Run();
