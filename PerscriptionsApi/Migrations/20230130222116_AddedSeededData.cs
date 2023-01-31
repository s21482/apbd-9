using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerscriptionsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeededData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "IdPatient", "BirthDate", "FirstName", "LastName" },
                values: new object[] { 1, new DateTime(1991, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mateusz", "Dziuba" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "IdPatient",
                keyValue: 1);
        }
    }
}
