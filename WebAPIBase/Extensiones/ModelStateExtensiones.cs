using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensiones
{
    public static class ModelStateExtensiones
    {
        public static List<string> ObtenerMensajesDeError(this ModelStateDictionary dicionario)
        {
            return dicionario.SelectMany(m => m.Value.Errors)
                             .Select(m => m.ErrorMessage)
                             .ToList();
        }
    }
}
