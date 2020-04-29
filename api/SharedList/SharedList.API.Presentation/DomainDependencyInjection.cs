using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SharedList.Core.Abstractions;

namespace SharedList.API.Presentation
{
    public static class DomainDependencyInjection
    {
        public static IServiceCollection AddNowProvider(this IServiceCollection collection)
        {
            collection.AddSingleton<INowProvider>();
            return collection;
        }
    }
}
