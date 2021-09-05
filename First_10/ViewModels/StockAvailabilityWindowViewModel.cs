using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using First_10.BusinessLogic;
using First_10.BusinessLogic.Services;
using First_10.ViewModels.Models;
using First_10.ViewModels.Models.Common;
using First_10.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace First_10.ViewModels
{
    public class StockAvailabilityWindowViewModel : BindableBase
    {
        private readonly StockAvailabilityService _stockAvailabilityService;
        private readonly MessageBoxService _messageBoxService;
        private readonly ProductService _productService;
        private ObservableCollection<ProductInParent> _productStockAvailabilities;

        private ICommand? _updateStockAvailabilityCommand = default!;
        private ICommand? _createStockAvailabilityCommand = default!;
        private ICommand? _deleteStockAvailabilityCommand = default!;

        private string? _searchText = default!;

        private StockAvailabilityViewModel _stockAvailability;

        public StockAvailabilityWindowViewModel(StockAvailabilityService stockAvailabilityService,
                                                MessageBoxService messageBoxService,
                                                ProductService productService)
        {
            _stockAvailabilityService = stockAvailabilityService;
            _messageBoxService = messageBoxService;
            _productService = productService;
            ProductStockAvailabilities = new ObservableCollection<ProductInParent>(productService.GetProductStockAvailabilities());
        }

        public string? SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);

                if (value == null)
                    return;

                if (ProductStockAvailabilities.FirstOrDefault(x => x.Title.Contains(value)) != null)
                    return;

                if (!_messageBoxService.ShowWarningWithAnswer($"Not found product with matching: {value}. Do you want to create it?"))
                    return;

                var window = new ProductWindow();

                ((ProductWindowViewModel)window.DataContext).Product = new ProductViewModel
                                                                       {
                                                                           Title = value
                                                                       };

                window.ShowDialog();

                ProductStockAvailabilities = new ObservableCollection<ProductInParent>(_productService.GetProductStockAvailabilities());
            }
        }

        public StockAvailabilityViewModel StockAvailability
        {
            get => _stockAvailability;
            set
            {
                if (SetProperty(ref _stockAvailability, value))
                    if (ProductStockAvailabilities?.Count > 0)
                        _stockAvailability.Product = ProductStockAvailabilities.FirstOrDefault(x => x.ProductId == value?.ProductId);
            }
        }

        public ObservableCollection<ProductInParent> ProductStockAvailabilities
        {
            get => _productStockAvailabilities;
            set => SetProperty(ref _productStockAvailabilities, value);
        }

        public ICommand UpdateStockAvailabilityCommand =>
            _updateStockAvailabilityCommand
                ??= new DelegateCommand(() =>
                                        {
                                            if (!CanCreateOrUpdateStockAvailability())
                                            {
                                                _messageBoxService.ShowError("Can't update a new Stock Availability due some mandatory field are not filled");

                                                return;
                                            }

                                            _stockAvailabilityService.Update(StockAvailability);
                                        },
                                        () => (StockAvailability?.Id ?? 0) != 0).ObservesProperty(() => StockAvailability);

        public ICommand CreateStockAvailabilityCommand =>
            _createStockAvailabilityCommand
                ??= new DelegateCommand(() =>
                                        {
                                            if (!CanCreateOrUpdateStockAvailability())
                                            {
                                                _messageBoxService.ShowError("Can't create a new Stock Availability due some mandatory field are not filled");

                                                return;
                                            }

                                            _stockAvailabilityService.Add(StockAvailability);
                                        });

        public ICommand DeleteStockAvailabilityCommand =>
            _deleteStockAvailabilityCommand
                ??= new DelegateCommand(() =>
                                        {
                                            if (!_messageBoxService.AreYouSure($"Are you sure to delete Stock Availability with Id = {StockAvailability.Id}?"))
                                                return;

                                            _stockAvailabilityService.Delete(StockAvailability.Id);
                                        },
                                        () => (StockAvailability?.Id ?? 0) != 0).ObservesProperty(() => StockAvailability);

        private bool CanCreateOrUpdateStockAvailability() =>
            StockAvailability.Discount is null or >= 0
            && StockAvailability.ProductId > 0
            && StockAvailability.Price >= 0
            && StockAvailability.WarehouseCount >= 0;
    }
}