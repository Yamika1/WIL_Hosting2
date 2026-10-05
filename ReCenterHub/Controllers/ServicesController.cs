using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReCenterHub.Controllers
{

    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
