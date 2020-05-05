using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace MkeAlerts.Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "CommonParcels",
                columns: table => new
                {
                    MAP_ID = table.Column<int>(nullable: false),
                    Outline = table.Column<Geometry>(nullable: true),
                    MinLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MinLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonParcels", x => x.MAP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Crimes",
                columns: table => new
                {
                    IncidentNum = table.Column<string>(maxLength: 20, nullable: false),
                    Point = table.Column<Point>(nullable: true),
                    MinLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MinLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    ReportedDateTime = table.Column<DateTime>(nullable: false),
                    ReportedYear = table.Column<decimal>(nullable: false),
                    ReportedMonth = table.Column<decimal>(nullable: false),
                    Location = table.Column<string>(maxLength: 128, nullable: true),
                    WeaponUsed = table.Column<string>(maxLength: 128, nullable: true),
                    ALD = table.Column<decimal>(nullable: false),
                    NSP = table.Column<decimal>(nullable: false),
                    POLICE = table.Column<decimal>(nullable: false),
                    TRACT = table.Column<decimal>(nullable: false),
                    WARD = table.Column<decimal>(nullable: false),
                    ZIP = table.Column<decimal>(nullable: false),
                    RoughX = table.Column<double>(nullable: false),
                    RoughY = table.Column<double>(nullable: false),
                    Arson = table.Column<int>(nullable: false),
                    AssaultOffense = table.Column<int>(nullable: false),
                    Burglary = table.Column<int>(nullable: false),
                    CriminalDamage = table.Column<int>(nullable: false),
                    Homicide = table.Column<int>(nullable: false),
                    LockedVehicle = table.Column<int>(nullable: false),
                    Robbery = table.Column<int>(nullable: false),
                    SexOffense = table.Column<int>(nullable: false),
                    Theft = table.Column<int>(nullable: false),
                    VehicleTheft = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crimes", x => x.IncidentNum);
                });

            migrationBuilder.CreateTable(
                name: "FireDispatchCalls",
                columns: table => new
                {
                    CFS = table.Column<string>(maxLength: 12, nullable: false),
                    ReportedDateTime = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(maxLength: 60, nullable: false),
                    Apt = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 50, nullable: true),
                    NatureOfCall = table.Column<string>(maxLength: 40, nullable: false),
                    Disposition = table.Column<string>(maxLength: 60, nullable: true),
                    Geometry = table.Column<Geometry>(nullable: true),
                    MinLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MinLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    Accuracy = table.Column<int>(nullable: false),
                    Source = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireDispatchCalls", x => x.CFS);
                });

            migrationBuilder.CreateTable(
                name: "FireDispatchCallTypes",
                columns: table => new
                {
                    NatureOfCall = table.Column<string>(maxLength: 40, nullable: false),
                    IsCritical = table.Column<bool>(nullable: false),
                    IsFire = table.Column<bool>(nullable: false),
                    IsMedical = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireDispatchCallTypes", x => x.NatureOfCall);
                });

            migrationBuilder.CreateTable(
                name: "JobRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    JobName = table.Column<string>(maxLength: 40, nullable: true),
                    StartTime = table.Column<DateTimeOffset>(nullable: false),
                    EndTime = table.Column<DateTimeOffset>(nullable: true),
                    SuccessCount = table.Column<int>(nullable: false),
                    FailureCount = table.Column<int>(nullable: false),
                    ErrorMessages = table.Column<string>(nullable: true),
                    ErrorStackTrace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoliceDispatchCalls",
                columns: table => new
                {
                    CallNumber = table.Column<string>(maxLength: 12, nullable: false),
                    ReportedDateTime = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(maxLength: 60, nullable: false),
                    District = table.Column<int>(nullable: false),
                    NatureOfCall = table.Column<string>(maxLength: 20, nullable: false),
                    Status = table.Column<string>(maxLength: 60, nullable: true),
                    Geometry = table.Column<Geometry>(nullable: true),
                    MinLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MinLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    Accuracy = table.Column<int>(nullable: false),
                    Source = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliceDispatchCalls", x => x.CallNumber);
                });

            migrationBuilder.CreateTable(
                name: "PoliceDispatchCallTypes",
                columns: table => new
                {
                    NatureOfCall = table.Column<string>(maxLength: 20, nullable: false),
                    IsCritical = table.Column<bool>(nullable: false),
                    IsViolent = table.Column<bool>(nullable: false),
                    IsProperty = table.Column<bool>(nullable: false),
                    IsDrug = table.Column<bool>(nullable: false),
                    IsTraffic = table.Column<bool>(nullable: false),
                    IsOtherCrime = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliceDispatchCallTypes", x => x.NatureOfCall);
                });

            migrationBuilder.CreateTable(
                name: "Streets",
                columns: table => new
                {
                    CLINEID = table.Column<int>(nullable: false),
                    Outline = table.Column<Geometry>(nullable: true),
                    MinLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MinLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    OBJECTID = table.Column<int>(nullable: false),
                    DIR = table.Column<string>(maxLength: 2, nullable: true),
                    STREET = table.Column<string>(maxLength: 24, nullable: true),
                    STTYPE = table.Column<string>(maxLength: 4, nullable: true),
                    PDIR = table.Column<string>(maxLength: 1, nullable: true),
                    LOW = table.Column<string>(maxLength: 5, nullable: true),
                    HIGH = table.Column<string>(maxLength: 5, nullable: true),
                    LEFTFR = table.Column<string>(maxLength: 5, nullable: true),
                    LEFTTO = table.Column<string>(maxLength: 5, nullable: true),
                    RIGHTFR = table.Column<string>(maxLength: 5, nullable: true),
                    RIGHTTO = table.Column<string>(maxLength: 5, nullable: true),
                    MUNI = table.Column<string>(maxLength: 15, nullable: true),
                    FCC = table.Column<string>(maxLength: 3, nullable: true),
                    OWNER = table.Column<string>(maxLength: 1, nullable: true),
                    R_MCD = table.Column<string>(maxLength: 5, nullable: true),
                    L_MCD = table.Column<string>(maxLength: 5, nullable: true),
                    SHIELD = table.Column<string>(maxLength: 1, nullable: true),
                    HIGHWAY = table.Column<string>(maxLength: 16, nullable: true),
                    SOURCE = table.Column<string>(maxLength: 1, nullable: true),
                    COMMENT = table.Column<string>(maxLength: 1, nullable: true),
                    DATE_CHANG = table.Column<string>(maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streets", x => x.CLINEID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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

            migrationBuilder.CreateTable(
                name: "DispatchCallSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: false),
                    DispatchCallType = table.Column<int>(nullable: false),
                    Point = table.Column<Point>(nullable: true),
                    Distance = table.Column<int>(nullable: false),
                    SDIR = table.Column<string>(maxLength: 1, nullable: true),
                    STREET = table.Column<string>(maxLength: 18, nullable: true),
                    STTYPE = table.Column<string>(maxLength: 2, nullable: true),
                    HOUSE_NR = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchCallSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispatchCallSubscriptions_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalCredentials",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: false),
                    Provider = table.Column<string>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalCredentials_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickupDateSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: false),
                    HoursBefore = table.Column<int>(nullable: false),
                    NextGarbagePickupDate = table.Column<DateTime>(nullable: true),
                    NextRecyclingPickupDate = table.Column<DateTime>(nullable: true),
                    NextGarbagePickupNotification = table.Column<DateTime>(nullable: true),
                    NextRecyclingPickupNotification = table.Column<DateTime>(nullable: true),
                    LADDR = table.Column<string>(maxLength: 10, nullable: true),
                    SDIR = table.Column<string>(maxLength: 1, nullable: true),
                    SNAME = table.Column<string>(maxLength: 18, nullable: true),
                    STYPE = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickupDateSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickupDateSubscriptions_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parcels",
                columns: table => new
                {
                    TAXKEY = table.Column<string>(maxLength: 10, nullable: false),
                    MinLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MinLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    OBJECTID = table.Column<int>(nullable: false),
                    MAP_ID = table.Column<int>(nullable: false),
                    PARCEL_KEY = table.Column<string>(maxLength: 50, nullable: true),
                    PARCEL_DES = table.Column<string>(maxLength: 30, nullable: true),
                    OVERLAP = table.Column<int>(nullable: false),
                    MCD = table.Column<string>(maxLength: 5, nullable: true),
                    SOURCE = table.Column<string>(maxLength: 39, nullable: true),
                    COMMENT = table.Column<string>(maxLength: 84, nullable: true),
                    RECID = table.Column<int>(nullable: false),
                    RECSOURCE = table.Column<string>(maxLength: 7, nullable: true),
                    MUNINAME = table.Column<string>(maxLength: 15, nullable: true),
                    PARCELNO = table.Column<string>(maxLength: 10, nullable: true),
                    OWNERNAME1 = table.Column<string>(maxLength: 84, nullable: true),
                    OWNERNAME2 = table.Column<string>(maxLength: 84, nullable: true),
                    OWNERNAME3 = table.Column<string>(maxLength: 28, nullable: true),
                    OWNERADDR = table.Column<string>(maxLength: 49, nullable: true),
                    OWNERCTYST = table.Column<string>(maxLength: 27, nullable: true),
                    OWNERZIP = table.Column<string>(maxLength: 10, nullable: true),
                    HOUSENR = table.Column<string>(maxLength: 10, nullable: true),
                    HOUSENRHI = table.Column<string>(maxLength: 10, nullable: true),
                    HOUSENRSFX = table.Column<string>(maxLength: 3, nullable: true),
                    STREETDIR = table.Column<string>(maxLength: 2, nullable: true),
                    STREETNAME = table.Column<string>(maxLength: 20, nullable: true),
                    STREETTYPE = table.Column<string>(maxLength: 4, nullable: true),
                    SUFFIXDIR = table.Column<string>(maxLength: 2, nullable: true),
                    UNITNUMBER = table.Column<string>(maxLength: 8, nullable: true),
                    POSTOFFICE = table.Column<string>(maxLength: 15, nullable: true),
                    LEGALDESCR = table.Column<string>(maxLength: 254, nullable: true),
                    CONDO_NAME = table.Column<string>(maxLength: 74, nullable: true),
                    UNIT_TYPE = table.Column<string>(maxLength: 7, nullable: true),
                    ACRES = table.Column<double>(nullable: false),
                    ASSESSEDVA = table.Column<int>(nullable: false),
                    LANDVALUE = table.Column<int>(nullable: false),
                    IMPVALUE = table.Column<int>(nullable: false),
                    CLASS = table.Column<string>(maxLength: 1, nullable: true),
                    CODE = table.Column<string>(maxLength: 2, nullable: true),
                    DESCRIPTIO = table.Column<string>(maxLength: 23, nullable: true),
                    ZONING_COD = table.Column<string>(maxLength: 1, nullable: true),
                    ZONING_DES = table.Column<string>(maxLength: 29, nullable: true),
                    ZONING_URL = table.Column<string>(maxLength: 88, nullable: true),
                    EXM_TYP = table.Column<string>(maxLength: 3, nullable: true),
                    EXM_TYP_DE = table.Column<string>(maxLength: 94, nullable: true),
                    EXM_CLASS_ = table.Column<string>(maxLength: 76, nullable: true),
                    TAX_INFO_U = table.Column<string>(maxLength: 1, nullable: true),
                    ASSESSMENT = table.Column<string>(maxLength: 88, nullable: true),
                    PARCEL_TYP = table.Column<string>(maxLength: 13, nullable: true),
                    TAX_YR = table.Column<int>(nullable: false),
                    FAIR_MKT_V = table.Column<int>(nullable: false),
                    GROSS_TAX = table.Column<double>(nullable: false),
                    NET_TAX = table.Column<double>(nullable: false),
                    GIS_ACRES = table.Column<double>(nullable: false),
                    SCHOOL_DIS = table.Column<string>(maxLength: 41, nullable: true),
                    SCHOOL_ID = table.Column<string>(maxLength: 4, nullable: true),
                    PAR_ZIP = table.Column<string>(maxLength: 5, nullable: true),
                    PAR_ZIP_EX = table.Column<string>(maxLength: 4, nullable: true),
                    ADDRESS = table.Column<string>(maxLength: 32, nullable: true),
                    DWELLING_C = table.Column<string>(maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcels", x => x.TAXKEY);
                    table.ForeignKey(
                        name: "FK_Parcels_CommonParcels_MAP_ID",
                        column: x => x.MAP_ID,
                        principalTable: "CommonParcels",
                        principalColumn: "MAP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    ADDRESS_ID = table.Column<int>(nullable: false),
                    HouseNumber = table.Column<int>(nullable: false),
                    Point = table.Column<Point>(nullable: true),
                    MinLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MinLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    OBJECTID = table.Column<int>(nullable: false),
                    TAXKEY = table.Column<string>(maxLength: 10, nullable: true),
                    HOUSENO = table.Column<string>(maxLength: 15, nullable: true),
                    HOUSESX = table.Column<string>(maxLength: 2, nullable: true),
                    ALT_ID = table.Column<string>(maxLength: 15, nullable: true),
                    DIR = table.Column<string>(maxLength: 1, nullable: true),
                    STREET = table.Column<string>(maxLength: 19, nullable: true),
                    STTYPE = table.Column<string>(maxLength: 4, nullable: true),
                    PDIR = table.Column<string>(maxLength: 1, nullable: true),
                    MUNI = table.Column<string>(maxLength: 15, nullable: true),
                    UNIT = table.Column<string>(maxLength: 5, nullable: true),
                    ZIP_CODE = table.Column<string>(maxLength: 9, nullable: true),
                    COMMENT = table.Column<string>(maxLength: 140, nullable: true),
                    SOURCE = table.Column<string>(maxLength: 36, nullable: true),
                    SOURCE_ID = table.Column<string>(maxLength: 21, nullable: true),
                    MAILABLE = table.Column<int>(nullable: false),
                    FULLADDR = table.Column<string>(maxLength: 30, nullable: true),
                    X = table.Column<string>(maxLength: 1, nullable: true),
                    Y = table.Column<string>(maxLength: 1, nullable: true),
                    DD_LAT = table.Column<string>(maxLength: 1, nullable: true),
                    DD_LONG = table.Column<string>(maxLength: 1, nullable: true),
                    TAG = table.Column<string>(maxLength: 41, nullable: true),
                    CLINEID = table.Column<string>(maxLength: 1, nullable: true),
                    BUILDING_I = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.ADDRESS_ID);
                    table.ForeignKey(
                        name: "FK_Addresses_Parcels_TAXKEY",
                        column: x => x.TAXKEY,
                        principalTable: "Parcels",
                        principalColumn: "TAXKEY",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"), "e909918a-cd7e-430a-a755-ece85c836e1b", "SiteAdmin", "SiteAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("85f00d40-d578-4988-9f22-4d023175f852"), 0, "d5c41c9c-bb3f-4d48-852e-561181cb1963", "cwilson@mkealerts.com", true, null, null, false, null, "cwilson@mkealerts.com", "cwilson@mkealerts.com", "AQAAAAEAACcQAAAAEKSL1hEhkNfMHWL96Vl2GgIGahgLgmegiR/orxlRVQls/1Sz+1X8zDnRJy4D8aHSJg==", null, false, "", false, "cwilson@mkealerts.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("85f00d40-d578-4988-9f22-4d023175f852"), new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157") });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_TAXKEY",
                table: "Addresses",
                column: "TAXKEY");

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
                name: "IX_CommonParcels_MaxLat",
                table: "CommonParcels",
                column: "MaxLat");

            migrationBuilder.CreateIndex(
                name: "IX_CommonParcels_MaxLng",
                table: "CommonParcels",
                column: "MaxLng");

            migrationBuilder.CreateIndex(
                name: "IX_CommonParcels_MinLat",
                table: "CommonParcels",
                column: "MinLat");

            migrationBuilder.CreateIndex(
                name: "IX_CommonParcels_MinLng",
                table: "CommonParcels",
                column: "MinLng");

            migrationBuilder.CreateIndex(
                name: "IX_Crimes_ReportedDateTime",
                table: "Crimes",
                column: "ReportedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchCallSubscriptions_ApplicationUserId",
                table: "DispatchCallSubscriptions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCredentials_ApplicationUserId",
                table: "ExternalCredentials",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FireDispatchCalls_ReportedDateTime",
                table: "FireDispatchCalls",
                column: "ReportedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_JobRuns_StartTime",
                table: "JobRuns",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_MAP_ID",
                table: "Parcels",
                column: "MAP_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PickupDateSubscriptions_ApplicationUserId",
                table: "PickupDateSubscriptions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliceDispatchCalls_ReportedDateTime",
                table: "PoliceDispatchCalls",
                column: "ReportedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_MaxLat",
                table: "Streets",
                column: "MaxLat");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_MaxLng",
                table: "Streets",
                column: "MaxLng");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_MinLat",
                table: "Streets",
                column: "MinLat");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_MinLng",
                table: "Streets",
                column: "MinLng");
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
                name: "Crimes");

            migrationBuilder.DropTable(
                name: "DispatchCallSubscriptions");

            migrationBuilder.DropTable(
                name: "ExternalCredentials");

            migrationBuilder.DropTable(
                name: "FireDispatchCalls");

            migrationBuilder.DropTable(
                name: "FireDispatchCallTypes");

            migrationBuilder.DropTable(
                name: "JobRuns");

            migrationBuilder.DropTable(
                name: "PickupDateSubscriptions");

            migrationBuilder.DropTable(
                name: "PoliceDispatchCalls");

            migrationBuilder.DropTable(
                name: "PoliceDispatchCallTypes");

            migrationBuilder.DropTable(
                name: "Streets");

            migrationBuilder.DropTable(
                name: "Parcels");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CommonParcels");
        }
    }
}
