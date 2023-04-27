﻿// <auto-generated />
using System;
using JobFileSystem.Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Application.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220702150000_refectored jobfile")]
    partial class refectoredjobfile
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Core.Entities.Contact", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContactMethod")
                        .HasColumnType("int");

                    b.Property<int>("ContactType")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Core.Entities.Estimate", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LongDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RequestForQuoteReceived")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Estimates");
                });

            modelBuilder.Entity("Core.Entities.JobFile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ContactId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateReceived")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EstimateId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PurchaseOrderNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.HasIndex("EstimateId")
                        .IsUnique();

                    b.ToTable("JobFiles");
                });

            modelBuilder.Entity("Core.Entities.MaterialTestReport", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Diameter")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("HeatNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Length")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<ushort>("MaterialForm")
                        .HasColumnType("int");

                    b.Property<string>("MaterialGrade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaterialType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("Thickness")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("UnitOfMeasure")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vendor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Width")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id");

                    b.ToTable("MaterialTestReports");
                });

            modelBuilder.Entity("Core.Entities.Estimate", b =>
                {
                    b.HasOne("Core.Entities.Contact", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Core.Entities.Attachment", "Attachments", b1 =>
                        {
                            b1.Property<string>("EstimateId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Id")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("BlobPath")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("ContentType")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FileExtention")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FileName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("EstimateId", "Id");

                            b1.ToTable("Estimates_Attachments");

                            b1.WithOwner()
                                .HasForeignKey("EstimateId");
                        });

                    b.OwnsMany("Core.Entities.LineItem", "LineItems", b1 =>
                        {
                            b1.Property<string>("EstimateId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Id")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("CreatedBy")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTime?>("CreatedOn")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Delivery")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Description")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<decimal>("EstimatedUnitPrice")
                                .HasColumnType("decimal(18,4)");

                            b1.Property<bool>("IsDeleted")
                                .HasColumnType("bit");

                            b1.Property<DateTime?>("LastModified")
                                .HasColumnType("datetime2");

                            b1.Property<string>("LastModifiedBy")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<decimal>("Qty")
                                .HasColumnType("decimal(18,4)");

                            b1.Property<string>("UnitOfMeasure")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("EstimateId", "Id");

                            b1.ToTable("LineItem");

                            b1.WithOwner()
                                .HasForeignKey("EstimateId");

                            b1.OwnsMany("Core.Entities.Attachment", "Attachments", b2 =>
                                {
                                    b2.Property<string>("LineItemEstimateId")
                                        .HasColumnType("nvarchar(450)");

                                    b2.Property<string>("LineItemId")
                                        .HasColumnType("nvarchar(450)");

                                    b2.Property<string>("Id")
                                        .HasColumnType("nvarchar(450)");

                                    b2.Property<string>("BlobPath")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("ContentType")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("FileExtention")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("FileName")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.HasKey("LineItemEstimateId", "LineItemId", "Id");

                                    b2.ToTable("LineItem_Attachments");

                                    b2.WithOwner()
                                        .HasForeignKey("LineItemEstimateId", "LineItemId");
                                });

                            b1.Navigation("Attachments");
                        });

                    b.Navigation("Attachments");

                    b.Navigation("Client");

                    b.Navigation("LineItems");
                });

            modelBuilder.Entity("Core.Entities.JobFile", b =>
                {
                    b.HasOne("Core.Entities.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.Estimate", "Estimate")
                        .WithOne()
                        .HasForeignKey("Core.Entities.JobFile", "EstimateId")
                        .IsRequired();

                    b.OwnsMany("Core.Entities.Attachment", "Attachments", b1 =>
                        {
                            b1.Property<string>("JobFileId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Id")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("BlobPath")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("ContentType")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FileExtention")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FileName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("JobFileId", "Id");

                            b1.ToTable("JobFiles_Attachments");

                            b1.WithOwner()
                                .HasForeignKey("JobFileId");
                        });

                    b.Navigation("Attachments");

                    b.Navigation("Contact");

                    b.Navigation("Estimate");
                });

            modelBuilder.Entity("Core.Entities.MaterialTestReport", b =>
                {
                    b.OwnsMany("Core.Entities.Attachment", "Attachments", b1 =>
                        {
                            b1.Property<string>("MaterialTestReportId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Id")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("BlobPath")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("ContentType")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FileExtention")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FileName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("MaterialTestReportId", "Id");

                            b1.ToTable("MaterialTestReports_Attachments");

                            b1.WithOwner()
                                .HasForeignKey("MaterialTestReportId");
                        });

                    b.Navigation("Attachments");
                });
#pragma warning restore 612, 618
        }
    }
}
