using System.Collections.ObjectModel;
using System.Windows.Input;
using First_10.BusinessLogic;
using First_10.DataAccess.Models;
using First_10.ViewModels.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace First_10.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly ProductService _productService;
        private readonly IRegionManager _regionManager;

        private ObservableCollection<ProductViewModel> _products;
        private ProductViewModel _selectedProduct;

        private ICommand _updateProductsCommand;
        private ICommand _callProductWindowCommand;

        public MainWindowViewModel(ProductService productService,
                                   IRegionManager regionManager)
        {
            _productService = productService;
            _regionManager = regionManager;
        }

        public ICommand UpdateProductsCommand =>
            _updateProductsCommand
            ?? new DelegateCommand(() => Products = new ObservableCollection<ProductViewModel>(_productService.GetAll()));

        public ICommand CallProductWindowCommand =>
            _callProductWindowCommand
            ?? new DelegateCommand(() => _regionManager.RequestNavigate("ProductWindow", "Views/ProductWindow.xaml"));

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
    }
}
