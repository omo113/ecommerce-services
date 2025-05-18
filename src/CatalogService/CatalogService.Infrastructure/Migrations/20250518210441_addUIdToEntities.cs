using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addUIdToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddColumn<Guid>(
                name: "UId",
                table: "Categories",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Products_UId",
                table: "Products",
                column: "UId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Categories_UId",
                table: "Categories",
                column: "UId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Products_UId",
                table: "Products");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Categories_UId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "Categories");
        }
    }
}
