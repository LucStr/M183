using System.Web.Mvc;

namespace TwoFactorAuthenticationMVC.Controllers
{
  using System.IO;
  using System.Net;
  using System.Text;

  public class AccountController : Controller
  {
    [HttpPost]
    public ActionResult Login()
    {
      string username, password;
      username = this.Request[nameof(username)];
      password = this.Request[nameof(password)];

      const string usernameTest = "test";
      const string passwordTest = "pwTest";
      if (username == usernameTest && password == passwordTest)
      {
        const string nexmoLink = "https://rest.nexmo.com/sms/json";
        var request = (HttpWebRequest) WebRequest.Create(nexmoLink);

        var secret = "TEST_SECRET";

        var postData = "api_key=";
        postData += "&api_secret=";
        postData += "&to=";
        postData += "&from=\"\"NEXMO\"\"";
        postData += $"&text=\"{secret}\"";
        var data = Encoding.ASCII.GetBytes(postData);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
          stream.Write(data, 0, data.Length);
        }

        var response = (HttpWebResponse) request.GetResponse();

        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        this.ViewBag.Message = responseString;
      }
      else
      {
        this.ViewBag.Message = "wrong credentials";
      }

      return View();
    }


  }
}