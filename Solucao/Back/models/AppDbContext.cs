using Back.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<EscalaMes> Escalas { get; set; }
    public DbSet<EscalaRobo> EscalasRobo { get; set; }  // Adicionando DbSet para EscalaRobo
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Projeto> Projetos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relacionamento: Um funcionário pode ser responsável por vários projetos
        modelBuilder.Entity<Funcionario>()
            .HasMany(f => f.ProjetosComoResponsavel)
            .WithOne(p => p.Responsavel)
            .HasForeignKey(p => p.ResponsavelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento: Um funcionário pode ser sub-responsável em vários projetos
        modelBuilder.Entity<Funcionario>()
            .HasMany(f => f.ProjetosComoSubResponsavel)
            .WithOne(p => p.SubResponsavel)
            .HasForeignKey(p => p.SubResponsavelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento entre Funcionario e EscalaRobo
        modelBuilder.Entity<Funcionario>()
            .HasMany(f => f.EscalasRobo)  // Relacionamento com EscalaRobo
            .WithOne(e => e.Funcionario)
            .HasForeignKey(e => e.FuncionarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento: Um funcionário pode ter várias escalas mensais (EscalaMes)
        modelBuilder.Entity<Funcionario>()
            .HasMany(f => f.EscalasMes)
            .WithOne()  // Não é necessário um relacionamento reverso para EscalaMes
            .OnDelete(DeleteBehavior.Cascade);

        // Definição de conversão para lista de datas
        var dateTimeConverter = new ValueConverter<List<DateTime>, string>(
            v => string.Join(",", v.Select(d => d.ToString("yyyy-MM-dd"))),  // Converte lista de DateTime para string
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)          // Converte de volta para lista de DateTime
                .Select(date => DateTime.Parse(date))
                .ToList()
        );

        // Definindo o ValueComparer para comparação de listas de datas
        var dateTimeListComparer = new ValueComparer<List<DateTime>>(
            (l1, l2) => l1.SequenceEqual(l2),  // Comparação das listas
            l => l.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Gerando o hash para comparação de listas
            l => l.ToList()  // Cria uma cópia da lista
        );

        // Aplicando tanto o conversor quanto o comparador à propriedade 'Datas' em EscalaRobo
        modelBuilder.Entity<EscalaRobo>()
            .Property(e => e.Datas)
            .HasConversion(dateTimeConverter)
            .HasComment("Lista de datas associadas à escala do robô.")
            .Metadata.SetValueComparer(dateTimeListComparer);

        // Aplicando o conversor e comparador à propriedade 'DatasDisponiveis' em EscalaMes
        modelBuilder.Entity<EscalaMes>()
            .Property(e => e.DatasDisponiveis)
            .HasConversion(dateTimeConverter)
            .HasComment("Lista de datas associadas à escala do mês.")
            .Metadata.SetValueComparer(dateTimeListComparer);

        base.OnModelCreating(modelBuilder);
    }

}
