namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Agenda
    {
        [Key]
        public int idAgenda { get; set; }

        public int? Prioridad { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FechaFin { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FechaVista { get; set; }

        [StringLength(500)]
        public string Notas { get; set; }

        public int idCliente { get; set; }

        public int IdVendedor { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual Vendedor Vendedor { get; set; }
    }
}
