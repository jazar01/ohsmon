using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ohsmon.Migrations
{
    public partial class UpdateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "MonitorItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "MonitorItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "MonitorItems");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "MonitorItems");
        }
    }
}
