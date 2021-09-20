using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Packing.Mvc.Controllers
{
    public class VistaProductos : Controller
    {
        [Authorize(Roles="Administrador")]
        public IActionResult EditarProducto(int idProducto)
        {
            ViewData["idProducto"] = idProducto.ToString();
            return View();
        }
    }
}
