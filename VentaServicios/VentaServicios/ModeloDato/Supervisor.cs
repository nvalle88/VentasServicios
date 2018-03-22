namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Supervisor")]
    public partial class Supervisor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supervisor()
        {
            Vendedor = new HashSet<Vendedor>();
        }

        [Key]
        public int IdSupervisor { get; set; }

        [StringLength(128)]
        public string IdUsuario { get; set; }

        public int IdGerente { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual Gerente Gerente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vendedor> Vendedor { get; set; }
    }
}
