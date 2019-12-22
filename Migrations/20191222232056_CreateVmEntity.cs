using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EsxiRestfulApi.Migrations
{
    public partial class CreateVmEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VMs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorldId = table.Column<int>(nullable: false),
                    ProcessId = table.Column<int>(nullable: false),
                    VMXCartelId = table.Column<int>(nullable: false),
                    Uuid = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    ConfigFile = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VMs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VMs");
        }
    }
}
