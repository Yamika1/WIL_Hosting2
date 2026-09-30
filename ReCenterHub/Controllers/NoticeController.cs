using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReCenterHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NoticeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
