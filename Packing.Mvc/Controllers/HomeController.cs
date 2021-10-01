using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Packing.Mvc.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;
using Packing.Mvc.Models.Pedidos;
using Packing.Mvc.Servicios.Interfaces;

namespace Packing.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEnviadorCorreos EmailSender;
        private readonly ICarroCompra CarroCompra;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, 
                                SignInManager<AppUser> signInManager, ApplicationDbContext context, IEnviadorCorreos emailSender, 
                                ICarroCompra carroCompra)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            EmailSender = emailSender;
            CarroCompra = carroCompra;
        }

        
        [Authorize(Roles = "Administrador,Cliente")]
        public async Task<IActionResult> Index()
        {
            ViewData["Notificaciones"] = await _context.Notificaciones.Where(x => x.Notificado == false).ToListAsync();
            var PedidosPendientes =
                await _context.Notificaciones.Where(x => x.Criticidad == 1 && x.Notificado == false).ToListAsync();
            var SolicitudesPendientes = await _context.Notificaciones.Where(x => x.Criticidad == 2 && x.Notificado == false).ToListAsync();
            ViewData["HayPedidosPendientes"] = PedidosPendientes.Count > 0;
            ViewData["HaySolicitudesPendientes"] = SolicitudesPendientes.Count > 0;
            var listaPedidos = new List<Pedido>();

            if (await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "Cliente"))
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                var usuarioData = await _context.Usuarios.Where(x => x.Email.Equals(user.Email)).Include(x => x.Empresa).FirstOrDefaultAsync();
                if (usuarioData is null) return BadRequest("Error de usuario.");
                listaPedidos = await _context.Pedidos
                    .Where(x => x.EmpresaMandante.RutEmpresa.Equals(usuarioData.Empresa.RutEmpresa)).Include(x => x.EmpresaMandante)
                    .Include(x => x.Estado).Include(x => x.ProductosEnPedido).ToListAsync();
            }
            ViewData["Pedidos"] = listaPedidos;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Pedidos()
        {
            ViewData["Pedidos"] = await _context.Pedidos.Include(x => x.Estado).Where(x => x.Estado.IdEstadoPedido > 0)
                .Include(x => x.EmpresaMandante).Include(x => x.Estado).OrderByDescending(x => x.FechaPedido).ToListAsync();
            return View();
        }

        [Authorize(Roles="Administrador")]
        public async Task<IActionResult> ConfiguracionUsuario()
        {
            var result = await _context.Usuarios.Include(user => user.Empresa).ToListAsync();
            ViewData["Usuarios"] = result;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> VistaProductos()
        {
            var resultProductos = await _context.Productos.Where(x => x.Disponibilidad)
                .Include(prod => prod.Grupo)
                .Include(x => x.Formato).Include(x => x.Presentacion).ToListAsync();
            var resultGrupos = await _context.Grupos.ToListAsync();
            var resultFormatos = await _context.Formatos.OrderBy(x => x.UnidadPorFormato).ToListAsync();
            var resultPresentacion = await _context.Presentaciones.ToListAsync();

            ViewData["Productos"] = resultProductos;
            ViewData["Grupos"] = resultGrupos;
            ViewData["Formatos"] = resultFormatos;
            ViewData["Presentaciones"] = resultPresentacion;
            
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
