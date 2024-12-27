using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Berber_Shop.Migrations
{
    public partial class AddIsAdminAndSaltColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rol",
                table: "Kullanicilar",
                newName: "Salt");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Kullanicilar",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Kullanicilar");

            migrationBuilder.RenameColumn(
                name: "Salt",
                table: "Kullanicilar",
                newName: "Rol");
        }
    }
}
