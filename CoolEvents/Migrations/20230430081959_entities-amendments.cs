using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoolEvents.Migrations
{
    public partial class entitiesamendments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTickets_Events_EventId",
                table: "UserTickets");

            migrationBuilder.DropIndex(
                name: "IX_UserTickets_EventId",
                table: "UserTickets");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "UserTickets");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "UserTickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTickets_EventId",
                table: "UserTickets",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTickets_Events_EventId",
                table: "UserTickets",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
