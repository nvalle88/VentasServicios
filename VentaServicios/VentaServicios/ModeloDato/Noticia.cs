namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Noticia")]
    public partial class Noticia
    {
        [Key]
        public int IdNoticia { get; set; }

        [StringLength(1000)]
        public string Descripcion { get; set; }

        [StringLength(250)]
        public string UrlFoto { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Fecha { get; set; }

        public int id { get; set; }

        public virtual Empresa Empresa { get; set; }
    }
}
