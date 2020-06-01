using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SharedList.API.Presentation
{
    public static class AWSConfigurationHelper
    {
        const string AWS_BEANSTALK_CONFIG_PATH = @"C:\Program Files\Amazon\ElasticBeanstalk\config\containerconfiguration";
        const string ENV_KEY = "ASPNETCORE_ENVIORNMENT";

        public static IConfiguration CreateConfiguration(IConfiguration baseConfiguration)
        {
            if(!File.Exists(AWS_BEANSTALK_CONFIG_PATH))
            {
                return baseConfiguration;
            }

            throw new Exception("LOADING EB CONFIG");
            var tempConfigBuilder = new ConfigurationBuilder()
                    .AddJsonFile(
                        AWS_BEANSTALK_CONFIG_PATH,
                        optional: true,
                        reloadOnChange: true);

            var ebItems = ParseEBConfig(tempConfigBuilder.Build());
            var configbuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (ebItems.ContainsKey(ENV_KEY))
            {
                configbuilder.AddJsonFile($"appsettings.{ebItems[ENV_KEY]}.json");
            }

            return 
                configbuilder
                .AddInMemoryCollection(ebItems)
                .Build();
        }

        private static IDictionary<string, string> ParseEBConfig(IConfiguration configuration)
        {
            return
                configuration.GetSection("iis:env")
                    .GetChildren()
                    .Select(pair => pair.Value.Split(new[] { '=' }, 2))
                    .ToDictionary(keypair => keypair[0], keypair => keypair[1]);
        }
    }
}
