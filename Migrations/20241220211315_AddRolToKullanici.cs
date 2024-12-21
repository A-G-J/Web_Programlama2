using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Berber_Shop.Migrations
{
    public partial class AddRolToKullanici : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Calisanlar");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Calisanlar");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Calisanlar");

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Kullanicilar");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Calisanlar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Calisanlar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "Calisanlar",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }
    }
}
