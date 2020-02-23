using Microsoft.Extensions.DependencyInjection;

namespace DbTool.Core
{
    public interface IServiceModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}
