using System;

namespace First_10.DataAccess.Models
{
    public class Sell
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; } = default!;

        public string Size { get; set; } = default!;

        public DateTime SellDate { get; set; }

        public int Count { get; set; }
    }
}
