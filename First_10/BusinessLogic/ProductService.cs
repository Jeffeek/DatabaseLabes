using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLabes.SharedKernel.DataAccess;
using DatabaseLabes.SharedKernel.Shared;
using First_10.DataAccess.Models;
using First_10.Queries;
using First_10.ViewModels.Models;
using Microsoft.EntityFrameworkCore;

namespace First_10.BusinessLogic
{
    public class ProductService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductService(ApplicationDbContext dbContext,
                              IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<ProductViewModel> GetAll()
        {
            var products =
                _dbContext.Set<Product>()
                          .AsNoTracking()
                          .ApplyQuery(new ProductGetAllQuery())
                          .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                          .ToList();

            return ConfigureImages(products);
        }

        private static IEnumerable<ProductViewModel> ConfigureImages(List<ProductViewModel> products)
        {
            foreach (var productViewModel in products.Where(productViewModel => productViewModel.PhotoLink != null))
                productViewModel.Photo = Convert(productViewModel.PhotoLink!);

            return products;
        }

        private static BitmapImage? Convert(string link)
        {
            if (!File.Exists(link))
                return null!;

            try
            {
                return new BitmapImage(new Uri(link, UriKind.Absolute));
            }
            catch (Exception e)
            {
                return null!;
            }
        }
    }
}