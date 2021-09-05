using System;
using System.Windows.Input;
using First_10.BusinessLogic;
using First_10.BusinessLogic.Services;
using First_10.ViewModels.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace First_10.ViewModels
{
    public class ProductWindowViewModel : BindableBase
    {
        private readonly ImageService _imageService;
        private readonly MessageBoxService _messageBoxService;
        private readonly ProductService _productService;
        private ProductViewModel _product = default!;

        private ICommand? _deleteImageCommand = default!;
        private ICommand? _pasteImageCommand = default!;
        private ICommand? _updateProductCommand = default!;
        private ICommand? _createProductCommand = default!;
        private ICommand? _deleteProductCommand = default!;

        public ProductViewModel Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        public ProductWindowViewModel(ImageService imageService,
                                      MessageBoxService messageBoxService,
                                      ProductService productService)
        {
            _imageService = imageService;
            _messageBoxService = messageBoxService;
            _productService = productService;
        }

        public ICommand PasteImageCommand =>
            _pasteImageCommand
                ??= new DelegateCommand(() =>
                                        {
                                            var image = _imageService.PasteImage();

                                            if (image == null)
                                                return;

                                            Product.Photo = image;
                                            Product.PhotoLink = image.UriSource.LocalPath;
                                        });

        public ICommand DeleteImageCommand =>
            _deleteImageCommand
                ??= new DelegateCommand(() =>
                                        {
                                            Product.Photo = null!;
                                            Product.PhotoLink = null!;
                                        },
                                        () => Product?.Photo != null);

        public ICommand UpdateProductCommand =>
            _updateProductCommand
                ??= new DelegateCommand(() =>
                                        {
                                            if (!CanCreateOrUpdateProduct())
                                            {
                                                _messageBoxService.ShowError("Can't update a new product due some mandatory field are not filled");

                                                return;
                                            }

                                            _productService.Update(Product);
                                        },
                                        () => (Product?.Id ?? 0) != 0).ObservesProperty(() => Product);

        public ICommand CreateProductCommand =>
            _createProductCommand
                ??= new DelegateCommand(() =>
                                        {
                                            if (!CanCreateOrUpdateProduct())
                                            {
                                                _messageBoxService.ShowError("Can't create a new product due some mandatory field are not filled");

                                                return;
                                            }

                                            if (_productService.ExistWithTitle(Product.Title))
                                            {
                                                _messageBoxService.ShowWarning("Can't create a new product. The same name already exists");

                                                return;
                                            }

                                            _productService.Add(Product);
                                        });

        public ICommand DeleteProductCommand =>
            _deleteProductCommand
                ??= new DelegateCommand(() =>
                                        {
                                            if (!_messageBoxService.AreYouSure($"Are you sure to delete Product with Id = {Product.Id}?"))
                                                return;

                                            _productService.Delete(Product.Id);
                                        },
                                        () => (Product?.Id ?? 0) != 0).ObservesProperty(() => Product);

        private bool CanCreateOrUpdateProduct() =>
            !String.IsNullOrEmpty(Product.Title)
            && !String.IsNullOrEmpty(Product.Producer)
            && !String.IsNullOrEmpty(Product.Category);
    }
}