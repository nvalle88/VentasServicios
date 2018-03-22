namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Compromiso")]
    public partial class Compromiso
    {
        [Key]
        [StringLength(10)]
        public string IdCompromiso { get; set; }

        [StringLength(10)]
        public string IdTipoCompromiso { get; set; }

        public int? idVisita { get; set; }

        [Required]
        [StringLength(80)]
        public string Descripcion { get; set; }

        [StringLength(80)]
        public string Solucion { get; set; }

        public virtual TipoCompromiso TipoCompromiso { get; set; }

        public virtual Visita Visita { get; set; }
    }
}
