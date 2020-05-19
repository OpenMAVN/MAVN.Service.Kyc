using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Kyc.MsSqlRepositories.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "kyc");

            migrationBuilder.CreateTable(
                name: "kyc_information",
                schema: "kyc",
                columns: table => new
                {
                    partner_id = table.Column<Guid>(nullable: false),
                    admin_user_id = table.Column<Guid>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false),
                    comment = table.Column<string>(nullable: true),
                    status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kyc_information", x => x.partner_id);
                });

            migrationBuilder.CreateTable(
                name: "kyc_status_change",
                schema: "kyc",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    partner_id = table.Column<Guid>(nullable: false),
                    admin_user_id = table.Column<Guid>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false),
                    comment = table.Column<string>(nullable: true),
                    old_status = table.Column<int>(nullable: true),
                    new_status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kyc_status_change", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kyc_information",
                schema: "kyc");

            migrationBuilder.DropTable(
                name: "kyc_status_change",
                schema: "kyc");
        }
    }
}
