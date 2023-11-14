using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orders.Web.Migrations
{
    /// <inheritdoc />
    public partial class EditOrderLines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductPrice",
                table: "OrderLine",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderLine",
                newName: "ProductPrice");
        }
    }
}
