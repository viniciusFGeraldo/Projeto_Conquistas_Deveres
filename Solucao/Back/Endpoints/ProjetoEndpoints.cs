using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back.models;
using Microsoft.EntityFrameworkCore;

namespace Back.Endpoints
{
    public static class ProjetoEndpoints
    {
        public static void MapProjetoEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/projetos");

            // 🔹 Criar um Projeto
            group.MapPost("/", async (AppDbContext db, Projeto projeto) =>
            {
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
            group.MapPut("/{id:int}", async (int id, Projeto projetoAtualizado, AppDbContext db) =>
            {
                var projeto = await db.Projetos.FindAsync(id);
                if (projeto is null) return Results.NotFound();

                projeto.Nome = projetoAtualizado.Nome;
                projeto.ResponsavelId = projetoAtualizado.ResponsavelId;
                projeto.SubResponsavelId = projetoAtualizado.SubResponsavelId;
                projeto.Resultado = projetoAtualizado.Resultado;
                projeto.Entrega1 = projetoAtualizado.Entrega1;
                projeto.Entrega2 = projetoAtualizado.Entrega2;
                projeto.Entrega3 = projetoAtualizado.Entrega3;
                projeto.Nota = projetoAtualizado.Nota;

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