namespace VentaServicios.ModeloDato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FormularioVisita")]
    public partial class FormularioVisita
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdFormularioVisita { get; set; }

        [Required]
        [StringLength(100)]
        public string Valor { get; set; }

        [StringLength(10)]
        public string idFormularioTipoDato { get; set; }

        public int? idVisita { get; set; }

        public virtual FormularioTipoDato FormularioTipoDato { get; set; }

        public virtual Visita Visita { get; set; }
    }
}
