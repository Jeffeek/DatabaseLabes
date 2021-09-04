using System.Windows.Media.Imaging;
using AutoMapper;
using First_10.DataAccess.Models;
using First_10.ViewModels.Models;

namespace First_10.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.PhotoLink, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.Photo, opt => opt.Ignore());

            CreateMap<Sell, SellViewModel>();
        }
    }
}
