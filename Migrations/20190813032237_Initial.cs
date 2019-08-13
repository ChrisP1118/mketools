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
                name: "Crimes",
                columns: table => new
                {
                    IncidentNum = table.Column<string>(maxLength: 20, nullable: false),
                    Point = table.Column<IPoint>(nullable: true),
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
                name: "DispatchCalls",
                columns: table => new
                {
                    CallNumber = table.Column<string>(maxLength: 12, nullable: false),
                    ReportedDateTime = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(maxLength: 60, nullable: false),
                    District = table.Column<int>(nullable: false),
                    NatureOfCall = table.Column<string>(maxLength: 20, nullable: false),
                    Status = table.Column<string>(maxLength: 60, nullable: true),
                    Geometry = table.Column<IGeometry>(nullable: true),
                    Accuracy = table.Column<int>(nullable: false),
                    Source = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchCalls", x => x.CallNumber);
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
                name: "Streets",
                columns: table => new
                {
                    NEWDIME_ID = table.Column<string>(maxLength: 10, nullable: false),
                    Outline = table.Column<IGeometry>(nullable: true),
                    MinLat = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    MaxLat = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    MinLng = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    MaxLng = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    FNODE = table.Column<int>(nullable: false),
                    TNODE = table.Column<int>(nullable: false),
                    LPOLY = table.Column<int>(nullable: false),
                    RPOLY = table.Column<int>(nullable: false),
                    LENGTH = table.Column<double>(nullable: false),
                    NEWDIMENR = table.Column<int>(nullable: false),
                    RCD_NBR = table.Column<int>(nullable: false),
                    TRANS_ID = table.Column<int>(nullable: false),
                    SQUAD_L = table.Column<int>(nullable: false),
                    BOILER_L = table.Column<int>(nullable: false),
                    BI_CONST_L = table.Column<int>(nullable: false),
                    BI_ELECT_L = table.Column<int>(nullable: false),
                    BI_ELEV_L = table.Column<int>(nullable: false),
                    BI_PLUMB_L = table.Column<int>(nullable: false),
                    BI_SPRINK_ = table.Column<int>(nullable: false),
                    BI_CNDMN_L = table.Column<int>(nullable: false),
                    CNTYNAME_L = table.Column<string>(maxLength: 15, nullable: true),
                    CNTY_L = table.Column<int>(nullable: false),
                    MUNI_L = table.Column<string>(maxLength: 15, nullable: true),
                    FMCD_L = table.Column<int>(nullable: false),
                    FBLOCK_L = table.Column<string>(maxLength: 4, nullable: true),
                    FTRACT_L = table.Column<string>(maxLength: 6, nullable: true),
                    ZIP_L = table.Column<int>(nullable: false),
                    QTR_SECT_L = table.Column<string>(maxLength: 5, nullable: true),
                    WW_PRES_L = table.Column<int>(nullable: false),
                    WW_SERV_L = table.Column<int>(nullable: false),
                    MPS_ELEM_L = table.Column<int>(nullable: false),
                    MPS_MS_L = table.Column<int>(nullable: false),
                    MPS_HS_L = table.Column<int>(nullable: false),
                    POLICE_L = table.Column<int>(nullable: false),
                    ST_MAIN_L = table.Column<string>(maxLength: 10, nullable: true),
                    SAN_DIST_L = table.Column<string>(maxLength: 2, nullable: true),
                    FOR_TR_L = table.Column<int>(nullable: false),
                    FOR_BL_L = table.Column<int>(nullable: false),
                    SUM_RT_L = table.Column<string>(maxLength: 12, nullable: true),
                    SUM_DA_L = table.Column<string>(maxLength: 3, nullable: true),
                    WARD2K_L = table.Column<int>(nullable: false),
                    TRACT2K_L = table.Column<int>(nullable: false),
                    BLOCK2K_L = table.Column<string>(maxLength: 4, nullable: true),
                    CONGR2K_L = table.Column<int>(nullable: false),
                    STSEN2K_L = table.Column<int>(nullable: false),
                    STASS2K_L = table.Column<int>(nullable: false),
                    CSUP2K_L = table.Column<int>(nullable: false),
                    FIREBAT_L = table.Column<int>(nullable: false),
                    SCHOOL2K_L = table.Column<int>(nullable: false),
                    POLRD_L = table.Column<int>(nullable: false),
                    ALD2004_L = table.Column<int>(nullable: false),
                    WIN_RT_L = table.Column<string>(maxLength: 12, nullable: true),
                    RECYC_SM_L = table.Column<string>(maxLength: 20, nullable: true),
                    MUNICODE_L = table.Column<string>(maxLength: 3, nullable: true),
                    WW_ROUT_L = table.Column<int>(nullable: false),
                    RECYC_DA_L = table.Column<string>(maxLength: 3, nullable: true),
                    RECYC_WN_L = table.Column<string>(maxLength: 20, nullable: true),
                    WTR16TH_L = table.Column<string>(maxLength: 5, nullable: true),
                    SANLEAF_L = table.Column<string>(maxLength: 5, nullable: true),
                    SANPLOW_L = table.Column<string>(maxLength: 3, nullable: true),
                    BROOM_L = table.Column<string>(maxLength: 5, nullable: true),
                    BROOMALL_L = table.Column<string>(maxLength: 5, nullable: true),
                    LOCDIST_L = table.Column<string>(maxLength: 8, nullable: true),
                    FOODINSP_L = table.Column<int>(nullable: false),
                    CIPAREA_L = table.Column<string>(maxLength: 15, nullable: true),
                    TRACT_L = table.Column<int>(nullable: false),
                    ALD_L = table.Column<int>(nullable: false),
                    WARD_L = table.Column<int>(nullable: false),
                    SCHOOL_L = table.Column<int>(nullable: false),
                    BLOCK_L = table.Column<string>(maxLength: 4, nullable: true),
                    STASS_L = table.Column<int>(nullable: false),
                    STSEN_L = table.Column<int>(nullable: false),
                    CNTYSUP_L = table.Column<int>(nullable: false),
                    COMBSEW_L = table.Column<string>(maxLength: 3, nullable: true),
                    SANBIZPL_L = table.Column<string>(maxLength: 10, nullable: true),
                    ST_OP_L = table.Column<int>(nullable: false),
                    FOR_PM_L = table.Column<int>(nullable: false),
                    CONSERVE_L = table.Column<string>(maxLength: 50, nullable: true),
                    SQUAD_R = table.Column<int>(nullable: false),
                    BOILER_R = table.Column<int>(nullable: false),
                    BI_CONST_R = table.Column<int>(nullable: false),
                    BI_ELECT_R = table.Column<int>(nullable: false),
                    BI_ELEV_R = table.Column<int>(nullable: false),
                    BI_PLUMB_R = table.Column<int>(nullable: false),
                    SPRINK_R = table.Column<int>(nullable: false),
                    BI_CNDMN_R = table.Column<int>(nullable: false),
                    CNTYNAME_R = table.Column<string>(maxLength: 15, nullable: true),
                    CNTY_R = table.Column<int>(nullable: false),
                    MUNI_R = table.Column<string>(maxLength: 15, nullable: true),
                    FMCD_R = table.Column<int>(nullable: false),
                    FBLOCK_R = table.Column<string>(maxLength: 4, nullable: true),
                    FTRACT_R = table.Column<string>(maxLength: 6, nullable: true),
                    ZIP_R = table.Column<int>(nullable: false),
                    QTR_SECT_R = table.Column<string>(maxLength: 5, nullable: true),
                    WW_PRES_R = table.Column<int>(nullable: false),
                    WW_SERV_R = table.Column<int>(nullable: false),
                    MPS_ELEM_R = table.Column<int>(nullable: false),
                    MPS_MS_R = table.Column<int>(nullable: false),
                    MPS_HS_R = table.Column<int>(nullable: false),
                    POLICE_R = table.Column<int>(nullable: false),
                    ST_MAIN_R = table.Column<string>(maxLength: 10, nullable: true),
                    SAN_DIST_R = table.Column<string>(maxLength: 2, nullable: true),
                    FOR_TR_R = table.Column<int>(nullable: false),
                    FOR_BL_R = table.Column<int>(nullable: false),
                    SUM_RT_R = table.Column<string>(maxLength: 12, nullable: true),
                    SUM_DA_R = table.Column<string>(maxLength: 3, nullable: true),
                    WARD2K_R = table.Column<int>(nullable: false),
                    TRACT2K_R = table.Column<int>(nullable: false),
                    BLOCK2K_R = table.Column<string>(maxLength: 4, nullable: true),
                    CONGR2K_R = table.Column<int>(nullable: false),
                    STSEN2K_R = table.Column<int>(nullable: false),
                    STASS2K_R = table.Column<int>(nullable: false),
                    CSUP2K_R = table.Column<int>(nullable: false),
                    FIREBAT_R = table.Column<int>(nullable: false),
                    SCHOOL2K_R = table.Column<int>(nullable: false),
                    POLRD_R = table.Column<int>(nullable: false),
                    ALD2004_R = table.Column<int>(nullable: false),
                    WIN_RT_R = table.Column<string>(maxLength: 12, nullable: true),
                    RECYC_SM_R = table.Column<string>(maxLength: 20, nullable: true),
                    MUNICODE_R = table.Column<string>(maxLength: 3, nullable: true),
                    WW_ROUT_R = table.Column<int>(nullable: false),
                    RECYC_DA_R = table.Column<string>(maxLength: 3, nullable: true),
                    RECYC_WN_R = table.Column<string>(maxLength: 20, nullable: true),
                    WTR16TH_R = table.Column<string>(maxLength: 5, nullable: true),
                    SANLEAF_R = table.Column<string>(maxLength: 5, nullable: true),
                    SANPLOW_R = table.Column<string>(maxLength: 3, nullable: true),
                    BROOM_R = table.Column<string>(maxLength: 5, nullable: true),
                    BROOMALL_R = table.Column<string>(maxLength: 5, nullable: true),
                    LOCDIST_R = table.Column<string>(maxLength: 8, nullable: true),
                    FOODINSP_R = table.Column<int>(nullable: false),
                    CIPAREA_R = table.Column<string>(maxLength: 15, nullable: true),
                    TRACT_R = table.Column<int>(nullable: false),
                    ALD_R = table.Column<int>(nullable: false),
                    WARD_R = table.Column<int>(nullable: false),
                    SCHOOL_R = table.Column<int>(nullable: false),
                    BLOCK_R = table.Column<string>(maxLength: 4, nullable: true),
                    STASS_R = table.Column<int>(nullable: false),
                    STSEN_R = table.Column<int>(nullable: false),
                    CNTYSUP_R = table.Column<int>(nullable: false),
                    COMBSEW_R = table.Column<string>(maxLength: 3, nullable: true),
                    SANBIZPL_R = table.Column<string>(maxLength: 10, nullable: true),
                    ST_OP_R = table.Column<int>(nullable: false),
                    FOR_PM_R = table.Column<int>(nullable: false),
                    CONSERVE_R = table.Column<string>(maxLength: 50, nullable: true),
                    SEG_L_TYPE = table.Column<string>(maxLength: 10, nullable: true),
                    LEVEL = table.Column<int>(nullable: false),
                    DIR = table.Column<string>(maxLength: 1, nullable: true),
                    STREET = table.Column<string>(maxLength: 18, nullable: true),
                    STTYPE = table.Column<string>(maxLength: 2, nullable: true),
                    LO_ADD_L = table.Column<int>(nullable: false),
                    HI_ADD_L = table.Column<int>(nullable: false),
                    LO_ADD_R = table.Column<int>(nullable: false),
                    HI_ADD_R = table.Column<int>(nullable: false),
                    BUS_L = table.Column<string>(maxLength: 10, nullable: true),
                    BUS_R = table.Column<string>(maxLength: 10, nullable: true),
                    STCLASS = table.Column<int>(nullable: false),
                    CFCC = table.Column<string>(maxLength: 3, nullable: true),
                    FROM_NODE = table.Column<int>(nullable: false),
                    TO_NODE = table.Column<int>(nullable: false),
                    LOW_X = table.Column<double>(nullable: false),
                    LOW_Y = table.Column<double>(nullable: false),
                    HIGH_X = table.Column<double>(nullable: false),
                    HIGH_Y = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streets", x => x.NEWDIME_ID);
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
                    table.ForeignKey(
                        name: "FK_Addresses_Properties_TAXKEY",
                        column: x => x.TAXKEY,
                        principalTable: "Properties",
                        principalColumn: "TAXKEY",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parcels",
                columns: table => new
                {
                    TAXKEY = table.Column<string>(maxLength: 10, nullable: false),
                    Outline = table.Column<IGeometry>(nullable: true),
                    MinLat = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    MaxLat = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    MinLng = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    MaxLng = table.Column<decimal>(type: "decimal(5, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcels", x => x.TAXKEY);
                    table.ForeignKey(
                        name: "FK_Parcels_Properties_TAXKEY",
                        column: x => x.TAXKEY,
                        principalTable: "Properties",
                        principalColumn: "TAXKEY",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"), "9c0e9687-af42-4d9c-be48-c5a3413b113e", "SiteAdmin", "SiteAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("85f00d40-d578-4988-9f22-4d023175f852"), 0, "31c8b4fa-8eef-413b-b98e-a3cd721ec381", "siteadmin@test.com", true, null, null, false, null, "siteadmin@test.com", "siteadmin", "AQAAAAEAACcQAAAAEBi4e9qjn43m5keehXMQlTpU8LbMaHSRC/c64iZR0Ltu3wTZc8oOqjF+2iG5C80PqQ==", null, false, "", false, "siteadmin" });

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
                name: "IX_Parcels_MaxLat",
                table: "Parcels",
                column: "MaxLat");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_MaxLng",
                table: "Parcels",
                column: "MaxLng");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_MinLat",
                table: "Parcels",
                column: "MinLat");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_MinLng",
                table: "Parcels",
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
                name: "DispatchCalls");

            migrationBuilder.DropTable(
                name: "Parcels");

            migrationBuilder.DropTable(
                name: "Streets");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Properties");
        }
    }
}
