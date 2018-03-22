namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoSuscripcion")]
    public partial class TipoSuscripcion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoSuscripcion()
        {
            Suscripcion = new HashSet<Suscripcion>();
        }

        [Key]
        public int IdTipoSuscripcion { get; set; }

        public double? Valor { get; set; }

        [StringLength(1000)]
        public string Descripcion { get; set; }

        public int? CantidadDispositivos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Suscripcion> Suscripcion { get; set; }
    }
}
