using API.Dominio.Repositorios.Comunicacion;
using API.Dominio.Servicios;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Extensiones;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controlador base genérico que contiene métodos CRUD de acuerdo a los tipos de parámetros instanceados.
    /// </summary>
    /// <typeparam name="TEntidad">Tipo relacionado con la clase dominio del Entity Framework Core</typeparam>
    /// <typeparam name="TRecurso">Tipo relacionado con la clase recurso utilizada en el Web API</typeparam>
    /// <typeparam name="TRecursoGuardar">Tipo relacionado con la clase recurso que se encarga de traducir lo que viene desde el front</typeparam>
    [ApiController]
    public class ApiControlBase<TEntidad, TRecurso, TRecursoGuardar> : Controller where TEntidad : class
    {
        /// <summary>
        /// Instancia de servicio base con métodos genéricos
        /// </summary>
        private readonly IServicioBase<TEntidad> _servicio;
        /// <summary>
        /// Instancia del package NuGet AutoMapper para realizar mapeos de instancias de clases.
        /// </summary>
        private readonly IMapper _mapeador;
        /// <summary>
        /// Constructor que se encarga de poblar las propiedades de la clase. Los parámetros vienen por inyección de dependencias
        /// declarados en el startup del proyecto.
        /// </summary>
        /// <param name="servicioBase">Instancia del servicio base proveniente desde la inyección por dependencias.</param>
        /// <param name="mapeador">Instancia del package NuGet AutoMapper proveniente desde la inyección por dependencias.</param>
        public ApiControlBase(IServicioBase<TEntidad> servicioBase, IMapper mapeador)
        {
            _servicio = servicioBase;
            _mapeador = mapeador;
        }

        /// <summary>
        /// Método Http Get genérico que obtiene un listado de la clase dominio para luego mapear y retornar el recurso de dicha clase.
        /// </summary>
        /// <returns>Retorna un listado de recurso de la entidad.</returns>
        [HttpGet]
        public virtual async Task<IEnumerable<TRecurso>> ObtenerListado()
        {
            IEnumerable<TEntidad> listado = await _servicio.Listar();
            IEnumerable<TRecurso> recursos = _mapeador.Map<IEnumerable<TEntidad>, IEnumerable<TRecurso>>(listado);
            return recursos;
        }

        /// <summary>
        /// Método Http Get que obtiene un listado filtrado por el id secuencia jerarquias.
        /// </summary>
        /// <param name="idSecuenciaJerarquias">Identificador el cual se filtrará el listado.</param>
        /// <returns>Listado de recurso de la entidad.</returns>
        [HttpGet("obtenerListadoFiltrado")]
        public virtual async Task<IEnumerable<TRecurso>> ObtenerListadoFiltrado(int idSecuenciaJerarquias)
        {
            IEnumerable<TEntidad> listado = await _servicio.Listar(idSecuenciaJerarquias, true);
            IEnumerable<TRecurso> recursos = _mapeador.Map<IEnumerable<TEntidad>, IEnumerable<TRecurso>>(listado);
            return recursos;
        }

        /// <summary>
        /// Método Http Get que obtiene el recurso de la entidad por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la entidad</param>
        /// <returns>Si es correcto, retorna el recurso de la entidad encontrada, si no, retorna BadRequest con el mensaje de error.</returns>
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> ObtenerInfoPorId(int id)
        {
            RespuestaModelo<TEntidad> resultado = await _servicio.Buscar(id);
            TRecurso modeloRecurso = _mapeador.Map<TEntidad, TRecurso>(resultado.Entidad);
            if (!resultado.Exitoso)
                return BadRequest(resultado.Mensaje);

            return Ok(modeloRecurso);
        }

        /// <summary>
        /// Método Http Post encargado de agregar una entidad a la base de datos.
        /// </summary>
        /// <param name="recurso">Instancia del recurso que guarda la entidad enviada.</param>
        /// <returns>Si es correcto, retorna el recurso creado, si no, retorna BadRequest con el mensaje de error.</returns>
        [HttpPost]
        public virtual async Task<IActionResult> Agregar([FromBody] TRecursoGuardar recurso)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ObtenerMensajesDeError());

            TEntidad info = _mapeador.Map<TRecursoGuardar, TEntidad>(recurso);
            RespuestaModelo<TEntidad> resultado = await _servicio.Guardar(info);

            if (!resultado.Exitoso)
                return BadRequest(resultado.Mensaje);

            TRecurso modeloRecurso = _mapeador.Map<TEntidad, TRecurso>(resultado.Entidad);
            return Ok(modeloRecurso);
        }

        /// <summary>
        /// Método Http Post encargado de actualizar la entidad en la base de datos.
        /// </summary>
        /// <param name="id">Identificador del recurso a actualizar.</param>
        /// <param name="recurso">Recurso que contiene la información a actualizar.</param>
        /// <returns>Si es correcto, retorna el recurso actualizado, si no, retorna BadRequest con el mensaje de error.</returns>
        [HttpPost("{id}")]
        public virtual async Task<IActionResult> Actualizar(int id, [FromBody] TRecursoGuardar recurso)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ObtenerMensajesDeError());

            TEntidad info = _mapeador.Map<TRecursoGuardar, TEntidad>(recurso);
            RespuestaModelo<TEntidad> resultado = await _servicio.Actualizar(id, info);

            if (!resultado.Exitoso)
                return BadRequest(resultado.Mensaje);

            TRecurso modeloRecurso = _mapeador.Map<TEntidad, TRecurso>(resultado.Entidad);
            return Ok(modeloRecurso);
        }
        /// <summary>
        /// Método Http Post encargado de eliminar la entidad en la base de datos.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>Si es correcto, retorna el recurso eliminado, si no, retorna BadRequest con el mensaje de error.</returns>
        [HttpPost("{id}")]
        public virtual async Task<IActionResult> Eliminar(int id)
        {
            RespuestaModelo<TEntidad> resultado = await _servicio.Eliminar(id);

            if (!resultado.Exitoso)
                return BadRequest(resultado.Mensaje);

            TRecurso modeloRecurso = _mapeador.Map<TEntidad, TRecurso>(resultado.Entidad);
            return Ok(modeloRecurso);
        }

        /// <summary>
        /// Método encargado de poblar los mensajes de error.
        /// </summary>
        /// <param name="nombre">Nombre de la entidad en minúsculas.</param>
        /// <param name="nombreEnMayus">Nombre de la entidad en mayúsculas.</param>
        public virtual void CargarTextosCRUD(string nombre, string nombreEnMayus)
        {
            _servicio.Textos = new Dictionary<string, string>
            {
                { "ErrorAgregar", $"Error al momento de agregar {nombre}: " },
                { "ErrorActualizar", $"Ha ocurrido un error mientras se actualizaba {nombre}: " },
                { "ErrorNoEncontrado", $"{nombreEnMayus} no encontrado." },
                { "ErrorBorrar", $"Ha ocurrido un error mientras se eliminaba {nombre}: " }
            };
        }
    }
}
