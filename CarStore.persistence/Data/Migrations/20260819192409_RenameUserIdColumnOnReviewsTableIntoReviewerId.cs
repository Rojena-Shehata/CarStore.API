using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarStore.persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserIdColumnOnReviewsTableIntoReviewerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reviews",
                newName: "ReviewerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewerId",
                table: "Reviews",
                newName: "UserId");
        }
    }
}
