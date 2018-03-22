namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FormularioTipoDato")]
    public partial class FormularioTipoDato
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FormularioTipoDato()
        {
            FormularioVisita = new HashSet<FormularioVisita>();
        }

        [Key]
        [StringLength(10)]
        public string idFormularioTipoDato { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        public int? idFormulario { get; set; }

        [StringLength(10)]
        public string idTipoDato { get; set; }

        public virtual Formulario Formulario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormularioVisita> FormularioVisita { get; set; }

        public virtual TipoDato TipoDato { get; set; }
    }
}
