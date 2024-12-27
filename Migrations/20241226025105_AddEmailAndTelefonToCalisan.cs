using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Berber_Shop.Migrations
{
    public partial class AddEmailAndTelefonToCalisan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Calisanlar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "Calisanlar",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Calisanlar");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Calisanlar");
        }
    }
}
