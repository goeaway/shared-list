using System;
using System.Collections.Generic;
using System.Text;

namespace SharedList.Core.Models.DTOs
{
    public class ListPreviewDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> OtherContributors{ get; set; }
            = new List<string>();
    }
}
