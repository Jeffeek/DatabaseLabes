using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DatabaseLabes.SharedKernel.Query;
using First_10.DataAccess.Models;

namespace First_10.Queries
{
    public class ProductGetAllQuery : Query<Product>
    {
        #region Overrides of Query<Product>

        /// <inheritdoc />
        public override List<Expression<Func<Product, object>>> GetIncludes()
        {
            var result = base.GetIncludes();

            result.Add(x => x.StockAvailabilities);
            result.Add(x => x.Sells);

            return result;
        }

        /// <inheritdoc />
        public override Expression<Func<Product, bool>> GetExpression() =>
            item => !item.IsDeleted;

        #endregion
    }
}
