using System;
using System.IO;
using System.Threading.Tasks;
using Back.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back.Endpoints
{
    public static class FuncionarioEndpoints
    {
        public static void MapFuncionarioEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/funcionarios");
            
            //Criar funcionarios
            group.MapPost("/", async ([FromServices] AppDbContext db, [FromForm] string nome, [FromForm] IFormFile? foto) =>
            {
                string? caminhoFoto = null;

                if (foto != null)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Fotos");
                    Directory.CreateDirectory(uploadsDir);

                    string fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
                    string filePath = Path.Combine(uploadsDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await foto.CopyToAsync(stream);
                    }

                    caminhoFoto = Path.Combine("Uploads", "Fotos", fileName).Replace("\\", "/");
                }

                var funcionario = new Funcionario
                {
                    Nome = nome,
                    FotoCaminho = caminhoFoto
                };

                db.Funcionarios.Add(funcionario);
                await db.SaveChangesAsync();

                return Results.Created($"/funcionarios/{funcionario.Id}", funcionario);
            }).DisableAntiforgery();



            // 🔹 Buscar todos os Funcionários
            group.MapGet("/", async (AppDbContext db) =>
                await db.Funcionarios.ToListAsync());

            // 🔹 Buscar um Funcionário por ID
            group.MapGet("/{id:int}", async (int id, AppDbContext db) =>
            {
                var funcionario = await db.Funcionarios.FindAsync(id);
                return funcionario is not null ? Results.Ok(funcionario) : Results.NotFound();
            });

            // 🔹 Atualizar um Funcionário (Nome ou Foto)
            group.MapPut("/{id:int}", async (int id, [FromForm] string? nome, [FromForm] IFormFile? foto, AppDbContext db) =>
            {
                var funcionario = await db.Funcionarios.FindAsync(id);
                if (funcionario is null) return Results.NotFound();

                if (!string.IsNullOrWhiteSpace(nome))
                {
                    funcionario.Nome = nome;
                }

                if (foto != null)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Fotos");
                    Directory.CreateDirectory(uploadsDir);

                    string fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
                    string filePath = Path.Combine(uploadsDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await foto.CopyToAsync(stream);
                    }

                    funcionario.FotoCaminho = Path.Combine("Uploads", "Fotos", fileName).Replace("\\", "/");
                }

                await db.SaveChangesAsync();
                return Results.Ok(funcionario);
            }).DisableAntiforgery();

            // 🔹 Deletar um Funcionário (e a Foto, se existir)
            group.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
            {
                var funcionario = await db.Funcionarios.FindAsync(id);
                if (funcionario is null) return Results.NotFound();

                if (!string.IsNullOrWhiteSpace(funcionario.FotoCaminho))
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), funcionario.FotoCaminho);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                db.Funcionarios.Remove(funcionario);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }).DisableAntiforgery();
        }
    }
}
