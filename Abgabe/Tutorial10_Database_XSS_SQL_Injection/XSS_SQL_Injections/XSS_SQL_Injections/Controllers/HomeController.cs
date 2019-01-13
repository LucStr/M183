using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XSS_SQL_Injections.Controllers
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
      
      SqlConnection connection = new SqlConnection();
      connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\kraemery\\Documents\\sql_xss_injection.mdf\";Integrated Security=True;Connect Timeout=30";
      SqlCommand cmd = new SqlCommand();
      SqlDataReader reader;
      //cmd.CommandText = $"Select [Id] ,[username] ,[password] FROM [dbo].[User] WHERE [username] = '{username}' AND [password] = '{password}'";
      cmd.CommandText = $"Select [Id] ,[{nameof(username)}] ,[{nameof(password)}] FROM [dbo].[User] WHERE [{nameof(username)}] = '{username}' AND [{nameof(password)}] = '{password}'";
      cmd.Connection = connection;

      connection.Open();

      reader = cmd.ExecuteReader();
      if (reader.HasRows)
      {
        ViewBag.Message = "success";
        while (reader.Read())
        {
          ViewBag.Message += $"{reader.GetInt32(0)} {reader.GetString(1)} {reader.GetString(2)}";
        }
      }
      else
      {
        ViewBag.Message = "No rows found.";
      }
      connection.Close();
      return View(nameof(Login));
    }

    [HttpGet]
    public ActionResult Feedback()
    {
      return View();
    }

    [HttpPost]
    public ActionResult DoFeedback()
    {
      string feedback = Request[nameof(feedback)];

      SqlConnection connection = new SqlConnection();
      connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\kraemery\\Documents\\sql_xss_injection.mdf\";Connect Timeout=30";
      SqlCommand cmd = new SqlCommand();
      SqlDataReader reader;
      //cmd.CommandText = $"Select [Id] ,[username] ,[password] FROM [dbo].[User] WHERE [username] = '{username}' AND [password] = '{password}'";
      cmd.CommandText = $"INSERT INTO Feedback SET [{nameof(feedback)}] = '{feedback}'";
      cmd.Connection = connection;

      connection.Open();

      reader = cmd.ExecuteReader();
      if (reader.HasRows)
      {
        ViewBag.Message = "success";
        while (reader.Read())
        {
          ViewBag.Message += $"{reader.GetInt32(0)} {reader.GetString(1)} {reader.GetString(2)}";
        }
      }
      else
      {
        ViewBag.Message = "No rows found.";
      }
      connection.Close();
      return View(nameof(Feedback));
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