namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductoVisita")]
    public partial class ProductoVisita
    {
        [Key]
        public int IdProductoVisita { get; set; }

        public int? Cantidad { get; set; }

        public double? PrecioVenta { get; set; }

        public int idVisita { get; set; }

        public int idProducto { get; set; }

        public virtual Producto Producto { get; set; }

        public virtual Visita Visita { get; set; }
    }
}
