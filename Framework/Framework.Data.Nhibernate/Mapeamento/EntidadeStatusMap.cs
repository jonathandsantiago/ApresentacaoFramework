using Framework.Dominio.Base;

namespace Framework.Data.Nhibernate.Mapeamento
{
    public class EntidadeStatusMap<TId, TEntidade> : EntidadeIdMap<TId, TEntidade>
        where TEntidade : EntidadeStatus<TId>
    {
        public EntidadeStatusMap()
        {
            OnMapearStatus();
        }

        protected virtual void OnMapearStatus()
        {
            MapIndex(c => c.Ativo).Default("1").Not.Nullable();
        }
    }
}