﻿// <auto-generated />
using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MkeAlerts.Web.Data;

namespace MkeAlerts.Web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                            RoleId = new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MkeAlerts.Web.Models.Data.Accounts.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                            ConcurrencyStamp = "3a98abb7-4625-406b-bde0-fc70d8aa1487",
                            Name = "SiteAdmin",
                            NormalizedName = "SiteAdmin"
                        });
                });

            modelBuilder.Entity("MkeAlerts.Web.Models.Data.Accounts.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100);

                    b.Property<string>("LastName")
                        .HasMaxLength(100);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "79bf4325-37ff-4e5f-b781-de0b7dc4d185",
                            Email = "siteadmin@test.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "siteadmin@test.com",
                            NormalizedUserName = "siteadmin",
                            PasswordHash = "AQAAAAEAACcQAAAAEOTVF3I+piymslOZc9jmaFEoCBKCZxh3aIuzipb/zSdV0kJRlk+OVq17GkLrXuG30g==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "",
                            TwoFactorEnabled = false,
                            UserName = "siteadmin"
                        });
                });

            modelBuilder.Entity("MkeAlerts.Web.Models.Data.Incidents.Crime", b =>
                {
                    b.Property<string>("IncidentNum")
                        .HasMaxLength(20);

                    b.Property<decimal>("ALD");

                    b.Property<int>("Arson");

                    b.Property<int>("AssaultOffense");

                    b.Property<int>("Burglary");

                    b.Property<int>("CriminalDamage");

                    b.Property<int>("Homicide");

                    b.Property<string>("Location")
                        .HasMaxLength(128);

                    b.Property<int>("LockedVehicle");

                    b.Property<decimal>("MaxLat")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("MaxLng")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("MinLat")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("MinLng")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("NSP");

                    b.Property<decimal>("POLICE");

                    b.Property<IPoint>("Point");

                    b.Property<DateTime>("ReportedDateTime");

                    b.Property<decimal>("ReportedMonth");

                    b.Property<decimal>("ReportedYear");

                    b.Property<int>("Robbery");

                    b.Property<double>("RoughX");

                    b.Property<double>("RoughY");

                    b.Property<int>("SexOffense");

                    b.Property<decimal>("TRACT");

                    b.Property<int>("Theft");

                    b.Property<int>("VehicleTheft");

                    b.Property<decimal>("WARD");

                    b.Property<string>("WeaponUsed")
                        .HasMaxLength(128);

                    b.Property<decimal>("ZIP");

                    b.HasKey("IncidentNum");

                    b.ToTable("Crimes");
                });

            modelBuilder.Entity("MkeAlerts.Web.Models.Data.Incidents.DispatchCall", b =>
                {
                    b.Property<string>("CallNumber")
                        .HasMaxLength(12);

                    b.Property<int>("Accuracy");

                    b.Property<int>("District");

                    b.Property<IGeometry>("Geometry");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<decimal>("MaxLat")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("MaxLng")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("MinLat")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("MinLng")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<string>("NatureOfCall")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<DateTime>("ReportedDateTime");

                    b.Property<int>("Source");

                    b.Property<string>("Status")
                        .HasMaxLength(60);

                    b.HasKey("CallNumber");

                    b.ToTable("DispatchCalls");
                });

            modelBuilder.Entity("MkeAlerts.Web.Models.Data.Places.Address", b =>
                {
                    b.Property<string>("RCD_NBR")
                        .HasMaxLength(10);

                    b.Property<string>("DIR")
                        .HasMaxLength(1);

                    b.Property<int>("HSE_NBR");

                    b.Property<int>("LAND_USE");

                    b.Property<int>("MAIL_ERROR_COUNT");

                    b.Property<string>("MAIL_STATUS")
                        .HasMaxLength(1);

                    b.Property<string>("RES_COM_FLAG")
                        .HasMaxLength(1);

                    b.Property<string>("SFX")
                        .HasMaxLength(3);

                    b.Property<string>("STREET")
                        .HasMaxLength(18);

                    b.Property<string>("STTYPE")
                        .HasMaxLength(2);

                    b.Property<string>("TAXKEY")
                        .HasMaxLength(10);

                    b.Property<string>("UNIT_NBR")
                        .HasMaxLength(5);

                    b.Property<int>("UPD_DATE");

                    b.Property<int>("WARD");

                    b.Property<string>("ZIP_CODE")
                        .HasMaxLength(9);

                    b.HasKey("RCD_NBR");

                    b.HasIndex("TAXKEY");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("MkeAlerts.Web.Models.Data.Places.Parcel", b =>
                {
                    b.Property<string>("TAXKEY")
                        .HasMaxLength(10);

                    b.Property<decimal>("MaxLat")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("MaxLng")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("MinLat")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("MinLng")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<IGeometry>("Outline");

                    b.HasKey("TAXKEY");

                    b.HasIndex("MaxLat");

                    b.HasIndex("MaxLng");

                    b.HasIndex("MinLat");

                    b.HasIndex("MinLng");

                    b.ToTable("Parcels");
                });

            modelBuilder.Entity("MkeAlerts.Web.Models.Data.Places.Property", b =>
                {
                    b.Property<string>("TAXKEY")
                        .HasMaxLength(10);

                    b.Property<string>("AIR_CONDITIONING")
                        .HasMaxLength(3);

                    b.Property<int>("ANGLE");

                    b.Property<string>("ATTIC")
                        .HasMaxLength(1);

                    b.Property<string>("BASEMENT")
                        .HasMaxLength(1);

                    b.Property<int>("BATHS");

                    b.Property<int>("BEDROOMS");

                    b.Property<string>("BI_VIOL")
                        .HasMaxLength(4);

                    b.Property<int>("BLDG_AREA");

                    b.Property<string>("BLDG_TYPE")
                        .HasMaxLength(9);

                    b.Property<string>("CHG_NR")
                        .HasMaxLength(6);

                    b.Property<string>("CHK_DIGIT")
                        .HasMaxLength(1);

                    b.Property<DateTime>("CONVEY_DATE");

                    b.Property<float>("CONVEY_FEE");

                    b.Property<string>("CONVEY_TYPE")
                        .HasMaxLength(2);

                    b.Property<string>("CORNER_LOT")
                        .HasMaxLength(2);

                    b.Property<string>("C_A_CLASS")
                        .HasMaxLength(1);

                    b.Property<int>("C_A_EXM_IMPRV");

                    b.Property<int>("C_A_EXM_LAND");

                    b.Property<int>("C_A_EXM_TOTAL");

                    b.Property<string>("C_A_EXM_TYPE")
                        .HasMaxLength(3);

                    b.Property<int>("C_A_IMPRV");

                    b.Property<int>("C_A_LAND");

                    b.Property<string>("C_A_SYMBOL")
                        .HasMaxLength(1);

                    b.Property<int>("C_A_TOTAL");

                    b.Property<int>("DIV_DROP");

                    b.Property<int>("DIV_ORG");

                    b.Property<string>("DPW_SANITATION")
                        .HasMaxLength(2);

                    b.Property<float>("EXM_ACREAGE");

                    b.Property<float>("EXM_PER_CT_IMPRV");

                    b.Property<float>("EXM_PER_CT_LAND");

                    b.Property<string>("FIREPLACE")
                        .HasMaxLength(1);

                    b.Property<string>("GARAGE_TYPE")
                        .HasMaxLength(2);

                    b.Property<int>("GEO_ALDER");

                    b.Property<int>("GEO_ALDER_OLD");

                    b.Property<int>("GEO_BI_MAINT");

                    b.Property<string>("GEO_BLOCK")
                        .HasMaxLength(4);

                    b.Property<int>("GEO_FIRE");

                    b.Property<int>("GEO_POLICE");

                    b.Property<int>("GEO_TRACT");

                    b.Property<int>("GEO_ZIP_CODE");

                    b.Property<string>("HIST_CODE")
                        .HasMaxLength(1);

                    b.Property<int>("HOUSE_NR_HI");

                    b.Property<int>("HOUSE_NR_LO");

                    b.Property<string>("HOUSE_NR_SFX")
                        .HasMaxLength(3);

                    b.Property<int>("LAND_USE");

                    b.Property<int>("LAND_USE_GP");

                    b.Property<DateTime>("LAST_NAME_CHG");

                    b.Property<DateTime>("LAST_VALUE_CHG");

                    b.Property<int>("LOT_AREA");

                    b.Property<string>("NEIGHBORHOOD")
                        .HasMaxLength(4);

                    b.Property<string>("NR_ROOMS")
                        .HasMaxLength(4);

                    b.Property<float>("NR_STORIES");

                    b.Property<int>("NR_UNITS");

                    b.Property<int>("NUMBER_OF_SPACES");

                    b.Property<string>("OWNER_CITY_STATE")
                        .HasMaxLength(28);

                    b.Property<string>("OWNER_MAIL_ADDR")
                        .HasMaxLength(28);

                    b.Property<string>("OWNER_NAME_1")
                        .HasMaxLength(28);

                    b.Property<string>("OWNER_NAME_2")
                        .HasMaxLength(28);

                    b.Property<string>("OWNER_NAME_3")
                        .HasMaxLength(28);

                    b.Property<string>("OWNER_ZIP")
                        .HasMaxLength(9);

                    b.Property<string>("OWN_OCPD")
                        .HasMaxLength(1);

                    b.Property<int>("PARKING_SPACES");

                    b.Property<string>("PARKING_TYPE")
                        .HasMaxLength(2);

                    b.Property<int>("PLAT_PAGE");

                    b.Property<int>("POWDER_ROOMS");

                    b.Property<string>("P_A_CLASS")
                        .HasMaxLength(1);

                    b.Property<int>("P_A_EXM_IMPRV");

                    b.Property<int>("P_A_EXM_LAND");

                    b.Property<int>("P_A_EXM_TOTAL");

                    b.Property<string>("P_A_EXM_TYPE")
                        .HasMaxLength(3);

                    b.Property<int>("P_A_IMPRV");

                    b.Property<int>("P_A_LAND");

                    b.Property<string>("P_A_SYMBOL")
                        .HasMaxLength(1);

                    b.Property<int>("P_A_TOTAL");

                    b.Property<int>("RAZE_STATUS");

                    b.Property<string>("REASON_FOR_CHG")
                        .HasMaxLength(3);

                    b.Property<string>("SDIR")
                        .HasMaxLength(1);

                    b.Property<string>("STREET")
                        .HasMaxLength(18);

                    b.Property<string>("STTYPE")
                        .HasMaxLength(2);

                    b.Property<int>("SUB_ACCT");

                    b.Property<string>("SWIM_POOL")
                        .HasMaxLength(1);

                    b.Property<int>("TAX_DELQ");

                    b.Property<string>("TAX_RATE_CD")
                        .HasMaxLength(2);

                    b.Property<string>("TOT_UNABATED")
                        .HasMaxLength(4);

                    b.Property<int>("YEARS_DELQ");

                    b.Property<string>("YR_ASSMT")
                        .HasMaxLength(4);

                    b.Property<int>("YR_BUILT");

                    b.Property<string>("ZONING")
                        .HasMaxLength(7);

                    b.HasKey("TAXKEY");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("MkeAlerts.Web.Models.Data.Places.Street", b =>
                {
                    b.Property<string>("NEWDIME_ID")
                        .HasMaxLength(10);

                    b.Property<int>("ALD2004_L");

                    b.Property<int>("ALD2004_R");

                    b.Property<int>("ALD_L");

                    b.Property<int>("ALD_R");

                    b.Property<int>("BI_CNDMN_L");

                    b.Property<int>("BI_CNDMN_R");

                    b.Property<int>("BI_CONST_L");

                    b.Property<int>("BI_CONST_R");

                    b.Property<int>("BI_ELECT_L");

                    b.Property<int>("BI_ELECT_R");

                    b.Property<int>("BI_ELEV_L");

                    b.Property<int>("BI_ELEV_R");

                    b.Property<int>("BI_PLUMB_L");

                    b.Property<int>("BI_PLUMB_R");

                    b.Property<int>("BI_SPRINK_");

                    b.Property<string>("BLOCK2K_L")
                        .HasMaxLength(4);

                    b.Property<string>("BLOCK2K_R")
                        .HasMaxLength(4);

                    b.Property<string>("BLOCK_L")
                        .HasMaxLength(4);

                    b.Property<string>("BLOCK_R")
                        .HasMaxLength(4);

                    b.Property<int>("BOILER_L");

                    b.Property<int>("BOILER_R");

                    b.Property<string>("BROOMALL_L")
                        .HasMaxLength(5);

                    b.Property<string>("BROOMALL_R")
                        .HasMaxLength(5);

                    b.Property<string>("BROOM_L")
                        .HasMaxLength(5);

                    b.Property<string>("BROOM_R")
                        .HasMaxLength(5);

                    b.Property<string>("BUS_L")
                        .HasMaxLength(10);

                    b.Property<string>("BUS_R")
                        .HasMaxLength(10);

                    b.Property<string>("CFCC")
                        .HasMaxLength(3);

                    b.Property<string>("CIPAREA_L")
                        .HasMaxLength(15);

                    b.Property<string>("CIPAREA_R")
                        .HasMaxLength(15);

                    b.Property<string>("CNTYNAME_L")
                        .HasMaxLength(15);

                    b.Property<string>("CNTYNAME_R")
                        .HasMaxLength(15);

                    b.Property<int>("CNTYSUP_L");

                    b.Property<int>("CNTYSUP_R");

                    b.Property<int>("CNTY_L");

                    b.Property<int>("CNTY_R");

                    b.Property<string>("COMBSEW_L")
                        .HasMaxLength(3);

                    b.Property<string>("COMBSEW_R")
                        .HasMaxLength(3);

                    b.Property<int>("CONGR2K_L");

                    b.Property<int>("CONGR2K_R");

                    b.Property<string>("CONSERVE_L")
                        .HasMaxLength(50);

                    b.Property<string>("CONSERVE_R")
                        .HasMaxLength(50);

                    b.Property<int>("CSUP2K_L");

                    b.Property<int>("CSUP2K_R");

                    b.Property<string>("DIR")
                        .HasMaxLength(1);

                    b.Property<string>("FBLOCK_L")
                        .HasMaxLength(4);

                    b.Property<string>("FBLOCK_R")
                        .HasMaxLength(4);

                    b.Property<int>("FIREBAT_L");

                    b.Property<int>("FIREBAT_R");

                    b.Property<int>("FMCD_L");

                    b.Property<int>("FMCD_R");

                    b.Property<int>("FNODE");

                    b.Property<int>("FOODINSP_L");

                    b.Property<int>("FOODINSP_R");

                    b.Property<int>("FOR_BL_L");

                    b.Property<int>("FOR_BL_R");

                    b.Property<int>("FOR_PM_L");

                    b.Property<int>("FOR_PM_R");

                    b.Property<int>("FOR_TR_L");

                    b.Property<int>("FOR_TR_R");

                    b.Property<int>("FROM_NODE");

                    b.Property<string>("FTRACT_L")
                        .HasMaxLength(6);

                    b.Property<string>("FTRACT_R")
                        .HasMaxLength(6);

                    b.Property<double>("HIGH_X");

                    b.Property<double>("HIGH_Y");

                    b.Property<int>("HI_ADD_L");

                    b.Property<int>("HI_ADD_R");

                    b.Property<double>("LENGTH");

                    b.Property<int>("LEVEL");

                    b.Property<string>("LOCDIST_L")
                        .HasMaxLength(8);

                    b.Property<string>("LOCDIST_R")
                        .HasMaxLength(8);

                    b.Property<double>("LOW_X");

                    b.Property<double>("LOW_Y");

                    b.Property<int>("LO_ADD_L");

                    b.Property<int>("LO_ADD_R");

                    b.Property<int>("LPOLY");

                    b.Property<int>("MPS_ELEM_L");

                    b.Property<int>("MPS_ELEM_R");

                    b.Property<int>("MPS_HS_L");

                    b.Property<int>("MPS_HS_R");

                    b.Property<int>("MPS_MS_L");

                    b.Property<int>("MPS_MS_R");

                    b.Property<string>("MUNICODE_L")
                        .HasMaxLength(3);

                    b.Property<string>("MUNICODE_R")
                        .HasMaxLength(3);

                    b.Property<string>("MUNI_L")
                        .HasMaxLength(15);

                    b.Property<string>("MUNI_R")
                        .HasMaxLength(15);

                    b.Property<decimal>("MaxLat")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("MaxLng")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("MinLat")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("MinLng")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                        .HasColumnType("decimal(5, 2)");

                    b.Property<int>("NEWDIMENR");

                    b.Property<IGeometry>("Outline");

                    b.Property<int>("POLICE_L");

                    b.Property<int>("POLICE_R");

                    b.Property<int>("POLRD_L");

                    b.Property<int>("POLRD_R");

                    b.Property<string>("QTR_SECT_L")
                        .HasMaxLength(5);

                    b.Property<string>("QTR_SECT_R")
                        .HasMaxLength(5);

                    b.Property<int>("RCD_NBR");

                    b.Property<string>("RECYC_DA_L")
                        .HasMaxLength(3);

                    b.Property<string>("RECYC_DA_R")
                        .HasMaxLength(3);

                    b.Property<string>("RECYC_SM_L")
                        .HasMaxLength(20);

                    b.Property<string>("RECYC_SM_R")
                        .HasMaxLength(20);

                    b.Property<string>("RECYC_WN_L")
                        .HasMaxLength(20);

                    b.Property<string>("RECYC_WN_R")
                        .HasMaxLength(20);

                    b.Property<int>("RPOLY");

                    b.Property<string>("SANBIZPL_L")
                        .HasMaxLength(10);

                    b.Property<string>("SANBIZPL_R")
                        .HasMaxLength(10);

                    b.Property<string>("SANLEAF_L")
                        .HasMaxLength(5);

                    b.Property<string>("SANLEAF_R")
                        .HasMaxLength(5);

                    b.Property<string>("SANPLOW_L")
                        .HasMaxLength(3);

                    b.Property<string>("SANPLOW_R")
                        .HasMaxLength(3);

                    b.Property<string>("SAN_DIST_L")
                        .HasMaxLength(2);

                    b.Property<string>("SAN_DIST_R")
                        .HasMaxLength(2);

                    b.Property<int>("SCHOOL2K_L");

                    b.Property<int>("SCHOOL2K_R");

                    b.Property<int>("SCHOOL_L");

                    b.Property<int>("SCHOOL_R");

                    b.Property<string>("SEG_L_TYPE")
                        .HasMaxLength(10);

                    b.Property<int>("SPRINK_R");

                    b.Property<int>("SQUAD_L");

                    b.Property<int>("SQUAD_R");

                    b.Property<int>("STASS2K_L");

                    b.Property<int>("STASS2K_R");

                    b.Property<int>("STASS_L");

                    b.Property<int>("STASS_R");

                    b.Property<int>("STCLASS");

                    b.Property<string>("STREET")
                        .HasMaxLength(18);

                    b.Property<int>("STSEN2K_L");

                    b.Property<int>("STSEN2K_R");

                    b.Property<int>("STSEN_L");

                    b.Property<int>("STSEN_R");

                    b.Property<string>("STTYPE")
                        .HasMaxLength(2);

                    b.Property<string>("ST_MAIN_L")
                        .HasMaxLength(10);

                    b.Property<string>("ST_MAIN_R")
                        .HasMaxLength(10);

                    b.Property<int>("ST_OP_L");

                    b.Property<int>("ST_OP_R");

                    b.Property<string>("SUM_DA_L")
                        .HasMaxLength(3);

                    b.Property<string>("SUM_DA_R")
                        .HasMaxLength(3);

                    b.Property<string>("SUM_RT_L")
                        .HasMaxLength(12);

                    b.Property<string>("SUM_RT_R")
                        .HasMaxLength(12);

                    b.Property<int>("TNODE");

                    b.Property<int>("TO_NODE");

                    b.Property<int>("TRACT2K_L");

                    b.Property<int>("TRACT2K_R");

                    b.Property<int>("TRACT_L");

                    b.Property<int>("TRACT_R");

                    b.Property<int>("TRANS_ID");

                    b.Property<int>("WARD2K_L");

                    b.Property<int>("WARD2K_R");

                    b.Property<int>("WARD_L");

                    b.Property<int>("WARD_R");

                    b.Property<string>("WIN_RT_L")
                        .HasMaxLength(12);

                    b.Property<string>("WIN_RT_R")
                        .HasMaxLength(12);

                    b.Property<string>("WTR16TH_L")
                        .HasMaxLength(5);

                    b.Property<string>("WTR16TH_R")
                        .HasMaxLength(5);

                    b.Property<int>("WW_PRES_L");

                    b.Property<int>("WW_PRES_R");

                    b.Property<int>("WW_ROUT_L");

                    b.Property<int>("WW_ROUT_R");

                    b.Property<int>("WW_SERV_L");

                    b.Property<int>("WW_SERV_R");

                    b.Property<int>("ZIP_L");

                    b.Property<int>("ZIP_R");

                    b.HasKey("NEWDIME_ID");

                    b.ToTable("Streets");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("MkeAlerts.Web.Models.Data.Accounts.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("MkeAlerts.Web.Models.Data.Accounts.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("MkeAlerts.Web.Models.Data.Accounts.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("MkeAlerts.Web.Models.Data.Accounts.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MkeAlerts.Web.Models.Data.Accounts.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("MkeAlerts.Web.Models.Data.Accounts.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MkeAlerts.Web.Models.Data.Places.Address", b =>
                {
                    b.HasOne("MkeAlerts.Web.Models.Data.Places.Property", "Property")
                        .WithMany("Addresses")
                        .HasForeignKey("TAXKEY");
                });

            modelBuilder.Entity("MkeAlerts.Web.Models.Data.Places.Parcel", b =>
                {
                    b.HasOne("MkeAlerts.Web.Models.Data.Places.Property", "Property")
                        .WithOne("Parcel")
                        .HasForeignKey("MkeAlerts.Web.Models.Data.Places.Parcel", "TAXKEY")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
