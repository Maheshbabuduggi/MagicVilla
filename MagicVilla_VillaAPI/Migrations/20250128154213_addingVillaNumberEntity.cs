using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class addingVillaNumberEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VillaNumbers",
                columns: table => new
                {
                    VillaNO = table.Column<int>(type: "int", nullable: false),
                    SpecialDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillaNumbers", x => x.VillaNO);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 28, 21, 12, 12, 571, DateTimeKind.Local).AddTicks(6213), new DateTime(2025, 1, 28, 21, 12, 12, 571, DateTimeKind.Local).AddTicks(6201) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 28, 21, 12, 12, 571, DateTimeKind.Local).AddTicks(6218), new DateTime(2025, 1, 28, 21, 12, 12, 571, DateTimeKind.Local).AddTicks(6216) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 28, 21, 12, 12, 571, DateTimeKind.Local).AddTicks(6221), new DateTime(2025, 1, 28, 21, 12, 12, 571, DateTimeKind.Local).AddTicks(6219) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 28, 21, 12, 12, 571, DateTimeKind.Local).AddTicks(6224), new DateTime(2025, 1, 28, 21, 12, 12, 571, DateTimeKind.Local).AddTicks(6222) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 28, 21, 12, 12, 571, DateTimeKind.Local).AddTicks(6226), new DateTime(2025, 1, 28, 21, 12, 12, 571, DateTimeKind.Local).AddTicks(6225) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VillaNumbers");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 27, 22, 26, 25, 49, DateTimeKind.Local).AddTicks(3602), new DateTime(2025, 1, 27, 22, 26, 25, 49, DateTimeKind.Local).AddTicks(3594) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 27, 22, 26, 25, 49, DateTimeKind.Local).AddTicks(3604), new DateTime(2025, 1, 27, 22, 26, 25, 49, DateTimeKind.Local).AddTicks(3603) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 27, 22, 26, 25, 49, DateTimeKind.Local).AddTicks(3607), new DateTime(2025, 1, 27, 22, 26, 25, 49, DateTimeKind.Local).AddTicks(3605) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 27, 22, 26, 25, 49, DateTimeKind.Local).AddTicks(3609), new DateTime(2025, 1, 27, 22, 26, 25, 49, DateTimeKind.Local).AddTicks(3607) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 27, 22, 26, 25, 49, DateTimeKind.Local).AddTicks(3611), new DateTime(2025, 1, 27, 22, 26, 25, 49, DateTimeKind.Local).AddTicks(3609) });
        }
    }
}
