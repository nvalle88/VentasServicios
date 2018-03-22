namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PromocionProducto")]
    public partial class PromocionProducto
    {
        [Key]
        public int IdPromocionProducto { get; set; }

        public int? IdProducto { get; set; }

        public int? IdPromocion { get; set; }

        public virtual Producto Producto { get; set; }

        public virtual Promocion Promocion { get; set; }
    }
}
