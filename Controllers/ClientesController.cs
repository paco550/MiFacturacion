using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiFacturacion.DTOs;
using MiFacturacion.Models;
using System.Runtime.Intrinsics.X86;

namespace MiFacturacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly MiFacturacionContext _context;


        public ClientesController(MiFacturacionContext context)
        {
            _context = context;
        }


        // Devolver todos los clientes.
        [HttpGet]
        public async Task<List<Cliente>> GetClientes()
        {
            // Busca en la base de datos todos los clientes
            var cliente = await _context.Clientes.ToListAsync();

            // Devuelve la lista de clientes
            return cliente;
        }

        // Devolver todos los clientes de una ciudad que se pasará en la ruta.
        [HttpGet("ciudad")]
        public async Task<ActionResult<Cliente>> GetClientesPorCiudad(string ciudad)
        {
            // Busca en la base de datos todos los clientes de la ciudad especificada
            var clientes = await _context.Clientes.Where(c => c.Ciudad.ToLower() == ciudad.ToLower()).ToListAsync();

            // Si no se encontraron clientes, devuelve un error 404 (NotFound)
            if (clientes == null)
            {
                return NotFound();
            }

            // Si se encontraron clientes, los devuelve
            return Ok(clientes);
        }

        // Agregar un cliente.
        [HttpPost("agregar")]
        public async Task<ActionResult> PosstAgregarCliente([FromForm] DTOCliente cliente)
        {
            // Crea un nuevo cliente con los datos proporcionados
            Cliente newCliente = new Cliente
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Ciudad = cliente.Ciudad,
            };

            // Agrega el nuevo cliente al contexto de la base de datos
            await _context.AddAsync(newCliente);

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devuelve el nuevo cliente
            return Ok(newCliente);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutCliente([FromRoute] int id, DTOCliente cliente)
        {
            // Comprueba si el id proporcionado en la ruta coincide con el IdCliente del objeto cliente
            if (id != cliente.IdCliente)
            {
                // Si no coinciden, devuelve un error 400 (BadRequest)
                return BadRequest("Los ids proporcionados son diferentes");
            }

            // Busca en la base de datos un cliente con el IdCliente proporcionado
            var clienteUpdate = await _context.Clientes.AsTracking().FirstOrDefaultAsync(x => x.IdCliente == id);

            // Si no se encontró ningún cliente, devuelve un error 404 (NotFound)
            if (clienteUpdate == null)
            {
                return NotFound();
            }

            // Actualiza el nombre y la ciudad del cliente
            clienteUpdate.Nombre = cliente.Nombre;
            clienteUpdate.Ciudad = cliente.Ciudad;

            // Actualiza el cliente en el contexto de la base de datos
            _context.Update(clienteUpdate);

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devuelve un código de estado 204 (NoContent) para indicar que la operación fue exitosa
            return NoContent();
        }

        // Eliminar un cliente. Verificar antes que no haya facturas de ese cliente
        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            // Busca en la base de datos un cliente con el id proporcionado
            var cliente = await _context.Clientes.FindAsync(id);

            // Si no se encontró ningún cliente, devuelve un error 404 (NotFound)
            if (cliente == null)
            {
                return NotFound();
            }

            // Comprueba si hay facturas asociadas al cliente
            var facturasCliente = await _context.Facturas.AnyAsync(f => f.ClienteId == id);

            // Si hay facturas, devuelve un error 400 (BadRequest)
            if (facturasCliente)
            {
                return BadRequest("No se puede eliminar el cliente porque tiene facturas asociadas");
            }

            // Elimina el cliente del contexto de la base de datos
            _context.Clientes.Remove(cliente);

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devuelve un código de estado 204 (NoContent) para indicar que la operación fue exitosa
            return NoContent();
        }


    }

}

