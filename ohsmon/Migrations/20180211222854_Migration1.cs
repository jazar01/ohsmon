using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ohsmon.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonitorItems",
                columns: table => new
                {
                    ClientID = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(type: "Date", nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    ResponseTime = table.Column<long>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorItems", x => x.ClientID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitorItems");
        }
    }
}
