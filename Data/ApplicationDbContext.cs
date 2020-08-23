using MkeAlerts.Web.Models.Data.Accounts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MkeAlerts.Web.Models.Data;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Models.Data.Subscriptions;
using MkeAlerts.Web.Models.Data.AppHealth;
using MkeAlerts.Web.Models.Data.HistoricPhotos;

namespace MkeAlerts.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ExternalCredential> ExternalCredentials { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<CommonParcel> CommonParcels { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PoliceDispatchCall> PoliceDispatchCalls { get; set; }
        public DbSet<PoliceDispatchCallType> PoliceDispatchCallTypes { get; set; }
        public DbSet<FireDispatchCall> FireDispatchCalls { get; set; }
        public DbSet<FireDispatchCallType> FireDispatchCallTypes { get; set; }
        public DbSet<Crime> Crimes { get; set; }
        public DbSet<HistoricPhoto> HistoricPhotos { get; set; }

        public DbSet<DispatchCallSubscription> DispatchCallSubscriptions { get; set; }
        public DbSet<PickupDatesSubscription> PickupDateSubscriptions { get; set; }

        public DbSet<StringReference> StreetNames { get; set; }
        public DbSet<StringReference> StreetDirections { get; set; }
        public DbSet<StringReference> StreetTypes { get; set; }

        public DbSet<JobRun> JobRuns { get; set; }

        public DbSet<CurrentPropertyRecord> CurrentPropertyRecords { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* Places */

            modelBuilder.Entity<Parcel>()
                .HasKey(x => x.TAXKEY);

            modelBuilder.Entity<Address>()
                .HasKey(x => x.ADDRESS_ID);

            modelBuilder.Entity<Address>()
                .HasOne(x => x.Parcel)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.TAXKEY);

            // This combination of fields is used by the geocoding service
            modelBuilder.Entity<Address>()
                .HasIndex(x => new { x.DIR, x.STREET, x.STTYPE, x.HouseNumber });

            modelBuilder.Entity<Parcel>()
                .HasOne(x => x.CommonParcel)
                .WithMany(x => x.Parcels)
                .HasForeignKey(x => x.MAP_ID);

            // This combination of fields is used by the geocoding service
            modelBuilder.Entity<Parcel>()
                .HasIndex(x => new { x.STREETDIR, x.STREETNAME, x.STREETTYPE, x.HouseNumber });

            modelBuilder.Entity<CommonParcel>()
                .HasKey(x => x.MAP_ID);

            modelBuilder.Entity<CommonParcel>()
                .HasIndex(x => x.MinLat);

            modelBuilder.Entity<CommonParcel>()
                .HasIndex(x => x.MaxLat);

            modelBuilder.Entity<CommonParcel>()
                .HasIndex(x => x.MinLng);

            modelBuilder.Entity<CommonParcel>()
                .HasIndex(x => x.MaxLng);

            modelBuilder.Entity<Street>()
                .HasKey(x => x.CLINEID);

            modelBuilder.Entity<Street>()
                .HasIndex(x => x.MinLat);

            modelBuilder.Entity<Street>()
                .HasIndex(x => x.MaxLat);

            modelBuilder.Entity<Street>()
                .HasIndex(x => x.MinLng);

            modelBuilder.Entity<Street>()
                .HasIndex(x => x.MaxLng);

            // This combination of fields is used by the geocoding service
            modelBuilder.Entity<Street>()
                .HasIndex(x => new { x.DIR, x.STREET, x.STTYPE });

            modelBuilder.Entity<Property>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Property>()
                .HasIndex(x => x.TAXKEY);

            modelBuilder.Entity<Property>()
                .HasIndex(x => x.Source);

            modelBuilder.Entity<Property>()
                .HasOne(x => x.Parcel)
                .WithMany(x => x.Properties)
                .HasForeignKey(x => x.TAXKEY);

            modelBuilder.Entity<CurrentPropertyRecord>()
                .HasNoKey()
                .ToView(null);

            modelBuilder.Entity<CurrentProperty>()
                .ToView("CurrentProperties");

            modelBuilder.Entity<Parcel>()
                .HasOne(x => x.CurrentProperty)
                .WithOne(x => x.Parcel)
                .HasForeignKey<CurrentProperty>(p => p.TAXKEY);


            /* Incidents */

            modelBuilder.Entity<PoliceDispatchCall>()
                .HasKey(x => x.CallNumber);

            modelBuilder.Entity<PoliceDispatchCall>()
                .HasIndex(x => x.ReportedDateTime);

            modelBuilder.Entity<PoliceDispatchCallType>()
                .HasKey(x => x.NatureOfCall);

            modelBuilder.Entity<FireDispatchCall>()
                .HasKey(x => x.CFS);

            modelBuilder.Entity<FireDispatchCall>()
                .HasIndex(x => x.ReportedDateTime);

            modelBuilder.Entity<FireDispatchCallType>()
                .HasKey(x => x.NatureOfCall);

            modelBuilder.Entity<Crime>()
                .HasKey(x => x.IncidentNum);

            modelBuilder.Entity<Crime>()
                .HasIndex(x => x.ReportedDateTime);

            modelBuilder.Entity<DispatchCallSubscription>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.DispatchCallSubscriptions)
                .HasForeignKey(x => x.ApplicationUserId);

            /* Pickup Dates */

            modelBuilder.Entity<PickupDatesSubscription>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.PickupDateSubscriptions)
                .HasForeignKey(x => x.ApplicationUserId);

            /* Historic Photos */

            modelBuilder.Entity<HistoricPhoto>()
                .HasKey(x => x.Id);

            // TODO: Should we index Min/Max Lat/Lng? On HistoricPhotos, but also some of these other tables?

            /* Accounts */

            modelBuilder.Entity<ExternalCredential>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.ExternalCredentials)
                .HasForeignKey(x => x.ApplicationUserId);

            modelBuilder.Entity<StringReference>()
               .HasNoKey()
               .ToView(null);

            modelBuilder.Entity<JobRun>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<JobRun>()
                .HasIndex(x => x.StartTime);

            SeedSampleData(modelBuilder);
        }

        public void SeedSampleData(ModelBuilder modelBuilder)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole()
            {
                Id = Guid.Parse("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                Name = ApplicationRole.SiteAdminRole,
                NormalizedName = ApplicationRole.SiteAdminRole
            });

            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser()
            {
                Id = Guid.Parse("85f00d40-d578-4988-9f22-4d023175f852"),
                UserName = "cwilson@mkealerts.com",
                NormalizedUserName = "cwilson@mkealerts.com",
                Email = "cwilson@mkealerts.com",
                NormalizedEmail = "cwilson@mkealerts.com",
                EmailConfirmed = true,
                PasswordHash = passwordHasher.HashPassword(null, "abc123"),
                SecurityStamp = string.Empty
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>()
            {
                RoleId = Guid.Parse("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                UserId = Guid.Parse("85f00d40-d578-4988-9f22-4d023175f852")
            });
        }
    }
}
