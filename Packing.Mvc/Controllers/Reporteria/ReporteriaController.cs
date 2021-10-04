using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;

namespace Packing.Mvc.Controllers.Reporteria
{
    public class ReporteriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReporteriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerCantidadACosechar()
        {
            //obtener las cabeceras que no estén procesadas
            //GRUPOS PARA SABER QUE SE VA A COSECHAR
            //POR CADA PEDIDO OBTENER LOS GRUPOS Y SUS CANTIDADES TOTALES
            //AGREGAR A UN HASHTABLE CON CLAVE STRING,INT - NOMBRE,CANTIDAD E IR ACTUALIZANDOS SEGÚN TENGA
            var diccionarioGrupos = new Dictionary<string,uint>();
            var diccionarioJefePacking = new Dictionary<string, uint>();
            var datosDelPedido = await _context.Pedidos.Include(pedido => pedido.Estado)
                .Where(pedido => pedido.Estado.IdEstadoPedido > 1 && pedido.Estado.IdEstadoPedido < 5)
                .Include(pedido => pedido.ProductosEnPedido).ThenInclude(detalles => detalles.ProductoInterno)
                .ToListAsync();
            foreach (var pedido in datosDelPedido)
            {
                foreach (var detalle in pedido.ProductosEnPedido)
                {
                    detalle.ProductoInterno = await _context.Productos
                        .Where(x => x.IdProducto == detalle.ProductoInterno.IdProducto)
                        .Include(x => x.Formato).Include(x => x.Presentacion)
                        .Include(x => x.Grupo).FirstAsync();
                    //agregamos la cantidad del grupo
                    if (diccionarioGrupos.ContainsKey(detalle.ProductoInterno.Grupo.NombreGrupo))
                    {
                        var cantidadAnterior = diccionarioGrupos[detalle.ProductoInterno.Grupo.NombreGrupo];
                        var nuevaCantidad = cantidadAnterior + detalle.CantidadTotales;
                        diccionarioGrupos[detalle.ProductoInterno.Grupo.NombreGrupo] = nuevaCantidad;
                    }
                    else
                    {
                        diccionarioGrupos.Add(detalle.ProductoInterno.Grupo.NombreGrupo, detalle.CantidadTotales);
                    }
                    if(diccionarioJefePacking.ContainsKey(detalle.ProductoInterno.NombreParaBusqueda))
                    {
                        var cantidadAnterior = diccionarioJefePacking[detalle.ProductoInterno.NombreParaBusqueda];
                        var nuevaCantidad = cantidadAnterior + detalle.Cantidad;
                        diccionarioJefePacking[detalle.ProductoInterno.NombreParaBusqueda] = nuevaCantidad;
                    }
                    else
                    {
                        diccionarioJefePacking.Add(detalle.ProductoInterno.NombreParaBusqueda,detalle.Cantidad);
                    }
                }
            }

            foreach (var diccionarioGrupo in diccionarioGrupos)
            {
                Console.WriteLine(diccionarioGrupo.Key + " - " + diccionarioGrupo.Value);
            }
            Console.WriteLine("--------------------------------------------------");
            foreach (var diccionarioGrupo in diccionarioJefePacking)
            {
                Console.WriteLine(diccionarioGrupo.Key + " - " + diccionarioGrupo.Value);
            }
            //obtener todos los productos 
            return Ok();
        }
    }
}
