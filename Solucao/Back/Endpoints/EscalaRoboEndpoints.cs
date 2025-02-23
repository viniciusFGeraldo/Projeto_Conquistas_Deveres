using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back.Endpoints
{
    public static class EscalaRoboEndpoints
    {
        public static void MapEscalaEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/escalas");

            // 🔹 Obter todos os funcionários com suas escalas (mesmo para funcionários sem escala)
            group.MapGet("/funcionarios", async (AppDbContext db) =>
            {
                string mesAtual = DateTime.Now.ToString("MM/yyyy");

                var funcionarios = await db.Funcionarios
                    .Include(f => f.EscalasRobo)  // Incluindo as escalas do funcionário
                    .ToListAsync();

                var resultado = funcionarios.Select(f => new
                {
                    f.Nome,
                    f.FotoCaminho,
                    Datas = f.EscalasRobo
                        .Where(e => e.MesAtual == mesAtual)
                        .SelectMany(e => e.Datas)
                        .Select(d => d.ToString("yyyy-MM-dd"))
                        .ToList()
                }).ToList();

                return Results.Ok(resultado);
            });

            // 🔹 Adicionar/remover datas da escala de um funcionário e atualizar a lista de datas disponíveis
            group.MapPost("/atualizar/{funcionarioId:int}", async (int funcionarioId, [FromBody] DataRequest request, AppDbContext db) =>
            {
                if (request?.Datas == null || !request.Datas.Any())
                {
                    return Results.BadRequest("A lista de datas não pode ser vazia.");
                }

                // Convertendo as strings para DateTime
                var datasConvertidas = request.Datas.Select(d => DateTime.ParseExact(d, "yyyy-MM-dd", null)).ToList();

                // Buscar o funcionário e incluir suas escalas
                var funcionario = await db.Funcionarios.Include(f => f.EscalasRobo)
                    .FirstOrDefaultAsync(f => f.Id == funcionarioId);

                if (funcionario == null)
                {
                    return Results.NotFound("Funcionário não encontrado.");
                }

                string mesAtual = DateTime.Now.ToString("MM/yyyy");

                // Buscar a escala de robo para o funcionário
                var escalaRobo = funcionario.EscalasRobo.FirstOrDefault(e => e.MesAtual == mesAtual);

                if (escalaRobo == null)
                {
                    // Se não houver escala existente, criar uma nova
                    escalaRobo = new EscalaRobo
                    {
                        FuncionarioId = funcionarioId,
                        MesAtual = mesAtual,
                        Datas = new List<DateTime>()
                    };
                    db.EscalasRobo.Add(escalaRobo);
                }

                // Atualizar as datas
                escalaRobo.Datas = datasConvertidas.Distinct().ToList();

                // Salvar as mudanças
                await db.SaveChangesAsync();

                return Results.Ok(new
                {
                    funcionario.Nome,
                    funcionario.FotoCaminho,
                    Datas = escalaRobo.Datas.Select(d => d.ToString("yyyy-MM-dd")).ToList()
                });
            });

        }
    }
}
public class DataRequest
{
    public List<string> Datas { get; set; }
}