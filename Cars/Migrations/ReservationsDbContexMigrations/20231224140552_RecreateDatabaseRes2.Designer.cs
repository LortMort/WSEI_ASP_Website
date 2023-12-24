﻿// <auto-generated />
using System;
using Cars.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Cars.Migrations.ReservationsDbContexMigrations
{
    [DbContext(typeof(ReservationsDbContex))]
    [Migration("20231224140552_RecreateDatabaseRes2")]
    partial class RecreateDatabaseRes2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Cars.Models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Avalibity")
                        .HasColumnType("bit");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Power")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Car");
                });

            modelBuilder.Entity("Cars.Models.Reservation", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservationId"), 1L, 1);

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("PickupDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime");

                    b.HasKey("ReservationId");

                    b.HasIndex("CarId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Cars.Models.Reservation", b =>
                {
                    b.HasOne("Cars.Models.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });
#pragma warning restore 612, 618
        }
    }
}