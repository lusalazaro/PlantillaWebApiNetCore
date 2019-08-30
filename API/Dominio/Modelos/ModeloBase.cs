using System;
using System.Collections.Generic;
using System.Text;

namespace API.Dominio.Modelos
{
    public abstract class ModeloBase<TEntidad>
    {
        public virtual int Id { get; set; }
        public virtual bool Activo { get; set; }
        public int Orden { get; set; }
        public virtual int IdSecuenciaJerarquias { get; set; }

        public virtual void ActualizarInfo(TEntidad entidad) { }
    }
}
