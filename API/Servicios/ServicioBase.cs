using API.Dominio.Modelos;
using API.Dominio.Repositorios;
using API.Dominio.Repositorios.Comunicacion;
using API.Dominio.Servicios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.Servicios
{
    public class ServicioBase<TEntidad> : IServicioBase<TEntidad> where TEntidad : ModeloBase<TEntidad>
    {
        private readonly IRepositorioBase<TEntidad> _repositorio;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public Dictionary<string, string> Textos { get; set; }

        public ServicioBase(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _repositorio = unidadDeTrabajo.ObtenerRepositorio<TEntidad>();
            Textos = new Dictionary<string, string>();
        }

        public async Task<IEnumerable<TEntidad>> Listar()
        {
            return await _repositorio.ObtenerListado();
        }
        public async Task<IEnumerable<TEntidad>> Listar(int idSecuenciaJerarquias, bool? activo)
        {
            return await _repositorio.BuscarListadoPor(x =>
                        (x as ModeloBase<TEntidad>).IdSecuenciaJerarquias == idSecuenciaJerarquias &&
                        (!activo.HasValue || (x as ModeloBase<TEntidad>).Activo == activo.Value));
        }

        public async Task<RespuestaModelo<TEntidad>> Buscar(int id)
        {
            TEntidad modeloExistente = await _repositorio.BuscarEntidadPor(x => (x as ModeloBase<TEntidad>).Id == id);

            if (modeloExistente == null)
                return new RespuestaModelo<TEntidad>(Textos["ErrorNoEncontrado"]);

            return new RespuestaModelo<TEntidad>(modeloExistente);
        }
        public async Task<RespuestaModelo<TEntidad>> Guardar(TEntidad info)
        {
            try
            {
                await _repositorio.Agregar(info);
                await _unidadDeTrabajo.GuardarCambiosAsincrono();

                return new RespuestaModelo<TEntidad>(info);
            }
            catch (Exception ex)
            {
                return new RespuestaModelo<TEntidad>(Textos["ErrorAgregar"] + ex.Message);
            }
        }
        public async Task<RespuestaModelo<TEntidad>> Actualizar(int id, TEntidad info)
        {
            TEntidad infoExistente = await _repositorio.BuscarEntidadPor(x => (x as ModeloBase<TEntidad>).Id == id);
            if (infoExistente == null)
                return new RespuestaModelo<TEntidad>(Textos["ErrorNoEncontrado"]);

            (infoExistente as ModeloBase<TEntidad>).ActualizarInfo(info);
            try
            {
                await _repositorio.Actualizar(infoExistente);
                await _unidadDeTrabajo.GuardarCambiosAsincrono();
                return new RespuestaModelo<TEntidad>(info);
            }
            catch (Exception ex)
            {
                return new RespuestaModelo<TEntidad>(Textos["ErrorActualizar"] + ex.Message);
            }
        }
        public async Task<RespuestaModelo<TEntidad>> Eliminar(int id)
        {
            TEntidad infoExistente = await _repositorio.BuscarEntidadPor(x => (x as ModeloBase<TEntidad>).Id == id);
            if (infoExistente == null)
                return new RespuestaModelo<TEntidad>(Textos["ErrorNoEncontrado"]);

            try
            {
                await _repositorio.Eliminar(infoExistente);
                await _unidadDeTrabajo.GuardarCambiosAsincrono();

                return new RespuestaModelo<TEntidad>(infoExistente);
            }
            catch (Exception ex)
            {
                return new RespuestaModelo<TEntidad>(Textos["ErrorBorrar"] + ex.Message);
            }
        }
    }
}
