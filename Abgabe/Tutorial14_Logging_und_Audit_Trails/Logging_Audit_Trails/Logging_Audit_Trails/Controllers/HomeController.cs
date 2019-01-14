using Logging_Audit_Trails.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Logging_Audit_Trails.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public ActionResult DoLogin()
    {
      string username = Request[nameof(username)];
      string password = Request[nameof(password)];

      string ip = Request.ServerVariables["REMOTE_ADDR"];
      string platform = Request.Browser.Platform;
      string browser = Request.UserAgent;

      SqlConnection connection = new SqlConnection();
      connection.ConnectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\kraemery\\Documents\\logging_intrusion_detection.mdf; Integrated Security = True; Connect Timeout = 30";

      SqlCommand cmd_credentials = new SqlCommand();
      cmd_credentials.CommandText = $"Select [Id] FROM [dbo].[User] WHERE [{nameof(username)}] = '{username}' AND [{nameof(password)}] = '{password}'";
      cmd_credentials.Connection = connection;

      connection.Open();

      SqlDataReader reader = cmd_credentials.ExecuteReader();
      if (reader.HasRows)
      {
        int userId = 0;
        if (reader.Read())
        {
          userId = reader.GetInt32(0);
        }

        connection.Close();
        connection.Open();

        SqlCommand cmd_user_using_usual_browser = new SqlCommand();
        cmd_user_using_usual_browser.CommandText = $"SELECT Id FROM [dbo].[UserLog] WHERE [UserId] = '{userId}' AND [IP] LIKE '{ip.Substring(0, 2)}%' AND browser LIKE '{platform}%'";
        cmd_user_using_usual_browser.Connection = connection;

        SqlDataReader reader_usual_browser = cmd_user_using_usual_browser.ExecuteReader();

        connection.Close();
        connection.Open();

        SqlCommand log_cmd = new SqlCommand();
        if (!reader_usual_browser.HasRows)
        {
          // inform user that he/she should change browser
          
          log_cmd.CommandText = $"INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, Browser, AdditionalInformation) VALUES ('{userId}', '{ip}', '{nameof(DoLogin)}', 'success', GETDATE(), '{platform}', 'other browser')";
        }
        else
        {
          log_cmd.CommandText = $"INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, Browser) VALUES ('{userId}', '{ip}', '{nameof(DoLogin)}', 'success', GETDATE(), '{platform}')";
        }
        log_cmd.Connection = connection;
        log_cmd.ExecuteReader();

        ViewBag.Message = "success";
      }
      else
      {
        connection.Close();
        connection.Open();

        SqlCommand cmd_userId_by_name = new SqlCommand();

        cmd_userId_by_name.CommandText = $"SELECT [Id] FROM [dbo].[User] WHERE [{nameof(username)}] = '{username}'";
        cmd_userId_by_name.Connection = connection;

        SqlDataReader reader_userId_by_name = cmd_userId_by_name.ExecuteReader();

        if (reader_userId_by_name.HasRows)
        {
          int userId = 0;
          if (reader.Read())
          {
            userId = reader.GetInt32(0);
          }

          connection.Close();
          connection.Open();

          const int maxLoginAttempts = 5;

          SqlCommand failed_log_cmd = new SqlCommand();
          failed_log_cmd.CommandText = $"SELECT COUNT(ID) FROM [dbo].[UserLog] WHERE UserId = '{userId}' AND Result = 'failed' AND CAST(CreatedOn AS date) = '{DateTime.Now.ToShortDateString().Substring(0, 10)}'";
          failed_log_cmd.Connection = connection;

          SqlDataReader failedLoginCount = failed_log_cmd.ExecuteReader();

          int attempts = 0;
          if (failedLoginCount.HasRows && reader.Read())
          {
            attempts = reader.GetInt32(0);
          }

          if (attempts >= maxLoginAttempts || password.Length < 4 || password.Length > 20)
          {
            // block user
          }

          connection.Close();
          connection.Open();

          // log behaviour
          SqlCommand log_cmd = new SqlCommand();
          log_cmd.CommandText = $"INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, Browser) VALUES('{userId}', '{ip}', 'login', 'failed', 'GETDATE()', '{platform}')";
          log_cmd.Connection = connection;
          log_cmd.ExecuteReader();

          ViewBag.Message = "No user found";
        }
        else
        {
          connection.Close();
          connection.Open();

          // username is wrong too
          SqlCommand log_cmd = new SqlCommand();
          log_cmd.CommandText = $"INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, AdditionalInformation, Browser) VALUES('0', '{ip}', '{nameof(DoLogin)}', 'failed', GETDATE(), 'No user found', '{platform}')";
          log_cmd.Connection = connection;
          log_cmd.ExecuteReader();

          ViewBag.Message = "No user found";
        }
      }
      connection.Close();

      return View(nameof(Logs));
    }

    public ActionResult Logs()
    {
      SqlConnection connection = new SqlConnection();
      connection.ConnectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\kraemery\\Documents\\logging_intrusion_detection.mdf; Integrated Security = True; Connect Timeout = 30";

      // check credentials
      // prevent SQL injection
      SqlCommand cmd_credentials = new SqlCommand();
      //cmd_credentials.CommandText = "SELECT * FROM [dbo].[UserLog] ul JOIN [dbo].[User] u ON ul.UserId = u.Id ORDER BY ul.CreatedOn DESC";
      cmd_credentials.CommandText = "SELECT * FROM [dbo].[UserLog] ul ORDER BY ul.CreatedOn DESC";
      cmd_credentials.Connection = connection;

      connection.Open();

      SqlDataReader reader = cmd_credentials.ExecuteReader();

      if (reader.HasRows)
      {
        List<HomeControllerViewModel> viewModels = new List<HomeControllerViewModel>();
        while (reader.Read())
        {
          HomeControllerViewModel viewModel = new HomeControllerViewModel();
          viewModel.UserId = reader.GetValue(0).ToString();
          viewModel.LogId = reader.GetValue(1).ToString();
          viewModel.LogCreatedOn = reader.GetValue(2).ToString();
          // load other values too...

          viewModels.Add(viewModel);
        }

        return View(viewModels);
      }
      else
      {
        ViewBag.message = "No results found";
        return View();
      }
    }

    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }
  }
}