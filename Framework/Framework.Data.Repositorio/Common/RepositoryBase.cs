using Framework.Data.Repositorio.Interfaces;
using Framework.Dominio.Base;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Data.Repositorio.Common
{
    public class RepositoryBase<TEntidade, TId> : IRepositoryBase<TEntidade, TId>, IDisposable
      where TEntidade : EntidadeId<TId>
    {
        private ISessionFactory sessionFactory;

        public RepositoryBase(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public virtual void Editar(TEntidade entidade)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    Auditar(entidade);
                    session.Save(entidade);
                }
            }
        }

        public virtual void Excluir(TEntidade entidade)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(entidade);
                }
            }
        }

        public virtual void Inserir(TEntidade entidade)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    Auditar(entidade);
                    session.Save(entidade);
                    transaction.Commit();
                }
            }
        }

        public virtual IList<TEntidade> Obter(Expression<Func<TEntidade, bool>> predicate)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                return session.Query<TEntidade>()
                .Where(predicate)
                .ToList();
            }
        }

        public virtual TEntidade ObterPorId(TId id)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                return session.Query<TEntidade>()
                .Where(c => (object)c.Id == (object)id)
                .ToList()
                .FirstOrDefault();
            }
        }

        public virtual IList<TEntidade> ObterTodos()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                return session.Query<TEntidade>().ToList();
            }
        }

        public virtual TEntidade Salvar(TEntidade entidade)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                return session.Merge(entidade);
            }
        }

        public virtual void Dispose()
        {
            sessionFactory.Dispose();
        }

        protected virtual void Auditar(TEntidade entidade)
        {
            if (!(entidade is EntidadeAuditStatus<TId>))
            {
                return;
            }

            EntidadeAuditStatus<TId> entidadeAudit = entidade as EntidadeAuditStatus<TId>;
            entidadeAudit.Revisao += 1;

            if (entidadeAudit.Id.Equals(default(TId)))
            {
                entidadeAudit.DataCadastro = DateTime.Now;
                entidadeAudit.UsuarioCadastro = "Sistemas";
            }
            else
            {
                entidadeAudit.DataAlteracao = DateTime.Now;
                entidadeAudit.UsuarioAlteracao = "Sistemas";
            }
        }
    }
}