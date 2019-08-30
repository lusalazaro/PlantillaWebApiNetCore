using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Mapping
{
    /// <summary>
    /// Clase encargada de crear los mapas de conversión entre clases de recurso a dominio.
    /// </summary>
    public class PerfilRecursoEntidad : Profile
    {
        /// <summary>
        /// Constructor donde se crean los mapas de las clases origen y destino, opciones personalizables, etc.
        /// </summary>
        public PerfilRecursoEntidad()
        {
            //CreateMap<TipoSimulacion, RecursoTipoSimulacion>();

            //CreateMap<Simulacion, RecursoSimulacion>()
            //    .ForMember(recurso => recurso.IdPeriodo, opciones => opciones.MapFrom(modelo => modelo.IndicePeriodo))
            //    .AfterMap((modelo, recurso) =>
            //    {
            //        foreach (RecursoSolucionDetalle detalle in recurso.Detalles)
            //        {
            //        }
            //    });
        }
    }
}
