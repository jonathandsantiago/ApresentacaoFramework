using FluentNHibernate.Mapping;
using Framework.Helper.Helpers;
using System;
using System.Linq.Expressions;

namespace Framework.Data.Nhibernate.Extensoes
{
    public static class NhibernateMapExtensions
    {
        public static IdentityPart Sequence<TEntidade>(this IdentityGenerationStrategyBuilder<IdentityPart> generateByPart)
        {
            return generateByPart.Sequence($"Seq{typeof(TEntidade).Name}");
        }

        public static ManyToOnePart<TOther> ReferenceIndex<T, TOther>(this ClasslikeMapBase<T> map, Expression<Func<T, TOther>> memberExpression)
        {
            return map.References(memberExpression, $"{ExpressionHelper.GetPropertyName(memberExpression)}Id").Index($"Idx{ExpressionHelper.GetPropertyName(memberExpression)}");
        }
    }
}