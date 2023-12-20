using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _23._1News.Data.Migrations
{
    public partial class ReceiveNewsletters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReceiveNewsletters",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiveNewsletters",
                table: "AspNetUsers");
        }
    }
}
