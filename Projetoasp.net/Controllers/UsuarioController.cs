using Microsoft.AspNetCore.Mvc;

namespace Projetoasp.net.Controllers
{
    public class Usuario : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
