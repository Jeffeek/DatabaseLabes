using System;
using First_10.ViewModels.Models.Common;
using Prism.Mvvm;

namespace First_10.ViewModels.Models
{
    public class SellViewModel : BindableBase
    {
        private string _size = default!;
        private DateTime _sellDate;
        private int _count;
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

        public string Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        public DateTime SellDate
        {
            get => _sellDate;
            set => SetProperty(ref _sellDate, value);
        }

        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }
    }
}