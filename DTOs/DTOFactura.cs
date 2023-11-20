using MiFacturacion.Models;

namespace MiFacturacion.DTOs
{
    public class DTOFactura
    {
        public DateTime? Fecha { get; set; } // Fecha de la factura. Puede ser nulo.
        public decimal? Importe { get; set; } // Importe de la factura. Puede ser nulo.
        public bool? Pagada { get; set; } // Indica si la factura está pagada. Puede ser nulo.
        public int? ClienteId { get; set; } // ID del cliente asociado a la factura. Puede ser nulo.
        public int NFactura {  get; set; }
    }
}
