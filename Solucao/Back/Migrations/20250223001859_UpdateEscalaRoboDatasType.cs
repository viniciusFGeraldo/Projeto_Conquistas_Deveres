using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEscalaRoboDatasType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    FotoCaminho = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Escalas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Mes = table.Column<string>(type: "TEXT", nullable: false),
                    DatasDisponiveis = table.Column<string>(type: "TEXT", nullable: false, comment: "Lista de datas associadas à escala do mês."),
                    FuncionarioId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escalas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Escalas_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EscalasRobo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    MesAtual = table.Column<string>(type: "TEXT", nullable: false),
                    FuncionarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    Datas = table.Column<string>(type: "TEXT", nullable: false, comment: "Lista de datas associadas à escala do robô.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscalasRobo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EscalasRobo_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projetos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    ResponsavelId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubResponsavelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Resultado = table.Column<string>(type: "TEXT", nullable: true),
                    Entrega1 = table.Column<string>(type: "TEXT", nullable: true),
                    Entrega2 = table.Column<string>(type: "TEXT", nullable: true),
                    Entrega3 = table.Column<string>(type: "TEXT", nullable: true),
                    Nota = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projetos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projetos_Funcionarios_ResponsavelId",
                        column: x => x.ResponsavelId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projetos_Funcionarios_SubResponsavelId",
                        column: x => x.SubResponsavelId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Escalas_FuncionarioId",
                table: "Escalas",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_EscalasRobo_FuncionarioId",
                table: "EscalasRobo",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_ResponsavelId",
                table: "Projetos",
                column: "ResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_SubResponsavelId",
                table: "Projetos",
                column: "SubResponsavelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Escalas");

            migrationBuilder.DropTable(
                name: "EscalasRobo");

            migrationBuilder.DropTable(
                name: "Projetos");

            migrationBuilder.DropTable(
                name: "Funcionarios");
        }
    }
}
