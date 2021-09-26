using System;
using Microsoft.AspNetCore.Mvc;

namespace Packing.Mvc.Controllers
{
    public class PedidosController : Controller
    {
        public IActionResult EditarPedido(Guid guidPedido)
        {
            ViewData["PedidoId"] = guidPedido.ToString();
            return View();
        }

        public IActionResult CrearPedido()
        {
            return View();
        }
    }
}
