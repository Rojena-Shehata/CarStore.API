using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarStore.persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class CarStore_Application_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buys_Cars_CarId",
                table: "Buys");

            migrationBuilder.DropForeignKey(
                name: "FK_Buys_DriverDetails_DriverId",
                table: "Buys");

            migrationBuilder.DropForeignKey(
                name: "FK_Finances_Buys_PurchaseId",
                table: "Finances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Buys",
                table: "Buys");

            migrationBuilder.RenameTable(
                name: "Buys",
                newName: "PurchaseRequests");

            migrationBuilder.RenameIndex(
                name: "IX_Buys_ReferenceNumber",
                table: "PurchaseRequests",
                newName: "IX_PurchaseRequests_ReferenceNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Buys_DriverId",
                table: "PurchaseRequests",
                newName: "IX_PurchaseRequests_DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_Buys_CarId",
                table: "PurchaseRequests",
                newName: "IX_PurchaseRequests_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseRequests",
                table: "PurchaseRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Finances_PurchaseRequests_PurchaseId",
                table: "Finances",
                column: "PurchaseId",
                principalTable: "PurchaseRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_Cars_CarId",
                table: "PurchaseRequests",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_DriverDetails_DriverId",
                table: "PurchaseRequests",
                column: "DriverId",
                principalTable: "DriverDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Finances_PurchaseRequests_PurchaseId",
                table: "Finances");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_Cars_CarId",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_DriverDetails_DriverId",
                table: "PurchaseRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseRequests",
                table: "PurchaseRequests");

            migrationBuilder.RenameTable(
                name: "PurchaseRequests",
                newName: "Buys");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseRequests_ReferenceNumber",
                table: "Buys",
                newName: "IX_Buys_ReferenceNumber");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseRequests_DriverId",
                table: "Buys",
                newName: "IX_Buys_DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseRequests_CarId",
                table: "Buys",
                newName: "IX_Buys_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Buys",
                table: "Buys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Buys_Cars_CarId",
                table: "Buys",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Buys_DriverDetails_DriverId",
                table: "Buys",
                column: "DriverId",
                principalTable: "DriverDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Finances_Buys_PurchaseId",
                table: "Finances",
                column: "PurchaseId",
                principalTable: "Buys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
