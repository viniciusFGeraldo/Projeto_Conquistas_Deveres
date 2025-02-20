using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Back.models
{
    public class AppDbContext : DbContext
    {
        // ✅ Construtor correto para permitir injeção de dependência
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<EscalaRobo> Escalas { get; set; }
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

            // Relacionamento: Um funcionário pode ter várias escalas na EscalaRobo
            modelBuilder.Entity<Funcionario>()
                .HasMany(f => f.Escalas)
                .WithOne(e => e.Funcionario)
                .HasForeignKey(e => e.FuncionarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração da conversão para a lista de datas
            modelBuilder.Entity<EscalaRobo>()
                .Property(e => e.Datas)
                .HasConversion(
                    v => string.Join(",", v.Select(d => d.ToString("yyyy-MM-dd"))),   // Converte a lista de datas para string
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)          // Converte de volta para a lista de DateTime
                        .Select(date => DateTime.Parse(date))
                        .ToList()
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
