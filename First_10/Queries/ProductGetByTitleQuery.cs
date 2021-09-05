using System;
using System.Linq.Expressions;
using DatabaseLabes.SharedKernel.Query;
using First_10.DataAccess.Models;

namespace First_10.Queries
{
    public class ProductGetByTitleQuery : Query<Product>
    {
        public string Title { get; set; } = default!;

        public override Expression<Func<Product, bool>> GetExpression() => item => !item.IsDeleted && item.Title == Title;
    }
}