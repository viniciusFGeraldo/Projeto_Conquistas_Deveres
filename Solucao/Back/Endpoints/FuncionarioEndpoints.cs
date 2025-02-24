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

// Criar funcionários
group.MapPost("/", async ([FromServices] AppDbContext db, [FromForm] string nome, [FromForm] IFormFile? foto) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            return Results.BadRequest("O nome do funcionário é obrigatório.");
        }

        string? caminhoFoto = null;

        if (foto != null)
        {
            // Validar tipo de arquivo (somente imagens)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(foto.FileName).ToLower();

            Console.WriteLine($"Arquivo recebido: {foto.FileName}, Extensão: {fileExtension}");

            if (!allowedExtensions.Contains(fileExtension))
            {
                return Results.BadRequest("Arquivo inválido. Por favor, envie uma imagem.");
            }

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Fotos");
            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
                Console.WriteLine($"Diretório criado: {uploadsDir}");
            }

            // Nome do arquivo com timestamp e GUID para garantir unicidade
            string fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid()}{fileExtension}";
            string filePath = Path.Combine(uploadsDir, fileName);

            // Salvar o arquivo no diretório
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await foto.CopyToAsync(stream);
            }

            // Caminho relativo para o arquivo
            caminhoFoto = Path.Combine("Uploads", "Fotos", fileName).Replace("\\", "/");
            Console.WriteLine($"Foto salva em: {caminhoFoto}");
        }

        var funcionario = new Funcionario
        {
            Nome = nome,
            FotoCaminho = caminhoFoto
        };

        db.Funcionarios.Add(funcionario);
        await db.SaveChangesAsync();

        Console.WriteLine($"Funcionário criado: {funcionario.Nome}, ID: {funcionario.Id}");

        return Results.Created($"/funcionarios/{funcionario.Id}", funcionario);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao criar funcionário: {ex.Message}");
        return Results.Problem("Erro interno ao processar a requisição.");
    }
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
