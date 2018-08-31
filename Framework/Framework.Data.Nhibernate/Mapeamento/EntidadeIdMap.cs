using FluentNHibernate.Mapping;
using Framework.Data.Nhibernate.Extensoes;
using Framework.Dominio.Base;
using Framework.Helper.Helpers;
using System;
using System.Linq.Expressions;

namespace Framework.Data.Nhibernate.Mapeamento
{
    public class EntidadeIdMap<TId, TEntidade> : ClassMap<TEntidade>
        where TEntidade : EntidadeId<TId>
    {
        public EntidadeIdMap()
        {
            OnMapearId();
        }

        protected virtual void OnMapearId()
        {
            Id(c => c.Id).GeneratedBy.Identity();
        }

        public PropertyPart MapIndex(Expression<Func<TEntidade, object>> memberExpression)
        {
            return Map(memberExpression).Index("Idx" + ExpressionHelper.GetPropertyName(memberExpression));
        }

        public ManyToOnePart<TOther> ReferenceIndex<TOther>(Expression<Func<TEntidade, TOther>> memberExpression)
        {
            return NhibernateMapExtensions.ReferenceIndex(this, memberExpression).ForeignKey($"Fk_{typeof(TEntidade).Name}_{ExpressionHelper.GetPropertyName(memberExpression)}");
        }
    }
}