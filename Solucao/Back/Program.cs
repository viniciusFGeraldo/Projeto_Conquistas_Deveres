using System.Text.Json.Serialization;
using Back.Endpoints;
using Back.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados (SQLite)
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=banco.db"));

// Configuração para ignorar ciclos de referência durante a serialização JSON
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => 
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Configuração de CORS (Acesso Total)
builder.Services.AddCors(options =>
    options.AddPolicy("Acesso Total",
        configs => configs
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod())
);

// Construção da aplicação
var app = builder.Build();

// Configuração para servir arquivos estáticos (como imagens na pasta 'Uploads')
app.UseStaticFiles();  // Serve arquivos na pasta wwwroot por padrão

// Se você tiver uma pasta personalizada, como 'Uploads', use o seguinte código
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/Uploads"
});

// Registrando Endpoints
app.MapGet("/", () => "Hello World!");
app.MapFuncionarioEndpoints();
app.MapProjetoEndpoints();
app.MapEscalaEndpoints();

// Ativar CORS
app.UseCors("Acesso Total");

// Executar a aplicação
app.Run();

