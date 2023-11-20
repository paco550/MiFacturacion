using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiFacturacion.DTOs;
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

        // Devolver las facturas pagadas.
        [HttpGet("facturas/pagadas")]
        public async Task<ActionResult<List<Factura>>> GetFacturasPagadas()
        {
           // Busca en la base de datos todas las facturas que están pagadas
            // e incluye los datos del cliente relacionado
            var facturasPagadas = await _context.Facturas
            .Include(f => f.Cliente)
            .Where(f => f.Pagada == true)
            .Select(f => new
            {
                Numero = f.Nfactura,
                Fecha = f.Fecha,
                Importe = f.Importe,
                Pagada = f.Pagada,
                Id = f.ClienteId,
                nombre = f.Cliente.Nombre // Incluye el nombre del cliente
            })
        .ToListAsync();

            // Devuelve la lista de facturas pagadas
            return Ok(facturasPagadas);
        }

        // Devolver las facturas de un cliente que se pasará en la ruta.
        [HttpGet("facturas/cliente/{id:int}")]
        public async Task<ActionResult<List<Factura>>> GetFacturasPorCliente(int id)
        {
            // Busca en la base de datos todas las facturas del cliente con el id proporcionado
            var facturasCliente = await _context.Facturas.Include(f => f.Cliente).Where(f => f.ClienteId == id).Select(f => new
            {
                Numero = f.Nfactura,
                Fecha = f.Fecha,
                Importe = f.Importe,
                Pagada = f.Pagada,
                Id = f.ClienteId,
                nombre = f.Cliente.Nombre // Incluye el nombre del cliente
            }).ToListAsync();

            // Devuelve la lista de facturas del cliente
            return Ok(facturasCliente);
        }

        // Agregar una factura
        [HttpPost("facturas/agregar")]
        public async Task<ActionResult> PostAgregarFactura([FromBody] DTOFactura factura)
        {
            // Crea una nueva factura con los datos proporcionados
            Factura newFactura = new Factura
            {
               
                Fecha = factura.Fecha ?? DateTime.Now, // Fecha de la factura. Si no se proporciona, se usa la fecha actual
                Importe = factura.Importe, // Importe de la factura
                Pagada = factura.Pagada, // Indica si la factura está pagada
                ClienteId = factura.ClienteId // ID del cliente asociado a la factura
            
            };

            // Agrega la nueva factura al contexto de la base de datos
            await _context.Facturas.AddAsync(newFactura);

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devuelve la nueva factura
            return Ok(newFactura);
        }


        // Modificar una factura
        [HttpPut("modificar/{id}")]
        public async Task<IActionResult> PutFactura([FromRoute] int id, DTOModificarFactura factura)
        {
            // Comprueba si el id proporcionado es igual al ClienteId de la factura
            if (id != factura.ClienteId)
            {
                return BadRequest("Los ids proporcionados son diferentes");
            }
            // Busca la factura en la base de datos usando el id proporcionado
            var facturaUpdate = await _context.Facturas.FindAsync(id);

            // Si no se encuentra la factura, devuelve un error de no encontrado
            if (facturaUpdate == null)
            {
                return NotFound();
            }
            // Actualiza los campos de la factura con los valores proporcionados
            facturaUpdate.Fecha = factura.Fecha;
            facturaUpdate.Importe = factura.Importe;
            facturaUpdate.Pagada = factura.Pagada;
            facturaUpdate.ClienteId = factura.ClienteId;
            // Actualiza la factura en la base de datos
            _context.Facturas.Update(facturaUpdate);

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();
            // Devuelve una respuesta de no contenido
            return NoContent();
        }

        // Eliminar una factura
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteFactura([FromRoute] int id)
        {
            // Busca la factura en la base de datos usando el id proporcionado
            var factura = await _context.Facturas.FindAsync(id);
            // Si no se encuentra la factura, devuelve un error de no encontrado
            if (factura == null)
            {
                return NotFound();
            }
            // Elimina la factura de la base de datos
            _context.Facturas.Remove(factura);
            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devuelve una respuesta de no contenido
            return NoContent();
        }

        // Obtener una lista de clientes
        [HttpGet("clientes")]
        public async Task<ActionResult<IEnumerable<DTOCliente>>> GetClientes()
        {
            // Devuelve una lista de clientes con sus detalles y facturas
            return await _context.Clientes.Select(c => new DTOCliente
            {
                IdCliente = c.IdCliente,
                Nombre = c.Nombre,
                TotalFacturado = c.Facturas.Sum(f => f.Importe),
                ListaFacturas = c.Facturas.Select(f => new DTOFactura
                {
                    NFactura = f.Nfactura,
                    Fecha = f.Fecha,
                    Importe = f.Importe
                }).ToList()
            }).ToListAsync();
        }

        // Comprueba si existe una factura con el id proporcionado
        private bool FacturaExists(int id)
        {
            return _context.Facturas.Any(e => e.Nfactura == id);
        }

    }
}
