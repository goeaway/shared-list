using System;
using System.Collections.Generic;
using System.Text;

namespace SharedList.Core.Abstractions
{
    public interface INowProvider
    {
        DateTime Now { get; }
    }
}
