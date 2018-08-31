using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Framework.Data.Nhibernate.Conventions
{
    public class ComponentConvention : IComponentConvention
    {
        public void Apply(IComponentInstance instance)
        {
            if (instance.Property.Name == "Auditoria")
            {
                return;
            }

            foreach (var item in instance.Properties)
            {
                item.Column(instance.Property.Name + item.Property.Name);
            }
        }
    }
}