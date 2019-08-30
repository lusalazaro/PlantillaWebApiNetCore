using API.Dominio.Repositorios;
using API.Persistencia.Contextos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Persistencia.Repositorios
{
    public class RepositorioBase<TEntidad> : IRepositorioBase<TEntidad> where TEntidad : class
    {
        private readonly AppDbContext _contexto;
        public RepositorioBase(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        public virtual async Task<bool> Agregar(TEntidad entidad)
        {
            try
            {
                _contexto.Set<TEntidad>().Add(entidad);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public virtual async Task<List<TEntidad>> ObtenerListado()
        {
            return await _contexto.Set<TEntidad>().AsNoTracking().ToListAsync();
        }
        public virtual async Task<List<TEntidad>> ObtenerListado(params Expression<Func<TEntidad, object>>[] includes)
        {
            IQueryable<TEntidad> resultado = _contexto.Set<TEntidad>().Where(i => true);

            foreach (Expression<Func<TEntidad, object>> expresionInclude in includes)
                resultado = resultado.Include(expresionInclude);

            return await resultado.AsNoTracking().ToListAsync();
        }
        public virtual async Task<List<TEntidad>> BuscarListadoPor(Expression<Func<TEntidad, bool>> buscarPor, params Expression<Func<TEntidad, object>>[] includes)
        {
            IQueryable<TEntidad> resultado = _contexto.Set<TEntidad>().Where(buscarPor);

            foreach (Expression<Func<TEntidad, object>> expresionInclude in includes)
                resultado = resultado.Include(expresionInclude);

            return await resultado.AsNoTracking().ToListAsync();
        }
        public virtual async Task<TEntidad> BuscarEntidadPor(Expression<Func<TEntidad, bool>> condicion, params Expression<Func<TEntidad, object>>[] includes)
        {
            IQueryable<TEntidad> resultado = _contexto.Set<TEntidad>().Where(condicion);

            foreach (Expression<Func<TEntidad, object>> expresionInclude in includes)
                resultado = resultado.Include(expresionInclude);

            return await resultado.AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual async Task<bool> Actualizar(TEntidad entidad)
        {
            try
            {
                _contexto.Set<TEntidad>().Attach(entidad);
                _contexto.Entry(entidad).State = EntityState.Modified;

                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public virtual async Task<bool> Eliminar(Expression<Func<TEntidad, bool>> identidad, params Expression<Func<TEntidad, object>>[] includes)
        {
            IQueryable<TEntidad> resultados = _contexto.Set<TEntidad>().Where(identidad);

            foreach (Expression<Func<TEntidad, object>> includeExpression in includes)
                resultados = resultados.Include(includeExpression);
            try
            {
                _contexto.Set<TEntidad>().RemoveRange(resultados);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }
        public virtual async Task<bool> Eliminar(TEntidad entidad)
        {
            _contexto.Set<TEntidad>().Remove(entidad);
            return await Task.FromResult(true);
        }
    }
}
