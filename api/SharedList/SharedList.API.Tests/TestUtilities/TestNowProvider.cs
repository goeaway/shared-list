using System;
using System.Collections.Generic;
using System.Text;
using SharedList.Core.Abstractions;

namespace SharedList.API.Tests.TestUtilities
{
    public class TestNowProvider : INowProvider
    {
        private readonly DateTime? _now;
        public DateTime Now => _now ?? DateTime.Now;

        public TestNowProvider(DateTime? now)
        {
            _now = now;
        }

        public TestNowProvider()
        {

        }
    }
}
