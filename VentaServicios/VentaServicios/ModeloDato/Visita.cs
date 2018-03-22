namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Visita")]
    public partial class Visita
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Visita()
        {
            Compromiso = new HashSet<Compromiso>();
            FormularioVisita = new HashSet<FormularioVisita>();
            ProductoVisita = new HashSet<ProductoVisita>();
        }

        [Key]
        public int idVisita { get; set; }

        public int? Calificacion { get; set; }

        [StringLength(250)]
        public string Firma { get; set; }

        public double? Venta { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Fecha { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }

        [StringLength(100)]
        public string Foto { get; set; }

        public int IdVendedor { get; set; }

        public int idTipoVisita { get; set; }

        [StringLength(10)]
        public string idCliente { get; set; }

        public virtual Cliente Cliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Compromiso> Compromiso { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormularioVisita> FormularioVisita { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductoVisita> ProductoVisita { get; set; }

        public virtual TipoVisita TipoVisita { get; set; }

        public virtual Vendedor Vendedor { get; set; }
    }
}
