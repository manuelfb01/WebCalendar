﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebCalendar.Datos;

#nullable disable

namespace WebCalendar.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230916105725_Usuarios")]
    partial class Usuarios
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebCalendar.Models.Usuarios", b =>
                {
                    b.Property<int>("ID_Usuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Usuario"));

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("FechaActualizado")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaValidado")
                        .HasColumnType("datetime2");

                    b.Property<string>("KeyPass")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Validado")
                        .HasColumnType("bit");

                    b.HasKey("ID_Usuario");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Usuarios");
                });
#pragma warning restore 612, 618
        }
    }
}