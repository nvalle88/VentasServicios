namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Promocion")]
    public partial class Promocion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Promocion()
        {
            PromocionProducto = new HashSet<PromocionProducto>();
        }

        [Key]
        public int IdPromocion { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FechaInicio { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? FechaFin { get; set; }

        public int? CantidadPromocion { get; set; }

        public double? Descuento { get; set; }

        public int? Tipo { get; set; }

        public int? CantidadMinima { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string Detalle { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PromocionProducto> PromocionProducto { get; set; }
    }
}
