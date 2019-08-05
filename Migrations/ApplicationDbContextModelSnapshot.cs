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
                            ConcurrencyStamp = "a9de8910-53b3-4610-9a9c-5130eb65ec0d",
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
                            ConcurrencyStamp = "484b3728-afe6-4ddd-a53a-240f5b17407e",
                            Email = "siteadmin@test.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "siteadmin@test.com",
                            NormalizedUserName = "siteadmin",
                            PasswordHash = "AQAAAAEAACcQAAAAECQBY4XdYQ4dodQi8fFUpo4KY6VwSRfW4K2rp+Pezuu1h3MN2G+fZjhX65+J+nXfjA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "",
                            TwoFactorEnabled = false,
                            UserName = "siteadmin"
                        });
                });

            modelBuilder.Entity("MkeAlerts.Web.Models.Data.Properties.Property", b =>
                {
                    b.Property<string>("TAXKEY")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10);

                    b.Property<string>("AIR_CONDIT")
                        .HasMaxLength(3);

                    b.Property<float>("ANGLE");

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

                    b.Property<DateTime?>("CONVEY_DAT");

                    b.Property<float>("CONVEY_FEE");

                    b.Property<string>("CONVEY_TYP")
                        .HasMaxLength(2);

                    b.Property<string>("CORNER_LOT")
                        .HasMaxLength(1);

                    b.Property<string>("C_A_CLASS")
                        .HasMaxLength(1);

                    b.Property<int>("C_A_EXM_IM");

                    b.Property<int>("C_A_EXM_LA");

                    b.Property<int>("C_A_EXM_TO");

                    b.Property<string>("C_A_EXM_TY")
                        .HasMaxLength(3);

                    b.Property<int>("C_A_IMPRV");

                    b.Property<int>("C_A_LAND");

                    b.Property<string>("C_A_SYMBOL")
                        .HasMaxLength(1);

                    b.Property<int>("C_A_TOTAL");

                    b.Property<IPoint>("Centroid");

                    b.Property<int>("DIV_DROP");

                    b.Property<int>("DIV_ORG");

                    b.Property<string>("DPW_SANITA")
                        .HasMaxLength(2);

                    b.Property<float>("EXM_ACREAG");

                    b.Property<float>("EXM_PER_CT");

                    b.Property<float>("EXM_PER__1");

                    b.Property<string>("FIREPLACE")
                        .HasMaxLength(1);

                    b.Property<string>("GEO_ALDER")
                        .HasMaxLength(2);

                    b.Property<string>("GEO_ALDER_")
                        .HasMaxLength(2);

                    b.Property<string>("GEO_BI_MAI")
                        .HasMaxLength(2);

                    b.Property<string>("GEO_BLOCK")
                        .HasMaxLength(4);

                    b.Property<string>("GEO_FIRE")
                        .HasMaxLength(2);

                    b.Property<string>("GEO_POLICE")
                        .HasMaxLength(2);

                    b.Property<string>("GEO_TRACT")
                        .HasMaxLength(6);

                    b.Property<string>("GEO_ZIP_CO")
                        .HasMaxLength(9);

                    b.Property<string>("HIST_CODE")
                        .HasMaxLength(1);

                    b.Property<int>("HOUSE_NR_H");

                    b.Property<int>("HOUSE_NR_L");

                    b.Property<string>("HOUSE_NR_S")
                        .HasMaxLength(3);

                    b.Property<string>("LAND_USE")
                        .HasMaxLength(4);

                    b.Property<string>("LAND_USE_G")
                        .HasMaxLength(2);

                    b.Property<DateTime?>("LAST_NAME_");

                    b.Property<DateTime?>("LAST_VALUE");

                    b.Property<int>("LOT_AREA");

                    b.Property<string>("NEIGHBORHO")
                        .HasMaxLength(4);

                    b.Property<string>("NR_ROOMS")
                        .HasMaxLength(4);

                    b.Property<float>("NR_STORIES");

                    b.Property<int>("NR_UNITS");

                    b.Property<string>("OWNER_CITY")
                        .HasMaxLength(23);

                    b.Property<string>("OWNER_MAIL")
                        .HasMaxLength(28);

                    b.Property<string>("OWNER_NAME")
                        .HasMaxLength(28);

                    b.Property<string>("OWNER_NA_1")
                        .HasMaxLength(28);

                    b.Property<string>("OWNER_NA_2")
                        .HasMaxLength(28);

                    b.Property<string>("OWNER_ZIP")
                        .HasMaxLength(9);

                    b.Property<string>("OWN_OCPD")
                        .HasMaxLength(1);

                    b.Property<float>("PARCEL_TYP");

                    b.Property<float>("PARKING_SP");

                    b.Property<string>("PARKING_TY")
                        .HasMaxLength(2);

                    b.Property<string>("PLAT_PAGE")
                        .HasMaxLength(5);

                    b.Property<int>("POWDER_ROO");

                    b.Property<string>("P_A_CLASS")
                        .HasMaxLength(1);

                    b.Property<int>("P_A_EXM_IM");

                    b.Property<int>("P_A_EXM_LA");

                    b.Property<int>("P_A_EXM_TO");

                    b.Property<string>("P_A_EXM_TY")
                        .HasMaxLength(3);

                    b.Property<int>("P_A_IMPRV");

                    b.Property<int>("P_A_LAND");

                    b.Property<string>("P_A_SYMBOL")
                        .HasMaxLength(1);

                    b.Property<int>("P_A_TOTAL");

                    b.Property<IGeometry>("Parcel");

                    b.Property<string>("RAZE_STATU")
                        .HasMaxLength(1);

                    b.Property<string>("REASON_FOR")
                        .HasMaxLength(3);

                    b.Property<string>("SDIR")
                        .HasMaxLength(1);

                    b.Property<string>("STREET")
                        .HasMaxLength(18);

                    b.Property<string>("STTYPE")
                        .HasMaxLength(2);

                    b.Property<string>("SUB_ACCT")
                        .HasMaxLength(1);

                    b.Property<string>("SWIM_POOL")
                        .HasMaxLength(1);

                    b.Property<int>("TAX_DELQ");

                    b.Property<string>("TAX_RATE_C")
                        .HasMaxLength(2);

                    b.Property<string>("YR_ASSMT")
                        .HasMaxLength(4);

                    b.Property<string>("YR_BUILT")
                        .HasMaxLength(4);

                    b.Property<string>("ZONING")
                        .HasMaxLength(7);

                    b.HasKey("TAXKEY");

                    b.HasIndex("TAXKEY")
                        .IsUnique();

                    b.ToTable("Properties");
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
#pragma warning restore 612, 618
        }
    }
}
