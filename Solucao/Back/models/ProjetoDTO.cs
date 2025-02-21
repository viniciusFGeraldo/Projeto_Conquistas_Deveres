using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.models
{
    public class ProjetoDTO
    {
        public required string Nome { get; set; }
        public required int ResponsavelId { get; set; }
        public required int SubResponsavelId { get; set; }
        public string? Resultado { get; set; }
        public string? Entrega1 { get; set; }
        public string? Entrega2 { get; set; }
        public string? Entrega3 { get; set; }
        public decimal? Nota { get; set; }
    }

}