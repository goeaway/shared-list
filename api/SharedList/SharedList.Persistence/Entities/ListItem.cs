﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharedList.Persistence.Entities
{
    public class ListItem
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Notes { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public List ParentList { get; set; }
    }
}
