using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Models;
using Packing.Mvc.Models.Empresas;
using Packing.Mvc.Models.Pedidos;

namespace Packing.Mvc.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Presentacion> Presentaciones { get; set; }
        public DbSet<GrupoProducto> Grupos { get; set; }
        public DbSet<Formato> Formatos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<EstadoPedido> EstadosPedidos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }
    }
}
