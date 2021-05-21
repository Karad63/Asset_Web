using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Asset_Web.Data.Migrations
{
    public partial class AddExRateUSDAndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExRateDate",
                table: "Currency",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "ExRateUSD",
                table: "Currency",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExRateDate",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "ExRateUSD",
                table: "Currency");
        }
    }
}
