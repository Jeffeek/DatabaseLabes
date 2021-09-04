using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DatabaseLabes.SharedKernel.Query
{
    public interface IQuery<TSource>
        where TSource : class
    {
        Expression<Func<TSource, bool>> GetExpression();

        List<Expression<Func<TSource, object>>> GetIncludes();
    }
}