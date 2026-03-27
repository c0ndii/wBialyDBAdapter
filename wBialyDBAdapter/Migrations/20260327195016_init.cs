using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace wBialyDBAdapter.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.PostId);
                });

            migrationBuilder.CreateTable(
                name: "EventTags",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTags", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "Gastros",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastros", x => x.PostId);
                });

            migrationBuilder.CreateTable(
                name: "GastroTags",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GastroID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastroTags", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "EventTagsJoin",
                columns: table => new
                {
                    EventTagsTagID = table.Column<int>(type: "int", nullable: false),
                    EventsPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTagsJoin", x => new { x.EventTagsTagID, x.EventsPostId });
                    table.ForeignKey(
                        name: "FK_EventTagsJoin_EventTags_EventTagsTagID",
                        column: x => x.EventTagsTagID,
                        principalTable: "EventTags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventTagsJoin_Events_EventsPostId",
                        column: x => x.EventsPostId,
                        principalTable: "Events",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GastroTagsJoin",
                columns: table => new
                {
                    GastroTagsTagID = table.Column<int>(type: "int", nullable: false),
                    GastrosPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastroTagsJoin", x => new { x.GastroTagsTagID, x.GastrosPostId });
                    table.ForeignKey(
                        name: "FK_GastroTagsJoin_GastroTags_GastroTagsTagID",
                        column: x => x.GastroTagsTagID,
                        principalTable: "GastroTags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GastroTagsJoin_Gastros_GastrosPostId",
                        column: x => x.GastrosPostId,
                        principalTable: "Gastros",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LatestModifyUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    IsPasswordManagerEnabled = table.Column<bool>(type: "bit", nullable: false),
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
                name: "CanModify",
                columns: table => new
                {
                    CanModifyUserId = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanModify", x => new { x.CanModifyUserId, x.MessageId });
                    table.ForeignKey(
                        name: "FK_CanModify_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanModify_Users_CanModifyUserId",
                        column: x => x.CanModifyUserId,
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

            migrationBuilder.CreateTable(
                name: "PartialPasswords",
                columns: table => new
                {
                    PartialPasswordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserSecurityProfileId = table.Column<int>(type: "int", nullable: false),
                    SlotNumber = table.Column<int>(type: "int", nullable: false),
                    PasswordLength = table.Column<int>(type: "int", nullable: false),
                    RequiredPositions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterHashes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartialPasswords", x => x.PartialPasswordId);
                    table.ForeignKey(
                        name: "FK_PartialPasswords_UserSecurityProfiles_UserSecurityProfileId",
                        column: x => x.UserSecurityProfileId,
                        principalTable: "UserSecurityProfiles",
                        principalColumn: "UserSecurityProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EventTags",
                columns: new[] { "TagID", "EventID", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Music" },
                    { 2, 0, "Sport" },
                    { 3, 0, "Culture" },
                    { 4, 0, "Family" },
                    { 5, 0, "Outdoor" },
                    { 6, 0, "Education" },
                    { 7, 0, "Art" },
                    { 8, 0, "Technology" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "PostId", "AddDate", "Author", "Description", "Link", "Place", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Największy festiwal rockowy w mieście.", "https://event.com/rock", "Białystok Arena", "Rock Festival" },
                    { 2, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Turniej siatkówki amatorskiej.", "https://event.com/sport", "Hala Sportowa", "Mecz siatkówki" },
                    { 3, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Curator", "Prezentacja dzieł lokalnych artystów.", "https://event.com/art", "Galeria Arsenał", "Wystawa sztuki współczesnej" },
                    { 4, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Dzień pełen zabaw dla całej rodziny.", "https://event.com/family", "Park Planty", "Piknik rodzinny w parku" },
                    { 5, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "TechOrg", "Konferencja dla programistów i entuzjastów technologii.", "https://event.com/tech", "Centrum Konferencyjne", "Tech Conference 2025" }
                });

            migrationBuilder.InsertData(
                table: "GastroTags",
                columns: new[] { "TagID", "GastroID", "Name" },
                values: new object[,]
                {
                    { 9, 0, "Pizza" },
                    { 10, 0, "Vegan" },
                    { 11, 0, "Asian" },
                    { 12, 0, "Italian" },
                    { 13, 0, "Dessert" },
                    { 14, 0, "Healthy" },
                    { 15, 0, "FastFood" },
                    { 16, 0, "Seafood" }
                });

            migrationBuilder.InsertData(
                table: "Gastros",
                columns: new[] { "PostId", "AddDate", "Author", "Description", "Link", "Place", "Title" },
                values: new object[,]
                {
                    { 6, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Promocje na pizzę w całym mieście.", "https://gastro.com/pizza", "PizzaHouse", "Pizza Day" },
                    { 7, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Święto kuchni wegańskiej.", "https://gastro.com/vegan", "GreenFood", "Vegan Fest" },
                    { 8, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chef Wang", "Tydzień kuchni azjatyckiej z degustacjami.", "https://gastro.com/asian", "Asia Restaurant", "Asian Food Week" },
                    { 9, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Świeże owoce morza prosto z wybrzeża.", "https://gastro.com/seafood", "Ocean Bistro", "Seafood Fiesta" },
                    { 10, new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pastry Chef", "Najlepsze desery w mieście w jednym miejscu.", "https://gastro.com/dessert", "Sweet Corner", "Dessert Paradise" }
                });

            migrationBuilder.InsertData(
                table: "EventTagsJoin",
                columns: new[] { "EventTagsTagID", "EventsPostId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 4, 4 },
                    { 7, 3 },
                    { 8, 5 }
                });

            migrationBuilder.InsertData(
                table: "GastroTagsJoin",
                columns: new[] { "GastroTagsTagID", "GastrosPostId" },
                values: new object[,]
                {
                    { 9, 6 },
                    { 10, 7 },
                    { 11, 8 },
                    { 13, 10 },
                    { 16, 9 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CanModify_MessageId",
                table: "CanModify",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTagsJoin_EventsPostId",
                table: "EventTagsJoin",
                column: "EventsPostId");

            migrationBuilder.CreateIndex(
                name: "IX_GastroTagsJoin_GastrosPostId",
                table: "GastroTagsJoin",
                column: "GastrosPostId");

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
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PartialPasswords_UserSecurityProfileId",
                table: "PartialPasswords",
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
                name: "CanModify");

            migrationBuilder.DropTable(
                name: "EventTagsJoin");

            migrationBuilder.DropTable(
                name: "GastroTagsJoin");

            migrationBuilder.DropTable(
                name: "LoginAttemptAudits");

            migrationBuilder.DropTable(
                name: "PartialPasswords");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "EventTags");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "GastroTags");

            migrationBuilder.DropTable(
                name: "Gastros");

            migrationBuilder.DropTable(
                name: "UserSecurityProfiles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
