using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaFederacion.Servicios;

namespace PruebaFederacion.Controllers
{
    //[Authorize]
    public class PanelController : Controller
    {
        private readonly IServicioUsuarios servicioUsuarios;

        public PanelController(IServicioUsuarios servicioUsuarios) {
            this.servicioUsuarios = servicioUsuarios;
        }
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
