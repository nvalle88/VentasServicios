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

        public int UsuarioEnvia { get; set; }

        public int UsuarioRecibe { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual Usuario Usuario1 { get; set; }
    }
}
