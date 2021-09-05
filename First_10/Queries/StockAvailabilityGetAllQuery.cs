using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DatabaseLabes.SharedKernel.Query;
using First_10.DataAccess.Models;

namespace First_10.Queries
{
    public class StockAvailabilityGetAllQuery : Query<StockAvailability>
    {
        #region Overrides of Query<StockAvailability>

        /// <inheritdoc />
        public override Expression<Func<StockAvailability, bool>> GetExpression() => item => true;

        /// <inheritdoc />
        public override List<Expression<Func<StockAvailability, object>>> GetIncludes()
        {
            var result = base.GetIncludes();
            result.Add(x => x.Product);

            return result;
        }

        #endregion
    }
}
