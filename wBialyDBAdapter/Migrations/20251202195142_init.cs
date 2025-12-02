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
                name: "Tag_Event",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag_Event", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "Tag_Gastro",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GastroID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag_Gastro", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "EventTag_Event",
                columns: table => new
                {
                    EventTagsTagID = table.Column<int>(type: "int", nullable: false),
                    EventsPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTag_Event", x => new { x.EventTagsTagID, x.EventsPostId });
                    table.ForeignKey(
                        name: "FK_EventTag_Event_Events_EventsPostId",
                        column: x => x.EventsPostId,
                        principalTable: "Events",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventTag_Event_Tag_Event_EventTagsTagID",
                        column: x => x.EventTagsTagID,
                        principalTable: "Tag_Event",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GastroTag_Gastro",
                columns: table => new
                {
                    GastroTagsTagID = table.Column<int>(type: "int", nullable: false),
                    GastrosPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastroTag_Gastro", x => new { x.GastroTagsTagID, x.GastrosPostId });
                    table.ForeignKey(
                        name: "FK_GastroTag_Gastro_Gastros_GastrosPostId",
                        column: x => x.GastrosPostId,
                        principalTable: "Gastros",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GastroTag_Gastro_Tag_Gastro_GastroTagsTagID",
                        column: x => x.GastroTagsTagID,
                        principalTable: "Tag_Gastro",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
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
                table: "Gastros",
                columns: new[] { "PostId", "AddDate", "Author", "Description", "Link", "Place", "Title" },
                values: new object[,]
                {
                    { 3, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Promocje na pizzę w całym mieście.", "https://gastro.com/pizza", "PizzaHouse", "Pizza Day" },
                    { 4, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Święto kuchni wegańskiej.", "https://gastro.com/vegan", "GreenFood", "Vegan Fest" }
                });

            migrationBuilder.InsertData(
                table: "Tag_Event",
                columns: new[] { "TagID", "EventID", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Music" },
                    { 2, 0, "Sport" }
                });

            migrationBuilder.InsertData(
                table: "Tag_Gastro",
                columns: new[] { "TagID", "GastroID", "Name" },
                values: new object[,]
                {
                    { 3, 0, "Pizza" },
                    { 4, 0, "Vegan" }
                });

            migrationBuilder.InsertData(
                table: "EventTag_Event",
                columns: new[] { "EventTagsTagID", "EventsPostId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "GastroTag_Gastro",
                columns: new[] { "GastroTagsTagID", "GastrosPostId" },
                values: new object[,]
                {
                    { 3, 3 },
                    { 4, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventTag_Event_EventsPostId",
                table: "EventTag_Event",
                column: "EventsPostId");

            migrationBuilder.CreateIndex(
                name: "IX_GastroTag_Gastro_GastrosPostId",
                table: "GastroTag_Gastro",
                column: "GastrosPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTag_Event");

            migrationBuilder.DropTable(
                name: "GastroTag_Gastro");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Tag_Event");

            migrationBuilder.DropTable(
                name: "Gastros");

            migrationBuilder.DropTable(
                name: "Tag_Gastro");
        }
    }
}
