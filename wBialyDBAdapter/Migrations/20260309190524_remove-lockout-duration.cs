using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wBialyDBAdapter.Migrations
{
    /// <inheritdoc />
    public partial class removelockoutduration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockoutDurationSeconds",
                table: "UserSecurityProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LockoutDurationSeconds",
                table: "UserSecurityProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
