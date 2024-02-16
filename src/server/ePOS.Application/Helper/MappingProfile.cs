using AutoMapper;
using ePOS.Application.ViewModels;
using ePOS.Domain.TenantAggregate;
using ePOS.Domain.UnitAggregate;

namespace ePOS.Application.Helper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tenant, TenantViewModel>().ReverseMap();
        CreateMap<Unit, UnitViewModel>().ReverseMap();
    }
}