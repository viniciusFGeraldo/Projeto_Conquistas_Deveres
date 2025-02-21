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
        .Include(f => f.Escalas) // Incluir as escalas do funcionário
        .ToListAsync(); // Obter os dados do banco de dados

    // Agora, formatar as datas no lado do cliente para o formato "yyyy-MM-dd"
    var resultado = escalas.Select(f => new
    {
        Funcionario = f.Nome,
        Escalas = f.Escalas.Select(e => new
        {
            e.MesAtual,
            // Formatando as datas para o formato "yyyy-MM-dd"
            Datas = e.Datas.Select(d => d.ToString("yyyy-MM-dd")).ToList()
        }).ToList()
    }).ToList();

    return Results.Ok(resultado);
});




            // 🔹 Adicionar dias na escala de um funcionário (verificar conflito de datas)
            group.MapPost("/adicionar/{funcionarioId:int}", async (int funcionarioId, [FromBody] EscalaRequest escalaRequest, AppDbContext db) =>
{
    // Verifica se a lista de datas não está vazia
    if (escalaRequest.Datas == null || !escalaRequest.Datas.Any())
    {
        return Results.BadRequest("A lista de datas não pode ser vazia.");
    }

    // Carregar o funcionário com suas escalas
    var funcionario = await db.Funcionarios.Include(f => f.Escalas)
        .FirstOrDefaultAsync(f => f.Id == funcionarioId);
    if (funcionario is null)
    {
        return Results.NotFound(); // Retorna se o funcionário não for encontrado
    }

    // Obter todas as escalas do funcionário para o mês atual
    var mesAtual = DateTime.Now.ToString("MM/yyyy");
    var escalasExistentes = await db.Escalas
        .Where(e => e.FuncionarioId == funcionarioId && e.MesAtual == mesAtual)
        .ToListAsync();

    // Converte as datas de string para DateTime
    var datasConvertidas = new List<DateTime>();
    foreach (var dataStr in escalaRequest.Datas)
    {
        if (DateTime.TryParse(dataStr, out var data))
        {
            datasConvertidas.Add(data);
        }
        else
        {
            return Results.BadRequest($"Data inválida: {dataStr}");
        }
    }

    // Verificar se há conflito de datas
    foreach (var data in datasConvertidas)
    {
        if (escalasExistentes.Any(e => e.Datas.Contains(data)))
        {
            return Results.Conflict($"A data {data:dd/MM/yyyy} já está ocupada.");
        }
    }

    // Criar uma nova escala com as datas convertidas para DateTime
    var novaEscala = new EscalaRobo
    {
        FuncionarioId = funcionarioId,
        MesAtual = mesAtual,
        Datas = datasConvertidas // Lista de DateTime
    };

    // Adicionar a nova escala ao banco de dados
    db.Escalas.Add(novaEscala);
    await db.SaveChangesAsync();

    return Results.Ok(novaEscala);
});




           // 🔹 Remover dias da escala de um funcionário
group.MapDelete("/remover/{funcionarioId:int}", async (int funcionarioId, [FromBody] EscalaRequest escalaRequest, AppDbContext db) =>
{
    var funcionario = await db.Funcionarios.Include(f => f.Escalas).FirstOrDefaultAsync(f => f.Id == funcionarioId);
    if (funcionario == null) return Results.NotFound();

    var mesAtual = DateTime.Now.ToString("MM/yyyy");
    var escala = funcionario.Escalas.FirstOrDefault(e => e.MesAtual == mesAtual);
    if (escala == null) return Results.NotFound();

    // Garantir que as datas da requisição sejam convertidas para DateTime
    var datasParaRemover = escalaRequest.Datas?.Select(d => DateTime.Parse(d)).ToList();

    if (datasParaRemover != null)
    {
        // Verificar se as datas a serem removidas estão na lista
        var datasExistentes = escala.Datas.Where(d => datasParaRemover.Contains(d)).ToList();

        // Se não houver datas a remover, retornar
        if (!datasExistentes.Any())
        {
            return Results.NotFound("Nenhuma data correspondente encontrada.");
        }

        // Remover as datas
        escala.Datas.RemoveAll(d => datasParaRemover.Contains(d));

        // Salvar as alterações no banco de dados
        await db.SaveChangesAsync();
    }

    return Results.NoContent(); // Retorna NoContent quando a operação é bem-sucedida
});



        }
    }
}