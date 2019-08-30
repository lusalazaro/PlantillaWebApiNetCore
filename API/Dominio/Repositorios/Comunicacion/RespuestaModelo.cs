using System;
using System.Collections.Generic;
using System.Text;

namespace API.Dominio.Repositorios.Comunicacion
{
    public class RespuestaModelo<TEntidad>
    {
        public bool Exitoso { get; protected set; }
        public string Mensaje { get; protected set; }
        public TEntidad Entidad { get; set; }

        public RespuestaModelo(TEntidad objeto)
        {
            Exitoso = true;
            Mensaje = string.Empty;
            Entidad = objeto;
        }

        public RespuestaModelo(string mensaje)
        {
            Exitoso = false;
            Mensaje = mensaje;
            Entidad = default;
        }
    }
}
