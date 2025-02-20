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
            group.MapGet("/", async (AppDbContext db) =>
            {
                var escalas = await db.Funcionarios
                    .Select(f => new
                    {
                        Funcionario = f.Nome,
                        Escalas = f.Escalas.Select(e => new
                        {
                            e.MesAtual,
                            Datas = e.Datas.Select(d => d.ToString("dd/MM/yyyy")).ToList()
                        }).ToList()
                    }).ToListAsync();

                return Results.Ok(escalas);
            });

            // 🔹 Adicionar dias na escala de um funcionário (verificar conflito de datas)
            group.MapPost("/adicionar/{funcionarioId:int}", async (int funcionarioId, [FromBody]List<DateTime> datas, AppDbContext db) =>
            {
                // Carregar o funcionário com suas escalas
                var funcionario = await db.Funcionarios.Include(f => f.Escalas).FirstOrDefaultAsync(f => f.Id == funcionarioId);
                if (funcionario is null) return Results.NotFound();

                var mesAtual = DateTime.Now.ToString("MM/yyyy");

                // Verificar se as datas já estão ocupadas por outros funcionários
                foreach (var data in datas)
                {
                    var conflito = await db.Escalas
                        .Where(e => e.Datas.Contains(data) && e.FuncionarioId != funcionarioId)
                        .AnyAsync();

                    if (conflito)
                    {
                        return Results.Conflict($"A data {data:dd/MM/yyyy} já está ocupada.");
                    }
                }

                // Adicionar as datas ao funcionário
                var escala = funcionario.Escalas.FirstOrDefault(e => e.MesAtual == mesAtual);
                if (escala == null)
                {
                    // Criar uma nova escala, mas agora sem a necessidade de passar o Funcionario explicitamente
                    escala = new EscalaRobo { FuncionarioId = funcionarioId, MesAtual = mesAtual };
                    db.Escalas.Add(escala);
                }

                // Adicionar as datas na escala
                escala.Datas.AddRange(datas);

                // Salvar as alterações no banco de dados
                await db.SaveChangesAsync();
                return Results.Ok(escala);
            });

            // 🔹 Remover dias da escala de um funcionário
            group.MapDelete("/remover/{funcionarioId:int}", async (int funcionarioId, [FromBody]List<DateTime> datas, AppDbContext db) =>
            {
                var funcionario = await db.Funcionarios.Include(f => f.Escalas).FirstOrDefaultAsync(f => f.Id == funcionarioId);
                if (funcionario is null) return Results.NotFound();

                var mesAtual = DateTime.Now.ToString("MM/yyyy");
                var escala = funcionario.Escalas.FirstOrDefault(e => e.MesAtual == mesAtual);
                if (escala == null) return Results.NotFound();

                // Remover as datas
                escala.Datas.RemoveAll(d => datas.Contains(d));
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}