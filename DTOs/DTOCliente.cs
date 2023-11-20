namespace MiFacturacion.DTOs
{
    public class DTOCliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set;}
        public string Ciudad { get; set;}
        public decimal? TotalFacturado { get; internal set; }
        public List<DTOFactura> ListaFacturas { get; internal set; }
    }
}
