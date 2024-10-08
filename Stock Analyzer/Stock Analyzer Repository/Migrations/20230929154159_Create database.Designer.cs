﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Stock_Analyzer_Repository;

#nullable disable

namespace Stock_Analyzer_Repository.Migrations
{
    [DbContext(typeof(StockAnalyzerContext))]
    [Migration("20230929154159_Create database")]
    partial class Createdatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Stock_Analyzer_Repository.DataModels.BhavCopyInfoDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("AvgPrice")
                        .HasColumnType("float");

                    b.Property<double>("ClosePrice")
                        .HasColumnType("float");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("DeliveryPercentage")
                        .HasColumnType("float");

                    b.Property<double>("DeliveryQty")
                        .HasColumnType("float");

                    b.Property<double>("HighPrice")
                        .HasColumnType("float");

                    b.Property<double>("LastPrice")
                        .HasColumnType("float");

                    b.Property<double>("LowPrice")
                        .HasColumnType("float");

                    b.Property<double>("NoOfTrades")
                        .HasColumnType("float");

                    b.Property<double>("OpenPrice")
                        .HasColumnType("float");

                    b.Property<double>("PreviousClose")
                        .HasColumnType("float");

                    b.Property<string>("Series")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TtlTrdQnty")
                        .HasColumnType("float");

                    b.Property<double>("TurnOverLacs")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("BhavCopyInfo");
                });

            modelBuilder.Entity("Stock_Analyzer_Repository.DataModels.BulkDealDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyFullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DealDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StockAction")
                        .HasColumnType("int");

                    b.Property<double>("TradePrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("CompanyId");

                    b.ToTable("BulkDeal");
                });

            modelBuilder.Entity("Stock_Analyzer_Repository.DataModels.ClientDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Client");
                });

            modelBuilder.Entity("Stock_Analyzer_Repository.DataModels.CompanyDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Symbol")
                        .IsUnique();

                    b.ToTable("Company");
                });

            modelBuilder.Entity("Stock_Analyzer_Repository.DataModels.BhavCopyInfoDataModel", b =>
                {
                    b.HasOne("Stock_Analyzer_Repository.DataModels.CompanyDataModel", "Company")
                        .WithMany("BhavCopyInfos")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Stock_Analyzer_Repository.DataModels.BulkDealDataModel", b =>
                {
                    b.HasOne("Stock_Analyzer_Repository.DataModels.ClientDataModel", "Client")
                        .WithMany("Deals")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Stock_Analyzer_Repository.DataModels.CompanyDataModel", "Company")
                        .WithMany("BulkDeals")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Stock_Analyzer_Repository.DataModels.ClientDataModel", b =>
                {
                    b.Navigation("Deals");
                });

            modelBuilder.Entity("Stock_Analyzer_Repository.DataModels.CompanyDataModel", b =>
                {
                    b.Navigation("BhavCopyInfos");

                    b.Navigation("BulkDeals");
                });
#pragma warning restore 612, 618
        }
    }
}
