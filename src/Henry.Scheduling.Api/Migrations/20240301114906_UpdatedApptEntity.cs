using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Henry.Scheduling.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedApptEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Provider_ProviderId",
                table: "Appointment");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProviderId",
                table: "Appointment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Provider_ProviderId",
                table: "Appointment",
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

            migrationBuilder.AlterColumn<Guid>(
                name: "ProviderId",
                table: "Appointment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Provider_ProviderId",
                table: "Appointment",
                column: "ProviderId",
                principalTable: "Provider",
                principalColumn: "Id");
        }
    }
}
