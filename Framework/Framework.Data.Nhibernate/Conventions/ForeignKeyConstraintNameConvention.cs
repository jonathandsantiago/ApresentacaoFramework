using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using Framework.Helper.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Data.Nhibernate.Conventions
{
    public class ForeignKeyConstraintNameConvention : IClassConvention, ISubclassConvention, IJoinedSubclassConvention, IComponentConvention, IHasManyToManyConvention, ICompositeIdentityConvention
    {
        private Dictionary<string, IInspector> components;

        public ForeignKeyConstraintNameConvention()
        {
            components = new Dictionary<string, IInspector>();
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Key.ForeignKey("Fk" + instance.EntityType.Name + instance.Member.Name + "_" + GetColumnName(instance.Key.Columns));
            instance.Relationship.ForeignKey("Fk" + instance.EntityType.Name + instance.Member.Name + "_" + GetColumnName(instance.Relationship.Columns));
        }

        public void Apply(IComponentInstance instance)
        {
            foreach (var item in instance.References)
            {
                SetManyToOneForeignKey(item, instance);
            }
        }

        public void Apply(IClassInstance instance)
        {
            foreach (var item in instance.References)
            {
                SetManyToOneForeignKey(item, instance);
            }
        }

        public void Apply(ISubclassInstance instance)
        {
            foreach (var item in instance.References)
            {
                SetManyToOneForeignKey(item, instance);
            }
        }

        public void Apply(IJoinedSubclassInstance instance)
        {
            foreach (var item in instance.References)
            {
                SetManyToOneForeignKey(item, instance);
            }

            instance.Key.ForeignKey("Fk" + instance.EntityType.Name + "_" + GetColumnName(instance.Key.Columns));
        }

        public void Apply(ICompositeIdentityInstance instance)
        {
            foreach (var item in instance.KeyManyToOnes)
            {
                var column = item.Columns.FirstOrDefault();
                item.ForeignKey("Fk" + instance.EntityType.Name + "_" + (column != null ? column.Name.RemoveFromEnd("Id") : string.Empty));
            }
        }

        private void SetManyToOneForeignKey(IManyToOneInspector manyToOneInspector, IInspector instance)
        {
            manyToOneInspector.ForeignKey("Fk" + instance.EntityType.Name + "_" + GetColumnName(manyToOneInspector.Columns));
        }

        private string GetColumnName(IEnumerable<IColumnInspector> columns)
        {
            return columns.FirstOrDefault().Name.RemoveFromEnd("Id", "_id");
        }
    }

    public static class ManyToOneInspectorExtensions
    {
        static Dictionary<Type, FieldInfo> mappingFieldsInfo;

        static ManyToOneInspectorExtensions()
        {
            mappingFieldsInfo = new Dictionary<Type, FieldInfo>();
        }

        public static ManyToOneMapping GetMapping(this IManyToOneInspector manyToOneInspector)
        {
            Type type = manyToOneInspector.GetType();
            FieldInfo fieldInfo = null;
            mappingFieldsInfo.TryGetValue(type, out fieldInfo);

            if (fieldInfo == null)
            {
                fieldInfo = manyToOneInspector.GetType().GetField("mapping", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            if (fieldInfo != null)
            {
                var manyToOneMapping = fieldInfo.GetValue(manyToOneInspector) as ManyToOneMapping;
                return manyToOneMapping;
            }

            return null;
        }

        public static void ForeignKey(this IManyToOneInspector manyToOneInspector, string foreignKeyName)
        {
            var mapping = manyToOneInspector.GetMapping();
            mapping.ForeignKey(foreignKeyName);
        }
    }

    public static class ManyToOneMappingExtensions
    {
        public static void ForeignKey(this ManyToOneMapping manyToOneMapping, string foreignKeyName)
        {
            if (!manyToOneMapping.IsSpecified("ForeignKey"))
            {
                manyToOneMapping.Set<string>(c => c.ForeignKey, 1, foreignKeyName);
            }
        }

        public static void ForeignKey(this KeyManyToOneMapping keyManyToOneMapping, string foreignKeyName)
        {
            if (!keyManyToOneMapping.IsSpecified("ForeignKey"))
            {
                keyManyToOneMapping.Set<string>(c => c.ForeignKey, 1, foreignKeyName);
            }
        }
    }
}