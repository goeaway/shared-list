using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
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

        public static IServiceCollection AddRandomWordsProvider(this IServiceCollection collection)
        {
            var random = new Random();
            collection.AddSingleton<IRandomisedWordProvider>(new RandomisedWordProvider(random));
            return collection;
        }

        public static IServiceCollection AddFileLogger(this IServiceCollection collection)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "log-{Date}.log");
            var logger = new LoggerConfiguration()
                .WriteTo.RollingFile(path)
                .CreateLogger();
            collection.AddSingleton<ILogger>(logger);
            return collection;
        }

        public static IServiceCollection AddDbLogger(this IServiceCollection collection, string connectionString)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(connectionString);
            return collection;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddSingleton(configuration);
            return collection;
        }
    }
}
