using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Back.models
{
    public class EscalaRobo
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = "Escala do Robô";

    [Required]
    public string MesAtual { get; set; } = DateTime.Now.ToString("MM/yyyy");

    [Required]
    public int FuncionarioId { get; set; }
    [ForeignKey("FuncionarioId")]
    public Funcionario? Funcionario { get; set; }

    // Lista de datas associadas à escala do funcionário
    public List<DateTime> Datas { get; set; } = new();

    // Construtor
    public EscalaRobo() { }

    // Construtor para inicializar com o funcionário
    public EscalaRobo(Funcionario funcionario)
    {
        Funcionario = funcionario ?? throw new ArgumentNullException(nameof(funcionario));
    }
}

}