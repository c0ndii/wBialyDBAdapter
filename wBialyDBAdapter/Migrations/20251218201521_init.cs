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
