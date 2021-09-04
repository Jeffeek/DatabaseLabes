using System;
using System.Collections.Generic;
using DatabaseLabes.SharedKernel.DataAccess.Models;

namespace First_10.DataAccess.Models
{
    public class Product : ISoftDelete
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Producer { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        public string? Photo { get; set; }

        public string? Availability { get; set; }

        public ICollection<StockAvailability> StockAvailabilities { get; set; } = default!;

        public ICollection<Sell>? Sells { get; set; } = default!;

        public bool IsDeleted { get; set; }

        public DateTime? Deleted { get; set; }
    }
}