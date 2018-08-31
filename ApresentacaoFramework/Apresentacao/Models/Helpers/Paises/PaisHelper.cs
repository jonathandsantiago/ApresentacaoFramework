using ApresentacaoFramework.Helpers;
using Dominio.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace ApresentacaoFramework.Models.Helpers.Paises
{
    public static class PaisHelper
    {
        public static HtmlString GetHtmlHeaderModel(this IHtmlHelper<IEnumerable<Pais>> helper)
        {
            string html = string.Concat(helper.SDisplayNameFor(model => model.Id),
                                        helper.SDisplayNameFor(model => model.Nome));

            return new HtmlString(html);
        }

        public static HtmlString GetHtmlRowsModel(this IHtmlHelper<IEnumerable<Pais>> helper, IEnumerable<Pais> model)
        {
            string html = string.Empty;

            foreach (Pais item in model.ToList())
            {
                html += string.Concat("<tr> ",
                                        helper.SDisplayFor(modelItem => item.Id),
                                        helper.SDisplayFor(modelItem => item.Nome),
                                        helper.ActionButtons(route: item.Id).ToHtmlString(),
                                    "</tr> ");
            }

            return new HtmlString(html);
        }

        public static HtmlString GetHtmlFildsModel(this IHtmlHelper<Pais> helper, bool editable = true)
        {
            string html = string.Concat(helper.SEditFor(model => model.Nome, "Nome", editable));

            return new HtmlString(html);
        }
    }
}
