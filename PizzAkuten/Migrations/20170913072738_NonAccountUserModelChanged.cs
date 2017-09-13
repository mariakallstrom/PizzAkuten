using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PizzAkuten.Migrations
{
    public partial class NonAccountUserModelChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_NonAccountUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "NonAccountUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_NonAccountUserId",
                table: "Orders",
                column: "NonAccountUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_NonAccountUserId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "NonAccountUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_NonAccountUserId",
                table: "Orders",
                column: "NonAccountUserId",
                unique: true,
                filter: "[NonAccountUserId] IS NOT NULL");
        }
    }
}
