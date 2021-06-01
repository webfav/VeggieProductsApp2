using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VeggieProductsApp2.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Kategori")]
        [Required]
        public string CategoryName { get; set; }

        [Display(Name = "Beskrivelse")]
        public string CategoryDescription { get; set; }

    }
}
