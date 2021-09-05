using AutoMapper;
using First_10.DataAccess.Models;
using First_10.ViewModels.Models;
using First_10.ViewModels.Models.Common;

namespace First_10.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Sell, Sell>();
            CreateMap<Product, Product>();
            CreateMap<StockAvailability, StockAvailability>();

            CreateMap<ProductViewModel, ProductViewModel>();
            CreateMap<SellViewModel, SellViewModel>();
            CreateMap<StockAvailabilityViewModel, StockAvailabilityViewModel>();

            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.PhotoLink, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.Photo, opt => opt.Ignore());

            CreateMap<Product, ProductInParent>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<Sell, SellViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(_ => default(Product)))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(a => a.Product.ProductId));

            CreateMap<StockAvailability, StockAvailabilityViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(_ => default(Product)))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(a => a.Product.ProductId));

            CreateMap<ProductViewModel, Product>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Producer, opt => opt.MapFrom(src => src.Producer))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => src.Availability))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.PhotoLink))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
