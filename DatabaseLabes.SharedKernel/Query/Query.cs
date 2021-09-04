using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DatabaseLabes.SharedKernel.Query
{
    public abstract class Query<TSource> : IQuery<TSource>
        where TSource : class
    {
        public virtual Expression<Func<TSource, bool>> GetExpression()
        {
            Expression<Func<TSource, bool>> filter = uniqueEntity => true;

            return filter;
        }

        public virtual List<Expression<Func<TSource, object>>> GetIncludes() =>
            new List<Expression<Func<TSource, object>>>();
    }
}
