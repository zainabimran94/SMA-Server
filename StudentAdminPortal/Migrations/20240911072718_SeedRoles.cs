using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentAdminPortal.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "124a1abb-b08c-4ac0-908e-81787fc01329");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd5c9bfe-2a11-464c-9ae4-a15f35a55ddb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8a6f1e55-30d3-4fb6-806e-474020f66b30", null, "Admin", "ADMIN" },
                    { "e08bcfc1-257d-4d9b-842f-b95fe0ac16d6", null, "Student", "STUDENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a6f1e55-30d3-4fb6-806e-474020f66b30");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e08bcfc1-257d-4d9b-842f-b95fe0ac16d6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "124a1abb-b08c-4ac0-908e-81787fc01329", null, "Admin", "ADMIN" },
                    { "fd5c9bfe-2a11-464c-9ae4-a15f35a55ddb", null, "Student", "STUDENT" }
                });
        }
    }
}
