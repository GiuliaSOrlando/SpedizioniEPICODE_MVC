using SpedizioniEPICODE_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpedizioniEPICODE_MVC.Controllers
{

    public class ShipmentController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Shipments shipment)
        {
            if (ModelState.IsValid)
            {
                bool spedizioneDaCreare = shipment.CreaSpedizione();
                if (spedizioneDaCreare)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Impossibile creare la spedizione. Riprova");
                }
            }

            return View(shipment);
        }

        public ActionResult Track()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CercaSpedizionePrivati(string CF, string numeroSpedizione)
        {
            List<Shipments> shipments = Shipments.TracciaSpedizionePerCF(CF, numeroSpedizione);
            return Json(shipments);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewAll()
        {
            List<Shipments> spedizioniDB = Shipments.CaricaSpedizioniDB();
            return View(spedizioniDB);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            Shipments shipment = Shipments.DettagliSpedizione(id);
            if (shipment != null)
            {
                return View(shipment);
            }
            else
                return HttpNotFound();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int id)
        {

            Shipments spedizioneDaModificare = Shipments.DettagliSpedizione(id);

            if (spedizioneDaModificare == null)
            {

                return HttpNotFound();
            }

            return View(spedizioneDaModificare);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Shipments spedizioneModificata)
        {
            if (ModelState.IsValid)
            {
                bool successo = spedizioneModificata.ModificaSpedizione(spedizioneModificata);

                if (successo)
                {
                    return RedirectToAction("ViewAll");
                }
                else
                {
                    ModelState.AddModelError("", "Si è verificato un errore durante la modifica della spedizione.");
                }
            }

            return View(spedizioneModificata);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewDate()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult VisualizzaSpedizioniFiltro(
            string period,
            DateTime? selectedDate,
            string numeroIdentificativo,
            string statoSpedizione,
            string cittaDestinataria,
            decimal? pesoMinimo,
            decimal? pesoMassimo,
            DateTime? dataConsegnaPrevistaMinima,
            DateTime? dataConsegnaPrevistaMassima,
            decimal? costoMinimo,
            decimal? costoMassimo,
            string clienteNome,
            string clienteCognome)
        {
            try
            {
                DateTime? dataInizio = null;
                DateTime? dataFine = null;

                if (!string.IsNullOrEmpty(period))
                {
                    DateTime today = DateTime.Today;

                    switch (period)
                    {
                        case "oggi":
                            dataInizio = today;
                            dataFine = today;
                            break;
                        case "settimana":
                            dataInizio = today.AddDays(-6);
                            dataFine = today;
                            break;
                        case "mese":
                            dataInizio = today.AddMonths(-1);
                            dataFine = today;
                            break;
                        case "sei-mesi":
                            dataInizio = today.AddMonths(-6);
                            dataFine = today;
                            break;
                        default:
                            return Json(new { error = "Periodo non valido." });
                    }
                }
                else if (selectedDate.HasValue)
                {
                    dataInizio = selectedDate.Value;
                    dataFine = selectedDate.Value;
                }

                List<Shipments> spedizioni = Shipments.VisualizzaSpedizioneFiltro(
                    dataInizio,
                    dataFine,
                    numeroIdentificativo,
                    statoSpedizione,
                    cittaDestinataria,
                    pesoMinimo,
                    pesoMassimo,
                    dataConsegnaPrevistaMinima,
                    dataConsegnaPrevistaMassima,
                    costoMinimo,
                    costoMassimo,
                    clienteNome,
                    clienteCognome);

                return Json(spedizioni);
            }
            catch (Exception ex)
            {

                return Json(new { error = ex.Message });
            }


        }

        [HttpGet]
        public ActionResult CountState()
        {
            return View();
        }

        [HttpPost]
        public JsonResult TotaleInAttesa()
        {
            int totalInAttesa = Shipments.ContaInAttesa();

            var result = new
            {
                TotalCount = totalInAttesa
            };

            return Json(result);
        }

        [HttpGet]
        public ActionResult GroupCity()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GruppiCitta()
        {
            List<string> cittaDestinatarie = Shipments.RaggruppaPerCitta();

            return Json(cittaDestinatarie);
        }
    }
    }


   




