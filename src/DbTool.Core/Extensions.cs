using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DbTool.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Register TDbProvider service
        /// </summary>
        /// <typeparam name="TDbProvider">DbProvider type</typeparam>
        /// <param name="serviceCollection">services</param>
        /// <returns>services</returns>
        public static IServiceCollection AddDbProvider<TDbProvider>(this IServiceCollection serviceCollection) where TDbProvider : IDbProvider
        {
            serviceCollection.TryAddEnumerable(new ServiceDescriptor(typeof(IDbProvider), typeof(TDbProvider), ServiceLifetime.Singleton));
            return serviceCollection;
        }

        /// <summary>
        /// Register DbDocImporter service
        /// </summary>
        /// <typeparam name="TDbDocImporter">DbDocImporter type</typeparam>
        /// <param name="serviceCollection">services</param>
        /// <returns>services</returns>
        public static IServiceCollection AddDbDocImporter<TDbDocImporter>(this IServiceCollection serviceCollection) where TDbDocImporter : IDbDocImporter
        {
            serviceCollection.TryAddEnumerable(new ServiceDescriptor(typeof(IDbDocImporter), typeof(TDbDocImporter), ServiceLifetime.Singleton));
            return serviceCollection;
        }

        /// <summary>
        /// Register DbDocExporter service
        /// </summary>
        /// <typeparam name="TDbDocExporter">DbDocExporter type</typeparam>
        /// <param name="serviceCollection">services</param>
        /// <returns>services</returns>
        public static IServiceCollection AddDbDocExporter<TDbDocExporter>(this IServiceCollection serviceCollection) where TDbDocExporter : IDbDocExporter
        {
            serviceCollection.TryAddEnumerable(new ServiceDescriptor(typeof(IDbDocExporter), typeof(TDbDocExporter), ServiceLifetime.Singleton));
            return serviceCollection;
        }
    }
}
