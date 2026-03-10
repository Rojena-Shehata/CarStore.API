using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarStore.persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeEngineCapacityColumnDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Engine_Capacity",
                table: "Cars",
                type: "Decimal(10,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Engine_Capacity",
                table: "Cars",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(10,2)");
        }
    }
}
