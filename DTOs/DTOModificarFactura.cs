using MiFacturacion.Models;

namespace MiFacturacion.DTOs
{
    public class DTOModificarFactura
    {
       
        public DateTime? Fecha { get; set; } // Propiedad pública para la fecha de la factura. El signo de interrogación indica que es nullable, es decir, puede contener null.

        public decimal? Importe { get; set; } // Propiedad pública para el importe de la factura. El signo de interrogación indica que es nullable.

        public bool? Pagada { get; set; } // Propiedad pública para indicar si la factura está pagada. El signo de interrogación indica que es nullable.

        public int? ClienteId { get; set; } // Propiedad pública para el ID del cliente asociado a la factura. El signo de interrogación indica que es nullable.

    }
}
