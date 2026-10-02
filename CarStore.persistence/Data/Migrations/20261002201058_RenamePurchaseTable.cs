using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarStore.persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamePurchaseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_DriverDetails_DriveretailsId",
                table: "PurchaseRequests");

            migrationBuilder.RenameColumn(
                name: "DriveretailsId",
                table: "PurchaseRequests",
                newName: "DriverDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseRequests_DriveretailsId",
                table: "PurchaseRequests",
                newName: "IX_PurchaseRequests_DriverDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_DriverDetails_DriverDetailsId",
                table: "PurchaseRequests",
                column: "DriverDetailsId",
                principalTable: "DriverDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_DriverDetails_DriverDetailsId",
                table: "PurchaseRequests");

            migrationBuilder.RenameColumn(
                name: "DriverDetailsId",
                table: "PurchaseRequests",
                newName: "DriveretailsId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseRequests_DriverDetailsId",
                table: "PurchaseRequests",
                newName: "IX_PurchaseRequests_DriveretailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_DriverDetails_DriveretailsId",
                table: "PurchaseRequests",
                column: "DriveretailsId",
                principalTable: "DriverDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
