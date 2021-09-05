using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DatabaseLabes.SharedKernel.Query;
using First_10.DataAccess.Models;

namespace First_10.Queries
{
    public class SellGetAllQuery : Query<Sell>
    {
        #region Overrides of Query<Sell>

        /// <inheritdoc />
        public override Expression<Func<Sell, bool>> GetExpression() => item => true;

        #endregion

        #region Overrides of Query<Sell>

        /// <inheritdoc />
        public override List<Expression<Func<Sell, object>>> GetIncludes()
        {
            var result = base.GetIncludes();
            result.Add(x => x.Product);

            return result;
        }

        #endregion
    }
}