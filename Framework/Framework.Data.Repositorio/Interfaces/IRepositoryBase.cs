using Framework.Dominio.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Framework.Data.Repositorio.Interfaces
{
    public interface IRepositoryBase<TEntidade, TId>
         where TEntidade : EntidadeId<TId>
    {
        void Inserir(TEntidade entidade);
        void Editar(TEntidade entidade);
        void Excluir(TEntidade entidade);
        void Dispose();
        TEntidade Salvar(TEntidade entidade);
        TEntidade ObterPorId(TId id);
        IList<TEntidade> ObterTodos();
        IList<TEntidade> Obter(Expression<Func<TEntidade, bool>> predicate);
    }
}