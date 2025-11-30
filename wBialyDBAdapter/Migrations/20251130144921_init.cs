using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
