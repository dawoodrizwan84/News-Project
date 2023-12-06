using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _23._1News.Data.Migrations
{
    public partial class Weeklabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricalWeatherDatas");

            migrationBuilder.AddColumn<int>(
                name: "SubscriberCount",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserIdentifier",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WeekLabel",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "WeeklySubscriptionData",
                columns: table => new
                {
                    WeekLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriberCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeeklySubscriptionData");

            migrationBuilder.DropColumn(
                name: "SubscriberCount",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "UserIdentifier",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "WeekLabel",
                table: "Subscriptions");

            migrationBuilder.CreateTable(
                name: "HistoricalWeatherDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Humidity = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemperatureCelsius = table.Column<int>(type: "int", nullable: false),
                    WindSpeed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricalWeatherDatas", x => x.Id);
                });
        }
    }
}
