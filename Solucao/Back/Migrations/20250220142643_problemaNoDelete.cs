using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class problemaNoDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projetos_Funcionarios_ResponsavelId",
                table: "Projetos");

            migrationBuilder.DropForeignKey(
                name: "FK_Projetos_Funcionarios_SubResponsavelId",
                table: "Projetos");

            migrationBuilder.AddForeignKey(
                name: "FK_Projetos_Funcionarios_ResponsavelId",
                table: "Projetos",
                column: "ResponsavelId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projetos_Funcionarios_SubResponsavelId",
                table: "Projetos",
                column: "SubResponsavelId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projetos_Funcionarios_ResponsavelId",
                table: "Projetos");

            migrationBuilder.DropForeignKey(
                name: "FK_Projetos_Funcionarios_SubResponsavelId",
                table: "Projetos");

            migrationBuilder.AddForeignKey(
                name: "FK_Projetos_Funcionarios_ResponsavelId",
                table: "Projetos",
                column: "ResponsavelId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projetos_Funcionarios_SubResponsavelId",
                table: "Projetos",
                column: "SubResponsavelId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
