using System.Linq.Expressions;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class SearchExpressionBuilder : ISearchExpressionBuilder
    {
        public Expression<Func<T, bool>> BuildSearchExpression<T>(string searchText) where T : class
        {
            var parameter = Expression.Parameter(typeof(T), "e");
            var propertyChecks = new List<Expression>();
            var searchTerms = searchText.ToLower().Split(' '); // Splits on spaces to handle multiple terms

            foreach (var term in searchTerms)
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    if (property.PropertyType == typeof(string))
                    {
                        var propertyAccess = Expression.Property(parameter, property);
                        var toLower = Expression.Call(propertyAccess, "ToLower", Type.EmptyTypes);
                        var containsMethod = Expression.Call(toLower, "Contains", Type.EmptyTypes, Expression.Constant(term));
                        propertyChecks.Add(containsMethod);
                    }
                }
            }

            var orExpression = propertyChecks.Aggregate<Expression>((accumulate, next) => Expression.OrElse(accumulate, next));
            return Expression.Lambda<Func<T, bool>>(orExpression, parameter);
        }
    }
}
