using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class Login : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
