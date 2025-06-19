using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init_IsApproved_BloodRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "BloodRegistrations");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "BloodRegistrations",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "BloodRegistrations");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BloodRegistrations",
                type: "int",
                nullable: true);
        }
    }
}
