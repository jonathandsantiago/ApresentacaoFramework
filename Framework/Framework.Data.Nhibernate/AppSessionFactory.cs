using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace Framework.Data.Nhibernate
{
    public class AppSessionFactory : IDisposable
    {
        private ISessionFactory factory = null;
        private readonly IConfiguration configuration;
        private readonly bool createSchema;
        private readonly string assemblyMapName;
        private readonly string connectionStringsName;

        public AppSessionFactory()
        { }

        public AppSessionFactory(bool createSchema)
        {
            this.createSchema = createSchema;
        }

        public AppSessionFactory(string assemblyMapName, string connectionStringsName, bool createSchema = false)
            : this(createSchema)
        {
            this.assemblyMapName = assemblyMapName;
            this.connectionStringsName = connectionStringsName;
        }

        public AppSessionFactory(IConfiguration configuration, string assemblyMapName, string connectionStringsName, bool createSchema = false)
           : this(assemblyMapName, connectionStringsName, createSchema)
        {
            this.configuration = configuration;
        }

        public AppSessionFactory(IConfiguration configuration, bool createSchema = true)
            : this(createSchema)
        {
            this.configuration = configuration;
        }

        public ISession OpenSession()
        {
            ISession session = GetFactory().OpenSession();

            if (session == null)
            {
                throw new InvalidOperationException("OpenSession() is null.");
            }

            return session;
        }

        public ISessionFactory GetFactory()
        {
            if (factory == null)
            {
                Assembly assembly = GetAssembly();
                DbConnectionStringBuilder connStringBuilder = GetConnectionStringBuilder();
                FluentConfiguration configuration = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connStringBuilder.ConnectionString).ShowSql())
                    .Mappings(m => m.FluentMappings.AddFromAssembly(assembly));
                
                if (createSchema)
                {
                    SchemaUpdate(connStringBuilder, configuration.BuildConfiguration());
                }

                factory = configuration.BuildSessionFactory();

                if (factory == null)
                    throw new InvalidOperationException("BuildSessionFactory is null.");
            }

            return factory;
        }

        private Assembly GetAssembly()
        {
            string assemblyName = assemblyMapName ?? "AssemblyMapName";

            if (configuration != null)
            {
                return Assembly.Load(configuration.GetSection(assemblyName).Value);
            }

            return Assembly.Load(ConfigurationManager.AppSettings[assemblyName]);
        }

        private string GetConnectionString()
        {
            string conectionString = connectionStringsName ?? "ConnectionName";

            if (configuration != null)
            {
                return configuration.GetConnectionString(conectionString);
            }

            return ConfigurationManager.ConnectionStrings[conectionString].ConnectionString;
        }

        private void SchemaUpdate(DbConnectionStringBuilder connStringBuilder, NHibernate.Cfg.Configuration configuration)
        {
            bool commented = false;

            Action<string, string> salvarAction = (fileName, s) =>
            {
                using (var file = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                {
                    using (var sw = new StreamWriter(file))
                    {
                        if (!commented)
                        {
                            string dataBaseName = string.Empty;

                            if (connStringBuilder is SqlConnectionStringBuilder)
                            {
                                dataBaseName = " / " + (connStringBuilder as SqlConnectionStringBuilder).InitialCatalog;
                            }

                            sw.WriteLine("-- Schema Update: " + DateTime.Now.ToString() + dataBaseName);
                            commented = true;
                        }

                        sw.WriteLine(s.Trim() + ";");
                        sw.Close();
                    }
                }
            };

            new SchemaUpdate(configuration).Execute((s) => salvarAction("schemaupdate." + Assembly.GetEntryAssembly().GetName().Version + ".sql", s), false);
        }

        private DbConnectionStringBuilder GetConnectionStringBuilder()
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringsSection configSection = configuration.GetSection("connectionStrings") as ConnectionStringsSection;

            if (!configSection.ElementInformation.IsLocked && !configSection.SectionInformation.IsLocked)
            {
                if (configSection.SectionInformation.IsProtected)
                {
                    configSection.SectionInformation.UnprotectSection();
                }
            }

            return new SqlConnectionStringBuilder(GetConnectionString());
        }

        public void Dispose()
        {
            factory.Close();
        }
    }
}