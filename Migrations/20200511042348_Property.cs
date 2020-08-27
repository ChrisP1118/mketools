using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeTools.Web.Migrations
{
    public partial class Property : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    TAXKEY = table.Column<string>(maxLength: 10, nullable: false),
                    Source = table.Column<string>(maxLength: 12, nullable: true),
                    YR_ASSMT = table.Column<string>(maxLength: 4, nullable: true),
                    CHK_DIGIT = table.Column<string>(maxLength: 1, nullable: true),
                    TAX_RATE_CD = table.Column<string>(maxLength: 2, nullable: true),
                    PLAT_PAGE = table.Column<string>(maxLength: 5, nullable: true),
                    HOUSE_NR_LO = table.Column<int>(nullable: true),
                    HOUSE_NR_HI = table.Column<int>(nullable: true),
                    HOUSE_NR_SFX = table.Column<string>(maxLength: 3, nullable: true),
                    SDIR = table.Column<string>(maxLength: 1, nullable: true),
                    STREET = table.Column<string>(maxLength: 18, nullable: true),
                    STTYPE = table.Column<string>(maxLength: 2, nullable: true),
                    C_A_CLASS = table.Column<string>(maxLength: 1, nullable: true),
                    C_A_SYMBOL = table.Column<string>(maxLength: 1, nullable: true),
                    C_A_LAND = table.Column<int>(nullable: true),
                    C_A_IMPRV = table.Column<int>(nullable: true),
                    C_A_TOTAL = table.Column<int>(nullable: true),
                    C_A_EXM_TYPE = table.Column<string>(maxLength: 3, nullable: true),
                    C_A_EXM_LAND = table.Column<int>(nullable: true),
                    C_A_EXM_IMPRV = table.Column<int>(nullable: true),
                    C_A_EXM_TOTAL = table.Column<int>(nullable: true),
                    P_A_CLASS = table.Column<string>(maxLength: 1, nullable: true),
                    P_A_SYMBOL = table.Column<string>(maxLength: 4, nullable: true),
                    P_A_LAND = table.Column<int>(nullable: true),
                    P_A_IMPRV = table.Column<int>(nullable: true),
                    P_A_TOTAL = table.Column<int>(nullable: true),
                    P_A_EXM_LAND = table.Column<int>(nullable: true),
                    P_A_EXM_IMPRV = table.Column<int>(nullable: true),
                    P_A_EXM_TOTAL = table.Column<int>(nullable: true),
                    LAST_VALUE_CHG = table.Column<DateTime>(nullable: true),
                    REASON_FOR_CHG = table.Column<string>(maxLength: 3, nullable: true),
                    CONVEY_DATE = table.Column<DateTime>(nullable: true),
                    CONVEY_TYPE = table.Column<string>(maxLength: 2, nullable: true),
                    CONVEY_FEE = table.Column<float>(nullable: true),
                    DIV_ORG = table.Column<int>(nullable: true),
                    OWNER_NAME_1 = table.Column<string>(maxLength: 28, nullable: true),
                    OWNER_NAME_2 = table.Column<string>(maxLength: 28, nullable: true),
                    OWNER_NAME_3 = table.Column<string>(maxLength: 28, nullable: true),
                    OWNER_MAIL_ADDR = table.Column<string>(maxLength: 28, nullable: true),
                    OWNER_CITY_STATE = table.Column<string>(maxLength: 28, nullable: true),
                    OWNER_ZIP = table.Column<string>(maxLength: 9, nullable: true),
                    LAST_NAME_CHG = table.Column<DateTime>(nullable: true),
                    NEIGHBORHOOD = table.Column<string>(maxLength: 8, nullable: true),
                    BLDG_TYPE = table.Column<string>(maxLength: 9, nullable: true),
                    NR_STORIES = table.Column<float>(nullable: true),
                    BASEMENT = table.Column<string>(maxLength: 1, nullable: true),
                    ATTIC = table.Column<string>(maxLength: 1, nullable: true),
                    NR_UNITS = table.Column<int>(nullable: true),
                    BLDG_AREA = table.Column<int>(nullable: true),
                    YR_BUILT = table.Column<int>(nullable: true),
                    FIREPLACE = table.Column<string>(maxLength: 1, nullable: true),
                    AIR_CONDITIONING = table.Column<string>(maxLength: 3, nullable: true),
                    BEDROOMS = table.Column<int>(nullable: true),
                    BATHS = table.Column<int>(nullable: true),
                    POWDER_ROOMS = table.Column<int>(nullable: true),
                    GARAGE_TYPE = table.Column<string>(maxLength: 2, nullable: true),
                    LOT_AREA = table.Column<int>(nullable: true),
                    ZONING = table.Column<string>(maxLength: 7, nullable: true),
                    LAND_USE = table.Column<string>(maxLength: 4, nullable: true),
                    LAND_USE_GP = table.Column<string>(maxLength: 2, nullable: true),
                    OWN_OCPD = table.Column<string>(maxLength: 1, nullable: true),
                    GEO_TRACT = table.Column<int>(nullable: true),
                    GEO_BLOCK = table.Column<string>(maxLength: 4, nullable: true),
                    GEO_ZIP_CODE = table.Column<int>(nullable: true),
                    GEO_POLICE = table.Column<int>(nullable: true),
                    GEO_ALDER = table.Column<int>(nullable: true),
                    GEO_ALDER_OLD = table.Column<int>(nullable: true),
                    HIST_CODE = table.Column<string>(maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.TAXKEY);
                    table.ForeignKey(
                        name: "FK_Properties_Parcels_TAXKEY",
                        column: x => x.TAXKEY,
                        principalTable: "Parcels",
                        principalColumn: "TAXKEY",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "8fe6ac9b-4954-46d1-8e66-4dad56e6cfa4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3cb7c226-5bb3-4cec-8cd6-f4337e4a9fbc", "AQAAAAEAACcQAAAAEBbz+D04htrBzXyhbona/OJbGvhfIP9otU7jIGkv5a3mZfTGL2ezTxvDcwtKGKnRQQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "a6d0f79d-483c-408d-8b95-da1e5da1e80e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "491e3952-6d52-4b27-a9c1-2ff32ecdef8d", "AQAAAAEAACcQAAAAEFWWqAZbg+hMiAyzp4OFjXAjW2cDYCJ3gKopoFk5dfeqwWwYOXE7S2bfebU8+OxgAA==" });
        }
    }
}
