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
                                  .Where(x => !x.IsDeleted)
                                  .Include(x => x.Product)
                                  .Where(x => !x.Product.IsDeleted)
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
                          .Where(x => !x.Product.IsDeleted)
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
                          .Where(x => !x.Product.IsDeleted)
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

        public string GetProductsByWarehouseCount(int warehouseCountBound)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var sells = db.Set<StockAvailability>()
                          .AsNoTracking()
                          .Where(x => !x.IsDeleted)
                          .Include(x => x.Product)
                          .Where(x => x.Price < 10000 && x.WarehouseCount > warehouseCountBound)
                          .AsEnumerable()
                          .GroupBy(x => x.ProductId)
                          .ToList();

            var sb = new StringBuilder();

            foreach (var product in sells.Select(sell => sell.First()
                                                             .Product))
            {
                sb.Append($"{product.Title} : {product.Producer}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string GetProducersBySellCount(int priceBound)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var producersCount = db.Set<StockAvailability>()
                                   .AsNoTracking()
                                   .Where(x => !x.IsDeleted)
                                   .Include(x => x.Product)
                                   .AsEnumerable()
                                   .GroupBy(x => x.Product.Producer)
                                   .ToList();

            var sb = new StringBuilder();

            foreach (var availabilities in producersCount.Where(x => x.Sum(z => z.Price) > priceBound))
            {
                sb.Append($"{availabilities.Key}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public long GetOverallWarehouseCount()
        {
            using var db = _dbContextFactory.CreateDbContext();

            var overallCount = db.Set<StockAvailability>()
                                 .AsNoTracking()
                                 .Where(x => !x.IsDeleted)
                                 .Sum(x => (long)x.WarehouseCount);

            return overallCount;
        }

        public string GetOverallWarehouseCountBySize()
        {
            using var db = _dbContextFactory.CreateDbContext();

            var sells = db.Set<Sell>()
                          .AsNoTracking()
                          .Include(x => x.Product)
                          .ThenInclude(x => x.StockAvailabilities.Where(z => !z.IsDeleted))
                          .Where(x => !x.Product.IsDeleted)
                          .AsEnumerable()
                          .GroupBy(x => x.Size)
                          .ToList();

            var sb = new StringBuilder();

            foreach (var sell in sells)
            {
                sb.Append($"{sell.Key} : {sell.Select(x => x.Product).Sum(x => x.StockAvailabilities.Sum(z => z.WarehouseCount))}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string GetSellsCountByDate(int sellsCountBound)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var sellsByDate = db.Set<Sell>()
                                .AsNoTracking()
                                .GroupBy(x => x.SellDate)
                                .Select(x => new
                                             {
                                                 Date = x.Key,
                                                 Count = x.Count()
                                             })
                                .Where(x => x.Count < sellsCountBound)
                                .ToList();

            var resultList = new List<(DateTime, int)>();

            foreach (var sell in sellsByDate.Where(sell => resultList.Count(x => x.Item1.Year == sell.Date.Year
                                                                                 && x.Item1.Month == sell.Date.Month
                                                                                 && x.Item1.Day == sell.Date.Day)
                                                           == 0))
                resultList.Add((sell.Date, sell.Count));

            var sb = new StringBuilder();

            foreach (var tuple in resultList)
            {
                sb.Append($"{tuple.Item1:yyyy-MM-dd} : {tuple.Item2}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string GetProductWithoutSells()
        {
            using var db = _dbContextFactory.CreateDbContext();

            var productsWithoutSells = db.Set<Product>()
                                         .AsNoTracking()
                                         .Where(x => !x.IsDeleted)
                                         .Include(x => x.Sells)
                                         .Where(x => !x.Sells!.Any())
                                         .Select(x => x.Title)
                                         .ToList();

            return String.Join(Environment.NewLine, productsWithoutSells);
        }

        public string GetProductsInformationByCategoryName(string category)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var productsByCategory = db.Set<Product>()
                                       .AsNoTracking()
                                       .Include(x => x.StockAvailabilities)
                                       .Where(x => !x.IsDeleted && x.Category == category)
                                       .AsEnumerable()
                                       .Select(x => new
                                                    {
                                                        ProductName = x.Title,
                                                        Sum = x.StockAvailabilities.Sum(c => c.Discount != null ? c.Price * (c.Discount / 100.0d) * c.WarehouseCount : c.Price * c.WarehouseCount)
                                                    })
                                       .ToList();

            var sb = new StringBuilder();

            foreach (var categoryGroup in productsByCategory)
            {
                sb.Append($"{categoryGroup.ProductName} : {categoryGroup.Sum}");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}