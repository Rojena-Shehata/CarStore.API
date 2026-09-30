using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarStore.persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPurchaseDriverDetailsRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_DriverDetails_DriverId",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_DriverId",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "PurchaseRequests");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_DriveretailsId",
                table: "PurchaseRequests",
                column: "DriveretailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_DriverDetails_DriveretailsId",
                table: "PurchaseRequests",
                column: "DriveretailsId",
                principalTable: "DriverDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_DriverDetails_DriveretailsId",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_DriveretailsId",
                table: "PurchaseRequests");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "PurchaseRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_DriverId",
                table: "PurchaseRequests",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_DriverDetails_DriverId",
                table: "PurchaseRequests",
                column: "DriverId",
                principalTable: "DriverDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
