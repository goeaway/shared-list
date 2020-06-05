using SharedList.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedList.API.Presentation.Models
{
    public class UpdateListHubDTO
    {
        public ListDTO DTO { get; set; }
        public string UserIdent { get; set; }
    }
}
