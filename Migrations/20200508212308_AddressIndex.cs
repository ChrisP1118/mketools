using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeTools.Web.Migrations
{
    public partial class AddressIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "13d76b35-3071-4f2a-befb-8f9facfb39fb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d4545db8-a826-48ee-975c-0f2dd35a5478", "AQAAAAEAACcQAAAAEFeoyMEkvOdRgQbZLwQMdXtlulRjGNv/u1mf1Tfe/Wbrw2R72Xa5QGezp/LiGm21FA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_STREETDIR_STREETNAME_STREETTYPE_HouseNumber",
                table: "Parcels",
                columns: new[] { "STREETDIR", "STREETNAME", "STREETTYPE", "HouseNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_DIR_STREET_STTYPE_HouseNumber",
                table: "Addresses",
                columns: new[] { "DIR", "STREET", "STTYPE", "HouseNumber" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Parcels_STREETDIR_STREETNAME_STREETTYPE_HouseNumber",
                table: "Parcels");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_DIR_STREET_STTYPE_HouseNumber",
                table: "Addresses");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "02ed99b0-536d-471d-8672-612e036e3f20");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b49f4a80-974c-4c7d-9bf5-e1dae1071ba3", "AQAAAAEAACcQAAAAEN1Iez9ivDuQILfrrkAsXITG7DbeWYR/NCw9JxET3sO+5XG+6gPBDLkXTvR0KxMvvw==" });
        }
    }
}
