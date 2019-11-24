using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelShare.DAL.Migrations
{
    public partial class AddedNewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameStateId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "UnitsInStock",
                table: "Hotel",
                newName: "AvailableRooms");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Hotel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Hotel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Hotel");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Hotel");

            migrationBuilder.RenameColumn(
                name: "AvailableRooms",
                table: "Hotel",
                newName: "UnitsInStock");

            migrationBuilder.AddColumn<Guid>(
                name: "GameStateId",
                table: "Comment",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
