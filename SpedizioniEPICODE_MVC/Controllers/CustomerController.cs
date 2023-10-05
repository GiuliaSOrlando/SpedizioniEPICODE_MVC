using SpedizioniEPICODE_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpedizioniEPICODE_MVC.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Create()
        {
            Customers customer = new Customers();
            return View(customer);
        }

        [HttpPost]
        public ActionResult Create(Customers customer)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SpedizioniEPICODE"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                try
                {
                    sqlConnection.Open();
                    SqlCommand inserisciCliente = new SqlCommand("INSERT INTO Clienti(Nome, Cognome, IsAzienda, PartitaIva, CF)" +
                        " VALUES(@Nome, @Cognome, @IsAzienda, @PartitaIva, @CF)", sqlConnection);

                    inserisciCliente.Parameters.AddWithValue("@Nome", customer.Nome);
                    inserisciCliente.Parameters.AddWithValue("@Cognome", customer.Cognome);
                    inserisciCliente.Parameters.AddWithValue("@IsAzienda", customer.IsAzienda);
                    inserisciCliente.Parameters.AddWithValue("@PartitaIva", (object)customer.PartitaIva ?? DBNull.Value);
                    inserisciCliente.Parameters.AddWithValue("@CF", (object)customer.CF ?? DBNull.Value);

                    inserisciCliente.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    sqlConnection.Close();
                }
            }
            return View(customer);
        }
    }
}