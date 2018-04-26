using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;
using WeihanLi.Extensions;

namespace DbTool.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        public string Test()
        {
            return "Hello Api world";
        }

        [HttpPost("[action]")]
        public int SendNotification(string title, string message)
        {
            if (HybridSupport.IsElectronActive)
            {
                // notify
                Electron.Notification.Show(new NotificationOptions(title.IsNotNullOrEmpty() ? title : "DbTool", message));
                return 1;
            }
            return 0;
        }
    }
}
