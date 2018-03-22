namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Chat")]
    public partial class Chat
    {
        [Key]
        public int IdChat { get; set; }

        public int? Estado { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Fecha { get; set; }

        [StringLength(500)]
        public string Mensaje { get; set; }

        [Required]
        [StringLength(128)]
        public string UsuarioEnvia { get; set; }

        [Required]
        [StringLength(128)]
        public string UsuarioRecibe { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual AspNetUsers AspNetUsers1 { get; set; }
    }
}
