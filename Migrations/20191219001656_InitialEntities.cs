using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EsxiRestfulApi.Migrations
{
    public partial class InitialEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filesystems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Size = table.Column<string>(nullable: true),
                    Free = table.Column<string>(nullable: true),
                    VolumeName = table.Column<string>(nullable: true),
                    Uuid = table.Column<Guid>(nullable: false),
                    Mounted = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filesystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VSwitches",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    NumPorts = table.Column<int>(nullable: false),
                    UsedPorts = table.Column<int>(nullable: false),
                    ConfiguredPorts = table.Column<int>(nullable: false),
                    MTU = table.Column<int>(nullable: false),
                    CDPStatus = table.Column<string>(nullable: true),
                    BeaconEnabled = table.Column<bool>(nullable: false),
                    BeaconInterval = table.Column<int>(nullable: false),
                    BeaconThreshold = table.Column<int>(nullable: false),
                    BeaconRequiredBy = table.Column<string>(nullable: true),
                    Uplinks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VSwitches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PortGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    VSwitchId = table.Column<Guid>(nullable: false),
                    ActiveClients = table.Column<int>(nullable: false),
                    VLANId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortGroups_VSwitches_VSwitchId",
                        column: x => x.VSwitchId,
                        principalTable: "VSwitches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortGroups_VSwitchId",
                table: "PortGroups",
                column: "VSwitchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filesystems");

            migrationBuilder.DropTable(
                name: "PortGroups");

            migrationBuilder.DropTable(
                name: "VSwitches");
        }
    }
}
