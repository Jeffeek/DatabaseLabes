using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLabes.SharedKernel.DataAccess;
using DatabaseLabes.SharedKernel.Shared;
using First_10.BusinessLogic.Repositories;
using First_10.DataAccess.Models;
using First_10.Queries;
using First_10.ViewModels.Models;
using First_10.ViewModels.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace First_10.BusinessLogic.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public ProductService(ProductRepository productRepository,
                              IMapper mapper,
                              IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _dbContextFactory = dbContextFactory;
        }

        public ICollection<ProductViewModel> GetAll()
        {
            var products = _mapper.Map<List<ProductViewModel>>(_productRepository.GetAll());

            foreach (var productViewModel in products.Where(productViewModel => productViewModel.PhotoLink != null))
            {
                if (!File.Exists(productViewModel.PhotoLink!))
                    return null!;

                try
                {
                    productViewModel.Photo = new BitmapImage(new Uri(productViewModel.PhotoLink!, UriKind.Absolute));
                }
                catch (Exception)
                {
                    return null!;
                }
            }

            return products;
        }

        public void Add(ProductViewModel model) =>
            _productRepository.Add(_mapper.Map<Product>(model));

        public void Update(ProductViewModel model) =>
            _productRepository.Update(_mapper.Map<Product>(model));

        public void Delete(int id) => _productRepository.Remove(id);

        public bool ExistWithTitle(string title) =>
            _productRepository.GetByTitle(title) != null;

        public ICollection<ProductInParent> GetProductStockAvailabilities()
        {
            using var db = _dbContextFactory.CreateDbContext();

            return db.Set<Product>()
                     .AsNoTracking()
                     .ApplyQuery(new ProductMetadataGetAllQuery())
                     .Where(x => !x.IsDeleted)
                     .ProjectTo<ProductInParent>(_mapper.ConfigurationProvider)
                     .ToList();
        }

        public string GetWarehouseCostForEachProduct()
        {
            using var db = _dbContextFactory.CreateDbContext();

            var groupingStock = db.Set<StockAvailability>()
                                  .AsNoTracking()
                                  .Include(x => x.Product)
                                  .AsEnumerable()
                                  .GroupBy(x => x.ProductId)
                                  .ToList();

            var sb = new StringBuilder();

            foreach (var stock in groupingStock)
            {
                sb.Append($"{stock.Key}.({stock.First().Product.Title}) : {stock.Sum(x => x.Discount != null ? x.Price * (x.Discount / 100.0d) * x.WarehouseCount : x.Price * x.WarehouseCount)}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string GetProductsSellsUpperSumOn(int sum)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var sells = db.Set<Sell>()
                          .AsNoTracking()
                          .Include(x => x.Product)
                          .ThenInclude(x => x.StockAvailabilities)
                          .Where(x => x.Product.StockAvailabilities.Sum(z => z.Discount != null ? z.Price * (z.Discount / 100.0d) * z.WarehouseCount : z.Price * z.WarehouseCount) > sum)
                          .AsEnumerable()
                          .GroupBy(x => x.ProductId)
                          .ToList();

            var sb = new StringBuilder();

            foreach (var product in sells.Select(sell => sell.First()
                                                             .Product))
            {
                sb.Append($"{product.Title} : {product.Description}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string GetProductsByMaxSell()
        {
            using var db = _dbContextFactory.CreateDbContext();

            var sells = db.Set<Sell>()
                          .AsNoTracking()
                          .Include(x => x.Product)
                          .AsEnumerable()
                          .GroupBy(x => x.ProductId)
                          .ToList();

            var max = sells.Max(x => x.Sum(z => z.Count));

            var sb = new StringBuilder();

            foreach (var sell in sells.Where(x => x.Sum(z => z.Count) == max))
            {
                sb.Append($"{sell.First().Product.Title}");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}