using Framework.Aplicacao.Servico.Interfaces;
using Framework.Data.Repositorio.Interfaces;
using Framework.Dominio.Base;
using Framework.Validator.Validation;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Framework.Aplicacao.Servico.Common
{
    public class AppServiceBase<TEntidade, TId, TRepository> : IAppServiceBase<TEntidade, TId>
          where TEntidade : EntidadeId<TId>
          where TRepository : IRepositoryBase<TEntidade, TId>
    {
        private readonly TRepository _repository;
        protected virtual Validation<TEntidade> Validador { get; set; }
        public virtual ValidationResult Resultado { get; set; }

        public AppServiceBase(ISessionFactory sessionFactory)
        {
            _repository = (TRepository)Activator.CreateInstance(typeof(TRepository), args: sessionFactory);
        }

        public virtual TEntidade ObterPorId(TId id)
        {
            return _repository.ObterPorId(id);
        }

        public virtual IList<TEntidade> ObterTodos()
        {
            return _repository.ObterTodos();
        }

        public virtual IList<TEntidade> Obter(Expression<Func<TEntidade, bool>> predicate)
        {
            return _repository.Obter(predicate);
        }

        public virtual bool Inserir(TEntidade entidade)
        {
            if (!Validar(entidade))
            {
                return false;
            }

            _repository.Inserir(entidade);
            return true;
        }

        public virtual bool Editar(TEntidade entidade)
        {
            if (!Validar(entidade))
            {
                return false;
            }

            _repository.Editar(entidade);
            return true;
        }

        public virtual bool Excluir(TEntidade entidade)
        {
            if (!Validar(entidade))
            {
                return false;
            }

            _repository.Excluir(entidade);
            return true;
        }

        protected virtual bool Validar(TEntidade entidade)
        {
            if (Validador != null)
            {
                Resultado = Validador.Valid(entidade);
            }

            return Resultado.IsValid;
        }

        public virtual void Dispose()
        {
            _repository.Dispose();
        }
    }
}