using System;
using System.Collections.Generic;
using System.Text;
using SharedList.Core.Abstractions;

namespace SharedList.Core.Implementations
{
    public class NowProvider : INowProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
