﻿// <auto-generated />
using System;
using DSProject.MonitoringAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DSProject.MonitoringAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231124154800_added_new_column")]
    partial class added_new_column
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.2.23480.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DSProject.MonitoringAPI.Model.Device", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("LastHourComsumption")
                        .HasColumnType("double precision");

                    b.Property<double>("LastMeasurement")
                        .HasColumnType("double precision");

                    b.Property<double>("MaximumHourlyConsumption")
                        .HasColumnType("double precision");

                    b.Property<Guid?>("userId")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.ToTable("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}
