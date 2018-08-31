using ApresentacaoFramework.Helpers;
using Framework.Dominio.Base;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ApresentacaoFramework.Models.Helpers.Bases
{
    public static class EntidadeBaseHelper
    {
        public static HtmlString GetHtmlAuditRowModel<TId>(this IHtmlHelper<IEnumerable<EntidadeAuditStatus<TId>>> helper, EntidadeAuditStatus<TId> model)
        {
            string html = string.Concat("<tr> ",
                                    helper.SDisplayFor(modelItem => model.UsuarioCadastro),
                                    helper.SDisplayFor(modelItem => model.UsuarioAlteracao),
                                    helper.SDisplayFor(modelItem => model.DataAlteracao),
                                    helper.SDisplayFor(modelItem => model.DataCadastro),
                                "</tr> ");

            return new HtmlString(html);
        }
    }
}