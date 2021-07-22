using AutoMapper;
using ProductManagement;
using StudentManagement.Entities;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Register, User>();
            CreateMap<Login, User>();
            CreateMap<ClassModel, Class>();
            CreateMap<StudentModel, Student>();
        }
    }
}
