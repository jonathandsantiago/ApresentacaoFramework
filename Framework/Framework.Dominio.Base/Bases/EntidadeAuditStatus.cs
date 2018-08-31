using System;

namespace Framework.Dominio.Base
{
    public class EntidadeAuditStatus<TId> : EntidadeStatus<TId>
    {
        public virtual string UsuarioCadastro { get; set; }
        public virtual string UsuarioAlteracao { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataAlteracao { get; set; }
        public virtual int Revisao { get; set; }
    }
}