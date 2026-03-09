using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wBialyDBAdapter.Migrations
{
    /// <inheritdoc />
    public partial class usersecurityauth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSecurityProfiles",
                columns: table => new
                {
                    UserSecurityProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SuccessfulLoginCount = table.Column<int>(type: "int", nullable: false),
                    FailedLoginCountTotal = table.Column<int>(type: "int", nullable: false),
                    FailedLoginCountSinceLastSuccess = table.Column<int>(type: "int", nullable: false),
                    LastFailedLoginAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSuccessfulLoginAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsLockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    MaxFailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    LockoutDurationSeconds = table.Column<int>(type: "int", nullable: false),
                    LockedUntilUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextAllowedLoginAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSecurityProfiles", x => x.UserSecurityProfileId);
                    table.ForeignKey(
                        name: "FK_UserSecurityProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginAttemptAudits",
                columns: table => new
                {
                    LoginAttemptAuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttemptedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoginIdentifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsExistingUser = table.Column<bool>(type: "bit", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    FailureCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppliedDelaySeconds = table.Column<int>(type: "int", nullable: false),
                    LockedUntilUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserSecurityProfileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginAttemptAudits", x => x.LoginAttemptAuditId);
                    table.ForeignKey(
                        name: "FK_LoginAttemptAudits_UserSecurityProfiles_UserSecurityProfileId",
                        column: x => x.UserSecurityProfileId,
                        principalTable: "UserSecurityProfiles",
                        principalColumn: "UserSecurityProfileId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttemptAudits_AttemptedAtUtc",
                table: "LoginAttemptAudits",
                column: "AttemptedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttemptAudits_LoginIdentifier",
                table: "LoginAttemptAudits",
                column: "LoginIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttemptAudits_UserSecurityProfileId",
                table: "LoginAttemptAudits",
                column: "UserSecurityProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSecurityProfiles_UserId",
                table: "UserSecurityProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginAttemptAudits");

            migrationBuilder.DropTable(
                name: "UserSecurityProfiles");
        }
    }
}
