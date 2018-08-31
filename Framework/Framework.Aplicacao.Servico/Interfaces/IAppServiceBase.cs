using Framework.Dominio.Base;
using Framework.Validator.Validation;
using System.Collections.Generic;

namespace Framework.Aplicacao.Servico.Interfaces
{
    public interface IAppServiceBase<TEntidade, TId>
        where TEntidade : EntidadeId<TId>
    {
        TEntidade ObterPorId(TId id);
        IList<TEntidade> ObterTodos();
        bool Inserir(TEntidade entidade);
        bool Editar(TEntidade entidade);
        bool Excluir(TEntidade entidade);
        ValidationResult Resultado { get; set; }
    }
}