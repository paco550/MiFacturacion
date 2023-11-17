using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiFacturacion.Models;

namespace MiFacturacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly MiFacturacionContext _context;

        public FacturasController(MiFacturacionContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Factura>> GetFacturas()
        {
            // Busca en la base de datos todas las facturas
            var factura = await _context.Facturas.ToListAsync();

            // Devuelve la lista de clientes
            return factura;
        }

        //	Devolver la factura que corresponda a un número concreto que se pasará en la ruta
        [HttpGet("PorNumero")]
        public async Task<ActionResult<Factura>> GetFacturaPorSuNumero(int nFactura)
        {
            // Busca en la base de datos todas las facturas por su numero especifico devuelve una solo
            var factura = await _context.Facturas.FirstOrDefaultAsync(c => c.Nfactura == nFactura); 

            // Si no se encontraron facturas, devuelve un error 404 (NotFound)
            if (factura == null)
            {
                return NotFound();
            }

            // Si se encontraron facturas, los devuelve
            return Ok(factura);
        }

        // Devolver las facturas cuyo importe supere uno que se pasará en la ruta
        [HttpGet("PorImporte")]
        public async Task<ActionResult<Factura>> GetFacturaPorImporte(int importe)
        {
            // Busca en la base de datos todas las facturas cuyo importe supere el especificado
            var facturas = await _context.Facturas.Where(c => c.Importe > importe).ToListAsync();

            // Si no se encontraron facturas, devuelve un error 404 (NotFound)
            if (facturas.Count == 0)
            {
                return NotFound();
            }

            // Si se encontraron facturas, los devuelve
            return Ok(facturas);
        }
    }
}
