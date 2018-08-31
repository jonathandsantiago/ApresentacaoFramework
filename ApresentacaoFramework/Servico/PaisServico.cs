using Dominio.Models;
using Framework.Aplicacao.Servico.Common;
using NHibernate;
using Repositorio;

namespace Servico
{
    public class PaisServico : AppServiceBase<Pais, int, PaisRepositorio>
    {
        public PaisServico(ISessionFactory sessionFactory) : base(sessionFactory)
        {
            Validador = new PaisValidador();
        }
    }
}