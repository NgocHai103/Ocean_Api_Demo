using System.Linq.Expressions;

namespace Infrastructure.Persistence.Dapper.Core.Extentions;

internal static class LamdaExpressionExtensions
{
    public static string GetMemberName<T, TProperty>(this Expression<Func<T, TProperty>> expression)
    {

        if (expression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        if (expression.Body is UnaryExpression unaryExpression
            && unaryExpression.Operand is MemberExpression operandMemberExpress)
        {
            return operandMemberExpress.Member.Name;
        }

        throw new ArgumentException("Invalid expression");
    }
}