using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Back.models
{
    public class Funcionario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Nome { get; set; }

        public string? FotoCaminho { get; set; } // Armazena apenas o caminho do arquivo

        public List<Projeto> ProjetosComoResponsavel { get; set; } = new();
        public List<Projeto> ProjetosComoSubResponsavel { get; set; } = new();
        public List<EscalaRobo> Escalas { get; set; } = new();
    }
}