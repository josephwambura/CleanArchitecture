using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clean.Architecture.Web;

public static class HtmlHelpers
{
  public static string IsSelected(this IHtmlHelper html, string? controller = null, string? action = null, string? page = null, string? pageController = null, string? cssClass = null)
  {
    if (string.IsNullOrWhiteSpace(cssClass))
      cssClass = "active";

    string? currentAction = $"{html.ViewContext.RouteData.Values["action"]}";
    string? currentController = $"{html.ViewContext.RouteData.Values["controller"]}";
    string? currentPage = $"{html.ViewContext.RouteData.Values["page"]}";

    if (!string.IsNullOrWhiteSpace(pageController))
    {
      var currentPageList = currentPage.Split('/').ToList();

      currentPageList.RemoveAll(i => string.IsNullOrWhiteSpace(i));

      string? currentPageController = currentPageList.Count == 2 ? currentPageList[0] : string.Empty;

      return pageController == currentPageController ? cssClass : string.Empty;
    }

    if (string.IsNullOrWhiteSpace(controller))
      controller = currentController;

    if (string.IsNullOrWhiteSpace(action))
      action = currentAction;

    if (string.IsNullOrWhiteSpace(page))
      page = currentPage;

    if (!string.IsNullOrWhiteSpace(page))
      return page == currentPage ? cssClass : string.Empty;

    return controller == currentController && action == currentAction ?
        cssClass : string.Empty;
  }

  public static string? PageClass(this IHtmlHelper htmlHelper)
  {
    string? currentAction = $"{htmlHelper.ViewContext.RouteData.Values["action"]}";
    return currentAction;
  }

}
