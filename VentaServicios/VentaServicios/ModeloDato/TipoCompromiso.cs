namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoCompromiso")]
    public partial class TipoCompromiso
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoCompromiso()
        {
            Compromiso = new HashSet<Compromiso>();
        }

        [Key]
        [StringLength(10)]
        public string IdTipoCompromiso { get; set; }

        [Required]
        [StringLength(80)]
        public string Descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Compromiso> Compromiso { get; set; }
    }
}
