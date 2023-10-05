using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpedizioniEPICODE_MVC.Models
{
    public class Users
    {
        public int IdUtente { get; set; }

        [Required(ErrorMessage = "Il campo username è obbligatorio.")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Il campo password è obbligatorio.")]
        [StringLength(20)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Il campo ruolo è obbligatorio.")]
        [StringLength(20)]
        public string Ruolo { get; set; } = "User";
    }
}