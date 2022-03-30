﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace War.RussianLosses.Api.Infrastructure.Migrations
{
    [DbContext(typeof(WarContext))]
    partial class WarContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LossType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NameEnglish")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("uri")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LossTypes");
                });

            modelBuilder.Entity("RussinLoss", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("LossTypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LossTypeId");

                    b.ToTable("RussinLosses");
                });

            modelBuilder.Entity("RussinLoss", b =>
                {
                    b.HasOne("LossType", "LosType")
                        .WithMany("RussinLosses")
                        .HasForeignKey("LossTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LosType");
                });

            modelBuilder.Entity("LossType", b =>
                {
                    b.Navigation("RussinLosses");
                });
#pragma warning restore 612, 618
        }
    }
}
