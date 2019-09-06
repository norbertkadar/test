using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExamenAppDotNet.Migrations
{
    public partial class AddedPackages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountryOrigin = table.Column<string>(nullable: false),
                    Expeditor = table.Column<string>(nullable: false),
                    DestinationCountry = table.Column<string>(nullable: false),
                    Destinatar = table.Column<string>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    TrackingCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Packages_TrackingCode",
                table: "Packages",
                column: "TrackingCode",
                unique: true,
                filter: "[TrackingCode] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Packages");
        }
    }
}
