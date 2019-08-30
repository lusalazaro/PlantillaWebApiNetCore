using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Dominio.Repositorios
{
    public interface IRepositorioBase<TEntidad> where TEntidad : class
    {
        Task<bool> Agregar(TEntidad entidad);

        Task<List<TEntidad>> ObtenerListado();

        Task<List<TEntidad>> ObtenerListado(params Expression<Func<TEntidad, object>>[] includes);

        Task<List<TEntidad>> BuscarListadoPor(Expression<Func<TEntidad, bool>> buscarPor, params Expression<Func<TEntidad, object>>[] includes);

        Task<TEntidad> BuscarEntidadPor(Expression<Func<TEntidad, bool>> condicion, params Expression<Func<TEntidad, object>>[] includes);

        Task<bool> Actualizar(TEntidad entidad);

        Task<bool> Eliminar(Expression<Func<TEntidad, bool>> identidad, params Expression<Func<TEntidad, object>>[] includes);

        Task<bool> Eliminar(TEntidad entidad);

    }
}
