using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpedizioniEPICODE_MVC.Models
{
    public class Customers
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Il campo nome è obbligatorio.")]
        [Display(Name = "Nome")]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il campo cognome è obbligatorio.")]
        [Display(Name = "Cognome")]
        [StringLength(50)]
        public string Cognome { get; set; }

        [Display(Name = "Tipo cliente")]
        public bool IsAzienda { get; set; }

        [Display(Name = "Partita IVA")]
        [StringLength(20)]
        public string PartitaIva { get; set; }

        [Display(Name = "Codice Fiscale")]
        [StringLength(16)]
        public string CF { get; set; }
    }
}