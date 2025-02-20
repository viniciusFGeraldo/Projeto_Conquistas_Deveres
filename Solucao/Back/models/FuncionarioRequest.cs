using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Back.models
{
    public record FuncionarioRequest([property: FromForm] string Nome);

}