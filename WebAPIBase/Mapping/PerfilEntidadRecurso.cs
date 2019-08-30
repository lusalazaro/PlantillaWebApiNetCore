using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Mapping
{
    /// <summary>
    /// Clase encargada de crear los mapas de conversión entre clases de dominio a recurso.
    /// </summary>
    public class PerfilEntidadRecurso : Profile
    {
        /// <summary>
        /// Constructor donde se crean los mapas de las clases origen y destino, opciones personalizables, etc.
        /// </summary>
        public PerfilEntidadRecurso()
        {
            //CreateMap<ClaseOrigen, ClaseDestino>();

            //CreateMap<ClaseOrigen, ClaseDestino>()
            //    .ForMember(instanciaOrigen => instanciaOrigen.Propiedad, opciones => opciones.MapFrom(instanciaDestino => instanciaDestino.Propiedad))
            //    .AfterMap((instanciaOrigen, instanciaDestino) =>
            //    {
            //        foreach (var item in instanciaDestino.PropiedadIterar) ...
            //    });
        }
    }
}
