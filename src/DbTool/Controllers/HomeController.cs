using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DbTool.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {            
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Settings()
        {
            return Json("");
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
