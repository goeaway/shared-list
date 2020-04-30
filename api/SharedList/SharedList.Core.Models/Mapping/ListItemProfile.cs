using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using SharedList.Core.Models.DTOs;
using SharedList.Persistence.Models.Entities;

namespace SharedList.Core.Models.Mapping
{
    public class ListItemProfile : Profile
    {
        public ListItemProfile()
        {
            CreateMap<ListItem, ListItemDTO>();
        }
    }
}
