﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfCoreSample.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Mileage",
                table: "Car",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "Car");
        }
    }
}
