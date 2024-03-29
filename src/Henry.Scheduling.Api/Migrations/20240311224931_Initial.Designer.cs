﻿// <auto-generated />
using System;
using Henry.Scheduling.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Henry.Scheduling.Api.Migrations
{
    [DbContext(typeof(AppDataContext))]
    [Migration("20240311224931_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Henry.Scheduling.Api.Infrastructure.Data.Entities.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CausationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ConfirmedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExpiredUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ModifiedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SlotId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.ToTable("Appointment", (string)null);
                });

            modelBuilder.Entity("Henry.Scheduling.Api.Infrastructure.Data.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CausationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ModifiedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Client", (string)null);
                });

            modelBuilder.Entity("Henry.Scheduling.Api.Infrastructure.Data.Entities.Provider", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CausationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ModifiedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Provider", (string)null);
                });

            modelBuilder.Entity("Henry.Scheduling.Api.Infrastructure.Data.Entities.Slot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AppointmentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CausationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EndUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ModifiedUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.ToTable("Slot", (string)null);
                });

            modelBuilder.Entity("Henry.Scheduling.Api.Infrastructure.Data.Entities.Appointment", b =>
                {
                    b.HasOne("Henry.Scheduling.Api.Infrastructure.Data.Entities.Provider", null)
                        .WithMany("Appointments")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Henry.Scheduling.Api.Infrastructure.Data.Entities.Slot", b =>
                {
                    b.HasOne("Henry.Scheduling.Api.Infrastructure.Data.Entities.Provider", null)
                        .WithMany("Slots")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Henry.Scheduling.Api.Infrastructure.Data.Entities.Provider", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Slots");
                });
#pragma warning restore 612, 618
        }
    }
}
