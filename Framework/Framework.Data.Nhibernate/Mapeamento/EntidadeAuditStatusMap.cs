using Framework.Dominio.Base;

namespace Framework.Data.Nhibernate.Mapeamento
{
    public class EntidadeAuditStatusMap<TId, TEntidade> : EntidadeStatusMap<TId, TEntidade>
        where TEntidade : EntidadeAuditStatus<TId>
    {
        public EntidadeAuditStatusMap()
        {
            OnMapearAditoria();
        }

        protected virtual void OnMapearAditoria()
        {
            MapIndex(c => c.Revisao).Default("0").Not.Nullable();
            MapIndex(c => c.UsuarioCadastro).Not.Nullable();
            MapIndex(c => c.UsuarioAlteracao).Nullable();
            MapIndex(c => c.DataCadastro).Not.Nullable();
            MapIndex(c => c.DataAlteracao).Nullable();
        }
    }
}