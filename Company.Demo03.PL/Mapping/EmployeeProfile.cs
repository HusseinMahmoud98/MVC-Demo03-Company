using AutoMapper;
using Company.DAL.Models;
using Company.Demo03.PL.Dtos;

namespace Company.Demo03.PL.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            //CreateMap<DtoEmployee, Employees>().ReverseMap();
            CreateMap<DtoEmployee, Employees>()
                .ForMember(e => e.Name, o=> o.MapFrom(s => s.Name));
            CreateMap<Employees, DtoEmployee>()
                .ForMember(e => e.DepartmentName, o =>o.MapFrom(s => s.Department.Name));
           
           

        }
    }
}
