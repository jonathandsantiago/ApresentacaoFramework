using FluentNHibernate.Mapping;
using Framework.Data.Nhibernate.Extensoes;
using Framework.Dominio.Base.Interfaces;
using Framework.Helper.Helpers;
using System;
using System.Linq.Expressions;

namespace Framework.Data.Nhibernate.Mapeamento
{
    public class EntidadeSubclassMap<TEntidade> : SubclassMap<TEntidade>
        where TEntidade : IEntidade
    {
        public PropertyPart MapIndex(Expression<Func<TEntidade, object>> memberExpression)
        {
            return Map(memberExpression).Index("Idx" + ExpressionHelper.GetPropertyName(memberExpression));
        }

        public ManyToOnePart<TOther> ReferenceIndex<TOther>(Expression<Func<TEntidade, TOther>> memberExpression)
        {
            return NhibernateMapExtensions.ReferenceIndex(this, memberExpression);
        }
    }
}