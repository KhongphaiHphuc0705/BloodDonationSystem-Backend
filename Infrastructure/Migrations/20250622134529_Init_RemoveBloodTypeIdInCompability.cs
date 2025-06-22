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
            migrationBuilder.DropForeignKey(
                name: "FK_BloodCompatibilities_BloodTypes_BloodTypeId",
                table: "BloodCompatibilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BloodCompatibilities",
                table: "BloodCompatibilities");

            migrationBuilder.DropIndex(
                name: "IX_BloodCompatibilities_BloodTypeId",
                table: "BloodCompatibilities");

            migrationBuilder.DropColumn(
                name: "BloodTypeId",
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

            migrationBuilder.AddColumn<int>(
                name: "BloodTypeId",
                table: "BloodCompatibilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BloodCompatibilities",
                table: "BloodCompatibilities",
                columns: new[] { "Id", "BloodTypeId", "DonorTypeId", "RecipientTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_BloodCompatibilities_BloodTypeId",
                table: "BloodCompatibilities",
                column: "BloodTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BloodCompatibilities_BloodTypes_BloodTypeId",
                table: "BloodCompatibilities",
                column: "BloodTypeId",
                principalTable: "BloodTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
