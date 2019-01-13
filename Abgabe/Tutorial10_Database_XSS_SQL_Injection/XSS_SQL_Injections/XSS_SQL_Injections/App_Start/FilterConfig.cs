using System.Web;
using System.Web.Mvc;

namespace XSS_SQL_Injections
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }
  }
}
