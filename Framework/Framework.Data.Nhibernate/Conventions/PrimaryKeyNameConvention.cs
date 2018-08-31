using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Framework.Data.Nhibernate.Conventions
{
    public class PrimaryKeyNameConvention : IIdConvention
    {
        public PrimaryKeyNameConvention()
        { }

        public void Apply(IIdentityInstance instance)
        {
            instance.Column(instance.EntityType.Name + "Id");
        }
    }
}