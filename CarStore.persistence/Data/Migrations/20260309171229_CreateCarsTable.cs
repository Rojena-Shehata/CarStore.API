using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarStore.persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateCarsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    SeatsNumber = table.Column<int>(type: "int", nullable: false),
                    TransmissionType = table.Column<int>(type: "int", nullable: true),
                    FuelType = table.Column<int>(type: "int", nullable: false),
                    Drivetrain = table.Column<int>(type: "int", nullable: false),
                    Engine_Capacity = table.Column<double>(type: "float", nullable: false),
                    Engine_CylindersNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.CheckConstraint("CK_Car_Year", "[Year]>=1885 And [Year]<=2026");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
