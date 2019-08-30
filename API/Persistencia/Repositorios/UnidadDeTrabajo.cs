using API.Dominio.Repositorios;
using API.Persistencia.Contextos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Persistencia.Repositorios
{
    public class UnidadDeTrabajo : IUnidadDeTrabajo
    {
        private readonly AppDbContext _contexto;
        public UnidadDeTrabajo(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        public IRepositorioBase<TModelo> ObtenerRepositorio<TModelo>() where TModelo : class
        {
            return new RepositorioBase<TModelo>(_contexto);
        }

        public async Task GuardarCambiosAsincrono()
        {
            await _contexto.BulkSaveChangesAsync();
        }

        public void GuardarCambios()
        {
            _contexto.SaveChanges();
        }
    }
}
