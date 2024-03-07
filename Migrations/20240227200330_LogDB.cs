using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raspagem_API.Migrations
{
    /// <inheritdoc />
    public partial class LogDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logrobo",
                columns: table => new
                {
                    IdLog = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoRobo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioRobo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateLog = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Etapa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InformacaoLog = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProdutoAPI = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.IdLog);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logrobo");
        }
    }
}
