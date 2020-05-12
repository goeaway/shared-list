using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedList.Migrations.Migrations
{
    public partial class AddedUserIdentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListContributor",
                columns: table => new
                {
                    ListId = table.Column<string>(nullable: false),
                    UserIdent = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListContributor", x => new { x.ListId, x.UserIdent });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListContributor");
        }
    }
}
