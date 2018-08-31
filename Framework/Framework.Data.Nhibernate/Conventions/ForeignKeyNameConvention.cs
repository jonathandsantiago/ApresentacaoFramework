using FluentNHibernate;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Linq;

namespace Framework.Data.Nhibernate.Conventions
{
    public class ForeignKeyNameConvention : IReferenceConvention, IConvention<IManyToOneInspector, IManyToOneInstance>, IHasManyToManyConvention, IConvention<IManyToManyCollectionInspector, IManyToManyCollectionInstance>, IJoinedSubclassConvention, IConvention<IJoinedSubclassInspector, IJoinedSubclassInstance>, IJoinConvention, IConvention<IJoinInspector, IJoinInstance>, ICollectionConvention, IConvention<ICollectionInspector, ICollectionInstance>, IConvention
    {
        public void Apply(ICollectionInstance instance)
        {
            var relationshipType = instance.ChildType;

            var property = relationshipType.GetInstanceProperties().FirstOrDefault(c => c.PropertyType == instance.EntityType);
            string keyName = this.GetKeyName(property != null ? property.Name : null, instance.EntityType);
            instance.Key.Column(keyName);
        }

        public void Apply(IJoinedSubclassInstance instance)
        {
            string keyName = this.GetKeyName(null, instance.Type.BaseType);
            instance.Key.Column(keyName);
        }

        public void Apply(IJoinInstance instance)
        {
            string keyName = this.GetKeyName(null, instance.EntityType);
            instance.Key.Column(keyName);
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            string keyName = this.GetKeyName(null, instance.EntityType);
            string columnName = this.GetKeyName(null, instance.ChildType);
            instance.Key.Column(keyName);
            instance.Relationship.Column(columnName);
        }

        public void Apply(IManyToOneInstance instance)
        {
            string keyName = this.GetKeyName(instance.Property.Name, instance.Class.GetUnderlyingSystemType());
            instance.Column(keyName);
        }

        private string GetKeyName(string propertyName, Type type)
        {
            return (propertyName != null ? propertyName : type.Name) + "Id";
        }
    }
}