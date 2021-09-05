using System;
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
    public class SellWindowViewModel : BindableBase
    {
        private static readonly DateTime _dateTimeBound = new(1800,
                                                              1,
                                                              1,
                                                              1,
                                                              1,
                                                              1,
                                                              DateTimeKind.Utc);

        private readonly SellService _sellService;
        private readonly MessageBoxService _messageBoxService;
        private readonly ProductService _productService;
        private string? _searchText = default!;
        private ObservableCollection<ProductInParent> _productSells;

        private SellViewModel _sell;

        private ICommand? _updateSellCommand = default!;
        private ICommand? _createSellCommand = default!;
        private ICommand? _deleteSellCommand = default!;

        public SellWindowViewModel(SellService sellService,
                                   MessageBoxService messageBoxService,
                                   ProductService productService)
        {
            _sellService = sellService;
            _messageBoxService = messageBoxService;
            _productService = productService;
            ProductSells = new ObservableCollection<ProductInParent>(productService.GetProductStockAvailabilities());
        }

        public string? SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);

                if (value == null)
                    return;

                if (ProductSells.FirstOrDefault(x => x.Title.Contains(value)) != null)
                    return;

                if (!_messageBoxService.ShowWarningWithAnswer($"Not found product with matching: {value}. Do you want to create it?"))
                    return;

                var window = new ProductWindow();

                ((ProductWindowViewModel)window.DataContext).Product = new ProductViewModel
                                                                       {
                                                                           Title = value
                                                                       };

                window.ShowDialog();

                ProductSells = new ObservableCollection<ProductInParent>(_productService.GetProductStockAvailabilities());
            }
        }

        public ObservableCollection<ProductInParent> ProductSells
        {
            get => _productSells;
            set => SetProperty(ref _productSells, value);
        }

        public SellViewModel Sell
        {
            get => _sell;
            set
            {
                if (SetProperty(ref _sell, value))
                    if (ProductSells?.Count > 0)
                        Sell.Product = ProductSells.FirstOrDefault(x => x.ProductId == value?.ProductId);
            }
        }

        public ICommand UpdateSellCommand =>
            _updateSellCommand
                ??= new DelegateCommand(() =>
                                        {
                                            if (!CanCreateOrUpdateSell())
                                            {
                                                _messageBoxService.ShowError("Can't update a new sell due some mandatory field are not filled");

                                                return;
                                            }

                                            _sellService.Update(Sell);
                                        },
                                        () => (Sell?.Id ?? 0) != 0).ObservesProperty(() => Sell);

        public ICommand CreateSellCommand =>
            _createSellCommand
                ??= new DelegateCommand(() =>
                                        {
                                            if (!CanCreateOrUpdateSell())
                                            {
                                                _messageBoxService.ShowError("Can't create a new sell due some mandatory field are not filled or incorrect");

                                                return;
                                            }

                                            _sellService.Add(Sell);
                                        });

        public ICommand DeleteSellCommand =>
            _deleteSellCommand
                ??= new DelegateCommand(() =>
                                        {
                                            if (!_messageBoxService.AreYouSure($"Are you sure to delete Sell with Id = {Sell.Id}?"))
                                                return;

                                            _sellService.Delete(Sell.Id);
                                        },
                                        () => (Sell?.Id ?? 0) != 0).ObservesProperty(() => Sell);

        private bool CanCreateOrUpdateSell() =>
            !String.IsNullOrEmpty(Sell.Size)
            && Sell.SellDate > _dateTimeBound
            && Sell.Count >= 0;
    }
}