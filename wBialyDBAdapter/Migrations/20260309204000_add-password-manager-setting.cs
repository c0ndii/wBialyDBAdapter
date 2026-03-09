using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using wBialyDBAdapter.Database.ObjectRelational;

#nullable disable

namespace wBialyDBAdapter.Migrations
{
    [DbContext(typeof(ORDB))]
    [Migration("20260309204000_add-password-manager-setting")]
    public partial class addpasswordmanagersetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPasswordManagerEnabled",
                table: "UserSecurityProfiles",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPasswordManagerEnabled",
                table: "UserSecurityProfiles");
        }
    }
}
