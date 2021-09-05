using First_10.ViewModels.Models.Common;
using Prism.Mvvm;

namespace First_10.ViewModels.Models
{
    public class StockAvailabilityViewModel : BindableBase
    {
        private int _price;
        private int _warehouseCount;
        private int? _discount;
        private string? _availability;
        private ProductInParent _product;

        public int Id { get; set; }

        public int ProductId { get; set; }

        public ProductInParent Product
        {
            get => _product;
            set
            {
                if (SetProperty(ref _product, value))
                    if (value != null)
                        ProductId = value.ProductId;
            }
        }

        public int Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public int WarehouseCount
        {
            get => _warehouseCount;
            set => SetProperty(ref _warehouseCount, value);
        }

        public int? Discount
        {
            get => _discount;
            set => SetProperty(ref _discount, value);
        }

        public string? Availability
        {
            get => _availability;
            set => SetProperty(ref _availability, value);
        }
    }
}