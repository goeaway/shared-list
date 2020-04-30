using System;
using System.Collections.Generic;
using System.Text;

namespace SharedList.Core.Models.DTOs
{
    public class ListDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public ICollection<ListItemDTO> Items { get; set; }
            = new List<ListItemDTO>();
    }
}
