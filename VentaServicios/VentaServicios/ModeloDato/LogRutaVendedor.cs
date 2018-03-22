namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogRutaVendedor")]
    public partial class LogRutaVendedor
    {
        [Key]
        public int IdLogRutaVendedor { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Fecha { get; set; }

        public int IdVendedor { get; set; }

        public virtual Vendedor Vendedor { get; set; }
    }
}
