namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoDato")]
    public partial class TipoDato
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoDato()
        {
            FormularioTipoDato = new HashSet<FormularioTipoDato>();
        }

        [Key]
        [StringLength(10)]
        public string idTipoDato { get; set; }

        [StringLength(10)]
        public string Nombre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormularioTipoDato> FormularioTipoDato { get; set; }
    }
}
