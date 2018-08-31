using Dominio.Models;
using Framework.Data.Repositorio.Common;
using NHibernate;

namespace Repositorio
{
    public class PaisRepositorio : RepositoryBase<Pais, int>
    {
        public PaisRepositorio(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}