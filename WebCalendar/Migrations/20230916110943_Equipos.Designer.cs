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
    [Migration("20230916110943_Equipos")]
    partial class Equipos
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebCalendar.Models.Competiciones", b =>
                {
                    b.Property<int>("ID_Competicion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Competicion"));

                    b.Property<bool>("Alternar_Local_Vuelta")
                        .HasColumnType("bit");

                    b.Property<int>("ID_Usuario")
                        .HasColumnType("int");

                    b.Property<int>("Jor_Rep_Enfrentamiento")
                        .HasColumnType("int");

                    b.Property<string>("NombreCompeticion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Num_Jor_Loc")
                        .HasColumnType("int");

                    b.HasKey("ID_Competicion");

                    b.HasIndex("ID_Usuario");

                    b.ToTable("Competiciones");
                });

            modelBuilder.Entity("WebCalendar.Models.Equipos", b =>
                {
                    b.Property<int>("ID_Equipo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Equipo"));

                    b.Property<int>("ID_Competicion")
                        .HasColumnType("int");

                    b.Property<string>("NombreEquipo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID_Equipo");

                    b.HasIndex("ID_Competicion");

                    b.ToTable("Equipos");
                });

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

            modelBuilder.Entity("WebCalendar.Models.Competiciones", b =>
                {
                    b.HasOne("WebCalendar.Models.Usuarios", "Cliente")
                        .WithMany()
                        .HasForeignKey("ID_Usuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("WebCalendar.Models.Equipos", b =>
                {
                    b.HasOne("WebCalendar.Models.Competiciones", "Competicion")
                        .WithMany()
                        .HasForeignKey("ID_Competicion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Competicion");
                });
#pragma warning restore 612, 618
        }
    }
}
