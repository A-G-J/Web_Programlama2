using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Berber_Shop.Migrations
{
    public partial class UpdateCalisanModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Kullanicilar_KullaniciId",
                table: "Randevular");

            migrationBuilder.RenameColumn(
                name: "KullaniciId",
                table: "Randevular",
                newName: "HizmetId");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_KullaniciId",
                table: "Randevular",
                newName: "IX_Randevular_HizmetId");

            migrationBuilder.AddColumn<string>(
                name: "Ad",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimlikNo",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Saat",
                table: "Randevular",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Soyad",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Uzmanlik",
                table: "Calisanlar",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Soyad",
                table: "Calisanlar",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "Calisanlar",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Hizmetler_HizmetId",
                table: "Randevular",
                column: "HizmetId",
                principalTable: "Hizmetler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Hizmetler_HizmetId",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "Ad",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "KimlikNo",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "Saat",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "Soyad",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Calisanlar");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Calisanlar");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Calisanlar");

            migrationBuilder.RenameColumn(
                name: "HizmetId",
                table: "Randevular",
                newName: "KullaniciId");

            migrationBuilder.RenameIndex(
                name: "IX_Randevular_HizmetId",
                table: "Randevular",
                newName: "IX_Randevular_KullaniciId");

            migrationBuilder.AlterColumn<string>(
                name: "Uzmanlik",
                table: "Calisanlar",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Soyad",
                table: "Calisanlar",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "Calisanlar",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Kullanicilar_KullaniciId",
                table: "Randevular",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
