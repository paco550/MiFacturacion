using System; // Importa el espacio de nombres System que contiene clases fundamentales y clases base que definen tipos de datos de valor y de referencia utilizados comúnmente.
using System.Collections.Generic; // Importa el espacio de nombres System.Collections.Generic que contiene interfaces y clases que definen colecciones genéricas.

namespace MiFacturacion.Models; // Define el espacio de nombres en el que se encuentra la clase Factura.

public partial class Factura // Declara una clase pública parcial llamada Factura.
{
    public int Nfactura { get; set; } // Propiedad pública para el número de factura.

    public DateTime? Fecha { get; set; } // Propiedad pública para la fecha de la factura. El signo de interrogación indica que es nullable, es decir, puede contener null.

    public decimal? Importe { get; set; } // Propiedad pública para el importe de la factura. El signo de interrogación indica que es nullable.

    public bool? Pagada { get; set; } // Propiedad pública para indicar si la factura está pagada. El signo de interrogación indica que es nullable.

    public int? ClienteId { get; set; } // Propiedad pública para el ID del cliente asociado a la factura. El signo de interrogación indica que es nullable.

    public virtual Cliente? Cliente { get; set; } // Propiedad de navegación para el cliente asociado a la factura. La palabra clave virtual permite a Entity Framework sobrescribir esta propiedad con el cliente asociado cuando se carga la factura.
}

