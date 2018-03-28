namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoVisita")]
    public partial class TipoVisita
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoVisita()
        {
            Visita = new HashSet<Visita>();
        }

        [Key]
        public int idTipoVisita { get; set; }

        [StringLength(500)]
        public string Icono { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string Detalle { get; set; }

        public int? idEmpresa { get; set; }

        public virtual Empresa Empresa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visita> Visita { get; set; }
    }
}
