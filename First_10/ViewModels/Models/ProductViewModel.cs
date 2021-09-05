using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Prism.Mvvm;

namespace First_10.ViewModels.Models
{
    public class ProductViewModel : BindableBase
    {
        private string _title = default!;
        private string _producer = default!;
        private string _category = default!;
        private string? _description;
        private string? _photoLink;
        private string? _availability;
        private BitmapImage? _photo;
        private ICollection<SellViewModel>? _sells = default!;
        private ICollection<StockAvailabilityViewModel>? _stockAvailabilities = default!;

        public int Id { get; set; }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Producer
        {
            get => _producer;
            set => SetProperty(ref _producer, value);
        }

        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string? PhotoLink
        {
            get => _photoLink;
            set => SetProperty(ref _photoLink, value);
        }

        public string? Availability
        {
            get => _availability;
            set => SetProperty(ref _availability, value);
        }

        public BitmapImage? Photo
        {
            get => _photo;
            set => SetProperty(ref _photo, value);
        }

        public ICollection<SellViewModel>? Sells
        {
            get => _sells;
            set => SetProperty(ref _sells, value);
        }

        public ICollection<StockAvailabilityViewModel>? StockAvailabilities
        {
            get => _stockAvailabilities;
            set => SetProperty(ref _stockAvailabilities, value);
        }
    }
}