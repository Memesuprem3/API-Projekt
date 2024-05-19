using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Projekt_Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Projekt_Avancerad_.Net_Bokning.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Customer> customer { get; set; }
        public DbSet<BookingHistory> BookingHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // User - Appointment relationship
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId);

            // Customer - Appointment relationship
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Appointments)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId);

            // Company - Appointment relationship
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Appointments)
                .WithOne(a => a.Company)
                .HasForeignKey(a => a.CompanyId);

            // BookingHistory relationship
            modelBuilder.Entity<BookingHistory>()
                .HasOne(bh => bh.Appointment)
                .WithMany(a => a.BookingHistories)
                .HasForeignKey(bh => bh.AppointmentId);

            //SeedData
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = 1,
                FristName = "Anna",
                LastName = "Svensson",
                Adress = "1234 Main St",
                Phone = "123-456-7890",
                Email = "annaecool@hotmail.com"
            });
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = 2,
                FristName = "Jonas",
                LastName = "Hellqvist",
                Adress = "Vivolvägen 12",
                Phone = "7778889932",
                Email = "R41nFire@hotmail.com"
            });
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = 3,
                FristName = "Stefan",
                LastName = "Magnusson",
                Adress = "Hästhagen 3",
                Phone = "7778889932",
                Email = "bilarebra@hotmail.com"
            });
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = 4,
                FristName = "Ronny",
                LastName = "Ronnysson",
                Adress = "Hagelbrakare 41",
                Phone = "7778889932",
                Email = "fettmedraggarvalle@hotmail.com"
            });
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = 5,
                FristName = "Ragge",
                LastName = "Raggesson",
                Adress = "Hagelbrakare 42",
                Phone = "7778889932",
                Email = "fettmedraggarvalle111@hotmail.com"
            });


            modelBuilder.Entity<Company>().HasData(new Company
            {
                CompanyId = 1,
                CompanyName = "SaabParts"

            });
            modelBuilder.Entity<Company>().HasData(new Company
            {
                CompanyId = 2,
                CompanyName = "VoloParts"

            });
            modelBuilder.Entity<Company>().HasData(new Company
            {
                CompanyId = 3,
                CompanyName = "Macken"

            });



            modelBuilder.Entity<Appointment>().HasData(new Appointment
            {
                id = 1,
                AppointDiscription = "Initial Consultation",
                PlacedApp = new DateTime(2011, 06, 22),
                CustomerId = 1,
                CompanyId = 1
            });

            modelBuilder.Entity<Appointment>().HasData(new Appointment
            {
                id = 2,
                AppointDiscription = "Second Consultation",
                PlacedApp = new DateTime(2011, 06, 29),
                CustomerId = 1,
                CompanyId = 1
            });

            modelBuilder.Entity<Appointment>().HasData(new Appointment
            {
                id = 3,
                AppointDiscription = "Third Consultation",
                PlacedApp = new DateTime(2011, 07, 06),
                CustomerId = 1,
                CompanyId = 1
            });

            modelBuilder.Entity<Appointment>().HasData(new Appointment
            {
                id = 4,
                AppointDiscription = "Initial Consultation",
                PlacedApp = new DateTime(2011, 06, 22),
                CustomerId = 2,
                CompanyId = 2
            });



        }
    }
}
