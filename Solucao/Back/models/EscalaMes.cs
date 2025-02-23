using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back.models
{
    public class EscalaMes
    {
        [Key]
        public int Id { get; set; }

        // Mês da escala no formato MM/yyyy
        [Required]
        public string Mes { get; set; } = DateTime.Now.ToString("MM/yyyy");

        // Lista de datas disponíveis para o mês
        [Required]
        public List<DateTime> DatasDisponiveis { get; set; } = new();
    }

    public class EscalaRobo
    {
        [Key]
        public int Id { get; set; }

        // Título da escala (sempre "Escala do Robô")
        [Required]
        public string Title { get; set; } = "Escala do Robô";

        // Mês atual da escala no formato MM/yyyy
        [Required]
        public string MesAtual { get; set; } = DateTime.Now.ToString("MM/yyyy");

        // ID do funcionário associado à escala
        [Required]
        public int FuncionarioId { get; set; }

        // Relacionamento com a entidade Funcionario
        [ForeignKey("FuncionarioId")]
        public Funcionario? Funcionario { get; set; }

        public List<DateTime> Datas { get; set; } // Alterado de ICollection<DateTime> para List<DateTime>
    }

    
}
