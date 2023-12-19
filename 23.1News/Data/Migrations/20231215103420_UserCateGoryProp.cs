using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _23._1News.Data.Migrations
{
    public partial class UserCateGoryProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedCategoryId",
                table: "AspNetUsers");

            //migrationBuilder.AddColumn<bool>(
            //    name: "ReceiveNewsletters",
            //    table: "AspNetUsers",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CategoryUser",
                columns: table => new
                {
                    CategoryUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserCategoriesCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryUser", x => new { x.CategoryUsersId, x.UserCategoriesCategoryId });
                    table.ForeignKey(
                        name: "FK_CategoryUser_AspNetUsers_CategoryUsersId",
                        column: x => x.CategoryUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryUser_Categories_UserCategoriesCategoryId",
                        column: x => x.UserCategoriesCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryUser_UserCategoriesCategoryId",
                table: "CategoryUser",
                column: "UserCategoriesCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryUser");

            migrationBuilder.DropColumn(
                name: "ReceiveNewsletters",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "SelectedCategoryId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
