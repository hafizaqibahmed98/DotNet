using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicStructure.Migrations
{
    public partial class UserRolesSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "16907159-7a2a-4dba-a05d-53810767803f", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6ad2f70e-80a8-4531-b21d-e2b5c4310c87", "3", "Worker", "Worker" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c1c27f9a-8c93-4ec6-b7fc-4d21ca2e959a", "2", "Manager", "Manager" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16907159-7a2a-4dba-a05d-53810767803f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6ad2f70e-80a8-4531-b21d-e2b5c4310c87");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1c27f9a-8c93-4ec6-b7fc-4d21ca2e959a");
        }
    }
}
