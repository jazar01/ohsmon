﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ohsmon.Models;
using System;

namespace ohsmon.Migrations
{
    [DbContext(typeof(MonitorContext))]
    [Migration("20180222211314_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("ohsmon.Models.MonitorItem", b =>
                {
                    b.Property<int>("RecordID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientID");

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<string>("Memo");

                    b.Property<uint>("ResponseTime");

                    b.Property<TimeSpan>("Time");

                    b.Property<string>("Type");

                    b.HasKey("RecordID");

                    b.ToTable("MonitorItems");
                });
#pragma warning restore 612, 618
        }
    }
}