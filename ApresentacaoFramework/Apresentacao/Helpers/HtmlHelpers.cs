using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;

namespace ApresentacaoFramework.Helpers
{
    public static class HtmlHelpers
    {
        public static HtmlString ActionButtons(this IHtmlHelper helper, int route, string defaultAction = "Detail", string descriptionAction = "Detalhes", bool editable = true, bool removible = true, params Tuple<string, string>[] customActions)
        {
            string actions = string.Empty;

            if (customActions.Length > 0)
            {
                foreach (var action in customActions)
                {
                    string description = action.Item1 ?? action.Item2;
                    actions += helper.ActionLink(description, action.Item1, new { id = route }).ToHtmlString();
                }
            }
            string actionEdit = editable ? helper.ActionLink("Editar", "Edit", new { id = route }, new { @class = "btn btn-white btn-sm" }).ToHtmlString() : string.Empty;
            string actionDelete = removible ? helper.ActionLink("Deletar", "Delete", new { id = route }, new { @class = "btn btn-white btn-sm" }).ToHtmlString() : string.Empty;
            string html = $@"<div class='btn-group'>
                                {helper.ActionLink(descriptionAction, defaultAction, new { id = route }, new { @class = "btn btn-white btn-sm" }).ToHtmlString()}
                                {actionEdit}
                                {actionDelete}
                                {actions}
                            </div>";

            return new HtmlString($@"<td class='text-right'>{html}</td>");
        }

        public static HtmlString ActionGroup(this IHtmlHelper helper, int route, string defaultAction = "Detail", string descriptionAction = "Detalhes", bool editable = true, bool removible = true, params Tuple<string, string>[] customActions)
        {
            string actions = string.Empty;

            if (editable)
            {
                actions += CreateActionLink(helper, action: "Edit", route: route, description: "Editar");
            }

            if (removible)
            {
                actions += CreateActionLink(helper, action: "Delete", route: route, description: "Remover");
            }

            if (customActions.Length > 0)
            {
                actions += "<li role=\"separator\" class=\"divider\"></li>";

                foreach (var action in customActions)
                {
                    actions += CreateActionLink(helper, action.Item1, route, action.Item2);
                }
            }

            string html = $@"<div class='btn-group'>
                                {helper.ActionLink(descriptionAction, defaultAction, new { id = route }, new { @class = "btn btn-default btn-sm" }).ToHtmlString()}
                                <button type='button' class='btn btn-default btn-sm dropdown-toggle' data-toggle='dropdown'>
                                    <span class='caret'></span>
                                </button>
                                    <ul class='dropdown-menu'>{actions}</ul>
                            </div>";

            return new HtmlString(string.Concat("<td> ", html, "</td> "));
        }

        public static HtmlString SDisplayFor<TModel, TResult>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression)
        {
            return new HtmlString($@"<td>{helper.DisplayFor(expression).ToHtmlString()}</td>");
        }

        public static string SVersionSistem()
        {
            return "Ap.Framework - V1.0";
        }

        public static string SEditFor<TModel, TResult>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, string labelText, bool editable)
        {
            var readOnly = new { htmlAttributes = new { @class = "form-control", disabled = "disabled", @readonly = "readonly" } };

            return $@"<div class='editor-label'>
                            {helper.LabelFor(expression, labelText).ToHtmlString()}
                        </div>
                        <div class='editor-field'>
                            {helper.EditorFor(expression, editable ? null : readOnly).ToHtmlString()}
                            {helper.ValidationMessageFor(expression).ToHtmlString()}
                        </div>";
        }

        public static string SDisplayNameFor<TModelItem, TResult>(this IHtmlHelper<IEnumerable<TModelItem>> helper, Expression<Func<TModelItem, TResult>> expression)
        {
            return $@"<th>{helper.DisplayNameFor(expression)}</th>";
        }

        public static string IsSelected(this IHtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {
            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string PageClass(this IHtmlHelper htmlHelper)
        {
            string currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

        public static string ToHtmlString(this IHtmlContent content)
        {
            var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        private static string CreateActionLink(IHtmlHelper helper, string action, int route, string description = null)
        {
            description = description ?? action;
            return $@"<li>{helper.ActionLink(description, action, new { id = route }).ToHtmlString()}</li>";
        }
    }
}