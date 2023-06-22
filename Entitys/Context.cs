﻿using Microsoft.EntityFrameworkCore;

namespace FOLYFOOD.Entitys
{
    public class Context: DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Decentralization> Decentralizations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SevicerReview> SevicerReviews { get; set; }

        public DbSet<ReservationDetailsService> ReservationDetailsServices { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentOrder> PaymentOrders { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Info> Infos { get; set; }

        public DbSet<Slides> Slides { get; set; }
        public DbSet<Slide> Slide { get; set; }

        public DbSet<ImagePage> ImagePages { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure default value for CreatedAt and UpdateAt to be the current UTC time of the database
            modelBuilder.Entity<Account>()
                .Property(a => a.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Account>()
                .Property(a => a.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Account>()
               .Property(d => d.UpdatedAt)
               .ValueGeneratedOnUpdate();

            modelBuilder.Entity<Decentralization>()
                .Property(d => d.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Decentralization>()
                .Property(d => d.UpdateAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Decentralization>()
               .Property(d => d.UpdateAt)
               .ValueGeneratedOnUpdate();

            modelBuilder.Entity<User>()
               .Property(d => d.CreatedAt)
               .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<User>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<User>()
               .Property(d => d.UpdatedAt)
               .ValueGeneratedOnUpdate();

            modelBuilder.Entity<Staff>()
               .Property(d => d.CreatedAt)
               .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Staff>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Staff>()
               .Property(d => d.UpdatedAt)
               .ValueGeneratedOnUpdate();

            modelBuilder.Entity<ProductType>()
               .Property(d => d.CreatedAt)
               .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<ProductType>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<ProductType>()
               .Property(d => d.UpdatedAt)
               .ValueGeneratedOnUpdate();

            modelBuilder.Entity<Product>()
              .Property(d => d.CreatedAt)
              .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Product>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Product>()
               .Property(d => d.UpdatedAt)
               .ValueGeneratedOnUpdate();

            modelBuilder.Entity<ProductImage>()
             .Property(d => d.CreatedAt)
             .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<ProductImage>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<ProductImage>()
               .Property(d => d.UpdatedAt)
               .ValueGeneratedOnUpdate();

            modelBuilder.Entity<ProductReview>()
             .Property(d => d.CreatedAt)
             .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<ProductReview>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<ProductReview>()
                .Property(d => d.UpdatedAt)
                .ValueGeneratedOnUpdate();

            modelBuilder.Entity<ReservationDetailsService>()
            .Property(d => d.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<ReservationDetailsService>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<ReservationDetailsService>()
                .Property(d => d.UpdatedAt)
                .ValueGeneratedOnUpdate();

            modelBuilder.Entity<Booking>()
            .Property(d => d.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Booking>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Booking>()
                .Property(d => d.UpdatedAt)
                .ValueGeneratedOnUpdate();

            modelBuilder.Entity<Payment>()
            .Property(d => d.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Payment>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Payment>()
                .Property(d => d.UpdatedAt)
                .ValueGeneratedOnUpdate();

            modelBuilder.Entity<Contact>()
            .Property(d => d.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Contact>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Contact>()
                .Property(d => d.UpdatedAt)
                .ValueGeneratedOnUpdate();

            modelBuilder.Entity<Info>()
            .Property(d => d.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Info>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Info>()
                .Property(d => d.UpdatedAt)
                .ValueGeneratedOnUpdate();

            modelBuilder.Entity<Slides>()
            .Property(d => d.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Slides>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Slides>()
                .Property(d => d.UpdatedAt)
                .ValueGeneratedOnUpdate();

            modelBuilder.Entity<ImagePage>()
            .Property(d => d.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<ImagePage>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<ImagePage>()
                .Property(d => d.UpdatedAt)
                .ValueGeneratedOnUpdate();

            modelBuilder.Entity<PaymentOrder>()
               .Property(d => d.CreatedAt)
               .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<PaymentOrder>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<PaymentOrder>()
               .Property(d => d.UpdatedAt)
               .ValueGeneratedOnUpdate();

            modelBuilder.Entity<Order>()
             .Property(d => d.CreatedAt)
             .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Order>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Order>()
               .Property(d => d.UpdatedAt)
               .ValueGeneratedOnUpdate();

            modelBuilder.Entity<Voucher>()
              .Property(d => d.CreatedAt)
              .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Voucher>()
                .Property(d => d.UpdatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Voucher>()
               .Property(d => d.UpdatedAt)
               .ValueGeneratedOnUpdate();

        }   

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
              //optionsBuilder.UseSqlServer("Server=DESKTOP-NTJ1ROJ; Database=test; integrated security=sspi;TrustServerCertificate=True");
             optionsBuilder.UseSqlServer("Server=203.113.174.12\\MSSQLSERVER2019,1437; Database=polyfood; User Id=thuanlevan72; Password=@Anh123anh; TrustServerCertificate=True; MultipleActiveResultSets=True;");
        }
    }
}
