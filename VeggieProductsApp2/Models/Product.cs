using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VeggieProductsApp2.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Produktets navn skal tilføjes!")]
        [DisplayName("Produkt")]
        [StringLength(70, MinimumLength = 1)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "En beskrivelse produktet skal tilføjes!")]
        [DisplayName("Beskrivelse")]
        [StringLength(300, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Tilføj produktets mærke/brand")]
        [DisplayName("Mærke")]
        public string BrandName { get; set; }

        [Required(ErrorMessage = "Tilføj pris")]
        [DisplayName("Pris")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }

        [Required(ErrorMessage = "Tilføj dato")]
        [DisplayName("Dato")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Tilføj butikkens navn")]
        [DisplayName("Butik")]
        public string ShopName { get; set; }

        [Required(ErrorMessage = "Tilføj adresse")]
        [DisplayName("Adresse")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Tilføj postnr")]
        [DisplayName("Postnr")]
        public int? Zipcode { get; set; }

        [Required(ErrorMessage = "Tilføj by")]
        [DisplayName("By")]
        public string City { get; set; }

        [DisplayName("Kategori")]
        public int CategoryId { get; set; }

        [DisplayName("Kategori")]
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
