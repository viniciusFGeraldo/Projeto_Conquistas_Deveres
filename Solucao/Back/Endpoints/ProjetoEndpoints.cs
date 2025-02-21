using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back.Endpoints
{
    public static class ProjetoEndpoints
    {
        public static void MapProjetoEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/projetos");

            // 🔹 Criar um Projeto
            group.MapPost("/", async (AppDbContext db, ProjetoDTO projetoDto) =>
            {
                // Buscar os objetos no banco pelo ID
                var responsavel = await db.Funcionarios.FindAsync(projetoDto.ResponsavelId);
                var subResponsavel = await db.Funcionarios.FindAsync(projetoDto.SubResponsavelId);

                if (responsavel == null || subResponsavel == null)
                {
                    return Results.BadRequest("Responsável ou Sub-Responsável não encontrados.");
                }

                // Criar o objeto Projeto com os dados recebidos
                var projeto = new Projeto
                {
                    Nome = projetoDto.Nome,
                    ResponsavelId = projetoDto.ResponsavelId,
                    Responsavel = responsavel,
                    SubResponsavelId = projetoDto.SubResponsavelId,
                    SubResponsavel = subResponsavel,
                    Resultado = projetoDto.Resultado,
                    Entrega1 = projetoDto.Entrega1,
                    Entrega2 = projetoDto.Entrega2,
                    Entrega3 = projetoDto.Entrega3,
                    Nota = projetoDto.Nota
                };

                db.Projetos.Add(projeto);
                await db.SaveChangesAsync();

                return Results.Created($"/projetos/{projeto.Id}", projeto);
            });

            // 🔹 Buscar todos os Projetos
            group.MapGet("/", async (AppDbContext db) =>
                await db.Projetos.Include(p => p.Responsavel)
                                .Include(p => p.SubResponsavel)
                                .ToListAsync());

            // 🔹 Buscar um Projeto por ID
            group.MapGet("/{id:int}", async (int id, AppDbContext db) =>
            {
                var projeto = await db.Projetos.Include(p => p.Responsavel)
                                            .Include(p => p.SubResponsavel)
                                            .FirstOrDefaultAsync(p => p.Id == id);
                return projeto is not null ? Results.Ok(projeto) : Results.NotFound();
            });

            // 🔹 Atualizar um Projeto
            group.MapPut("/{id:int}", async (int id, [FromBody] ProjetoDTO projetoAtualizado, AppDbContext db) =>
            {
                // Encontrar o projeto pelo ID
                var projeto = await db.Projetos.FindAsync(id);
                if (projeto is null) return Results.NotFound();

                // Verificar se os responsáveis existem no banco de dados
                var responsavel = await db.Funcionarios.FindAsync(projetoAtualizado.ResponsavelId);
                var subResponsavel = await db.Funcionarios.FindAsync(projetoAtualizado.SubResponsavelId);

                if (responsavel is null || subResponsavel is null)
                {
                    return Results.BadRequest("Responsável ou Sub-Responsável não encontrados.");
                }

                // Atualizar os campos do projeto
                projeto.Nome = projetoAtualizado.Nome;
                projeto.ResponsavelId = projetoAtualizado.ResponsavelId;
                projeto.SubResponsavelId = projetoAtualizado.SubResponsavelId;
                projeto.Responsavel = responsavel;  // Atribui o objeto completo
                projeto.SubResponsavel = subResponsavel;  // Atribui o objeto completo
                projeto.Resultado = projetoAtualizado.Resultado;
                projeto.Entrega1 = projetoAtualizado.Entrega1;
                projeto.Entrega2 = projetoAtualizado.Entrega2;
                projeto.Entrega3 = projetoAtualizado.Entrega3;
                projeto.Nota = projetoAtualizado.Nota;

                // Salvar alterações no banco de dados
                await db.SaveChangesAsync();
                return Results.Ok(projeto);
            });



            // 🔹 Deletar um Projeto
            group.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
            {
                var projeto = await db.Projetos.FindAsync(id);
                if (projeto is null) return Results.NotFound();

                db.Projetos.Remove(projeto);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}