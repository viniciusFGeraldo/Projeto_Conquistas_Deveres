using System.Text.Json.Serialization;
using Back.Endpoints;
using Back.models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlite("Data Source=banco.db"));

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


//app.UseHttpsRedirection();

// 🔹 Registrar Endpoints
app.MapFuncionarioEndpoints();
app.MapProjetoEndpoints();
app.MapEscalaEndpoints();

app.Run();
