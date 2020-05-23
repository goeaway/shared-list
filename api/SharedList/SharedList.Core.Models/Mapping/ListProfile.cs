using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using SharedList.Core.Models.DTOs;
using SharedList.Persistence.Models.Entities;

namespace SharedList.Core.Models.Mapping
{
    public class ListProfile : Profile
    {
        public ListProfile()
        {
            CreateMap<List, ListDTO>();
            CreateMap<List, ListPreviewDTO>();
        }
    }
}
