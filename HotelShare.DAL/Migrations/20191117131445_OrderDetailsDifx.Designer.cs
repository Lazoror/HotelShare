﻿// <auto-generated />
using System;
using HotelShare.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HotelShare.DAL.Migrations
{
    [DbContext(typeof(HotelContext))]
    [Migration("20191117131445_OrderDetailsDifx")]
    partial class OrderDetailsDifx
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.AccountModels.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.AccountModels.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.ToTable("User");
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.AccountModels.UserRole", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.CommentModels.Comment", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Body");

                    b.Property<Guid>("HotelId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<Guid?>("ParentCommentId");

                    b.Property<string>("Quote");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.HasIndex("ParentCommentId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.GameModels.Hotel", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<DateTime>("AddDate");

                    b.Property<short>("AvailableRooms")
                        .HasColumnType("smallint");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Description");

                    b.Property<bool>("Discontinued");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Location");

                    b.Property<string>("Name");

                    b.Property<decimal>("Rating");

                    b.Property<int>("RatingQuantity");

                    b.Property<int>("ViewCount");

                    b.HasKey("Id");

                    b.ToTable("Hotel");
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.GameModels.HotelImage", b =>
                {
                    b.Property<Guid>("HotelId");

                    b.Property<Guid>("ImageId");

                    b.HasKey("HotelId", "ImageId");

                    b.HasIndex("ImageId");

                    b.ToTable("HotelImage");
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.OrderModels.Order", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<Guid>("CustomerId");

                    b.Property<DateTime>("OrderDate");

                    b.Property<int>("OrderStatus");

                    b.Property<string>("Shipper");

                    b.HasKey("Id");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.OrderModels.OrderDetail", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<float>("Discount")
                        .HasColumnType("Real");

                    b.Property<Guid>("OrderId");

                    b.Property<decimal>("Price")
                        .HasColumnType("Money");

                    b.Property<short>("Quantity")
                        .HasColumnType("smallint");

                    b.Property<Guid>("RoomId");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetail");
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.Room", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Description");

                    b.Property<string>("Facilities");

                    b.Property<Guid>("HotelId");

                    b.Property<bool>("IsAvailable");

                    b.Property<bool>("IsDeleted");

                    b.Property<decimal>("Price");

                    b.Property<string>("RoomName");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.AccountModels.UserRole", b =>
                {
                    b.HasOne("HotelShare.Domain.Models.SqlModels.AccountModels.Role", "Role")
                        .WithMany("Roles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HotelShare.Domain.Models.SqlModels.AccountModels.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.CommentModels.Comment", b =>
                {
                    b.HasOne("HotelShare.Domain.Models.SqlModels.GameModels.Hotel", "Hotel")
                        .WithMany("Comments")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HotelShare.Domain.Models.SqlModels.CommentModels.Comment", "ParentComment")
                        .WithMany()
                        .HasForeignKey("ParentCommentId");
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.GameModels.HotelImage", b =>
                {
                    b.HasOne("HotelShare.Domain.Models.SqlModels.GameModels.Hotel", "Hotel")
                        .WithMany("HotelImages")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HotelShare.Domain.Models.SqlModels.Image", "Image")
                        .WithMany("GameGenres")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.OrderModels.OrderDetail", b =>
                {
                    b.HasOne("HotelShare.Domain.Models.SqlModels.OrderModels.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HotelShare.Domain.Models.SqlModels.Room", b =>
                {
                    b.HasOne("HotelShare.Domain.Models.SqlModels.GameModels.Hotel", "Hotel")
                        .WithMany("Rooms")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
