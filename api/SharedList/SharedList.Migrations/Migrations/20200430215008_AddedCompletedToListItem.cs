using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedList.Migrations.Migrations
{
    public partial class AddedCompletedToListItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "ListItem",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "ListItem");
        }
    }
}
