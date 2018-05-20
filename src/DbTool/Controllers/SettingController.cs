using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DbTool.Controllers
{
    public class SettingController : BaseController
    {
        private readonly DbToolOption _option;
        private readonly Core.DbProviderFactory _dbProviderFactory;

        public SettingController(IOptionsSnapshot<DbToolOption> option, Core.DbProviderFactory dbProviderFactory)
        {
            _option = option.Value;
            _dbProviderFactory = dbProviderFactory;
        }

        [HttpGet("")]
        public JsonResult Index()
        {
            return Json(_option);
        }

        [HttpGet("[action]")]
        public JsonResult SupportedDbType()
        {
            return Json(_dbProviderFactory.AllowedDbTypes);
        }
    }
}
