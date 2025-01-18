using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CollegeApp_2.Migrations
{
    /// <inheritdoc />
    public partial class AddDateToStudentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Adres", "DOB", "Email", "StudentName" },
                values: new object[,]
                {
                    { 1, "Hyd, INDIA", new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "yunus@hotmail.com", "Yunus" },
                    { 2, "Banglore, INDIA", new DateTime(2022, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Anil@hotmail.com", "Anil" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
