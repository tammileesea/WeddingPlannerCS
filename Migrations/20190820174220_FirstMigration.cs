using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddingPlanner.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "weddings",
                columns: table => new
                {
                    WeddingId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PersonOne = table.Column<string>(nullable: false),
                    PersonTwo = table.Column<string>(nullable: false),
                    WeddingDate = table.Column<DateTime>(nullable: false),
                    WeddingAddress = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weddings", x => x.WeddingId);
                });

            migrationBuilder.CreateTable(
                name: "weddingGuests",
                columns: table => new
                {
                    WeddingGuestId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AttendantId = table.Column<int>(nullable: false),
                    MarriageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weddingGuests", x => x.WeddingGuestId);
                    table.ForeignKey(
                        name: "FK_weddingGuests_users_AttendantId",
                        column: x => x.AttendantId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_weddingGuests_weddings_MarriageId",
                        column: x => x.MarriageId,
                        principalTable: "weddings",
                        principalColumn: "WeddingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_weddingGuests_AttendantId",
                table: "weddingGuests",
                column: "AttendantId");

            migrationBuilder.CreateIndex(
                name: "IX_weddingGuests_MarriageId",
                table: "weddingGuests",
                column: "MarriageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "weddingGuests");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "weddings");
        }
    }
}
