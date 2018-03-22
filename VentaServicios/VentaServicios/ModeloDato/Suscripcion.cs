namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Suscripcion")]
    public partial class Suscripcion
    {
        [Key]
        public int IdSuscripcion { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FechaCorte { get; set; }

        public int IdTipoSuscripcion { get; set; }

        public int? id { get; set; }

        public virtual Empresa Empresa { get; set; }

        public virtual TipoSuscripcion TipoSuscripcion { get; set; }
    }
}
