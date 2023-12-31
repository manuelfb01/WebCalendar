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
    [Migration("20230917163423_Restricciones")]
    partial class Restricciones
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
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

            modelBuilder.Entity("WebCalendar.Models.Jornadas", b =>
                {
                    b.Property<int>("ID_Jornada")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Jornada"));

                    b.Property<int>("ID_Competicion")
                        .HasColumnType("int");

                    b.Property<int>("Num_Jornada")
                        .HasColumnType("int");

                    b.Property<int>("Num_Vuelta")
                        .HasColumnType("int");

                    b.HasKey("ID_Jornada");

                    b.HasIndex("ID_Competicion");

                    b.ToTable("Jornadas");
                });

            modelBuilder.Entity("WebCalendar.Models.Restricciones", b =>
                {
                    b.Property<int>("ID_Restriccion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Restriccion"));

                    b.Property<int>("ID_Competicion")
                        .HasColumnType("int");

                    b.Property<int>("ID_Equipo")
                        .HasColumnType("int");

                    b.Property<int?>("ID_EquipoRival")
                        .HasColumnType("int");

                    b.Property<int>("ID_Jornada")
                        .HasColumnType("int");

                    b.Property<int>("ID_Tipo_Restriccion")
                        .HasColumnType("int");

                    b.HasKey("ID_Restriccion");

                    b.HasIndex("ID_Competicion");

                    b.HasIndex("ID_Tipo_Restriccion", "ID_Competicion", "ID_Jornada", "ID_Equipo", "ID_EquipoRival")
                        .IsUnique()
                        .HasFilter("[ID_EquipoRival] IS NOT NULL");

                    b.ToTable("Restricciones");
                });

            modelBuilder.Entity("WebCalendar.Models.TiposRestricciones", b =>
                {
                    b.Property<int>("ID_Tipo_Restriccion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Tipo_Restriccion"));

                    b.Property<string>("Tipo_Restriccion")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("ID_Tipo_Restriccion");

                    b.ToTable("TiposRestricciones");

                    b.HasData(
                        new
                        {
                            ID_Tipo_Restriccion = 1,
                            Tipo_Restriccion = "Equipo tiene que ser local"
                        },
                        new
                        {
                            ID_Tipo_Restriccion = 2,
                            Tipo_Restriccion = "Equipo tiene que ser visitante"
                        },
                        new
                        {
                            ID_Tipo_Restriccion = 3,
                            Tipo_Restriccion = "Equipos se tienen que enfrentar"
                        },
                        new
                        {
                            ID_Tipo_Restriccion = 4,
                            Tipo_Restriccion = "Equipos NO se tienen que enfrentar"
                        });
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

            modelBuilder.Entity("WebCalendar.Models.Jornadas", b =>
                {
                    b.HasOne("WebCalendar.Models.Competiciones", "Competicion")
                        .WithMany()
                        .HasForeignKey("ID_Competicion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Competicion");
                });

            modelBuilder.Entity("WebCalendar.Models.Restricciones", b =>
                {
                    b.HasOne("WebCalendar.Models.Competiciones", "Competicion")
                        .WithMany()
                        .HasForeignKey("ID_Competicion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebCalendar.Models.TiposRestricciones", "TipoRestriccion")
                        .WithMany()
                        .HasForeignKey("ID_Tipo_Restriccion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Competicion");

                    b.Navigation("TipoRestriccion");
                });
#pragma warning restore 612, 618
        }
    }
}
