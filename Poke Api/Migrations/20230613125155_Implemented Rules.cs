using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poke_Api.Migrations
{
    /// <inheritdoc />
    public partial class ImplementedRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRule",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRule", x => new { x.UserId, x.RuleId });
                    table.ForeignKey(
                        name: "FK_UserRule_Rules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRule_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRule_RuleId",
                table: "UserRule",
                column: "RuleId");

            migrationBuilder.InsertData(
                table: "Rules",
                columns: new[] {"Id", "Name" },
                values: new object[]
                {
                     1 ,
                     "ADMIN" 

                }) ;
            migrationBuilder.InsertData(
             table: "Rules",
             columns: new[] { "Id", "Name" },
             values: new object[]
             {
                     2 ,
                     "USER"

             });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRule");

            migrationBuilder.DropTable(
                name: "Rules");
        }
    }
}
