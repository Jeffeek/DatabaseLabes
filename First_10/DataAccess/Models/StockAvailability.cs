using System;
using DatabaseLabes.SharedKernel.DataAccess.Models;

namespace First_10.DataAccess.Models
{
    public class StockAvailability : ISoftDelete
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; } = default!;

        public int Price { get; set; }

        public int WarehouseCount { get; set; }

        public int? Discount { get; set; }

        public string? Availability { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? Deleted { get; set; }
    }
}
