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
using Serilog.Sinks.AwsCloudWatch;
using Amazon.CloudWatchLogs;
using Amazon;

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

        public static IServiceCollection AddLogger(this IServiceCollection collection, IConfiguration configuration)
        {
            var logConfig = new LoggerConfiguration();
            var sink = configuration["Logging:Sink"];

            switch(sink)
            {
                case "AWSCloudWatch":
                    var accessKeyId = configuration["AWS:AccessKeyId"];
                    var accessSecretAccessKey = configuration["AWS:AccessSecretAccessKey"];

                    var client = new AmazonCloudWatchLogsClient(accessKeyId, accessSecretAccessKey, RegionEndpoint.EUWest2);
                    var options = new CloudWatchSinkOptions
                    {
                        LogGroupName = "shared-list-beanstalk-log",
                        Period = TimeSpan.FromSeconds(10),
                        BatchSizeLimit = 100,
                        QueueSizeLimit = 10000,
                        LogStreamNameProvider = new DefaultLogStreamProvider(),
                        RetryAttempts = 5
                    };

                    logConfig.WriteTo.AmazonCloudWatch(options, client);
                    break;
                default:
                    logConfig.WriteTo.RollingFile(Path.Combine(AppContext.BaseDirectory, "log-{Date}.log"));
                    break;
            }

            collection.AddSingleton<ILogger>(logConfig.CreateLogger());
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
