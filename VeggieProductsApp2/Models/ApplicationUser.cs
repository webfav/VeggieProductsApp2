using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace VeggieProductsApp2.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Navn")]
        public string FullName { get; set; }
        [DisplayName("Postnr")]
        public string Zipcode { get; set; }

    }
}
