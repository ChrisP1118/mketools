using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    RCD_NBR = table.Column<string>(maxLength: 10, nullable: false),
                    TAXKEY = table.Column<string>(maxLength: 10, nullable: true),
                    HSE_NBR = table.Column<int>(nullable: false),
                    SFX = table.Column<string>(maxLength: 3, nullable: true),
                    DIR = table.Column<string>(maxLength: 1, nullable: true),
                    STREET = table.Column<string>(maxLength: 18, nullable: true),
                    STTYPE = table.Column<string>(maxLength: 2, nullable: true),
                    UNIT_NBR = table.Column<string>(maxLength: 5, nullable: true),
                    ZIP_CODE = table.Column<string>(maxLength: 9, nullable: true),
                    LAND_USE = table.Column<int>(nullable: false),
                    UPD_DATE = table.Column<int>(nullable: false),
                    WARD = table.Column<int>(nullable: false),
                    MAIL_ERROR_COUNT = table.Column<int>(nullable: false),
                    MAIL_STATUS = table.Column<string>(maxLength: 1, nullable: true),
                    RES_COM_FLAG = table.Column<string>(maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.RCD_NBR);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DispatchCalls",
                columns: table => new
                {
                    CallNumber = table.Column<string>(maxLength: 12, nullable: false),
                    ReportedDateTime = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(maxLength: 60, nullable: false),
                    District = table.Column<int>(nullable: false),
                    NatureOfCall = table.Column<string>(maxLength: 20, nullable: false),
                    Status = table.Column<string>(maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchCalls", x => x.CallNumber);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    TAXKEY = table.Column<string>(maxLength: 10, nullable: false),
                    Parcel = table.Column<IGeometry>(nullable: true),
                    Centroid = table.Column<IPoint>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.TAXKEY);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    TAXKEY = table.Column<string>(maxLength: 10, nullable: false),
                    AIR_CONDITIONING = table.Column<string>(maxLength: 3, nullable: true),
                    ATTIC = table.Column<string>(maxLength: 1, nullable: true),
                    BASEMENT = table.Column<string>(maxLength: 1, nullable: true),
                    BATHS = table.Column<int>(nullable: false),
                    BEDROOMS = table.Column<int>(nullable: false),
                    BLDG_AREA = table.Column<int>(nullable: false),
                    BLDG_TYPE = table.Column<string>(maxLength: 9, nullable: true),
                    C_A_CLASS = table.Column<string>(maxLength: 1, nullable: true),
                    C_A_EXM_IMPRV = table.Column<int>(nullable: false),
                    C_A_EXM_LAND = table.Column<int>(nullable: false),
                    C_A_EXM_TOTAL = table.Column<int>(nullable: false),
                    C_A_EXM_TYPE = table.Column<string>(maxLength: 3, nullable: true),
                    C_A_IMPRV = table.Column<int>(nullable: false),
                    C_A_LAND = table.Column<int>(nullable: false),
                    C_A_SYMBOL = table.Column<string>(maxLength: 1, nullable: true),
                    C_A_TOTAL = table.Column<int>(nullable: false),
                    CHG_NR = table.Column<string>(maxLength: 6, nullable: true),
                    CHK_DIGIT = table.Column<string>(maxLength: 1, nullable: true),
                    CONVEY_DATE = table.Column<DateTime>(nullable: false),
                    CONVEY_FEE = table.Column<float>(nullable: false),
                    CONVEY_TYPE = table.Column<string>(maxLength: 2, nullable: true),
                    SDIR = table.Column<string>(maxLength: 1, nullable: true),
                    DIV_DROP = table.Column<int>(nullable: false),
                    DIV_ORG = table.Column<int>(nullable: false),
                    DPW_SANITATION = table.Column<string>(maxLength: 2, nullable: true),
                    EXM_ACREAGE = table.Column<float>(nullable: false),
                    EXM_PER_CT_IMPRV = table.Column<float>(nullable: false),
                    EXM_PER_CT_LAND = table.Column<float>(nullable: false),
                    FIREPLACE = table.Column<string>(maxLength: 1, nullable: true),
                    GARAGE_TYPE = table.Column<string>(maxLength: 2, nullable: true),
                    GEO_ALDER = table.Column<int>(nullable: false),
                    GEO_ALDER_OLD = table.Column<int>(nullable: false),
                    GEO_BI_MAINT = table.Column<int>(nullable: false),
                    GEO_BLOCK = table.Column<string>(maxLength: 4, nullable: true),
                    GEO_FIRE = table.Column<int>(nullable: false),
                    GEO_POLICE = table.Column<int>(nullable: false),
                    GEO_TRACT = table.Column<int>(nullable: false),
                    GEO_ZIP_CODE = table.Column<int>(nullable: false),
                    HIST_CODE = table.Column<string>(maxLength: 1, nullable: true),
                    HOUSE_NR_HI = table.Column<int>(nullable: false),
                    HOUSE_NR_LO = table.Column<int>(nullable: false),
                    HOUSE_NR_SFX = table.Column<string>(maxLength: 3, nullable: true),
                    LAND_USE = table.Column<int>(nullable: false),
                    LAND_USE_GP = table.Column<int>(nullable: false),
                    LAST_NAME_CHG = table.Column<DateTime>(nullable: false),
                    LAST_VALUE_CHG = table.Column<DateTime>(nullable: false),
                    LOT_AREA = table.Column<int>(nullable: false),
                    NEIGHBORHOOD = table.Column<string>(maxLength: 4, nullable: true),
                    NR_ROOMS = table.Column<string>(maxLength: 4, nullable: true),
                    NR_STORIES = table.Column<float>(nullable: false),
                    NR_UNITS = table.Column<int>(nullable: false),
                    NUMBER_OF_SPACES = table.Column<int>(nullable: false),
                    OWNER_CITY_STATE = table.Column<string>(maxLength: 28, nullable: true),
                    OWNER_MAIL_ADDR = table.Column<string>(maxLength: 28, nullable: true),
                    OWNER_NAME_1 = table.Column<string>(maxLength: 28, nullable: true),
                    OWNER_NAME_2 = table.Column<string>(maxLength: 28, nullable: true),
                    OWNER_NAME_3 = table.Column<string>(maxLength: 28, nullable: true),
                    OWNER_ZIP = table.Column<string>(maxLength: 9, nullable: true),
                    OWN_OCPD = table.Column<string>(maxLength: 1, nullable: true),
                    P_A_CLASS = table.Column<string>(maxLength: 1, nullable: true),
                    P_A_EXM_IMPRV = table.Column<int>(nullable: false),
                    P_A_EXM_LAND = table.Column<int>(nullable: false),
                    P_A_EXM_TOTAL = table.Column<int>(nullable: false),
                    P_A_EXM_TYPE = table.Column<string>(maxLength: 3, nullable: true),
                    P_A_IMPRV = table.Column<int>(nullable: false),
                    P_A_LAND = table.Column<int>(nullable: false),
                    P_A_SYMBOL = table.Column<string>(maxLength: 1, nullable: true),
                    P_A_TOTAL = table.Column<int>(nullable: false),
                    PLAT_PAGE = table.Column<int>(nullable: false),
                    POWDER_ROOMS = table.Column<int>(nullable: false),
                    RAZE_STATUS = table.Column<int>(nullable: false),
                    REASON_FOR_CHG = table.Column<string>(maxLength: 3, nullable: true),
                    STREET = table.Column<string>(maxLength: 18, nullable: true),
                    STTYPE = table.Column<string>(maxLength: 2, nullable: true),
                    SUB_ACCT = table.Column<int>(nullable: false),
                    SWIM_POOL = table.Column<string>(maxLength: 1, nullable: true),
                    TAX_RATE_CD = table.Column<string>(maxLength: 2, nullable: true),
                    TOT_UNABATED = table.Column<string>(maxLength: 4, nullable: true),
                    YEARS_DELQ = table.Column<int>(nullable: false),
                    YR_ASSMT = table.Column<string>(maxLength: 4, nullable: true),
                    YR_BUILT = table.Column<int>(nullable: false),
                    ZONING = table.Column<string>(maxLength: 7, nullable: true),
                    PARKING_SPACES = table.Column<int>(nullable: false),
                    PARKING_TYPE = table.Column<string>(maxLength: 2, nullable: true),
                    CORNER_LOT = table.Column<string>(maxLength: 2, nullable: true),
                    ANGLE = table.Column<int>(nullable: false),
                    TAX_DELQ = table.Column<int>(nullable: false),
                    BI_VIOL = table.Column<string>(maxLength: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.TAXKEY);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"), "2da8e871-33f5-432f-86c3-71aa990ac131", "SiteAdmin", "SiteAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("85f00d40-d578-4988-9f22-4d023175f852"), 0, "6fc05253-6b65-49dc-b021-b06426e6a1e9", "siteadmin@test.com", true, null, null, false, null, "siteadmin@test.com", "siteadmin", "AQAAAAEAACcQAAAAEJBPal9SAJI9VEuqBDwbXWBNpU3H+wnFO7Q899oYmeEmyPwD3uIaMYCrfF60Hw6GAQ==", null, false, "", false, "siteadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("85f00d40-d578-4988-9f22-4d023175f852"), new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157") });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_RCD_NBR",
                table: "Addresses",
                column: "RCD_NBR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchCalls_CallNumber",
                table: "DispatchCalls",
                column: "CallNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_TAXKEY",
                table: "Locations",
                column: "TAXKEY",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_TAXKEY",
                table: "Properties",
                column: "TAXKEY",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DispatchCalls");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
