using System;

namespace First_10.ViewModels.Models
{
    public class SellViewModel
    {
        public int Id { get; set; }

        public string Size { get; set; } = default!;

        public DateTime SellDate { get; set; }

        public int Count { get; set; }
    }
}