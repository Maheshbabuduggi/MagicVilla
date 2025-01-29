using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedForeignkeytoVillaNumbers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VillaId",
                table: "VillaNumbers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 28, 21, 43, 18, 481, DateTimeKind.Local).AddTicks(9620), new DateTime(2025, 1, 28, 21, 43, 18, 481, DateTimeKind.Local).AddTicks(9611) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 28, 21, 43, 18, 481, DateTimeKind.Local).AddTicks(9625), new DateTime(2025, 1, 28, 21, 43, 18, 481, DateTimeKind.Local).AddTicks(9622) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 28, 21, 43, 18, 481, DateTimeKind.Local).AddTicks(9628), new DateTime(2025, 1, 28, 21, 43, 18, 481, DateTimeKind.Local).AddTicks(9627) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 28, 21, 43, 18, 481, DateTimeKind.Local).AddTicks(9632), new DateTime(2025, 1, 28, 21, 43, 18, 481, DateTimeKind.Local).AddTicks(9630) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 1, 28, 21, 43, 18, 481, DateTimeKind.Local).AddTicks(9636), new DateTime(2025, 1, 28, 21, 43, 18, 481, DateTimeKind.Local).AddTicks(9634) });

            migrationBuilder.CreateIndex(
                name: "IX_VillaNumbers_VillaId",
                table: "VillaNumbers",
                column: "VillaId");

            migrationBuilder.AddForeignKey(
                name: "FK_VillaNumbers_Villas_VillaId",
                table: "VillaNumbers",
                column: "VillaId",
                principalTable: "Villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VillaNumbers_Villas_VillaId",
                table: "VillaNumbers");

            migrationBuilder.DropIndex(
                name: "IX_VillaNumbers_VillaId",
                table: "VillaNumbers");

            migrationBuilder.DropColumn(
                name: "VillaId",
                table: "VillaNumbers");

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
    }
}
