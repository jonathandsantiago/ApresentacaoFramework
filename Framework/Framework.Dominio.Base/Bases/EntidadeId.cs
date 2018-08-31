using Framework.Dominio.Base.Interfaces;

namespace Framework.Dominio.Base
{
    public class EntidadeId<TId> : IEntidade
    {
        public virtual TId Id { get; set; }
    }
}