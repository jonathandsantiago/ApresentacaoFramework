namespace Framework.Dominio.Base
{
    public class EntidadeStatus<TId>: EntidadeId<TId>
    {
        public virtual bool Ativo { get; set; }
    }
}