using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SpedizioniEPICODE_MVC.Models;

namespace SpedizioniEPICODE_MVC.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Users users)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SpedizioniEPICODE"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand inserisciUtente = new SqlCommand("INSERT INTO Utenti (Username, Password, Ruolo) VALUES (@Username, @Password, @Ruolo)", sqlConnection);

                inserisciUtente.Parameters.AddWithValue("@Username", users.Username);
                inserisciUtente.Parameters.AddWithValue("@Password", users.Password);
                inserisciUtente.Parameters.AddWithValue("@Ruolo", users.Ruolo);

                int recordaggiunto = inserisciUtente.ExecuteNonQuery();

                if(recordaggiunto > 0)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.MessaggioErrore = "Errore durante l'operazione di registrazione. Si prega di riprovare.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessaggioErrore = "Errore durante l'operazione di registrazione. Si prega di riprovare.";
                return View(users);
            }
            finally
            {
                sqlConnection.Close();
            }
            return View(users);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users users)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SpedizioniEPICODE"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                SqlCommand selezionaRuolo = new SqlCommand("SELECT Ruolo FROM Utenti WHERE Username = @Username AND Password = @Password", sqlConnection);

                selezionaRuolo.Parameters.AddWithValue("@Username", users.Username);
                selezionaRuolo.Parameters.AddWithValue("@Password", users.Password);

                var ruolo = selezionaRuolo.ExecuteScalar() as string;

                if (!string.IsNullOrEmpty(ruolo))
                {
                    ViewBag.RuoloUtente = ruolo;

                    if (ruolo == "Admin")
                    {
                    FormsAuthentication.SetAuthCookie(users.Username, false);
                    return RedirectToAction("Create", "Shipment");
                            
                     }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(users.Username, false);
                        return RedirectToAction("Index","Home");
                    }
                }
                else
                {
                    ViewBag.MessaggioErrore = "Credenziali non valide. Si prega di riprovare.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessaggioErrore = "Errore durante l'operazione di login. Si prega di riprovare.";
            }
            finally
            {
                sqlConnection.Close();
            }

                return View(users);

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }

}