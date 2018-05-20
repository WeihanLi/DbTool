using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DbTool.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ViewData["RequestId"] = requestId;
            return View();
        }
    }
}
