using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SharedList.Persistence;

namespace SharedList.API.Tests.TestUtilities
{
    public static class Setup
    {
        public static SharedListContext CreateContext(string databaseName = null)
        {
            if (databaseName == string.Empty)
            {
                throw new ArgumentException(nameof(databaseName) + " must be null or a non empty string");
            }

            var options = new DbContextOptionsBuilder<SharedListContext>().UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString()).Options;
            return new SharedListContext(options);
        }
    }
}
