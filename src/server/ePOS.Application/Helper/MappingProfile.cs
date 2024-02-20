using AutoMapper;
using ePOS.Application.ViewModels;
using ePOS.Domain.CategoryAggregate;
using ePOS.Domain.ItemAggregate;
using ePOS.Domain.TenantAggregate;
using ePOS.Domain.ToppingAggregate;
using ePOS.Domain.UnitAggregate;

namespace ePOS.Application.Helper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tenant, TenantViewModel>().ReverseMap();
        CreateMap<Unit, UnitViewModel>().ReverseMap();
        CreateMap<Item, ItemViewModel>()
            .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.Name));
        CreateMap<ItemImage, ItemImageViewModel>();
        CreateMap<ItemSize, ItemSizePriceViewModel>();
        CreateMap<Topping, ToppingViewModel>().ReverseMap();
        CreateMap<Category, CategoryViewModel>().ReverseMap();
    }
}