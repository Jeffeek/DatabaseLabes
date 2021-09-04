using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace First_10.ViewModels.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Producer { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        public string? PhotoLink { get; set; }

        public string? Availability { get; set; }

        public BitmapImage? Photo { get; set; }

        public ICollection<SellViewModel>? Sells { get; set; } = default!;
    }
}
