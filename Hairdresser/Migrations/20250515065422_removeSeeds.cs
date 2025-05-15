using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hairdresser.Migrations
{
    /// <inheritdoc />
    public partial class removeSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Treatments",
                columns: new[] { "Id", "Description", "Duration", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "En professionell klippning för att ge ditt hår en ny stil.", 60, "Klippning", 500.0 },
                    { 2, "En färgning av ditt hår för att ge det en ny look.", 90, "Färgning", 800.0 },
                    { 3, "En permanent behandling för att ge ditt hår mer volym och lockar.", 120, "Permanent", 1200.0 }
                });
        }
    }
}
