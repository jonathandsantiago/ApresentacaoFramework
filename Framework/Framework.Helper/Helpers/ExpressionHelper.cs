using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Helper.Helpers
{
    public static class ExpressionHelper
    {
        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            return GetPropertyName<T, object>(expression);
        }

        public static string[] GetPropertysNames<T>(params Expression<Func<T, object>>[] expression)
        {
            return GetPropertysNames<T, object>(expression);
        }

        public static string GetPropertyName<T, TMember>(Expression<Func<T, TMember>> expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        public static string[] GetPropertysNames<T, TOther>(params Expression<Func<T, TOther>>[] expression)
        {
            return expression.Select(c => GetPropertyName(c)).ToArray();
        }

        public static string GetNestledPropertyName<T, T2>(Expression<Func<T, T2>> expression)
        {
            var visitor = new NestedPropertyNameExpressionVisitor();
            visitor.Visit(expression.Body);
            return visitor.GetNomePropriedadeCompleto();
        }

        public static Expression GetNestledPropertyExpression(ParameterExpression param, string propertyName)
        {
            Expression body = param;

            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return body;
        }

        public static Expression GetNestledPropertyExpression(Type type, string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(type, "p");
            return GetNestledPropertyExpression(parameter, propertyName);
        }

        public static Expression<Func<T, TResult>> CreateExpressionProperty<T, TResult>(ParameterExpression param, string propertyName)
        {
            Expression body = param;

            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return Expression.Lambda<Func<T, TResult>>(body, param);
        }

        public static Expression CreateEqual(Type type, string property, object constatValue)
        {
            ParameterExpression parameter = Expression.Parameter(type, "p");
            Expression expression = ExpressionHelper.GetNestledPropertyExpression(parameter, property);
            ConstantExpression constantExpression = Expression.Constant(constatValue, constatValue.GetType());
            return Expression.Equal(expression, constantExpression);
        }

        public static object GetValue(LambdaExpression expression)
        {
            var del = expression.Compile();
            return del.DynamicInvoke();
        }

        public static object GetValue(object source, LambdaExpression expression)
        {
            var del = expression.Compile();
            return del.DynamicInvoke(source);
        }     

        public static Expression<Func<TInput, bool>> CombineWithAndAlso<TInput>(this Expression<Func<TInput, bool>> func1, Expression<Func<TInput, bool>> func2)
        {
            return Expression.Lambda<Func<TInput, bool>>(Expression.AndAlso(func1.Body, new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)), func1.Parameters);
        }

        public static Expression<Func<TInput, bool>> CombineWithOrElse<TInput>(this Expression<Func<TInput, bool>> func1, Expression<Func<TInput, bool>> func2)
        {
            return Expression.Lambda<Func<TInput, bool>>(
                Expression.AndAlso(
                    func1.Body, new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        public static Expression<Func<TDestination, TResult>> Convert<TOrigin, TDestination, TResult>(Expression<Func<TOrigin, TResult>> expr)
        {
            var parametersMap = expr.Parameters
                .Where(pe => pe.Type == typeof(TOrigin))
                .ToDictionary(pe => pe, pe => Expression.Parameter(typeof(TDestination)));

            var visitor = new DelegateConversionVisitor(parametersMap);
            var newBody = visitor.Visit(expr.Body);

            var parameters = expr.Parameters.Select(visitor.MapParameter);

            return Expression.Lambda<Func<TDestination, TResult>>(newBody, parameters);
        }

        public static Func<TOut, TR> ConvertFunc<TIn, TOut, TR>(Func<TIn, TR> func) where TIn : TOut
        {
            return p => func((TIn)p);
        }

        public static Expression<Func<T, TResult>> ReplaceParameterByConstant<T, TConstant, TResult>(Expression<Func<T, TConstant, TResult>> predicate, object constant)
        {
            var body = predicate.Body;
            var substitutionVisitor = new VariableSubstitutionVisitor(predicate.Parameters[1], Expression.Constant(constant, typeof(TConstant)));
            var visitedBody = substitutionVisitor.Visit(body).Reduce();
            return Expression.Lambda<Func<T, TResult>>(visitedBody, predicate.Parameters[0]);
        }

        public static Expression<Func<T, TResult>> ReplaceParameterByConstant<T, TConstant1, TConstant2, TResult>(Expression<Func<T, TConstant1, TConstant2, bool>> predicate,
            TConstant1 constant1, TConstant2 constant2)
        {
            var body = predicate.Body;
            var substitutionVisitor = new VariableSubstitutionVisitor(predicate.Parameters[1], Expression.Constant(constant1, typeof(TConstant1)));
            var visitedBody = substitutionVisitor.Visit(body).Reduce();
            var newLambda = Expression.Lambda<Func<T, TConstant2, TResult>>(visitedBody, predicate.Parameters[0], predicate.Parameters[2]);
            return ReplaceParameterByConstant<T, TConstant2, TResult>(newLambda, constant2);
        }

        public static Expression<Func<T, TResult>> ReplaceParameterByConstant<T, TResult>(Expression<Func<T, object, bool>> predicate, object constant)
        {
            var body = predicate.Body;
            var substitutionVisitor = new VariableSubstitutionVisitor(predicate.Parameters[1], Expression.Constant(constant, constant.GetType()));
            var visitedBody = substitutionVisitor.Visit(body).Reduce();
            return Expression.Lambda<Func<T, TResult>>(visitedBody, predicate.Parameters[0]);
        }

        public static Expression<Func<TDestination, TResult>> Convert<TOrigin, TDestination, TResult>(Expression<Func<TDestination, TOrigin>> member, Expression<Func<TOrigin, TResult>> expr)
        {
            var body = new ParameterExpressionReplacer { Source = expr.Parameters[0], Target = member.Body }.Visit(expr.Body);
            var result = Expression.Lambda<Func<TDestination, TResult>>(body, member.Parameters);
            return result;
        }
    }

    public class NestedPropertyNameExpressionVisitor : ExpressionVisitor
    {
        private string nomePropriedadeCompleta = string.Empty;

        protected override Expression VisitMember(MemberExpression node)
        {
            nomePropriedadeCompleta = node.Member.Name + "." + nomePropriedadeCompleta;
            return base.VisitMember(node);
        }

        public string GetNomePropriedadeCompleto()
        {
            return nomePropriedadeCompleta.Remove(nomePropriedadeCompleta.Length - 1);
        }
    }

    public class ExpressionParameterReplacer : ExpressionVisitor
    {
        public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
        {
            ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
            for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
                ParameterReplacements.Add(fromParameters[i], toParameters[i]);
        }

        private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            ParameterExpression replacement;

            if (ParameterReplacements.TryGetValue(node, out replacement))
            {
                node = replacement;
            }
            return base.VisitParameter(node);
        }
    }

    public sealed class DelegateConversionVisitor : ExpressionVisitor
    {
        IDictionary<ParameterExpression, ParameterExpression> parametersMap;

        public DelegateConversionVisitor(IDictionary<ParameterExpression, ParameterExpression> parametersMap)
        {
            this.parametersMap = parametersMap;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(this.MapParameter(node));
        }

        public ParameterExpression MapParameter(ParameterExpression source)
        {
            var target = source;
            this.parametersMap.TryGetValue(source, out target);
            return target;
        }
    }


    public class VariableSubstitutionVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;
        private readonly ConstantExpression _constant;

        public VariableSubstitutionVisitor(ParameterExpression parameter, ConstantExpression constant)
        {
            _parameter = parameter;
            _constant = constant;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node == _parameter)
            {
                return _constant;
            }

            return node;
        }
    }

    public class ParameterExpressionReplacer : ExpressionVisitor
    {
        public ParameterExpression Source { get; set; }
        public Expression Target { get; set; }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == Source ? Target : base.VisitParameter(node);
        }
    }
}