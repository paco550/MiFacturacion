using System;
using System.Collections.Generic;

namespace MiFacturacion.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string? Nombre { get; set; }

    public string? Ciudad { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
