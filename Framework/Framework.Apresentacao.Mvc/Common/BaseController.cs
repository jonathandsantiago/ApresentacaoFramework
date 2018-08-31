using Framework.Aplicacao.Servico.Interfaces;
using Framework.Dominio.Base;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using System;

namespace Framework.Apresentacao.Mvc.Common
{
    public class BaseController<TEntidade, TId, TAppService> : Controller
        where TEntidade : EntidadeId<TId>
        where TAppService : IAppServiceBase<TEntidade, TId>
    {
        private readonly TAppService _appService;

        public BaseController(ISessionFactory sessionFactory)
        {
            _appService = (TAppService)Activator.CreateInstance(typeof(TAppService), args: sessionFactory);
        }

        public virtual ActionResult Index()
        {
            return View(_appService.ObterTodos());
        }

        public virtual ActionResult Detail(TId id)
        {
            return View(_appService.ObterPorId(id));
        }

        public virtual ActionResult Create()
        {
            return View();
        }

        public virtual ActionResult Edit(TId id)
        {
            return View(_appService.ObterPorId(id));
        }

        public virtual ActionResult Delete(TId id)
        {
            return View(_appService.ObterPorId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(TEntidade entidade)
        {
            if (ModelState.IsValid && ExecultarPorAcao(Acao.Inserir, entidade))
            {
                return RedirectToAction("Index");
            }

            return View(entidade);
        }

        [HttpPost]
        public virtual ActionResult Edit(TEntidade entidade)
        {
            if (ModelState.IsValid && ExecultarPorAcao(Acao.Editar, entidade))
            {
                return RedirectToAction("Index");
            }

            return View(entidade);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteConfirmed(TId id)
        {
            var entidade = _appService.ObterPorId(id);

            if (ExecultarPorAcao(Acao.Excluir, entidade))
            {
                return RedirectToAction("Index");
            }

            return View(entidade);
        }

        protected virtual bool ExecultarPorAcao(Acao acao, TEntidade entidade)
        {
            bool resultado = false;

            switch (acao)
            {
                case Acao.Inserir:
                    resultado = _appService.Inserir(entidade);
                    break;
                case Acao.Editar:
                    resultado = _appService.Editar(entidade);
                    break;
                case Acao.Excluir:
                    resultado = _appService.Excluir(entidade);
                    break;
                default:
                    break;
            }

            foreach (var error in _appService.Resultado.Errors)
            {
                ModelState.AddModelError("", error.Message);
            }

            return resultado;
        }
    }
}