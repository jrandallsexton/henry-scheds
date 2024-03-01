using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Henry.Scheduling.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppointmentId",
                table: "Slot",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndUtc",
                table: "Slot",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "Slot",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartUtc",
                table: "Slot",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Appointment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredUtc",
                table: "Appointment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "Appointment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SlotId",
                table: "Appointment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Slot_ProviderId",
                table: "Slot",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ProviderId",
                table: "Appointment",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Provider_ProviderId",
                table: "Appointment",
                column: "ProviderId",
                principalTable: "Provider",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_Provider_ProviderId",
                table: "Slot",
                column: "ProviderId",
                principalTable: "Provider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Provider_ProviderId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Slot_Provider_ProviderId",
                table: "Slot");

            migrationBuilder.DropIndex(
                name: "IX_Slot_ProviderId",
                table: "Slot");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_ProviderId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "EndUtc",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "StartUtc",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "ExpiredUtc",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "Appointment");
        }
    }
}
