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

            migrationBuilder.InsertData(
                table: "EventTags",
                columns: new[] { "TagID", "EventID", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Music" },
                    { 2, 0, "Sport" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "PostId", "AddDate", "Author", "Description", "Link", "Place", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Największy festiwal rockowy w mieście.", "https://event.com/rock", "Białystok Arena", "Rock Festival" },
                    { 2, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Turniej siatkówki amatorskiej.", "https://event.com/sport", "Hala Sportowa", "Mecz siatkówki" }
                });

            migrationBuilder.InsertData(
                table: "GastroTags",
                columns: new[] { "TagID", "GastroID", "Name" },
                values: new object[,]
                {
                    { 3, 0, "Pizza" },
                    { 4, 0, "Vegan" }
                });

            migrationBuilder.InsertData(
                table: "Gastros",
                columns: new[] { "PostId", "AddDate", "Author", "Description", "Link", "Place", "Title" },
                values: new object[,]
                {
                    { 3, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Promocje na pizzę w całym mieście.", "https://gastro.com/pizza", "PizzaHouse", "Pizza Day" },
                    { 4, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Święto kuchni wegańskiej.", "https://gastro.com/vegan", "GreenFood", "Vegan Fest" }
                });

            migrationBuilder.InsertData(
                table: "EventTagsJoin",
                columns: new[] { "EventTagsTagID", "EventsPostId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "GastroTagsJoin",
                columns: new[] { "GastroTagsTagID", "GastrosPostId" },
                values: new object[,]
                {
                    { 3, 3 },
                    { 4, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventTagsJoin_EventsPostId",
                table: "EventTagsJoin",
                column: "EventsPostId");

            migrationBuilder.CreateIndex(
                name: "IX_GastroTagsJoin_GastrosPostId",
                table: "GastroTagsJoin",
                column: "GastrosPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTagsJoin");

            migrationBuilder.DropTable(
                name: "GastroTagsJoin");

            migrationBuilder.DropTable(
                name: "EventTags");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "GastroTags");

            migrationBuilder.DropTable(
                name: "Gastros");
        }
    }
}
