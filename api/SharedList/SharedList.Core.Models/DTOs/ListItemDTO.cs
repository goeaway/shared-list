﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharedList.Core.Models.DTOs
{
    public class ListItemDTO
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Notes { get; set; }
        public bool Completed { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
