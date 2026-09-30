using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReCenterHub.Controllers
{
    [Authorize(Roles = "Client")]
    public class SuccessTabController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
