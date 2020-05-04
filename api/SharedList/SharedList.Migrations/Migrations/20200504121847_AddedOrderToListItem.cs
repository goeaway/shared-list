using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedList.Migrations.Migrations
{
    public partial class AddedOrderToListItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ListItem",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ListItem");
        }
    }
}
