using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init_RemoveBloodTypeIdInCompability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BloodCompatibilities",
                table: "BloodCompatibilities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BloodCompatibilities",
                table: "BloodCompatibilities",
                columns: new[] { "Id", "DonorTypeId", "RecipientTypeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BloodCompatibilities",
                table: "BloodCompatibilities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BloodCompatibilities",
                table: "BloodCompatibilities",
                columns: new[] { "Id", "BloodTypeId", "DonorTypeId", "RecipientTypeId" });
        }
    }
}
