using API.Dominio.Repositorios.Comunicacion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Dominio.Servicios
{
    public interface IServicioBase<TEntidad> where TEntidad : class
    {
        Dictionary<string, string> Textos { get; set; }

        Task<IEnumerable<TEntidad>> Listar();
        Task<IEnumerable<TEntidad>> Listar(int idSecuenciaJerarquias, bool? activo);
        Task<RespuestaModelo<TEntidad>> Buscar(int id);

        Task<RespuestaModelo<TEntidad>> Actualizar(int id, TEntidad info);

        Task<RespuestaModelo<TEntidad>> Eliminar(int id);

        Task<RespuestaModelo<TEntidad>> Guardar(TEntidad info);
    }
}
