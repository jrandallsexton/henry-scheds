using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Henry.Scheduling.Api.Migrations
{
    /// <inheritdoc />
    public partial class CorrelationCausation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CausationId",
                table: "Slot",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CorrelationId",
                table: "Slot",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CausationId",
                table: "Provider",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CorrelationId",
                table: "Provider",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CausationId",
                table: "Client",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CorrelationId",
                table: "Client",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CausationId",
                table: "Appointment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CorrelationId",
                table: "Appointment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CausationId",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "CausationId",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "CausationId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CausationId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "Appointment");
        }
    }
}
