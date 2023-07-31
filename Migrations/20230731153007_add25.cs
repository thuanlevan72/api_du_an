﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOLYFOOD.Migrations
{
    /// <inheritdoc />
    public partial class add25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_show",
                table: "news",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_show",
                table: "news");
        }
    }
}
