using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GroceryList.Infrastructure.Extension;
public static class ExpressionExtensions
{
    public static Expression ReplaceParameter(this Expression expression, ParameterExpression from, ParameterExpression to)
    {
        return new ParameterReplacer { From = from, To = to }.Visit(expression);
    }

    private class ParameterReplacer : ExpressionVisitor
    {
        public ParameterExpression From;
        public ParameterExpression To;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == From ? To : base.VisitParameter(node);
        }
    }
}
