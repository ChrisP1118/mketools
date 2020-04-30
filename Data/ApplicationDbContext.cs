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

namespace MkeAlerts.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ExternalCredential> ExternalCredentials { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<CommonParcel> CommonParcels { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<PoliceDispatchCall> PoliceDispatchCalls { get; set; }
        public DbSet<PoliceDispatchCallType> PoliceDispatchCallTypes { get; set; }
        public DbSet<FireDispatchCall> FireDispatchCalls { get; set; }
        public DbSet<FireDispatchCallType> FireDispatchCallTypes { get; set; }
        public DbSet<Crime> Crimes { get; set; }

        public DbSet<DispatchCallSubscription> DispatchCallSubscriptions { get; set; }
        public DbSet<PickupDatesSubscription> PickupDateSubscriptions { get; set; }

        public DbQuery<StringReference> StreetNames { get; set; }
        public DbQuery<StringReference> StreetDirections { get; set; }
        public DbQuery<StringReference> StreetTypes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Property>()
                .HasKey(x => x.TAXKEY);

            modelBuilder.Entity<Property>()
                .HasMany(x => x.Addresses)
                .WithOne(x => x.Property)
                .HasForeignKey(x => x.TAXKEY);

            //modelBuilder.Entity<Address>()
            //    .HasOne(x => x.Property)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Address>()
            //    .HasOne(x => x.Property)
            //    .WithMany();

            //modelBuilder.Entity<Property>()
            //    .HasMany(x => x.Addresses)
            //    .WithOne(y => y.Property)
            //    .IsRequired(false)

            modelBuilder.Entity<Parcel>()
                .HasOne(x => x.Property)
                .WithOne(x => x.Parcel)
                .HasForeignKey<Parcel>(x => x.Taxkey);

            modelBuilder.Entity<Parcel>()
                .HasKey(x => x.Taxkey);

            modelBuilder.Entity<CommonParcel>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<CommonParcel>()
                .HasMany(x => x.Parcels)
                .WithOne(x => x.CommonParcel)
                .HasForeignKey(x => x.CommonParcelId);

            //modelBuilder.Entity<Property>()
            //    .HasOne(x => x.Parcel)
            //    .WithOne(y => y.Property);

            //modelBuilder.Entity<Parcel>()
            //    .HasOne(x => x.CommonParcel)
            //    .WithMany(x => x.Parcels);

            //modelBuilder.Entity<CommonParcel>()
            //    .HasMany(x => x.Parcels)
            //    .WithOne(x => x.CommonParcel)
            //    .HasForeignKey(x => x.CommonParcelId)
            //    .IsRequired(true);

            modelBuilder.Entity<CommonParcel>()
                .HasIndex(x => x.MinLat);

            modelBuilder.Entity<CommonParcel>()
                .HasIndex(x => x.MaxLat);

            modelBuilder.Entity<CommonParcel>()
                .HasIndex(x => x.MinLng);

            modelBuilder.Entity<CommonParcel>()
                .HasIndex(x => x.MaxLng);

            modelBuilder.Entity<Address>()
                .HasKey(x => x.RCD_NBR);

            modelBuilder.Entity<Street>()
                .HasKey(x => x.NEWDIME_ID);

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

            modelBuilder.Entity<ExternalCredential>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.ExternalCredentials)
                .HasForeignKey(x => x.ApplicationUserId);

            modelBuilder.Entity<DispatchCallSubscription>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.DispatchCallSubscriptions)
                .HasForeignKey(x => x.ApplicationUserId);

            modelBuilder.Entity<PickupDatesSubscription>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.PickupDateSubscriptions)
                .HasForeignKey(x => x.ApplicationUserId);

            //modelBuilder.Entity<Role>()
            //    .HasKey(x => new { x.ApplicationUserId, x.StationId });

            //modelBuilder.Entity<Role>()
            //    .HasOne(x => x.ApplicationUser)
            //    .WithMany(x => x.Roles)
            //    .HasForeignKey(x => x.ApplicationUserId);

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
