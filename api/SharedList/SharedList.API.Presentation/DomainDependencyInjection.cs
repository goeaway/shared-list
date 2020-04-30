using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SharedList.Core.Abstractions;
using SharedList.Core.Implementations;

namespace SharedList.API.Presentation
{
    public static class DomainDependencyInjection
    {
        public static IServiceCollection AddNowProvider(this IServiceCollection collection)
        {
            collection.AddSingleton<INowProvider, NowProvider>();
            return collection;
        }

        public static IServiceCollection AddLogger(this IServiceCollection collection)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "log-{Date}.log");
            var logger = new LoggerConfiguration()
                .WriteTo.RollingFile(path)
                .CreateLogger();
            collection.AddSingleton<ILogger>(logger);
            return collection;
        }
    }
}
