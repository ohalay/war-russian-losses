using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace War.RussianLosses.Api.Infrastructure.Migrations
{
    public partial class AddUniqueIndexDateLossTypeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RussinLosses_Date_LossTypeId",
                table: "RussinLosses",
                columns: new[] { "Date", "LossTypeId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RussinLosses_Date_LossTypeId",
                table: "RussinLosses");
        }
    }
}
