using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Input;
using AutoMapper;
using DatabaseLabes.SharedKernel.DataAccess;
using First_10.BusinessLogic;
using First_10.BusinessLogic.Services;
using First_10.DataAccess.Models;
using First_10.ViewModels.Models;
using First_10.Views;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace First_10.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly ProductService _productService;
        private readonly IDialogService _dialogService;
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly IMapper _mapper;

        private ObservableCollection<ProductViewModel> _products;
        private ProductViewModel _selectedProduct;
        private SellViewModel _selectedSell;
        private StockAvailabilityViewModel _selectedStockAvailability;

        private ICommand? _updateProductsCommand;
        private ICommand? _callProductWindowCommand;
        private ICommand? _callEmptyProductWindowCommand;
        private ICommand? _callSellWindowCommand;
        private ICommand? _callEmptySellWindowCommand;
        private ICommand? _callStockAvailabilityWindowCommand;
        private ICommand? _callEmptyStockAvailabilityWindowCommand;
        private ICommand? _calculateCostForEachCommand;
        private ICommand? _showProductsBySumCommand;
        private ICommand? _showProductsByMaxSellCommand;
        private ICommand? _showProductsByWarehouseCountCommand;
        private ICommand? _showProducersBySellCountCommand;
        private ICommand? _showOverallWarehouseCountCommand;
        private ICommand? _showOverallWarehouseCountBySizeCommand;
        private ICommand? _showSellsCountByDateCommand;
        private ICommand? _showProductsWithoutSellsCommand;
        private ICommand? _showProductsByCategoryCommand;
        private ICommand? _createReportCommand;

        public MainWindowViewModel(ProductService productService,
                                   IDialogService dialogService,
                                   IDbContextFactory<ApplicationDbContext> dbContextFactory,
                                   IMapper mapper)
        {
            _productService = productService;
            _dialogService = dialogService;
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public ICommand ShowProducersBySellCountCommand =>
            _showProducersBySellCountCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var vm = new InputDialogWindowViewModel<int?>();

                                            var window = new InputDialogWindow
                                                         {
                                                             DataContext = vm
                                                         };

                                            window.ShowDialog();

                                            if (vm.Value.HasValue)
                                                _dialogService.ShowDialog("CustomDialog",
                                                                          new DialogParameters
                                                                          {
                                                                              {
                                                                                  "Message", _productService.GetProducersByAvgSellCount(vm.Value.Value)
                                                                              }
                                                                          },
                                                                          _ => { });
                                        });

        public ICommand CalculateCostForEachCommand =>
            _calculateCostForEachCommand
                ??= new DelegateCommand(() => _dialogService.ShowDialog("CustomDialog",
                                                                        new DialogParameters
                                                                        {
                                                                            {
                                                                                "Message", _productService.GetWarehouseCostForEachProduct()
                                                                            }
                                                                        },
                                                                        _ => { }));

        public ICommand ShowProductsBySumCommand =>
            _showProductsBySumCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var vm = new InputDialogWindowViewModel<int?>();

                                            var window = new InputDialogWindow
                                                         {
                                                             DataContext = vm
                                                         };

                                            window.ShowDialog();

                                            if (vm.Value is >= 0)
                                                _dialogService.ShowDialog("CustomDialog",
                                                                          new DialogParameters
                                                                          {
                                                                              {
                                                                                  "Message", _productService.GetProductsSellsUpperSumOn(vm.Value.Value)
                                                                              }
                                                                          },
                                                                          _ => { });
                                        });

        public ICommand ShowProductsByWarehouseCountCommand =>
            _showProductsByWarehouseCountCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var vm = new InputDialogWindowViewModel<int?>();

                                            var window = new InputDialogWindow
                                                         {
                                                             DataContext = vm
                                                         };

                                            window.ShowDialog();

                                            if (vm.Value is >= 0)
                                                _dialogService.ShowDialog("CustomDialog",
                                                                          new DialogParameters
                                                                          {
                                                                              {
                                                                                  "Message", _productService.GetProductsByWarehouseCount(vm.Value.Value)
                                                                              }
                                                                          },
                                                                          _ => { });
                                        });

        public ICommand ShowProductsByCategoryCommand =>
            _showProductsByCategoryCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var vm = new InputDialogWindowViewModel<string?>();

                                            var window = new InputDialogWindow
                                                         {
                                                             DataContext = vm
                                                         };

                                            window.ShowDialog();

                                            if (!String.IsNullOrEmpty(vm.Value))
                                                _dialogService.ShowDialog("CustomDialog",
                                                                          new DialogParameters
                                                                          {
                                                                              {
                                                                                  "Message", _productService.GetProductsInformationByCategoryName(vm.Value)
                                                                              }
                                                                          },
                                                                          _ => { });
                                        });

        public ICommand ShowSellsCountByDateCommand =>
            _showSellsCountByDateCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var vm = new InputDialogWindowViewModel<int?>();

                                            var window = new InputDialogWindow
                                                         {
                                                             DataContext = vm
                                                         };

                                            window.ShowDialog();

                                            if (vm.Value is >= 0)
                                                _dialogService.ShowDialog("CustomDialog",
                                                                          new DialogParameters
                                                                          {
                                                                              {
                                                                                  "Message", _productService.GetSellsCountByDate(vm.Value.Value)
                                                                              }
                                                                          },
                                                                          _ => { });
                                        });

        public ICommand ShowProductsByMaxSellCommand =>
            _showProductsByMaxSellCommand
                ??= new DelegateCommand(() => _dialogService.ShowDialog("CustomDialog",
                                                                        new DialogParameters
                                                                        {
                                                                            {
                                                                                "Message", _productService.GetProductsByMaxSell()
                                                                            }
                                                                        },
                                                                        _ => { }));

        public ICommand ShowOverallWarehouseCountCommand =>
            _showOverallWarehouseCountCommand
                ??= new DelegateCommand(() => _dialogService.ShowDialog("CustomDialog",
                                                                        new DialogParameters
                                                                        {
                                                                            {
                                                                                "Message", _productService.GetOverallWarehouseCount()
                                                                                    .ToString()
                                                                            }
                                                                        },
                                                                        _ => { }));

        public ICommand ShowProductsWithoutSellsCommand =>
            _showProductsWithoutSellsCommand
                ??= new DelegateCommand(() => _dialogService.ShowDialog("CustomDialog",
                                                                        new DialogParameters
                                                                        {
                                                                            {
                                                                                "Message", _productService.GetProductsWithoutSells()
                                                                            }
                                                                        },
                                                                        _ => { }));

        public ICommand ShowOverallWarehouseCountBySizeCommand =>
            _showOverallWarehouseCountBySizeCommand
                ??= new DelegateCommand(() => _dialogService.ShowDialog("CustomDialog",
                                                                        new DialogParameters
                                                                        {
                                                                            {
                                                                                "Message", _productService.GetOverallWarehouseCountBySize()
                                                                            }
                                                                        },
                                                                        _ => { }));

        public ICommand UpdateProductsCommand =>
            _updateProductsCommand
                ??= new DelegateCommand(() => Products = new ObservableCollection<ProductViewModel>(_productService.GetAll()));

        public ICommand CallProductWindowCommand =>
            _callProductWindowCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var window = new ProductWindow();
                                            ((ProductWindowViewModel)window.DataContext).Product = _mapper.Map<ProductViewModel>(SelectedProduct);
                                            window.ShowDialog();

                                            UpdateProductsCommand?.Execute(null);
                                        },
                                        () => SelectedProduct != null).ObservesProperty(() => SelectedProduct);

        public ICommand CallEmptyProductWindowCommand =>
            _callEmptyProductWindowCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var window = new ProductWindow();
                                            ((ProductWindowViewModel)window.DataContext).Product = new ProductViewModel();
                                            window.ShowDialog();

                                            UpdateProductsCommand?.Execute(null);
                                        });

        public ICommand CallSellWindowCommand =>
            _callSellWindowCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var window = new SellWindow();
                                            ((SellWindowViewModel)window.DataContext).Sell = _mapper.Map<SellViewModel>(SelectedSell);
                                            window.ShowDialog();

                                            UpdateProductsCommand?.Execute(null);
                                        },
                                        () => SelectedSell != null).ObservesProperty(() => SelectedSell);

        public ICommand CallEmptySellWindowCommand =>
            _callEmptySellWindowCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var window = new SellWindow();

                                            ((SellWindowViewModel)window.DataContext).Sell = new SellViewModel
                                                                                             {
                                                                                                 SellDate = DateTime.Now
                                                                                             };

                                            window.ShowDialog();

                                            UpdateProductsCommand?.Execute(null);
                                        });

        public ICommand CallStockAvailabilityWindowCommand =>
            _callStockAvailabilityWindowCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var window = new StockAvailabilityWindow();
                                            ((StockAvailabilityWindowViewModel)window.DataContext).StockAvailability = _mapper.Map<StockAvailabilityViewModel>(SelectedStockAvailability);
                                            window.ShowDialog();

                                            UpdateProductsCommand?.Execute(null);
                                        },
                                        () => SelectedStockAvailability != null).ObservesProperty(() => SelectedStockAvailability);

        public ICommand CallEmptyStockAvailabilityWindowCommand =>
            _callEmptyStockAvailabilityWindowCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var window = new StockAvailabilityWindow();

                                            ((StockAvailabilityWindowViewModel)window.DataContext).StockAvailability = new StockAvailabilityViewModel();

                                            window.ShowDialog();

                                            UpdateProductsCommand?.Execute(null);
                                        });

        public ICommand CreateReportCommand =>
            _createReportCommand ??= new DelegateCommand(() =>
                                                         {
                                                             var db = _dbContextFactory.CreateDbContext();

                                                             var getActiveProductsQuery = db.Set<Product>()
                                                                                            .AsNoTracking()
                                                                                            .Where(x => !x.IsDeleted)
                                                                                            .Select(x => x.Id);

                                                             var getSellsByToday = db.Set<Sell>()
                                                                                     .AsNoTracking()
                                                                                     .Where(x => x.SellDate.Day == DateTime.Now.Day && x.SellDate.Year == DateTime.Now.Year && x.SellDate.Month == DateTime.Now.Month)
                                                                                     .Select(x => new
                                                                                                  {
                                                                                                      x.Id,
                                                                                                      x.SellDate,
                                                                                                      x.Count
                                                                                                  });

                                                             var name = DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + ".docx";
                                                             name = Path.Combine(Directory.GetCurrentDirectory(), name);
                                                             var doc = DocX.Create(name);

                                                             var paragraph = doc.InsertParagraph();
                                                             paragraph.Append("Запрос №1 (вывести идентификаторы продуктов, которые не удалены): ");
                                                             paragraph.Alignment = Alignment.center;
                                                             paragraph.Color(Color.Brown);

                                                             paragraph = doc.InsertParagraph();
                                                             paragraph.Append($"Запрос: {getActiveProductsQuery.ToQueryString()}");

                                                             paragraph = doc.InsertParagraph();
                                                             paragraph.Append($"Выборка: {String.Join(',', getActiveProductsQuery.ToList())}");

                                                             paragraph = doc.InsertParagraph();
                                                             paragraph.Append("Запрос №2 (вывести продажи, которые были сегодня): ");
                                                             paragraph.Alignment = Alignment.center;
                                                             paragraph.Color(Color.Brown);

                                                             paragraph = doc.InsertParagraph();
                                                             paragraph.Append($"Запрос: {getSellsByToday.ToQueryString()}");

                                                             paragraph = doc.InsertParagraph();
                                                             paragraph.Append($"Выборка: {String.Join(',', getSellsByToday.ToList())}");

                                                             doc.SaveAs(name);
                                                         });

        public ObservableCollection<ProductViewModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public ProductViewModel SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        public SellViewModel SelectedSell
        {
            get => _selectedSell;
            set => SetProperty(ref _selectedSell, value);
        }

        public StockAvailabilityViewModel SelectedStockAvailability
        {
            get => _selectedStockAvailability;
            set => SetProperty(ref _selectedStockAvailability, value);
        }
    }
}
