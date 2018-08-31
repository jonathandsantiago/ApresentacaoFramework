using Dominio.Models;
using Framework.Apresentacao.Mvc.Common;
using NHibernate;
using Servico;

namespace ApresentacaoFramework.Controllers
{
    public class PaisController : BaseController<Pais, int, PaisServico>
    {
        public PaisController(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}