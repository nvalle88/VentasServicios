namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cliente")]
    public partial class Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cliente()
        {
            Agenda = new HashSet<Agenda>();
            Visita = new HashSet<Visita>();
        }

        [Key]
        public int idCliente { get; set; }

        public string Foto { get; set; }

        public string Firma { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }
        

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public int idTipoCliente { get; set; }

        public int IdVendedor { get; set; }

        public string TelefonoMovil { get; set; }

        public string Identificacion { get; set; }

        public string Direccion { get; set; }

        public int Estado { get; set; }

        public string RazonSocial { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agenda> Agenda { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visita> Visita { get; set; }

        public virtual TipoCliente TipoCliente { get; set; }

        public virtual Vendedor Vendedor { get; set; }
    }
}
