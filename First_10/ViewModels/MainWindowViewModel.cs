using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using First_10.BusinessLogic;
using First_10.BusinessLogic.Services;
using First_10.ViewModels.Models;
using First_10.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace First_10.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly ProductService _productService;
        private readonly MessageBoxService _messageBoxService;
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

        public MainWindowViewModel(ProductService productService,
                                   MessageBoxService messageBoxService,
                                   IMapper mapper)
        {
            _productService = productService;
            _messageBoxService = messageBoxService;
            _mapper = mapper;
        }

        public ICommand CalculateCostForEachCommand =>
            _calculateCostForEachCommand
                ??= new DelegateCommand(() => _messageBoxService.ShowInformation(_productService.GetWarehouseCostForEachProduct()));

        public ICommand ShowProductsBySumCommand =>
            _showProductsBySumCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var vm = new InputDialogViewModel<int?>();

                                            var window = new InputDialogWindow
                                                         {
                                                             DataContext = vm
                                                         };

                                            window.ShowDialog();

                                            if (vm.Value is >= 0)
                                                _messageBoxService.ShowInformation(_productService.GetProductsSellsUpperSumOn(vm.Value.Value));
                                        });

        public ICommand ShowProductsByWarehouseCountCommand =>
            _showProductsByWarehouseCountCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var vm = new InputDialogViewModel<int?>();

                                            var window = new InputDialogWindow
                                                         {
                                                             DataContext = vm
                                                         };

                                            window.ShowDialog();

                                            if (vm.Value is >= 0)
                                                _messageBoxService.ShowInformation(_productService.GetProductsByWarehouseCount(vm.Value.Value));
                                        });

        public ICommand ShowProductsByCategoryCommand =>
            _showProductsByCategoryCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var vm = new InputDialogViewModel<string?>();

                                            var window = new InputDialogWindow
                                                         {
                                                             DataContext = vm
                                                         };

                                            window.ShowDialog();

                                            if (!String.IsNullOrEmpty(vm.Value))
                                                _messageBoxService.ShowInformation(_productService.GetProductsInformationByCategoryName(vm.Value));
                                        });

        public ICommand ShowSellsCountByDateCommand =>
            _showSellsCountByDateCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var vm = new InputDialogViewModel<int?>();

                                            var window = new InputDialogWindow
                                                         {
                                                             DataContext = vm
                                                         };

                                            window.ShowDialog();

                                            if (vm.Value is >= 0)
                                                _messageBoxService.ShowInformation(_productService.GetSellsCountByDate(vm.Value.Value));
                                        });

        public ICommand ShowProductsByMaxSellCommand =>
            _showProductsByMaxSellCommand
                ??= new DelegateCommand(() => _messageBoxService.ShowInformation(_productService.GetProductsByMaxSell()));

        public ICommand ShowOverallWarehouseCountCommand =>
            _showOverallWarehouseCountCommand
                ??= new DelegateCommand(() => _messageBoxService.ShowInformation(_productService.GetOverallWarehouseCount().ToString()));

        public ICommand ShowProductsWithoutSellsCommand =>
            _showProductsWithoutSellsCommand
                ??= new DelegateCommand(() => _messageBoxService.ShowInformation(_productService.GetProductWithoutSells()));

        public ICommand ShowOverallWarehouseCountBySizeCommand =>
            _showOverallWarehouseCountBySizeCommand
                ??= new DelegateCommand(() => _messageBoxService.ShowInformation(_productService.GetOverallWarehouseCountBySize()));

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
