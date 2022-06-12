using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "DoB", "FullName", "Gender", "KnownFor", "Nationality", "Status" },
                values: new object[] { "4", new DateTime(2022, 6, 11, 22, 28, 42, 197, DateTimeKind.Local).AddTicks(9920), "Tony Stark", "Female", "Iron man", "American", false });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "DoB", "FullName", "Gender", "KnownFor", "Nationality", "Status" },
                values: new object[] { "5", new DateTime(2022, 6, 11, 22, 28, 42, 197, DateTimeKind.Local).AddTicks(9990), "Tony Dark", "Trans", "Spider man", "American psycho", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "DoB", "FullName", "Gender", "KnownFor", "Nationality", "Status" },
                values: new object[] { "1", new DateTime(2022, 6, 11, 22, 25, 42, 33, DateTimeKind.Local).AddTicks(4850), "Tony Stark", "Female", "Iron man", "American", false });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "DoB", "FullName", "Gender", "KnownFor", "Nationality", "Status" },
                values: new object[] { "2", new DateTime(2022, 6, 11, 22, 25, 42, 33, DateTimeKind.Local).AddTicks(4900), "Tony Dark", "Trans", "Spider man", "American psycho", false });
        }
    }
}
