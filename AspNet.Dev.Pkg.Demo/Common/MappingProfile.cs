using AutoMapper;

namespace AspNet.Dev.Pkg.Demo
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Demo, DemoCreate>().ReverseMap();
            CreateMap<DemoAgain, DemoAgainCreate>().ReverseMap();
            CreateMap<Test, TestCreate>().ReverseMap();
            CreateMap<Test, TestReturn>().ReverseMap();
        }
    }
}
