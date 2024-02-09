using System.Linq.Expressions;
using ePOS.Shared.ValueObjects;

namespace ePOS.Shared.Extensions;

public static class LinqExtensions
{
    public static IQueryable<T> ToPagedQuery<T>(this IQueryable<T> query, int pageIndex, int pageSize, out Paginator paginator)
    {
        paginator = new Paginator(pageIndex, pageSize, query.Count());
        return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }

    public static IOrderedQueryable<T> ToSortedQuery<T>(this IQueryable<T> query,
        Expression<Func<T, object>> expression, bool asc = false)
    {
        return asc ? query.OrderBy(expression) : query.OrderByDescending(expression);
    }
}