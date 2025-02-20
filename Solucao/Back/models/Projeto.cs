using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Back.models
{
    public class Projeto
    {
         [Key]
    public int Id { get; set; }
    
        [Required]
        public required string Nome { get; set; }
        
         // Chave estrangeira para Responsável
        [Required]
        public int ResponsavelId { get; set; }
        [ForeignKey("ResponsavelId")]
        public required Funcionario Responsavel { get; set; }

        // Chave estrangeira para Sub-Responsável
        [Required]
        public int SubResponsavelId { get; set; }
        [ForeignKey("SubResponsavelId")]
        public required Funcionario SubResponsavel { get; set; }
        
        public string? Resultado { get; set; }
        public string? Entrega1 { get; set; }
        public string? Entrega2 { get; set; }
        public string? Entrega3 { get; set; }
        public decimal? Nota { get; set; }
    }
}