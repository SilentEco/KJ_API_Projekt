using Microsoft.EntityFrameworkCore.Migrations;

namespace KJ_API_Projekt.Migrations
{
    public partial class testeingv2New : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_geoMessages",
                table: "geoMessages");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "geoMessages");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "geoMessages");

            migrationBuilder.RenameTable(
                name: "geoMessages",
                newName: "geoMessagesV2");

            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "geoMessagesV2",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_geoMessagesV2",
                table: "geoMessagesV2",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_geoMessagesV2_MessageId",
                table: "geoMessagesV2",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_geoMessagesV2_messages_MessageId",
                table: "geoMessagesV2",
                column: "MessageId",
                principalTable: "messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_geoMessagesV2_messages_MessageId",
                table: "geoMessagesV2");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_geoMessagesV2",
                table: "geoMessagesV2");

            migrationBuilder.DropIndex(
                name: "IX_geoMessagesV2_MessageId",
                table: "geoMessagesV2");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "geoMessagesV2");

            migrationBuilder.RenameTable(
                name: "geoMessagesV2",
                newName: "geoMessages");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "geoMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "geoMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_geoMessages",
                table: "geoMessages",
                column: "Id");
        }
    }
}
