using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Dominio.Repositorios
{
    public interface IUnidadDeTrabajo
    {
        IRepositorioBase<TEntity> ObtenerRepositorio<TEntity>() where TEntity : class;

        Task GuardarCambiosAsincrono();
        void GuardarCambios();
    }
}
