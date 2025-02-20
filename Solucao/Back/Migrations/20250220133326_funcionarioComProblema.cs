using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class funcionarioComProblema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Funcionarios");

            migrationBuilder.AddColumn<string>(
                name: "FotoCaminho",
                table: "Funcionarios",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoCaminho",
                table: "Funcionarios");

            migrationBuilder.AddColumn<byte[]>(
                name: "Foto",
                table: "Funcionarios",
                type: "BLOB",
                nullable: true);
        }
    }
}
