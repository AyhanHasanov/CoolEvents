using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoolEvents.Migrations
{
    public partial class entitiesamendments2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTickets_Tickets_UserId",
                table: "UserTickets");

            migrationBuilder.DropColumn(
                name: "ForEventId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_UserTickets_TicketId",
                table: "UserTickets",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTickets_Tickets_TicketId",
                table: "UserTickets",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTickets_Tickets_TicketId",
                table: "UserTickets");

            migrationBuilder.DropIndex(
                name: "IX_UserTickets_TicketId",
                table: "UserTickets");

            migrationBuilder.AddColumn<int>(
                name: "ForEventId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTickets_Tickets_UserId",
                table: "UserTickets",
                column: "UserId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
