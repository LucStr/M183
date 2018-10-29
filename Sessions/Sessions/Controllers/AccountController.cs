using System.Web.Mvc;

namespace Sessions.Controllers
{
  public class AccountController : Controller
  {
    // GET: Account
    public ActionResult Index()
    {
      return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Login(string username, string password)
    {
      if (CheckCredentials(username, password))
      {
        return RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
      }
      else
      {
        return View();
      }
    }

    private bool CheckCredentials(string username, string password)
    {
      return username == "user" && password == "pw";
    }
  }
}