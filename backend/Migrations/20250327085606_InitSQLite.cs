using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitSQLite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RobotArms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PositionX = table.Column<float>(type: "REAL", nullable: false),
                    PositionY = table.Column<float>(type: "REAL", nullable: false),
                    PositionZ = table.Column<float>(type: "REAL", nullable: false),
                    Rotation = table.Column<float>(type: "REAL", nullable: false),
                    JointAngles = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobotArms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Role = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    ConnectedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastCommandTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsControlling = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovementLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RobotArmId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Joint = table.Column<string>(type: "TEXT", nullable: false),
                    Angle = table.Column<float>(type: "REAL", nullable: false),
                    CommandType = table.Column<string>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovementLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovementLogs_RobotArms_RobotArmId",
                        column: x => x.RobotArmId,
                        principalTable: "RobotArms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovementLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovementLogs_RobotArmId",
                table: "MovementLogs",
                column: "RobotArmId");

            migrationBuilder.CreateIndex(
                name: "IX_MovementLogs_UserId",
                table: "MovementLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovementLogs");

            migrationBuilder.DropTable(
                name: "RobotArms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
