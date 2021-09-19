using System;
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
            return View();
        }

        [Authorize]
        public IActionResult Pedidos()
        {
            var Pedidos = new List<string>();
            ViewData["Pedidos"] = Pedidos;
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
            var resultProductos = await _context.Productos.Include(prod => prod.Grupo)
                .Include(x => x.Formato).Include(x => x.Presentacion).ToListAsync();
            var resultGrupos = await _context.Grupos.ToListAsync();
            var resultFormatos = await _context.Formatos.OrderBy(x => x.UnidadPorFormato).ToListAsync();
            var resultPresentacion = await _context.Presentaciones.ToListAsync();

            var usuarioLoggeado = User.Identity.Name;

            //CarroCompra.InsertarProductoAlCarro("Producto de prueba xdxd");
            var carros = CarroCompra.ObtenerProductosDelCarro();
            ViewData["Carrito"] = carros;

            ViewData["Productos"] = resultProductos;
            ViewData["Grupos"] = resultGrupos;
            ViewData["Formatos"] = resultFormatos;
            ViewData["Presentaciones"] = resultPresentacion;
            //EmailSender.EnviarEmail("mbertolla@loscolones.cl", "Cliente de prueba solicita actualización de datos", "<p>Han solicitado un cambio de datos</p>");
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
