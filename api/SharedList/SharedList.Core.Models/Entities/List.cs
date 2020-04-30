using System;
using System.Collections.Generic;
using System.Text;

namespace SharedList.Persistence.Models.Entities
{
    public class List
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public ICollection<ListItem> Items { get; set; }
            = new List<ListItem>();
    }
}