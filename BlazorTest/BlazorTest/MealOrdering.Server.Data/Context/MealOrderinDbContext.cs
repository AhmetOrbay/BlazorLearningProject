using MealOrdering.Server.Data.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealOrdering.Server.Data.Context
{
    public class MealOrderinDbContext :DbContext
    {
        public MealOrderinDbContext(DbContextOptions<MealOrderinDbContext> options): base(options)
        {

        }

        public DbSet<Users> User { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Supplier> Supplier { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("user", "public");

                entity.HasKey(i => i.Id).HasName("pk_user_id");

                entity.Property(i => i.Id).HasColumnName("id").HasColumnType("uuid").HasDefaultValueSql("UUID_GENERATE_V4()").IsRequired();
                entity.Property(i => i.CreateTime).HasColumnName("CreateTime").HasColumnType("timestamp without time zone").HasDefaultValueSql("NOW()").ValueGeneratedOnAdd();            });


            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("pk_supplier_id");

                entity.ToTable("suppliers", "public");

                entity.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").HasDefaultValueSql("public.uuid_generate_v4()").IsRequired();
                entity.Property(e => e.CreateTime).HasColumnName("CreateTime").HasColumnType("timestamp without time zone").HasDefaultValueSql("NOW()").ValueGeneratedOnAdd();


            });


            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("pk_order_id");

                entity.ToTable("orders", "public");

                entity.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").HasDefaultValueSql("public.uuid_generate_v4()");
                entity.Property(e => e.CreateTime).HasColumnName("CreateTime").HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("NOW()").ValueGeneratedOnAdd();
                entity.Property(e => e.ExpireTime).HasColumnName("ExpireTime").HasColumnType("timestamp without time zone").IsRequired();


            });

            modelBuilder.Entity<OrderItems>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("pk_orderItem_id");

                entity.ToTable("order_items", "public");

                entity.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").HasDefaultValueSql("public.uuid_generate_v4()");
                entity.Property(e => e.CreateTime).HasColumnName("CreateTime").HasColumnType("timestamp without time zone").HasDefaultValueSql("NOW()");

            });
            base.OnModelCreating(modelBuilder);

        }
    }
}
