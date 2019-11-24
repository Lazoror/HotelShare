using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelShare.DAL.Migrations
{
    public partial class OrderDetailsDifx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Hotel_HotelId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ShippedDate",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "HotelId",
                table: "OrderDetail",
                newName: "RoomId");

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelId",
                table: "Comment",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Hotel_HotelId",
                table: "Comment",
                column: "HotelId",
                principalTable: "Hotel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Hotel_HotelId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "OrderDetail",
                newName: "HotelId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippedDate",
                table: "Order",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelId",
                table: "Comment",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Hotel_HotelId",
                table: "Comment",
                column: "HotelId",
                principalTable: "Hotel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
